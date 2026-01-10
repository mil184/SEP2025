using Domain.Repository;
using Domain.Service;
using Infrastructure.Clients;
using Infrastructure.DataContext;
using Infrastructure.Repository;
using Infrastructure.Service;
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
        policy.WithOrigins("http://localhost:4201")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
    );
});

builder.Services.AddScoped<IBankPaymentRequestRepository, BankPaymentRequestRepository>();
builder.Services.AddScoped<IBankPaymentRequestService, BankPaymentRequestService>();
builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IBankMerchantInformationsRepository, BankMerchantInformationsRepository>();
builder.Services.AddScoped<IBankMerchantInformationsService, BankMerchantInformationsService>();

// bank client
builder.Services.AddHttpClient<BankClient, BankClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7147/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
