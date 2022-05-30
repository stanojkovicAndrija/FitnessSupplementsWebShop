using AutoMapper;
using FitnessSupplementsWebShop.Data;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Net;

namespace FitnessSupplementsWebShop.Controllers
{

    
    [ApiController]
    [Route("api/users")]
    [Produces("application/json", "application/xml")] 
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository usersRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public UsersController(IUsersRepository usersRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<UsersDto>> GetUser()
        {
            List<UsersEntity> users = usersRepository.GetUsers();
            if (users == null || users.Count == 0)
                return NoContent();
            List<UsersDto> usersDto = mapper.Map<List<UsersDto>>(users);
            return Ok(usersDto);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{userID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UsersDto> GetUserByID(int userID)
        {
            UsersEntity user = usersRepository.GetUserByID(userID);
            if (user == null)
                return NotFound();
            UsersDto usersDto = mapper.Map<UsersDto>(user);
            return Ok(usersDto);
        }


        [Authorize(Roles = "customer,admin")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<UsersDto> CreateUser([FromBody] UsersDto user)
        {
            try
            {
                UsersEntity us = mapper.Map<UsersEntity>(user);
                us.Role = "customer";
                UsersEntity u = usersRepository.CreateUser(us);
                usersRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetUser", "Users", new { userID = u.UserID });
                UsersDto usersDto = mapper.Map<UsersDto>(u);
                return Created(location, usersDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [Authorize(Roles = "admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        [HttpHead]
        public ActionResult<UsersDto> UpdateUser(UsersDto user)
        {

            try
            {
                UsersEntity oldUser = usersRepository.GetUserByID(user.UserID);

                if (oldUser == null)
                {
                    return NotFound();
                }
                UsersEntity usersEntity = mapper.Map<UsersEntity>(user);

                oldUser.FirstName = usersEntity.FirstName;
                oldUser.LastName = usersEntity.LastName;
                oldUser.Email = usersEntity.Email;
                oldUser.Password = usersEntity.Password;
                oldUser.Phone = usersEntity.Phone;
                oldUser.Address = usersEntity.Address;
                usersRepository.SaveChanges();
                return Ok(mapper.Map<UsersDto>(oldUser));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }

        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userID}")]
        public IActionResult DeleteUser(int userID)
        {
            try
            {
                UsersEntity user = usersRepository.GetUserByID(userID);
                if (user == null)
                {
                    return NotFound();
                }
                usersRepository.DeleteUser(userID);
                usersRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
