using AsyncInn.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IRoom
    {

        // Create

        /// <summary>
        /// Create - create a room object
        /// </summary>
        /// <param name="room">the room we want to create</param>
        /// <returns>the task of having made the room</returns>
        Task<RoomDTO> Create(RoomDTO room);

        /// <summary>
        /// AddAmenity - allows us to add an amenity to a room
        /// </summary>
        /// <param name="roomId">unique id of the room where we want to add the amenity to</param>
        /// <param name="amenityId">the unique id of the amenity we want to add</param>
        /// <returns>the created amenity object</returns>
        Task AddAmenity(int roomId, int amenityId);


        // Read

        /// <summary>
        /// GetRooms - gets a list of the rooms
        /// </summary>
        /// <returns>a list of the rooms</returns>
        Task<List<RoomDTO>> GetRooms();

        /// <summary>
        /// GetRoom - gets a room individually by id
        /// </summary>
        /// <param name="id">the id of the room we want to get </param>
        /// <returns>the returned room</returns>
        Task<RoomDTO> GetRoom(int id);


        // Update

        /// <summary>
        /// Update - allows us to update a room
        /// </summary>
        /// <param name="room">the room object we want to update</param>
        /// <returns>the updated room</returns>
        Task Update(RoomDTO room);


        // Delete

        /// <summary>
        /// Delete - allows us to delete a room
        /// </summary>
        /// <param name="Id">the id of the room we want to delete</param>
        /// <returns>the completed task</returns>
        Task Delete(int Id);

        /// <summary>
        /// RemoveAmenityFromRoom - removes a specified amenity from a specific room
        /// </summary>
        /// <param name="roomId">unique identifier of the room</param>
        /// <param name="amenityId">unique identifier of the amenity</param>
        /// <returns>task of completion</returns>
        Task RemoveAmenityFromRoom(int roomId, int amenityId);
    }
}
