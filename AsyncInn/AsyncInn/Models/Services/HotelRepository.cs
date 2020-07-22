using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Hotel> Create(Hotel hotel)
        {
            // When I have a hotel, I want to add them to the DB
            _context.Entry(hotel).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return hotel;
        }

        public async Task Delete(int Id)
        {
            Hotel hotel = await GetHotel(Id);

            _context.Entry(hotel).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();

        }

        public async Task<Hotel> GetHotel(int id)
        {
            // look in the db on the hotels table where the id is equal to the one brought in as an argument
            Hotel hotel = await _context.Hotels.FindAsync(id);
            return hotel;
        }

        public async Task<List<Hotel>> GetHotels()
        {
            var hotels = await _context.Hotels.ToListAsync();

            return hotels;
        }

        public async Task<Hotel> Update(Hotel hotel)
        {
            _context.Entry(hotel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return hotel;
        }
    }
}
