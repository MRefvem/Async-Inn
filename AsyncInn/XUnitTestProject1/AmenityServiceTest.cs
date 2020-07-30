using AsyncInn.Models;
using AsyncInn.Models.DTOs;
using AsyncInn.Models.Interfaces;
using AsyncInn.Models.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class AmenityServiceTest : DatabaseTest
    {
        private IAmenity BuildRepository()
        {
            return new AmenityRepository(_db);
        }

        // needs a method that turns our dto into an actual amenity

        [Fact]
        public async Task CanSaveAndGetAmenity()
        {
            // arrange
            var amenity = new AmenityDTO
            {
                Name = "BreakfastMachine",
            };

            var repository = BuildRepository();

            // act

            var saved = await repository.Create(amenity);

            // assert
            Assert.NotNull(saved);
            Assert.NotEqual(0, amenity.Id);
            Assert.Equal(saved.Id, amenity.Id);
            Assert.Equal(saved.Name, amenity.Name);
        }

        // need tests for all four Create, Read, Update and Delete
        // create an amenity
        // get all amenities
        // update amenity
        // delete amenity
    }
}
