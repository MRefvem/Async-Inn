using AsyncInn.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IAmenity
    {

        // CREATE

        /// <summary>
        /// Create - allows us to create a new ammenity we can then assign to a room
        /// </summary>
        /// <param name="amenity">the amenity object we want to create</param>
        /// <returns>the new amenity object</returns>
        Task<AmenityDTO> Create(AmenityDTO amenity);


        // READ

        /// <summary>
        /// GetAmenities - allows us to get a list of all amenities
        /// </summary>
        /// <returns>a list of all amenities</returns>
        Task<List<AmenityDTO>> GetAmenities();

        /// <summary>
        /// GetAmenity - allows us to get a single amenity by ID
        /// </summary>
        /// <param name="id">the unique id of the amenity we want to get</param>
        /// <returns>the requested amenity object</returns>
        Task<AmenityDTO> GetAmenity(int id);


        // UPDATE

        /// <summary>
        /// Update - allows us the ability to update details on an amenity
        /// </summary>
        /// <param name="amenity">the amenity object we want to update</param>
        /// <returns>the updated amenity object</returns>
        Task<Amenity> Update(Amenity amenity);


        // DELETE

        /// <summary>
        /// Delete - allows us to delete an amenity
        /// </summary>
        /// <param name="Id">the unique identifier of the amenity we want to delete</param>
        /// <returns>the task complete - the amenity was deleted</returns>
        Task Delete(int Id);
    }
}
