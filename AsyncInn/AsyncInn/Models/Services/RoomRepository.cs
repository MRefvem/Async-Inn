using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class RoomRepository : IRoom
    {
        private AsyncInnDbContext _context;

        public RoomRepository(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task<Room> Create(Room room)
        {
            // When I have a room, I want to add them to the DB
            _context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            // The hotel gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return room;
        }

        public async Task Delete(int Id)
        {
            Room room = await GetRoom(Id);

            _context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();

        }

        public async Task<Room> GetRoom(int id)
        {
            // look in the db on the room table where the id is equal to the one brought in as an argument
            Room room = await _context.Rooms.FindAsync(id);
            return room;
        }

        public async Task<List<Room>> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();

            return rooms;
        }

        public async Task<Room> Update(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return room;
        }
    }
}
