using Application.Interfaces;
using Application.Interfaces.Repository;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}
