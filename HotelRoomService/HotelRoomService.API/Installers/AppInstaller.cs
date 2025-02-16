using HotelRoomService.Application.Services;
using HotelRoomService.Application.Interfaces;
using HotelRoomService.Domain.Interfaces;
using HotelRoomService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelRoomService.API.Filters;
using System.Text.Json.Serialization;

namespace HotelRoomService.API.Installers
{
    public class AppInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<IRoomsService, RoomsService>();
            services.AddTransient<IRoomsRepository, RoomsRepository>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new GlobalExceptionFilter());
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                }); ;
        }
    }
}
