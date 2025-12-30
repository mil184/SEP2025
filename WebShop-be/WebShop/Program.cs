using Domain.Mapping;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.DataContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebShop.Validators;

var builder = WebApplication.CreateBuilder(args);

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

// Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();



// Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReservationService, ReservationService>();


// other

builder.Services.AddScoped<PasswordValidator, PasswordValidator>();
builder.Services.AddScoped<LoginValidator, LoginValidator>();
builder.Services.AddScoped<RegisterValidator, RegisterValidator>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new UserMappingProfile());
});

builder.Services.AddControllers();
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
