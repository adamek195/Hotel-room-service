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
        public async Task GetRoomByIdAsync_RoomDoesNotExist_ThrowsNotFoundException()
        {
            var roomId = Guid.NewGuid();
            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(null));

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomsService.GetRoomByIdAsync(roomId));
        }

        [Fact]
        public async Task CreateRoomAsync_RequestIsNull_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roomsService.CreateRoomAsync(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CreateRoomAsync_NameIsEmptyOrWhitespace_ThrowsBadRequestException(string name)
        {
            var request = new RoomRequest { Name = name, Size = 2, Details = null };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.CreateRoomAsync(request));
        }

        [Fact]
        public async Task CreateRoomAsync_NameIsInvalid_ThrowsBadRequestException()
        {
            var request = new RoomRequest { Name = new string('X', 51), Size = 2, Details = null };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.CreateRoomAsync(request));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(-1)]
        public async Task CreateRoomAsync_SizeIsInvalid_ThrowsBadRequestException(int? invalidSize)
        {
            var request = new RoomRequest { Name = "Room 101", Size = invalidSize, Details = null };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.CreateRoomAsync(request));
        }

        [Fact]
        public async Task CreateRoomAsync_DetailsAreInvalid_ThrowsBadRequestException()
        {
            var request = new RoomRequest { Name = "Room 101", Size = 2, Details = new string('X', 1001) };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.CreateRoomAsync(request));
        }

        [Fact]
        public async Task CreateRoomAsync_RequestIsValid_CreateRoomAndReturnId()
        {
            var request = new RoomRequest
            {
                Name = "Room 101",
                Size = 2,
                Details = null
            };

            _roomsRepository.AddRoomAsync(Arg.Any<Room>()).Returns(Task.CompletedTask);

            var result = await _roomsService.CreateRoomAsync(request);

            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task UpdateRoomAsync_RequestIsNull_ThrowsArgumentNullException()
        {
            var roomId = Guid.NewGuid();
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roomsService.UpdateRoomAsync(roomId, null));
        }

        [Fact]
        public async Task UpdateRoomAsync_RoomDoesNotExist_ThrowsNotFoundException()
        {
            var roomId = Guid.NewGuid();
            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(null));

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomsService.UpdateRoomAsync(roomId, new RoomRequest()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(-1)]
        public async Task UpdateRoomAsync_SizeIsInvalid_ThrowsBadRequestException(int size)
        {
            var roomId = Guid.NewGuid();
            var existingRoom = new Room { Id = roomId, Name = "Room 101", Size = 5, Details = null };

            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(existingRoom));

            var request = new RoomRequest { Size = size };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.UpdateRoomAsync(roomId, request));
        }

        [Fact]
        public async Task UpdateRoomAsync_ValidRequest_ReturnsMappedRoom()
        {
            var roomId = Guid.NewGuid();
            var existingRoom = new Room { Id = roomId, Name = "Room 101", Size = 2, Details = null };

            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(existingRoom));

            var request = new RoomRequest { Name = "Room 102", Size = 3, Details = "Room is available." };

            var result = await _roomsService.UpdateRoomAsync(roomId, request);

            Assert.Equal("Room 102", result.Name);
            Assert.Equal(3, result.Size);
            Assert.Equal("Room is available.", result.Details);
        }

        [Fact]
        public async Task ChangeStatusAsync_RequestIsNull_ThrowsArgumentNullException()
        {
            var roomId = Guid.NewGuid();
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roomsService.ChangeStatusAsync(roomId, null));
        }

        [Fact]
        public async Task ChangeStatusAsync_StatusIsNull_ThrowsBadRequestException()
        {
            var roomId = Guid.NewGuid();
            var request = new StatusRequest { Status = null, Details = null };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.ChangeStatusAsync(roomId, request));
        }

        [Theory]
        [InlineData(RoomStatus.ManuallyLocked)]
        [InlineData(RoomStatus.Maintenance)]
        public async Task ChangeStatusAsync_DetailsAreRequired_ThrowsBadRequestException(RoomStatus status)
        {
            var roomId = Guid.NewGuid();
            var request = new StatusRequest { Status = status, Details = null };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _roomsService.ChangeStatusAsync(roomId, request));
        }

        [Fact]
        public async Task ChangeStatusAsync_RoomDoesNotExist_ThrowsNotFoundException()
        {
            var roomId = Guid.NewGuid();
            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(null));

            var request = new StatusRequest { Status = RoomStatus.Available, Details = null };

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomsService.ChangeStatusAsync(roomId, request));
        }

        [Fact]
        public async Task ChangeStatusAsync_StatusIsAvailable_ReturnsIsAvailableRoom()
        {
            var roomId = Guid.NewGuid();
            var existingRoom = new Room { Id = roomId, Name = "Room 101", Size = 2, Details = null, Status = RoomStatus.Maintenance, IsAvailable = false };

            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(existingRoom));

            var request = new StatusRequest { Status = RoomStatus.Available, Details = "Room is available." };

            var result = await _roomsService.ChangeStatusAsync(roomId, request);

            Assert.Equal(RoomStatus.Available, result.Status);
            Assert.True(result.IsAvailable);
        }

        [Theory]
        [InlineData(RoomStatus.ManuallyLocked)]
        [InlineData(RoomStatus.Maintenance)]
        [InlineData(RoomStatus.Cleaning)]
        [InlineData(RoomStatus.Booked)]
        public async Task ChangeStatusAsync_StatusIsNotAvailable_ReturnsIsNotAvailableRoome(RoomStatus status)
        {
            var roomId = Guid.NewGuid();
            var existingRoom = new Room { Id = roomId, Name = "Room 101", Size = 2, Details = null, Status = RoomStatus.Available, IsAvailable = true };

            _roomsRepository.GetRoomByIdAsync(roomId).Returns(Task.FromResult<Room?>(existingRoom));

            var request = new StatusRequest { Status = status, Details = "Example details" };

            var result = await _roomsService.ChangeStatusAsync(roomId, request);

            Assert.Equal(status, result.Status);
            Assert.False(result.IsAvailable);
        }
    }
}