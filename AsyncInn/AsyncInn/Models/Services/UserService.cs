using AsyncInn.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class UserService
    {
        private AsyncInnDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AsyncInnDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        //public async bool ValidateUser(List<Claim> claims)
        //{
        //    // get the name claim
        //    var nameClaim = claims.FirstOrDefault(x => x.Type == "FirstName").Value;

        //    if (nameClaim == "Michael")
        //    {
        //        // do something 
                
        //    }

        //    return true;
        //}
    }
}
