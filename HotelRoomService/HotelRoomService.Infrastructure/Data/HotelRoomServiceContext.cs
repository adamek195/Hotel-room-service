using HotelRoomService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelRoomService.Infrastructure.Data
{
    public class HotelRoomServiceContext : DbContext
    {
        public HotelRoomServiceContext(DbContextOptions<HotelRoomServiceContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
