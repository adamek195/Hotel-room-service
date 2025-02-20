using HotelRoomService.Application.Dtos;
using System.Net.Http.Json;
using System.Net;
using HotelRoomService.Domain.Entities;
using Xunit;
using System.Text.Json.Serialization;
using System.Text.Json;


namespace HotelRoomService.IntegrationTests
{
    public class RoomsControllerTests : IClassFixture<PostgreSqlContainerFixture>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public RoomsControllerTests(PostgreSqlContainerFixture fixture)
        {
            var factory = new HotelRoomServiceWebApplicationFactory(fixture.ConnectionString);
            _client = factory.CreateClient();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        [Fact]
        public async Task CreateRoomAsync_RequestIsValid_CreateRoomAndReturnId()
        {
            var request = new RoomRequest { Name = "Room 102", Size = 2, Details = null };

            var response = await _client.PostAsJsonAsync("/api/Rooms", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var roomId = await response.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
            Assert.NotEqual(Guid.Empty, roomId);
        }

        [Fact]
        public async Task GetRooms_RoomsExist_ReturnsMappedRooms()
        {
            var request = new RoomRequest { Name = "Room 103", Size = 3, Details = null };
            await _client.PostAsJsonAsync("/api/Rooms", request);


            var response = await _client.GetAsync("/api/Rooms");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rooms = await response.Content.ReadFromJsonAsync<List<RoomDto>>(_jsonOptions);
            Assert.NotNull(rooms);
            Assert.NotEmpty(rooms);
        }

        [Fact]
        public async Task GetRoomById_RoomExists_ReturnsMappedRoom()
        {
            var request = new RoomRequest { Name = "Room 104", Size = 4, Details = null };
            var createResponse = await _client.PostAsJsonAsync("/api/Rooms", request);
            var roomId = await createResponse.Content.ReadFromJsonAsync<Guid>();

            var response = await _client.GetAsync($"/api/Rooms/{roomId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var room = await response.Content.ReadFromJsonAsync<RoomDto>(_jsonOptions);
            Assert.NotNull(room);
            Assert.Equal(room.Id, roomId);
        }

        [Fact]
        public async Task UpdateRoom_RequestIsValid_ReturnsMappedRoom()
        {
            var request = new RoomRequest { Name = "Room 204", Size = 4, Details = null };
            var createResponse = await _client.PostAsJsonAsync("/api/Rooms", request);
            var roomId = await createResponse.Content.ReadFromJsonAsync<Guid>();

            var updateRequest = new RoomRequest { Name = "Room 105", Size = 5, Details = "Test" };

            var updateResponse = await _client.PatchAsJsonAsync($"/api/Rooms/{roomId}", updateRequest);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            var updatedRoom = await updateResponse.Content.ReadFromJsonAsync<RoomDto>(_jsonOptions);
            Assert.NotNull(updatedRoom);
            Assert.Equal("Room 105", updatedRoom.Name);
            Assert.Equal(5, updatedRoom.Size);
        }

        [Fact]
        public async Task ChangeStatus_RequestIsValid_ReturnsMappedRoom()
        {
            var createRequest = new RoomRequest { Name = "202", Size = 2, Details = null };
            var createResponse = await _client.PostAsJsonAsync("/api/Rooms", createRequest);
            var roomId = await createResponse.Content.ReadFromJsonAsync<Guid>();

            var statusRequest = new StatusRequest { Status = RoomStatus.Maintenance, Details = "Under repair" };

            var statusResponse = await _client.PatchAsJsonAsync($"/api/Rooms/{roomId}/Status", statusRequest);

            Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);
            var updatedRoom = await statusResponse.Content.ReadFromJsonAsync<RoomDto>(_jsonOptions);
            Assert.NotNull(updatedRoom);
            Assert.Equal(RoomStatus.Maintenance, updatedRoom.Status);
            Assert.False(updatedRoom.IsAvailable);
        }
    }
}