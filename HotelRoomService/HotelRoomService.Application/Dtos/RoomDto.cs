using HotelRoomService.Domain.Entities;

namespace HotelRoomService.Application.Dtos
{
    public class RoomDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int Size { get; set; }

        public bool IsAvailable { get; set; }

        public RoomStatus Status { get; set; }

        public string? Details { get; set; }
    }
}
