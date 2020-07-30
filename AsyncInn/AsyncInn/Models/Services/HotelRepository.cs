using AsyncInn.Data;
using AsyncInn.Models.DTOs;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class HotelRepository : IHotel
    {
        private AsyncInnDbContext _context;

        public HotelRepository(AsyncInnDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create - allows us to create a new hotel object
        /// </summary>
        /// <param name="hotel">the hotel object we want to create</param>
        /// <returns>the new hotel object</returns>
        public async Task<HotelDTO> Create(HotelDTO hotel)
        {
            // Convert the DTO to an actual entity

            Hotel entity = new Hotel()
            {
                Id = hotel.Id,
                Name = hotel.Name,
                StreetAddress = hotel.StreetAddress,
                City = hotel.City,
                State = hotel.State,
                Phone = hotel.Phone
            };

            // When I have a hotel, I want to add them to the DB
            _context.Entry(hotel).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return hotel;
        }

        /// <summary>
        /// Delete - allows us to delete a hotel obejct
        /// </summary>
        /// <param name="Id">the unique identifier of the hotel we want to delete</param>
        /// <returns>the deleted hotel object</returns>
        public async Task Delete(int Id)
        {
            HotelDTO hotel = await GetHotel(Id);

            _context.Entry(hotel).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// GetHotel - allows us to get details on a single hotel
        /// </summary>
        /// <param name="id">the unique identifier of the hotel we want to select</param>
        /// <returns>details on that hotel</returns>
        public async Task<HotelDTO> GetHotel(int id)
        {
            // look in the db on the hotels table where the id is equal to the one brought in as an argument
            var hotel = await _context.Hotels.Include(x => x.HotelRooms)
                                            .FirstOrDefaultAsync(x => x.Id == id);

            HotelDTO dto = new HotelDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                StreetAddress = hotel.StreetAddress,
                City = hotel.City,
                State = hotel.State,
                Phone = hotel.Phone,
                
            };

            List<HotelRoomDTO> hotelRoomList = new List<HotelRoomDTO>();

            foreach (var hotelRoom in hotel.HotelRooms)
            {
                HotelRoomDTO newDTO = new HotelRoomDTO()
                {
                    HotelId = hotelRoom.HotelId,
                    RoomNumber = hotelRoom.RoomNumber,
                    Rate = hotelRoom.Rate,
                    PetFriendly = hotelRoom.PetFriendly,
                    RoomId = hotelRoom.RoomId,
                };

                hotelRoomList.Add(newDTO);

            };

            dto.Rooms = hotelRoomList;


            return dto;
        }

        /// <summary>
        /// GetHotels - allows us to get a list of all the hotels
        /// </summary>
        /// <returns>a list of all the hotels</returns>
        public async Task<List<HotelDTO>> GetHotels()
        {
            var hotels = await _context.Hotels.Include(x => x.HotelRooms).ToListAsync();

            List<HotelDTO> dtos = new List<HotelDTO>();

            foreach (var hotel in hotels)
            {
                dtos.Add(await GetHotel(hotel.Id));
            }

            return dtos;
        }

        /// <summary>
        /// Update - allows us to update details on a hotel
        /// </summary>
        /// <param name="hotel">the hotel object we want to modify</param>
        /// <returns>the modified hotel obejct</returns>
        public async Task<Hotel> Update(Hotel hotel)
        {
            _context.Entry(hotel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return hotel;
        }

        /// <summary>
        /// AddRoom - allows us to add a room to a hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel we want to add a room to</param>
        /// <param name="roomId">the id number associated with our new room</param>
        /// <returns>the completed task - a new room was created</returns>
        public async Task AddRoom(int hotelId, int roomId)
        {
            HotelRoom hotelRoom = new HotelRoom()
            {
                HotelId = hotelId,
                RoomId = roomId
            };

            _context.Entry(hotelRoom).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }
    }
}
