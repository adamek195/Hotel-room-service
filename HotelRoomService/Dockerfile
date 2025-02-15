FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 5000

COPY HotelRoomService.sln ./
COPY HotelRoomService.API/HotelRoomService.API.csproj HotelRoomService.API/
COPY HotelRoomService.Application/HotelRoomService.Application.csproj HotelRoomService.Application/
COPY HotelRoomService.Domain/HotelRoomService.Domain.csproj HotelRoomService.Domain/
COPY HotelRoomService.Infrastructure/HotelRoomService.Infrastructure.csproj HotelRoomService.Infrastructure/

RUN dotnet restore

COPY . ./
RUN dotnet publish HotelRoomService.API/HotelRoomService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "HotelRoomService.API.dll"]