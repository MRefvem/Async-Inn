using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hotel Name: ")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hotel Address: ")]
        public string StreetAddress { get; set; }
        [Required]
        [Display(Name = "City: ")]
        public string City { get; set; }
        [Required]
        [Display(Name = "State: ")]
        public string State { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        //Navigation property
        public ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
