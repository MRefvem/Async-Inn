using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInn.Models;
using AsyncInn.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsyncInn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Register - Method that allows a user to register an account in the DB
        /// </summary>
        /// <param name="register">RegisterDTO</param>
        /// <returns>The completed action - registered user</returns>
        // api/account/register
        [HttpPost,Route("register")]
        public async Task<ActionResult> Register(RegisterDTO register)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                UserName = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };

            // create the user
            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                // sign in if it was successful

                await _signInManager.SignInAsync(user, false);

                return Ok();

            }

            return BadRequest("Invalid Registration");

            // do something to put this into the database
        }

        /// <summary>
        /// Login - method allows the user to login to their account
        /// </summary>
        /// <param name="login">The credentials, email and password</param>
        /// <returns>The completed action - user login or unsuccessful attempt</returns>
        [HttpPost, Route("Login")]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (result.Succeeded)
            {
                // log the user in

                return Ok("logged in");
            }

            return BadRequest("Invalid Attempt");

        }
    }
}
