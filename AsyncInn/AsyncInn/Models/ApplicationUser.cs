using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class ApplicationRoles
    {
        public const string DistrictManager = "District Manager";
        public const string PropertyManager = "Property Manager";
        public const string CustomerAgent = "Customer Agent";
        public const string Customer = "Customer";
    }
}
