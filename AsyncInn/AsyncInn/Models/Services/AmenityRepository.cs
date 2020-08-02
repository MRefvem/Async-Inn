using AsyncInn.Data;
using AsyncInn.Models.DTOs;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class AmenityRepository : IAmenity
    {
        private AsyncInnDbContext _context;

        public AmenityRepository(AsyncInnDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create - allows us to create a new ammenity we can then assign to a room
        /// </summary>
        /// <param name="amenity">the amenity object we want to create</param>
        /// <returns>the new amenity object</returns>
        public async Task<AmenityDTO> Create(AmenityDTO amenity)
        {
            // convert the amenityDTO to an actual emenity

            Amenity entity = new Amenity()
            {
                Name = amenity.Name
            };

            // When I have an amenity, I want to add them to the DB
            _context.Entry(entity).State = EntityState.Added;
            // The amenity gets saved here and then associated with an id
            await _context.SaveChangesAsync();

            return amenity;
        }

        /// <summary>
        /// Delete - allows us to delete an amenity
        /// </summary>
        /// <param name="Id">the unique identifier of the amenity we want to delete</param>
        /// <returns>the task complete - the amenity was deleted</returns>
        public async Task Delete(int Id)
        {
            // THIS DOES NOT WORK
            //AmenityDTO amenityDTO = await GetAmenity(Id);

            //Amenity amenity = new Amenity()
            //{
            //    Id = amenityDTO.Id,
            //    Name = amenityDTO.Name,
            //};

            Amenity amenity = await _context.Amenities.FindAsync(Id);

            if (amenity == null)
            {
                return;
            }
            else
            {
                _context.Entry(amenity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// GetAmenities - allows us to get a list of all amenities
        /// </summary>
        /// <returns>a list of all amenities</returns>
        public async Task<List<AmenityDTO>> GetAmenities()
        {
            
            var list = await _context.Amenities.ToListAsync();

            var amenities = new List<AmenityDTO>();

            foreach (var item in list)
            {
                amenities.Add(await GetAmenity(item.Id));
            }

            return amenities;
        }

        /// <summary>
        /// GetAmenity - allows us to get a single amenity by ID
        /// </summary>
        /// <param name="id">the unique id of the amenity we want to get</param>
        /// <returns>the requested amenity object</returns>
        public async Task<AmenityDTO> GetAmenity(int id)
        {
            Amenity amenity = await _context.Amenities.FindAsync(id);

            var roomAmenities = await _context.RoomAmenities.Where(x => x.AmenityId == id)
                                                            .Include(x => x.Room)
                                                            .ToListAsync();

            if (amenity == null)
            {
                return null;
            }
            else
            {
                amenity.RoomAmenities = roomAmenities;

                AmenityDTO dto = new AmenityDTO()
                {
                    Id = amenity.Id,
                    Name = amenity.Name
                };

                return dto;
            }

        }

        /// <summary>
        /// Update - allows us the ability to update details on an amenity
        /// </summary>
        /// <param name="amenity">The amenity object we want to update</param>
        /// <returns>the updated amenity object</returns>
        public async Task<Amenity> Update(Amenity amenity)
        {
            if (amenity == null)
            {
                throw new Exception("amenity was null");
            }
            else
            {
                _context.Entry(amenity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return amenity;
            }
        }
    }
}
