using AsyncInn.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IHotelRoom
    {

        // CREATE

        /// <summary>
        /// Create - allows us to create a new hotel room
        /// </summary>
        /// <param name="hotelRoom">the hotel room object we want created</param>
        /// <returns>the completed task of creating the hotel room</returns>
        Task<HotelRoomDTO> Create(HotelRoomDTO hotelRoom, int hotelId);


        // READ

        /// <summary>
        /// GetHotelRoom - allows us to see an individual room of an individual hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel we want to look at</param>
        /// <param name="roomNumber">the unique identifier of the individual room we want to see</param>
        /// <returns>the completed task showing the room</returns>
        Task<HotelRoomDTO> GetHotelRoom(int hotelId, int roomNumber);

        /// <summary>
        /// GetAllHotelRooms - allows us to see a lise of all of the rooms that a hotel has
        /// </summary>
        /// <param name="hotelId">the unique ID of the hotel we want to see rooms for</param>
        /// <returns>a list of all the rooms that hotel has</returns>
        Task<List<HotelRoomDTO>> GetAllHotelRooms(int hotelId);


        // Update

        /// <summary>
        /// Update - allows us the ability to update features of an individual hotel room
        /// </summary>
        /// <param name="hotelRoom">the hotel room object that we want to modify</param>
        /// <returns>the modified hotel room object</returns>
        Task<HotelRoom> Update(HotelRoom hotelRoom);


        // Delete

        /// <summary>
        /// Delete - allows us to delete a room from a hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel</param>
        /// <param name="roomNumber">the unique identifier of the hotel room we're trying to delete</param>
        /// <returns>the completed task - hotel room no longer exists</returns>
        Task Delete(int hotelId, int roomNumber);


    }
}
