using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IAmenity
    {
        // Contains methods and properties that are required for the classes to implemented
        // Create an amenity
        Task<Amenity> Create(Amenity amenity);

        // Read
        // Get All
        Task<List<Amenity>> GetAmenities();
        // Get Individually (by Id)
        Task<Amenity> GetAmenity(int id);

        // Update
        Task<Amenity> Update(Amenity amenity);

        // Delete
        Task Delete(int Id);
    }
}
