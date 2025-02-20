# Hotel-room-service

Small hotel room microservice build with:

- .NET 8.0
- Docker
- Docker Compose

## How to run

1. In the main directory, create **.env** file and define environment variables:

- `POSTGRES_USER` - Database username
- `POSTGRES_PASSWORD` - Database user password
- `POSTGRES_DB` - Database names

2. Create file **appsettings.json** with the same settings as in **appsettings.defualt.json** in HotelRoomService.API folder.

3. In in appsetting.json, configure the database connection string:

- `"HotelRoomServiceCS"` - set the connection string with the same environment variables defined in the **.env** file in

4. In the main directory run the commands:

```bash
docker-compose build
docker-compose up
```

5. To stop the server press Ctrl + C, then run the command:

```bash
docker-compose down
```


## How to test
