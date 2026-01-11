using Infrastructure.Clients;
using Infrastructure.DataContext;
using Infrastructure.Repositories;
using Infrastructure.Repository;
using Infrastructure.Service;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// app db context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:4202")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
    );
});

// psp client
builder.Services.AddHttpClient<PspClient, PspClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7154/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddScoped<PaymentService, PaymentService>();
builder.Services.AddScoped<PaymentRepository, PaymentRepository>();
builder.Services.AddScoped<BankPaymentRequestService, BankPaymentRequestService>();
builder.Services.AddScoped<BankPaymentRequestRepository, BankPaymentRequestRepository>();
builder.Services.AddScoped<PaymentFinalizationRepository, PaymentFinalizationRepository>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
