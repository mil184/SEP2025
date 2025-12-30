using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class Bootstrap
    {
        public static IServiceCollection AddCorsConfiguration(
            this IServiceCollection services,
            string frontendUrl)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                    policy.WithOrigins(frontendUrl)
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .AllowAnyMethod());
            });

            return services;
        }
    }
}
