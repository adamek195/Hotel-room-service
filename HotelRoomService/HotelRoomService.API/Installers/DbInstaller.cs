using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HotelRoomService.Infrastructure.Data;

namespace HotelRoomService.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<HotelRoomServiceContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("HotelRoomServiceCS")));
        }
    }
}
