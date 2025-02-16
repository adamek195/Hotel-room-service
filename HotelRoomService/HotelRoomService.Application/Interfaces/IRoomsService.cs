using HotelRoomService.Application.Dtos;

namespace HotelRoomService.Application.Interfaces
{
    public interface IRoomsService
    {
        Task<IEnumerable<RoomDto>> GetRoomsAsync(string? name, int? size, bool? available);

        Task<Guid> CreateRoomAsync(CreateRoomRequest request);
    }
}
