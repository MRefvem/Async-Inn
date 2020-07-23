using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IRoom
    {
        // Contains methods and properties that are required for the classes to implemented
        // Create a room
        Task<Room> Create(Room room);

        // Read
        // Get All
        Task<List<Room>> GetRooms();
        // Get Individually (by Id)
        Task<Room> GetRoom(int id);

        // Update
        Task<Room> Update(Room room);

        // Delete
        Task Delete(int Id);

        Task AddAmenity(int roomId, int amenityId);

        /// <summary>
        /// Removes a specified amenity from a specific room
        /// </summary>
        /// <param name="roomId">unique identifier of the room</param>
        /// <param name="amenityId">unique identifier of the amenity</param>
        /// <returns>task of completion</returns>
        Task RemoveAmenityFromRoom(int roomId, int amenityId);
    }
}
