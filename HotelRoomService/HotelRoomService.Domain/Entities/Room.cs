using System;

namespace HotelRoomService.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public bool IsAvailable { get; set; }

        public RoomStatus Status { get; set; }

        public string? Details { get; set; }
    }
}
