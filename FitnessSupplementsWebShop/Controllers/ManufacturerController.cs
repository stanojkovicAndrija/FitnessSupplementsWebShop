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
    [Route("api/manufacturers")]
    [Produces("application/json", "application/xml")] 
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public ManufacturerController(IManufacturerRepository manufacturerRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.manufacturerRepository = manufacturerRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<ManufacturerDto>> GetManufacturer()
        {
            List<ManufacturerEntity> manufacturers = manufacturerRepository.GetManufacturer();
            if (manufacturers == null || manufacturers.Count == 0)
                return NoContent();
            List<ManufacturerDto> manufacturerDto = mapper.Map<List<ManufacturerDto>>(manufacturers);
            return Ok(manufacturerDto);
        }
        [AllowAnonymous]
        [HttpGet("{manufacturerID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ManufacturerDto> GetManufacturerByID(int manufacturerID)
        {
            ManufacturerEntity manufacturer = manufacturerRepository.GetManufacturerByID(manufacturerID);
            if (manufacturer == null)
                return NotFound();
            ManufacturerDto manufacturerDto = mapper.Map<ManufacturerDto>(manufacturer);
            return Ok(manufacturerDto);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ManufacturerDto> CreateManufacturer([FromBody] ManufacturerDto manufacturer)
        {
            try
            {
                ManufacturerEntity cat = mapper.Map<ManufacturerEntity>(manufacturer);
                ManufacturerEntity c = manufacturerRepository.CreateManufacturer(cat);
                manufacturerRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetManufacturer", "Manufacturer", new { manufacturerID = c.ManufacturerID });
                ManufacturerDto manufacturerDto = mapper.Map<ManufacturerDto>(c);
                return Created(location, manufacturerDto);
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
        public ActionResult<ManufacturerDto> UpdateManufacturer(ManufacturerDto manufacturer)
        {

            try
            {
                ManufacturerEntity oldManufacturer = manufacturerRepository.GetManufacturerByID(manufacturer.ManufacturerID);

                if (oldManufacturer == null)
                {
                    return NotFound();
                }
                ManufacturerEntity manufacturerEntity = mapper.Map<ManufacturerEntity>(manufacturer);

                oldManufacturer.Name = manufacturerEntity.Name;
                manufacturerRepository.SaveChanges();
                return Ok(mapper.Map<ManufacturerDto>(oldManufacturer));
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
        [HttpDelete("{manufacturerID}")]
        public IActionResult DeleteManufacturer(int manufacturerID)
        {
            try
            {
                ManufacturerEntity manufacturer = manufacturerRepository.GetManufacturerByID(manufacturerID);
                if (manufacturer == null)
                {
                    return NotFound();
                }
                manufacturerRepository.DeleteManufacturer(manufacturerID);
                manufacturerRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetManufacturerOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
