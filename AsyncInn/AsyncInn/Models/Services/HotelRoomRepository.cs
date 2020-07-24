using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class HotelRoomRepository : IHotelRoom
    {
        private AsyncInnDbContext _context;

        public HotelRoomRepository(AsyncInnDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create - allows us to create a new hotel room
        /// </summary>
        /// <param name="hotelRoom">the hotel room object we want created</param>
        /// <returns>the completed task of creating the hotel room</returns>
        public async Task<HotelRoom> Create(HotelRoom hotelRoom)
        {
            // When I have a room, I want to add them to the DB
            _context.Entry(hotelRoom).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return hotelRoom;
        }

        /// <summary>
        /// Delete - allows us to delete a room from a hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel</param>
        /// <param name="roomNumber">the unique identifier of the hotel room we're trying to delete</param>
        /// <returns>the completed task - hotel room no longer exists</returns>
        public async Task Delete(int hotelId, int roomNumber)
        {
            HotelRoom hotelRoom = await GetHotelRoom(hotelId, roomNumber);

            _context.Entry(hotelRoom).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// GetHotelRoom - allows us to see an individual room of an individual hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel we want to look at</param>
        /// <param name="roomNumber">the unique identifier of the individual room we want to see</param>
        /// <returns>the completed task showing the room</returns>
        public async Task<HotelRoom> GetHotelRoom(int hotelId, int roomNumber)
        {
            // look in the db on the room table where the id is equal to the one brought in as an argument
            HotelRoom hotelRoom = await _context.HotelRooms.FindAsync(hotelId, roomNumber);

            // include all the RoomAmenities that the room has
            var roomAmenities = await _context.RoomAmenities.Where(x => x.RoomId == hotelRoom.RoomId)
                                                            .Include(x => x.Amenity)
                                                            .ToListAsync();
            hotelRoom.Room.RoomAmenities = roomAmenities;
            return hotelRoom;
        }

        /// <summary>
        /// GetAllHotelRooms - allows us to see a lise of all of the rooms that a hotel has
        /// </summary>
        /// <param name="hotelId">the unique ID of the hotel we want to see rooms for</param>
        /// <returns>a list of all the rooms that hotel has</returns>
        public async Task<List<HotelRoom>> GetAllHotelRooms(int hotelId)
        {
            var hotelRooms = await _context.HotelRooms.Where(x => x.HotelId == hotelId).ToListAsync();

            return hotelRooms;
        }

        /// <summary>
        /// Update - allows us the ability to update features of an individual hotel room
        /// </summary>
        /// <param name="hotelRoom">the hotel room object that we want to modify</param>
        /// <returns>the modified hotel room object</returns>
        public async Task<HotelRoom> Update(HotelRoom hotelRoom)
        {
            _context.Entry(hotelRoom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return hotelRoom;
        }
    }
}
