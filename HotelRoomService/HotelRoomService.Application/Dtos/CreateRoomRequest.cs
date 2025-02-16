using System.ComponentModel.DataAnnotations;

namespace HotelRoomService.Application.Dtos
{
    public class CreateRoomRequest
    {
        [Required(ErrorMessage = "Room name is required.")]
        [MaxLength(50, ErrorMessage = "Room name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid value for size.")]
        public int Size { get; set; }

        [MaxLength(1000, ErrorMessage = "Details cannot exceed 1000 characters.")]
        public string? Details { get; set; }
    }
}
