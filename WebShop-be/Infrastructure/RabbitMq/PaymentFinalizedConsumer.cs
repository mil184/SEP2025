using Domain.Services;
using Infrastructure.RabbitMq;
using Infrastructure.RabbitMq.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public sealed class PaymentFinalizedConsumer : BackgroundService
{
    private readonly RabbitMQSetting _settings;
    private readonly IServiceScopeFactory _scopeFactory;

    private IConnection? _connection;
    private IChannel? _channel;

    public PaymentFinalizedConsumer(
        IOptions<RabbitMQSetting> options,
        IServiceScopeFactory scopeFactory)
    {
        _settings = options.Value;
        _scopeFactory = scopeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync();

        // IMPORTANT: the QueueDeclare settings must match what the publisher uses.
        await _channel.QueueDeclareAsync(
            queue: "payment.finalized",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        // Optional: process 1 message at a time per consumer instance
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null)
            throw new InvalidOperationException("Channel not initialized. Did StartAsync run?");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = JsonConvert.DeserializeObject<PaymentFinalizedEvent>(json);

                if (evt is null)
                    throw new Exception("Message deserialized to null.");

                using var scope = _scopeFactory.CreateScope();
                var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();

                var reservation = reservationService.GetById(evt.OrderId);
                reservation.Status = evt.Status;
                reservationService.Update(reservation);

                // If processing succeeded -> ACK
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {

            }
        };

        // Start consuming (manual acks => autoAck: false)
        await _channel.BasicConsumeAsync(
            queue: "payment.finalized",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        // Keep the background service alive
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken);

            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken);
        }
        finally
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        await base.StopAsync(cancellationToken);
    }
}
