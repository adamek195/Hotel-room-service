using HotelRoomService.Domain.Entities;
using HotelRoomService.Domain.Interfaces;
using HotelRoomService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelRoomService.Infrastructure.Repositories
{
    public class RoomsRepository : IRoomsRepository
    {
        private readonly HotelRoomServiceContext _context;

        public RoomsRepository(HotelRoomServiceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(string? name, int? size, bool? available)
        {
            IQueryable<Room> query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            if(size.HasValue)
                query = query.Where(x => x.Size == size.Value);

            if (available.HasValue)
                query = query.Where(x => x.IsAvailable == available.Value);

            return await query.ToListAsync();
        }

        public async Task AddRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }
    }
}
