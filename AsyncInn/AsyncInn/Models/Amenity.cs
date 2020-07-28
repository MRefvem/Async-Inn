using AsyncInn.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class Amenity
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Amenity Name: ")]
        public string Name { get; set; }

        // Nav properties
        public ICollection<RoomAmenities> RoomAmenities { get; set; }

    }
}
