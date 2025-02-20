using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HotelRoomService.Infrastructure.Data;

namespace HotelRoomService.IntegrationTests
{
    public class HotelRoomServiceWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _connectionString;

        public HotelRoomServiceWebApplicationFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HotelRoomServiceContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HotelRoomServiceContext>(options =>
                    options.UseNpgsql(_connectionString));

                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<HotelRoomServiceContext>();
                context.Database.Migrate();
            });
        }
    }
}
