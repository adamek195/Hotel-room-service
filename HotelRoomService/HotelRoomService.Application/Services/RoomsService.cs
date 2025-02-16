using HotelRoomService.Application.Dtos;
using HotelRoomService.Application.Interfaces;
using HotelRoomService.Domain.Entities;
using HotelRoomService.Domain.Interfaces;

namespace HotelRoomService.Application.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsService(IRoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsAsync(string? name, int? size, bool? available)
        {
            var rooms = await _roomsRepository.GetRoomsAsync(name, size, available);

            return rooms.Select(x => new RoomDto
            {
                Id = x.Id,
                Name = x.Name,
                Size = x.Size,
                IsAvailable = x.IsAvailable,
                Status = x.Status,
                Details = x.Details,
            });
        }

        public async Task<Guid> CreateRoomAsync(CreateRoomRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Room name cannot be empty.");

            if (request.Size <= 0 || request.Size >= 6)
                throw new ArgumentException("Size must be greater than zero and smaller than six.");

            var room = new Room
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Size = request.Size,
                IsAvailable = true,
                Status = RoomStatus.Available,
                Details = request.Details
            };

            await _roomsRepository.AddRoomAsync(room);
            return room.Id;
        }
    }
}
