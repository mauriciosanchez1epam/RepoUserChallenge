using Application.DTOs;
using Application.Interfaces.Services;
using Application.MappingProfiles;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddInApplication(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IUserService, UserService>();

            //FluentValidation
            services.AddValidatorsFromAssemblyContaining<UserDTO>();
            
            //AllMappers
            services.AddAutoMapper(typeof(UserProfile));

            return services;
        }
    }
}
