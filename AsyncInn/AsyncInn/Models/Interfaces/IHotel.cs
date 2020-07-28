using AsyncInn.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IHotel
    {

        // CREATE

        /// <summary>
        /// Create - allows us to create a new hotel object
        /// </summary>
        /// <param name="hotel">the hotel object we want to create</param>
        /// <returns>the new hotel object</returns>
        Task<HotelDTO> Create(HotelDTO hotel);

        /// <summary>
        /// AddRoom - allows us to add a room to a hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel we want to add a room to</param>
        /// <param name="roomId">the id number associated with our new room</param>
        /// <returns>the completed task - a new room was created</returns>
        Task AddRoom(int hotelId, int roomId);


        // READ

        /// <summary>
        /// GetHotels - allows us to get a list of all the hotels
        /// </summary>
        /// <returns>a list of all the hotels</returns>
        Task<List<HotelDTO>> GetHotels();

        /// <summary>
        /// GetHotel - allows us to get details on a single hotel
        /// </summary>
        /// <param name="id">the unique identifier of the hotel we want to select</param>
        /// <returns>details on that hotel</returns>
        Task<HotelDTO> GetHotel(int id);


        // UPDATE

        /// <summary>
        /// Update - allows us to update details on a hotel
        /// </summary>
        /// <param name="hotel">the hotel object we want to modify</param>
        /// <returns>the modified hotel obejct</returns>
        Task<Hotel> Update(Hotel hotel);


        // DELETE

        /// <summary>
        /// Delete - allows us to delete a hotel obejct
        /// </summary>
        /// <param name="Id">the unique identifier of the hotel we want to delete</param>
        /// <returns>the deleted hotel object</returns>
        Task Delete(int Id);

    }
}
