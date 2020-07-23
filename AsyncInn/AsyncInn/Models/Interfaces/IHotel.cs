using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IHotel
    {
        // Contains methods and properties that are required for the classes to implemented
        // Create a hotel
        Task<Hotel> Create(Hotel hotel);

        // Read
        // Get All
        Task<List<Hotel>> GetHotels();
        // Get Individually (by Id)
        Task<Hotel> GetHotel(int id);

        // Update
        Task<Hotel> Update(Hotel hotel);

        // Delete
        Task Delete(int Id);

        Task AddRoom(int hotelId, int roomId);

    }
}
