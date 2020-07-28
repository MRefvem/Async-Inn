using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class RoomAmenities
    {
        [Required]
        [Display(Name = "Room Type: ")]
        public int RoomId { get; set; }
        [Required]
        [Display(Name = "Amenity Name: ")]
        public int AmenityId { get; set; }

        // Navigation property
        public Room Room { get; set; }
        public Amenity Amenity { get; set; }
    }
}
