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

            builder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Size).IsRequired();
                entity.Property(r => r.IsAvailable).IsRequired();
                entity.Property(r => r.Status).IsRequired();
                entity.Property(r => r.Details).HasMaxLength(1000);
            });
        }
    }
}
