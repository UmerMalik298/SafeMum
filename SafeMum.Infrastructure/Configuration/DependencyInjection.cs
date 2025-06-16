using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Interfaces;
using SafeMum.Infrastructure.Services;




namespace SafeMum.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddScoped<ISupabaseClientFactory, SupabaseClientFactory>();

            service.AddHttpClient<ITranslationService, TranslationService>();

            service.AddScoped<IImageUploadService, ImageUploadService>();
  
            service.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterUserRequest).Assembly));

            return service;
        }
    }
}
