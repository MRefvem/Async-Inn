﻿using AsyncInn.Data;
using AsyncInn.Models.DTOs;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public async Task<HotelRoomDTO> Create(HotelRoomDTO hotelRoomDTO, int hotelId)
        {

            // Convert the DTO to an actual amenity

            HotelRoom entity = new HotelRoom()
            {
                HotelId = hotelId,
                RoomNumber = hotelRoomDTO.RoomNumber,
                Rate = hotelRoomDTO.Rate,
                PetFriendly = hotelRoomDTO.PetFriendly,
                RoomId = hotelRoomDTO.RoomId,
            };

            hotelRoomDTO.HotelId = hotelId;
            // When I have a room, I want to add them to the DB
            // Can only pass in entities to the DB, NOT DTOs
            _context.Entry(entity).State = EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return hotelRoomDTO;
        }

        /// <summary>
        /// Delete - allows us to delete a room from a hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel</param>
        /// <param name="roomNumber">the unique identifier of the hotel room we're trying to delete</param>
        /// <returns>the completed task - hotel room no longer exists</returns>
        public async Task Delete(int hotelId, int roomNumber)
        {
            HotelRoomDTO dto = await GetHotelRoom(hotelId, roomNumber);

            HotelRoom hotelRoom = new HotelRoom()
            {
                HotelId = dto.HotelId,
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                Rate = dto.Rate,
                PetFriendly = dto.PetFriendly,
            };

            _context.Entry(hotelRoom).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// GetHotelRoom - allows us to see an individual room of an individual hotel
        /// </summary>
        /// <param name="hotelId">the unique identifier of the hotel we want to look at</param>
        /// <param name="roomNumber">the unique identifier of the individual room we want to see</param>
        /// <returns>the completed task showing the room</returns>
        public async Task<HotelRoomDTO> GetHotelRoom(int hotelId, int roomNumber)
        {
            // look in the db on the room table where the id is equal to the one brought in as an argument
            await _context.HotelRooms.FindAsync(hotelId, roomNumber);

            // My solution:
            // include all the RoomAmenities that the room has.
            //var roomAmenities = await _context.RoomAmenities.Where(x => x.RoomId == hotelRoom.RoomId)
            //                                                .Include(x => x.Amenity)
            //                                                .ToListAsync();
            //hotelRoom.Room.RoomAmenities = roomAmenities;

            // EDIT FROM CLASS REPO: "MAGIC LINQ STUFF"
            var hotelRoom = await _context.HotelRooms.Where(x => x.HotelId == hotelId && x.RoomNumber == roomNumber)
                                                .Include(x => x.Hotel)
                                                .Include(x => x.Room)
                                                .ThenInclude(x => x.RoomAmenities)
                                                .ThenInclude(x => x.Amenity)
                                                .FirstOrDefaultAsync();

            List<AmenityDTO> amenityDTOs = new List<AmenityDTO>();

            foreach (var roomAmenities in hotelRoom.Room.RoomAmenities)
            {
                AmenityDTO amenityDTO = new AmenityDTO()
                {
                    Id = roomAmenities.Amenity.Id,
                    Name = roomAmenities.Amenity.Name,
                };

                amenityDTOs.Add(amenityDTO);
            }

            RoomDTO roomDTO = new RoomDTO()
            {
                Id = hotelRoom.Room.Id,
                Name = hotelRoom.Room.Name,
                Layout = hotelRoom.Room.Layout.ToString(),
                Amenities = amenityDTOs,
            };

            HotelRoomDTO dto = new HotelRoomDTO
            {
                HotelId = hotelRoom.HotelId,
                RoomNumber = hotelRoom.RoomNumber,
                Rate = hotelRoom.Rate,
                PetFriendly = hotelRoom.PetFriendly,
                RoomId = hotelRoom.RoomId,
                Room = roomDTO,
            };

            return dto;
        }

        /// <summary>
        /// GetAllHotelRooms - allows us to see a lise of all of the rooms that a hotel has
        /// </summary>
        /// <param name="hotelId">the unique ID of the hotel we want to see rooms for</param>
        /// <returns>a list of all the rooms that hotel has</returns>
        public async Task<List<HotelRoomDTO>> GetAllHotelRooms(int hotelId)
        {
            var hotelRooms = await _context.HotelRooms.Where(x => x.HotelId == hotelId)
                                                        .Include(x => x.Room)
                                                        .ThenInclude(x => x.RoomAmenities)
                                                        .ThenInclude(x => x.Amenity)
                                                        .ToListAsync();

            List<HotelRoomDTO> dtos = new List<HotelRoomDTO>();

            foreach (var hotelRoom in hotelRooms)
            {
                dtos.Add(await GetHotelRoom(hotelRoom.HotelId, hotelRoom.RoomNumber));
            }

            return dtos;
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
