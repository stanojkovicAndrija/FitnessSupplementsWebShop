using FitnessSupplementsWebShop.Auth;
using AutoMapper;
using FitnessSupplementsWebShop.Models;
using FitnessSupplementsWebShop.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Data;
using Microsoft.AspNetCore.Routing;

namespace FitnessSupplementsWebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public AuthController(LinkGenerator linkGenerator, IMapper mapper, IConfiguration config, IUsersRepository usersRepository)
        {
            _config = config;
            this.usersRepository = usersRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UsersDto newUser)
        {

            List<UsersEntity> allUsers = usersRepository.GetUsers();
            foreach(var all in allUsers)
            {
                if(all.Email==newUser.Email)
                {
                      return StatusCode(StatusCodes.Status400BadRequest,"Email is already taken!");
                }
            }

            UsersEntity us = mapper.Map<UsersEntity>(newUser);
            us.Role = "customer";
            UsersEntity u = usersRepository.CreateUser(us);
            usersRepository.SaveChanges();
            string location = linkGenerator.GetPathByAction("GetUser", "Users", new { userID = u.UserID });
            UsersDto usersDto = mapper.Map<UsersDto>(u);
            return Created(location,usersDto);
        }
        private string Generate(UsersDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddDays(20),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UsersDto Authenticate(LoginDto userLogin)
        {
            var currentUser = mapper.Map<UsersDto>(usersRepository.GetUserByEmail(userLogin));
            if (currentUser.Role == "admin")
            {
                if (currentUser.Password == userLogin.Password)
                    return currentUser;
                else return null;
            }
            if (currentUser != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, currentUser.Password)) 
            {
                return currentUser;
            }

            return null;
        }
    }
}