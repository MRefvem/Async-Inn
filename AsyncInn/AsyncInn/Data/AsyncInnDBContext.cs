using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Data
{
    public class AsyncInnDbContext : DbContext
    {
        public AsyncInnDbContext(DbContextOptions<AsyncInnDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Seed initial data into database
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Seattle Marriott Redmond",
                    StreetAddress = "7401 164th Ave NE",
                    City = "Redmond",
                    State = "Washington",
                    Phone = "+1 (425) 498-4000"
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Seattle Marriott Waterfront",
                    StreetAddress = "2100 Alaskan Way",
                    City = "Seattle",
                    State = "Washington",
                    Phone = "+1 (206) 443-5000"
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Seattle Marriott Bellevue",
                    StreetAddress = "200 110th Ave NE",
                    City = "Bellevue",
                    State = "Washington",
                    Phone = "+1 (425) 214-7600"
                }
                );
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Name = "Studio",
                    Layout = 0
                },
                new Room
                {
                    Id = 2,
                    Name = "One Bedroom",
                    Layout = 1
                },
                new Room
                {
                    Id = 3,
                    Name = "Two Bedroom",
                    Layout = 2
                }
                );
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity
                {
                    Id = 1,
                    Name = "Microwave"
                },
                new Amenity
                {
                    Id = 2,
                    Name = "Television"
                },
                new Amenity
                {
                    Id = 3,
                    Name = "Mini Safe"
                }
                );
        }

        // to create an initial migration
        // Install-Package Microsoft.EntityFrameworkCore.Tools
        // add-migration {migration name}
        // add-migration initial
        // "add-migration addCoursesSimpleModel"
        // then update my DB and see the new table added
        // "add-migration addCoursesSeededData"
        // run "update-database"

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
    }
}
