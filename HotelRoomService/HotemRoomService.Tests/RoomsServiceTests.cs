using HotelRoomService.Domain.Interfaces;
using HotelRoomService.Application.Services;
using NSubstitute;
using System.Threading.Tasks;
using HotelRoomService.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using HotelRoomService.Application.Dtos;
using HotelRoomService.Application.Exceptions;

namespace HotemRoomService.Tests
{
    public class RoomsServiceTests
    {
        private readonly IRoomsRepository _roomsRepository;
        private readonly RoomsService _roomsService;

        public RoomsServiceTests()
        {
            _roomsRepository = Substitute.For<IRoomsRepository>();
            _roomsService = new RoomsService(_roomsRepository);
        }

        [Fact]
        public async Task GetRoomsAsync_RoomsExist_ReturnsMappedRooms()
        {
            var rooms = new List<Room>
            {
                new Room { Id = Guid.NewGuid(), Name = "Room 101", Size = 3, IsAvailable = true, Details = null, Status = RoomStatus.Available  },
                new Room { Id = Guid.NewGuid(), Name = "Room 102", Size = 4, IsAvailable = false, Details = "Table is broken.", Status = RoomStatus.Maintenance}
            };

            _roomsRepository.GetRoomsAsync(Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<bool?>())
                       .Returns(Task.FromResult<IEnumerable<Room>>(rooms));

            var result = await _roomsService.GetRoomsAsync(null, null, null);

            Assert.Equal(rooms.Count, result.Count());
            Assert.Equal(rooms[0].Name, result.ElementAt(0).Name);
            Assert.Equal(rooms[1].Status, result.ElementAt(1).Status);
        }

        [Fact]
        public async Task GetRoomByIdAsync_RoomExists_ReturnsMappedRooms()
        {
            var roomId = Guid.NewGuid();
            var room = new Room { Id = roomId, Name = "Room 101", Size = 3, IsAvailable = true, Details = null, Status = RoomStatus.Available };
            var roomDto = new RoomDto { Id = roomId, Name = "Room 101", Size = 3, IsAvailable = true, Details = null, Status = RoomStatus.Available };

            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(room));

            var result = await _roomsService.GetRoomByIdAsync(roomId);

            Assert.NotNull(result);
            Assert.Equal(roomDto.Id, result.Id);
            Assert.Equal(roomDto.Name, result.Name);
            Assert.Equal(roomDto.Size, result.Size);
            Assert.Equal(roomDto.IsAvailable, result.IsAvailable);
            Assert.Equal(roomDto.Details, result.Details);
            Assert.Equal(roomDto.Status, result.Status);
        }

        [Fact]
        public async Task GetRoomByIdAsync_RoomDoesNotExist_ThrowNotFoundException()
        {
            var roomId = Guid.NewGuid();
            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(null));

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomsService.GetRoomByIdAsync(roomId));
            Assert.Equal($"Cannot find room with id '{roomId}'.", exception.Message);
        }


    }
}