using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IHotelRoom
    {
        // Contains methods and properties that are required for the classes to implemented
        // Create a hotel room
        Task<HotelRoom> Create(HotelRoom hotelRoom);

        // Read
        
        // Get Individually (by Id)
        Task<HotelRoom> GetHotelRoom(int hotelId, int roomNumber);

        Task<List<HotelRoom>> GetAllHotelRooms(int hotelId);

        // Update
        Task<HotelRoom> Update(HotelRoom hotelRoom);

        // Delete
        Task Delete(int hotelId, int roomNumber);


    }
}
