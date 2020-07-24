using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
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

        public RoomRepository(AsyncInnDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create - create a room object
        /// </summary>
        /// <param name="room">the room we want to create</param>
        /// <returns>the task of having made the room</returns>
        public async Task<Room> Create(Room room)
        {
            // When I have a room, I want to add them to the DB
            _context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return room;
        }

        /// <summary>
        /// GetRoom - allows us to delete a room
        /// </summary>
        /// <param name="Id">the ID of the room we want to delete</param>
        /// <returns>the completed task</returns>
        public async Task Delete(int Id)
        {
            Room room = await GetRoom(Id);

            _context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// GetRoom - gets a room individually by ID
        /// </summary>
        /// <param name="id">the id of the room we want to get </param>
        /// <returns>the returned room</returns>
        public async Task<Room> GetRoom(int id)
        {
            // look in the db on the room table where the id is equal to the one brought in as an argument
            Room room = await _context.Rooms.FindAsync(id);

            // include all the RoomAmenities that the room has
            var roomAmenities = await _context.RoomAmenities.Where(x => x.RoomId == id)
                                                            .Include(x => x.Amenity)
                                                            .ToListAsync();
            room.RoomAmenities = roomAmenities;
            return room;
        }

        /// <summary>
        /// GetRooms - gets a list of the rooms
        /// </summary>
        /// <returns>a list of the rooms</returns>
        public async Task<List<Room>> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();

            return rooms;
        }

        /// <summary>
        /// Update - allows us to update a room
        /// </summary>
        /// <param name="room">the room object we want to update</param>
        /// <returns>the updated room</returns>
        public async Task<Room> Update(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return room;
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
            _context.Entry(result).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
