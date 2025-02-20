using Testcontainers.PostgreSql;

namespace HotelRoomService.IntegrationTests
{
    public class PostgreSqlContainerFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer;

        public string ConnectionString => _postgreSqlContainer.GetConnectionString();

        public PostgreSqlContainerFixture()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("hotelroomservicetest")
                .WithUsername("testuser")
                .WithPassword("testpassword")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
        }
    }
}
