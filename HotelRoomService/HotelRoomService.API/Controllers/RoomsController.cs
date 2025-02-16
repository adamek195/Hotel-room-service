using HotelRoomService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelRoomService.Application.Dtos;
using System;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom([FromRoute] Guid id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomRequest request)
        {
            var roomId = await _roomService.CreateRoomAsync(request);
            return Created($"api/Rooms/{roomId}", roomId);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] Guid id, [FromBody] RoomRequest request)
        {
            var room = await _roomService.UpdateRoomAsync(id, request);
            return Ok(room);
        }
    }
}
