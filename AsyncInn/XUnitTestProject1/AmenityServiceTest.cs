using AsyncInn.Models;
using AsyncInn.Models.DTOs;
using AsyncInn.Models.Interfaces;
using AsyncInn.Models.Services;
using System;
using System.Collections.Generic;
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
        public async Task CanCheckIfThereAreNoAmenities()
        {
            // arrange
            var service = BuildRepository();

            await service.Delete(1);
            await service.Delete(2);
            await service.Delete(3);

            // act
            List<AmenityDTO> result = await service.GetAmenities();

            // assert
            Assert.Empty(result);

        }

        [Fact]
        public async Task CanSaveAndGetAmenity()
        {
            // arrange
            var amenity = new AmenityDTO
            {
                Id = 7,
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

        [Fact]
        public async Task GetSingleAmenity()
        {
            // arrange
            var amenity = new AmenityDTO
            {
                Name = "Breakfast Machine",
            };

            var amenity2 = new AmenityDTO
            {
                Name = "Lunch Machine",
            };

            var amenity3 = new AmenityDTO
            {
                Name = "Dinner Machine",
            };

            var amenity4 = new AmenityDTO
            {
                Name = "Dessert Machine",
            };

            var service = BuildRepository();

            var saved = await service.Create(amenity);
            var saved2 = await service.Create(amenity2);
            var saved3 = await service.Create(amenity3);
            var saved4 = await service.Create(amenity4);

            // act
            var result = await service.GetAmenity(1);
            var result2 = await service.GetAmenity(2);
            var result3 = await service.GetAmenity(3);
            var result4 = await service.GetAmenity(4);
            var result5 = await service.GetAmenity(5);
            var result6 = await service.GetAmenity(6);
            var result7 = await service.GetAmenity(7);

            // assert
            Assert.Equal("Microwave", result.Name);
            Assert.Equal("Television", result2.Name);
            Assert.Equal("Mini Safe", result3.Name);
            Assert.Equal("Breakfast Machine", result4.Name);
            Assert.Equal("Lunch Machine", result5.Name);
            Assert.Equal("Dinner Machine", result6.Name);
            Assert.Equal("Dessert Machine", result7.Name);

        }

        // get all amenities
        [Fact]
        public async Task GetAllAmenities()
        {
            // arrange
            var amenity = new AmenityDTO
            {
                Id = 7,
                Name = "BreakfastMachine",
            };

            var amenity2 = new AmenityDTO
            {
                Id = 8,
                Name = "LunchMachine",
            };

            var amenity3 = new AmenityDTO
            {
                Id = 9,
                Name = "DinnerMachine",
            };

            var amenity4 = new AmenityDTO
            {
                Id = 10,
                Name = "DessertMachine",
            };

            var service = BuildRepository();

            var saved = await service.Create(amenity);
            var saved2 = await service.Create(amenity2);
            var saved3 = await service.Create(amenity3);
            var saved4 = await service.Create(amenity4);

            // act
            List<AmenityDTO> result = await service.GetAmenities();

            // assert
            Assert.Equal(7, result.Count);

        }

       //update amenity
       [Fact]
        public async Task UpdateAmenity()
        {
            // arrange
           var amenity = new AmenityDTO
           {
               Name = "BreakfastMachine",
           };

            var amenity2 = new AmenityDTO
            {
                Name = "LunchMachine",
            };

            var amenity3 = new AmenityDTO
            {
                Name = "DinnerMachine",
            };

            var amenity4 = new AmenityDTO
            {
                Name = "DessertMachine",
            };

            var updateAmenityInDB = new Amenity
            {
                Id = 1,
                Name = "Mini Fridge",
            };

            var service = BuildRepository();

            var saved = await service.Create(amenity);
            var saved2 = await service.Create(amenity2);
            var saved3 = await service.Create(amenity3);
            var saved4 = await service.Create(amenity4);

            // act
            Amenity result = await service.Update(updateAmenityInDB);


            // assert
            Assert.Equal("Mini Fridge", result.Name);
            Assert.NotEqual("Toaster", result.Name);

        }

        // delete amenity
        [Fact]
        public async Task DeleteAmenity()
        {
            // arrange
            var amenity = new AmenityDTO
            {
                Name = "BreakfastMachine",
            };

            var amenity2 = new AmenityDTO
            {
                Name = "LunchMachine",
            };

            var amenity3 = new AmenityDTO
            {
                Name = "DinnerMachine",
            };

            var amenity4 = new AmenityDTO
            {
                Name = "DessertMachine",
            };

            var service = BuildRepository();

            var saved = await service.Create(amenity);
            var saved2 = await service.Create(amenity2);
            var saved3 = await service.Create(amenity3);
            var saved4 = await service.Create(amenity4);

            // act & assert
            List<AmenityDTO> result = await service.GetAmenities();
            Assert.Equal(7, result.Count);
            await service.Delete(1);
            List<AmenityDTO> result2 = await service.GetAmenities();
            Assert.Equal(6, result2.Count);

        }

        [Fact]
        public async Task CannotDeleteFromAnEmptyTable()
        {
            // arrange
            var service = BuildRepository();

            await service.Delete(1);
            await service.Delete(2);
            await service.Delete(3);
            // list is now empty

            // act
            List<AmenityDTO> result = await service.GetAmenities();

            // assert
            await service.Delete(4);
            Assert.Empty(result);


        }
    }
}
