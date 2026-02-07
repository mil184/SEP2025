using Domain.Mapping;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Clients;
using Infrastructure.DataContext;
using Infrastructure.RabbitMq;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebShop.Validators;

var builder = WebApplication.CreateBuilder(args);

// logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// app db context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

////builder.Services.AddDbContext<AppDbContext>(options =>
////    options.UseNpgsql(
////        connectionString,
////        b => b.MigrationsAssembly("Infrastructure")
////    ));

// bootstrap
//var frontendUrl = builder.Configuration["Frontend:BaseUrl"]!;
//builder.Services.AddCorsConfiguration(frontendUrl);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    );
});

builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddHostedService<PaymentFinalizedConsumer>();

// Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IPaymentInitializationRequestRepository, PaymentInitializationRequestRepository>();



// Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IPaymentInitializationRequestService, PaymentInitializationRequestService>();


// other

builder.Services.AddScoped<PasswordValidator, PasswordValidator>();
builder.Services.AddScoped<LoginValidator, LoginValidator>();
builder.Services.AddScoped<RegisterValidator, RegisterValidator>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new UserMappingProfile());
});

// psp client
builder.Services.AddHttpClient<PspClient, PspClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7154/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Hello from {Route} at {Time}", "/", DateTimeOffset.UtcNow);
    return "OK";
});

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
