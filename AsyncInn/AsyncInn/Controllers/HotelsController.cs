using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;
using AsyncInn.Models.Interfaces;
using AsyncInn.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace AsyncInn.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRoom _hotelRooms;
        private readonly IHotel _hotel;

        public HotelsController(IHotel hotel, IHotelRoom hotelRoom)
        {
            _hotelRooms = hotelRoom;
            _hotel = hotel;
        }

        // GET: api/Hotels
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HotelDTO>>> GetHotels()
        {
            return await _hotel.GetHotels();
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HotelDTO>> GetHotel(int id)
        {
            HotelDTO hotel = await _hotel.GetHotel(id);

            return hotel;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Policy = "MaxPrivileges")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            var updatedHotel = await _hotel.Update(hotel);

            return Ok(updatedHotel);
        }

        // POST: api/Hotels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Policy = "MaxPrivileges")]
        public async Task<ActionResult<HotelDTO>> PostHotel(HotelDTO hotel)
        {
            await _hotel.Create(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        [HttpPost]
        [Route("/api/Hotels/{hotelId}/Rooms")]
        // POST: {hotelId}/{roomId}/Rooms
        // Model Binding
        [Authorize(Policy = "ElevatedPrivileges")]
        public async Task<ActionResult<HotelRoomDTO>> AddRoomToHotel(int hotelId, HotelRoomDTO hotelRoomDTO)
        {
            if (hotelId != hotelRoomDTO.HotelId)
            {
                return BadRequest();
            }
            await _hotelRooms.Create(hotelRoomDTO, hotelId);
            return Ok();
        }

        // Get all room details for a specific room
        // For roomNumber, try 101
        [HttpGet]
        [Route("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
        [Authorize(Policy = "NormalPrivileges")]
        public async Task<ActionResult<HotelRoomDTO>> GetRoomDetails(int hotelId, int roomNumber)
        {
            var hotelRoom = await _hotelRooms.GetHotelRoom(hotelId, roomNumber);

            if (hotelRoom == null)
            {
                return NotFound();
            }

            return hotelRoom;
        }

        // Get all details for every room in a hotel
        [HttpGet]
        [Route("/api/Hotels/{hotelId}/Rooms")]
        [Authorize(Policy = "NormalPrivileges")]
        public async Task<ActionResult<List<HotelRoomDTO>>> GetAllRoomDetails(int hotelId)
        {
            List<HotelRoomDTO> hotelRooms = await _hotelRooms.GetAllHotelRooms(hotelId);

            return hotelRooms;
        }

        // Put update the details of a specific room
        [HttpPut]
        [Route("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
        [Authorize(Policy = "NormalPrivileges")]
        public async Task<ActionResult<HotelRoom>> UpdateRoomDetails(int hotelId, int roomNumber, HotelRoom hotelRoom)
        {
            // check
            if (hotelId != hotelRoom.HotelId || roomNumber != hotelRoom.RoomNumber)
            {
                return BadRequest();
            }

            var result = await _hotelRooms.Update(hotelRoom);

            return result;
        }


        [HttpDelete]
        [Route("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
        [Authorize(Policy = "MaxPrivileges")]
        public async Task<ActionResult<HotelRoom>> DeleteHotelRoom(int hotelId, int roomNumber)
        {
            await _hotelRooms.Delete(hotelId, roomNumber);
            return Ok();
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "MaxPrivileges")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            await _hotel.Delete(id);
            return NoContent();
        }  
    }
}
