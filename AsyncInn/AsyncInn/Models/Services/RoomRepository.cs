using AsyncInn.Data;
using AsyncInn.Models.DTOs;
using AsyncInn.Models.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class RoomRepository : IRoom
    {
        private AsyncInnDbContext _context;
        private IAmenity _amenities;

        public RoomRepository(AsyncInnDbContext context, IAmenity amenities)
        {
            _context = context;
            _amenities = amenities;
        }

        /// <summary>
        /// Create - create a room object
        /// </summary>
        /// <param name="room">the room we want to create</param>
        /// <returns>the task of having made the room</returns>
        public async Task<RoomDTO> Create(RoomDTO dto)
        {
            // convert a room to a room entity

            Enum.TryParse(dto.Layout, out Layout layout);

            Room room = new Room()
            {
                Name = dto.Name,
                Layout = layout
            };

            // When I have a room, I want to add them to the DB
            _context.Entry(room).State = EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            dto.Id = room.Id;
            return dto;
        }

        /// <summary>
        /// Delete - allows us to delete a room
        /// </summary>
        /// <param name="Id">the ID of the room we want to delete</param>
        /// <returns>the completed task</returns>
        public async Task Delete(int Id)
        {
            //var room = await GetRoom(Id);

            Room room = await _context.Rooms.FindAsync(Id);

            if (room == null)
            {
                return;
            }
            else
            {
                _context.Entry(room).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }

        }

        /// <summary>
        /// GetRoom - gets a room individually by ID
        /// </summary>
        /// <param name="id">the id of the room we want to get </param>
        /// <returns>the returned room</returns>
        public async Task<RoomDTO> GetRoom(int id)
        {
            // look in the db on the room table where the id is equal to the one brought in as an argument
            //Room room = await _context.Rooms.FindAsync(id);

            // include all the RoomAmenities that the room has
            //var roomAmenities = await _context.RoomAmenities.Where(x => x.RoomId == id)
            //                                                .Include(x => x.Amenity)
            //                                                .ToListAsync();

            // From class repo:

            var room = await _context.Rooms.Where(x => x.Id == id)
                                            .Include(x => x.RoomAmenities)
                                            .ThenInclude(x => x.Amenity)
                                            .FirstOrDefaultAsync();

            if (room == null)
            {
                return null;
            }
            else
            {
                RoomDTO dto = new RoomDTO
                {
                    Name = room.Name,
                    Layout = room.Layout.ToString(),
                    Id = room.Id
                };

                dto.Amenities = new List<AmenityDTO>();

                foreach (var item in room.RoomAmenities)
                {
                    dto.Amenities.Add(await _amenities.GetAmenity(item.AmenityId));
                }

                //room.RoomAmenities = roomAmenities;
                return dto;
            }

        }

        /// <summary>
        /// GetRooms - gets a list of the rooms
        /// </summary>
        /// <returns>a list of the rooms</returns>
        public async Task<List<RoomDTO>> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();

            List<RoomDTO> dtos = new List<RoomDTO>();

            foreach (var room in rooms)
            {
                dtos.Add(await GetRoom(room.Id));
            }

            return dtos;
        }

        /// <summary>
        /// Update - allows us to update a room
        /// </summary>
        /// <param name="room">the room object we want to update</param>
        /// <returns>the updated room</returns>
        public async Task Update(RoomDTO dto)
        {
            // convert the roomDTO to a room entity
            Enum.TryParse(dto.Layout, out Layout layout);

            Room room = new Room()
            {
                Layout = layout,
                Name = dto.Name,
                Id = dto.Id
            };

            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// AddAmenity - allows us to add an amenity to a room
        /// </summary>
        /// <param name="roomId">the unique identifier of the room we want to add an amenity to</param>
        /// <param name="amenityId">the unique identifier of the amenity we want to add</param>
        /// <returns>the returned room with the amenity added</returns>
        // Add a room and amenity together
        public async Task AddAmenity(int roomId, int amenityId)
        {
            RoomAmenities roomAmenities = new RoomAmenities()
            {
                RoomId = roomId,
                AmenityId = amenityId
            };

            _context.Entry(roomAmenities).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// RemoveAmenityFromRoom - removes a specified amenity from a specific room
        /// </summary>
        /// <param name="roomId">unique identifier of the room</param>
        /// <param name="amenityId">unique identifier of the amenity</param>
        /// <returns>task of completion</returns>
        public async Task RemoveAmenityFromRoom(int roomId, int amenityId)
        {
            // Look in the RoomAmenities table for the entry that matches the roomId and the amenityId
            var result = _context.RoomAmenities.FirstOrDefault(x => x.RoomId == roomId && x.AmenityId == amenityId);

            if (result != null)
            {
                _context.Entry(result).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }
    }
}
