using HotelRoomService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelRoomService.Application.Dtos;

namespace HotelRoomService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomService;

        public RoomsController(IRoomsService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms([FromQuery] string? name, [FromQuery] int? size, [FromQuery] bool? available)
        {
            var rooms = await _roomService.GetRoomsAsync(name, size, available);
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
        {
            var roomId = await _roomService.CreateRoomAsync(request);
            return Created($"api/Rooms/{roomId}", roomId);
        }
    }
}
