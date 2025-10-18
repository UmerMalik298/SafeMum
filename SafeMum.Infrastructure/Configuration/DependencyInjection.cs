using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SafeMum.Application.Features.Communication.ChatGroups.CreateChatGroup;
using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Interfaces;
using SafeMum.Application.Repositories;
using SafeMum.Application.Services;
using SafeMum.Infrastructure.Services;
using SafeMum.Infrastructure.Services.SafeMum.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;




namespace SafeMum.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
           // service.AddHttpContextAccessor();
            service.AddScoped<ISupabaseClientFactory, SupabaseClientFactory>();

            service.AddHttpClient<ITranslationService, TranslationService>();

            service.AddScoped<IImageUploadService, ImageUploadService>();
            service.AddScoped<IPregnancyTrackerService, PregnancyTrackerService>();
            service.AddScoped<IMessageRepository, MessageRepository>();
        
            service.AddScoped<ISupabaseAdminService, SupabaseAdminService>();
         
            service.AddScoped<IVoiceUploadService, VoiceUploadService>();




            //service.AddScoped<IPushNotificationService, FirebaseNotificationService>();
            service.AddScoped<IPushNotificationService, NodePushNotificationService>();

            service.AddScoped<IInAppNotificationService, InAppNotificationService>();
            service.AddScoped<IReminderJob, AppointmentReminderJob>();
            //service.AddScoped<SignalRNotificationGateway, >();
            //service.AddHostedService<PrenatalReminderScheduler>();


            service.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterUserRequest).Assembly));

            service.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateChatGroupHandler>();
            });


            return service;
        }
    }
}
