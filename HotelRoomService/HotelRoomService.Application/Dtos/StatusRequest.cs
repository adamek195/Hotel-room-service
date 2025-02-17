using HotelRoomService.Domain.Entities;

namespace HotelRoomService.Application.Dtos
{
    public class StatusRequest
    {
        public RoomStatus? Status { get; set; }

        public string? Details { get; set; }
    }
}
