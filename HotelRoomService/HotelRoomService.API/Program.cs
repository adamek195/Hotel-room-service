using HotelRoomService.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelRoomServiceContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("HotelRoomServiceCS")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelRoomServiceContext>();
    dbContext.Database.Migrate();
}

app.MapGet("/", () => "Hello World!");

app.Run();
