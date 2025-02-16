using HotelRoomService.API.Installers;
using HotelRoomService.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelRoomServiceContext>();
    dbContext.Database.Migrate();
}

app.UseRouting();
app.MapControllers();

app.Run();
