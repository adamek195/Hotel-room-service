using HotelRoomService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelRoomService.Domain.Interfaces
{
    public interface IRoomsRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync(string? name, int? size, bool? available);

        Task AddRoomAsync(Room room);
    }
}
