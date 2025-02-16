﻿using HotelRoomService.Application.Dtos;
using HotelRoomService.Application.Exceptions;
using HotelRoomService.Application.Interfaces;
using HotelRoomService.Domain.Entities;
using HotelRoomService.Domain.Interfaces;

namespace HotelRoomService.Application.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsService(IRoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsAsync(string? name, int? size, bool? available)
        {
            var rooms = await _roomsRepository.GetRoomsAsync(name, size, available);

            return rooms.Select(x => new RoomDto
            {
                Id = x.Id,
                Name = x.Name,
                Size = x.Size,
                IsAvailable = x.IsAvailable,
                Status = x.Status,
                Details = x.Details,
            });
        }

        public async Task<RoomDto> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomsRepository.GetRoomByIdAsync(id);
            if (room == null)
                throw new NotFoundException($"Cannot find room with id '{id}'.");

            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Size = room.Size,
                IsAvailable = room.IsAvailable,
                Status = room.Status,
                Details = room.Details
            };
        }

        public async Task<Guid> CreateRoomAsync(RoomRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException("Room name cannot be empty.");

            if (request.Name.Length > 50)
                throw new BadRequestException("Room name cannot exceed 50 characters.");

            if (!request.Size.HasValue)
                throw new BadRequestException("Room size has to have value.");

            if (request.Size <= 0 || request.Size >= 11)
                throw new BadRequestException("Size must be greater than zero and less than than eleven.");

            if (request.Details != null && request.Details.Length > 1000)
                throw new BadRequestException("Details cannot exceed 1000 characters.");

            var room = new Room
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Size = request.Size.Value,
                IsAvailable = true,
                Status = RoomStatus.Available,
                Details = request.Details
            };

            await _roomsRepository.AddRoomAsync(room);
            return room.Id;
        }

        public async Task<RoomDto> UpdateRoomAsync(Guid id, RoomRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var room = await _roomsRepository.GetRoomByIdAsync(id);
            if (room == null)
                throw new NotFoundException($"Cannot find room with id '{id}'.");

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                room.Name = request.Name;
            }

            if (request.Size.HasValue)
            {
                if (request.Size <= 0 || request.Size >= 11)
                    throw new BadRequestException("Size must be greater than zero and less than than eleven.");

                room.Size = request.Size.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.Details))
            {
                room.Details = request.Details;
            }

            await _roomsRepository.UpdateRoomAsync(room);

            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Size = room.Size,
                IsAvailable = room.IsAvailable,
                Status = room.Status,
                Details = room.Details
            };
        }
    }
}
