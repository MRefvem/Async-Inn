using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AsyncInn.Models;
using AsyncInn.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AsyncInn.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _config;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }

        /// <summary>
        /// Register - Method that allows a user to register an account in the DB
        /// </summary>
        /// <param name="register">RegisterDTO</param>
        /// <returns>The completed action - registered user</returns>
        // api/account/register
        [HttpPost,Route("register")]
        [Authorize(Policy = "ElevatedPrivileges")]
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
                if (user.Email == _config["PrincipalSeed"])
                {
                    register.Role = ApplicationRoles.DistrictManager;
                    await _userManager.AddToRoleAsync(user, ApplicationRoles.DistrictManager);
                }
                else
                {
                    if (User.IsInRole("Property Manager"))
                    {
                        if (register.Role == ApplicationRoles.CustomerAgent)
                        {
                            await _userManager.AddToRoleAsync(user, register.Role);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else if (User.IsInRole("District Manager"))
                    {
                        await _userManager.AddToRoleAsync(user, register.Role);
                    }
                }
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
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (result.Succeeded)
            {
                // look the user up
                var user = await _userManager.FindByEmailAsync(login.Email);

                // user is our identity "Principal"

                //var principalClaims = User.Claims;

                var identityRole = await _userManager.GetRolesAsync(user);

                var token = CreateToken(user, identityRole.ToList());

                // make them a JWT token based on their account

                // send that JWT token back to the user

                // log the user in

                return Ok(new
                {
                    jtw = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return BadRequest("Invalid Attempt");

        }

        /// <summary>
        /// AssignRoleToUser - Allows us to assign a role to a user
        /// </summary>
        /// <param name="assignment">The assignment parameter of the user</param>
        /// <returns>The completed task - the user has a role</returns>
        [HttpPost, Route("assign/role")]
        [AllowAnonymous]
        //[Authorize(Policy="ElevatedPrivileges")]
        public async Task AssignRoleToUser(AssignRoleDTO assignment)
        {
            var user = await _userManager.FindByEmailAsync(assignment.Email);
            // validation here to confirm the role is valid

            //string role = "";

            //if(assignment.Role.ToUpper == "ADVISOR")
            //{
            //    role = ApplicationRoles.Advisor;
            //}

            // district managers can add hotels
            // property managers can create rooms
            // customer agents can add amenities
            // customers can only view
            string assignedRole = GetRole(assignment);

            await _userManager.AddToRoleAsync(user, assignedRole);
        }

        /// <summary>
        /// GetRole - Method. Paul Rest helped me find this solution.
        /// </summary>
        /// <param name="registerDTO">Passing in an object of RegisterDTO</param>
        /// <returns>A string with the assigned role</returns>
        private string GetRole(AssignRoleDTO assignRoleDTO)
        {
            string role = "";
            switch (assignRoleDTO.Role.ToLower())
            {
                case "district manager":
                    role = ApplicationRoles.DistrictManager;
                    break;
                case "property manager":
                    role = ApplicationRoles.PropertyManager;
                    break;
                case "customer agent":
                    role = ApplicationRoles.CustomerAgent;
                    break;
                case "customer":
                    role = ApplicationRoles.Customer;
                    break;
                default:
                    break;
            }
            return role;
        }

        /// <summary>
        /// CreateToken - allows us to create a token for a user's request
        /// </summary>
        /// <param name="user">The user we want to create a token for</param>
        /// <param name="role">The role the user has</param>
        /// <returns>JWT Security Token</returns>
        private JwtSecurityToken CreateToken(ApplicationUser user, List<string> role)
        {
            // Token requires pieces of information called "claims"
            // person/user is the principal
            // a principal can have many forms of identity
            // an identity contains many claims
            // a claim is a single statement about the user

            var authClaims = new List<Claim>()
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("UserId", user.Id),
                // new Claim("FavoriteColor", true)
            };

            foreach (var item in role)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var token = AuthenticateToken(authClaims);

            return token;
        }

        /// <summary>
        /// Authenticate Token - Allows us to validate a user's request based on the status of their role
        /// </summary>
        /// <param name="claims">A list of claims based on the user</param>
        /// <returns>JWT Security Token</returns>
        private JwtSecurityToken AuthenticateToken(List<Claim> claims)
        {
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTKey"]));

            var token = new JwtSecurityToken(
                issuer: _config["JWTIssuer"],
                audience: _config["JWTIssuer"],
                expires: DateTime.UtcNow.AddHours(24),
                claims: claims,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }


    }
}
