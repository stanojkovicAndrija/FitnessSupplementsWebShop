using AutoMapper;
using FitnessSupplementsWebShop.Data;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;

namespace FitnessSupplementsWebShop.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json", "application/xml")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<CategoryDto>> GetCategory()
        {
            List<CategoryEntity> categories = categoryRepository.GetCategory();
            if (categories == null || categories.Count == 0)
                return NoContent();
            List<CategoryDto> categoryDto = mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoryDto);
        }
        
        [AllowAnonymous]
        [HttpGet("{categoryID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CategoryDto> GetCategoryByID(int categoryID)
        {
            CategoryEntity category = categoryRepository.GetCategoryByID(categoryID);
            if (category == null)
                return NotFound();
            CategoryDto categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }
        [Authorize(Roles ="admin")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<CategoryDto> CreateCategory([FromBody] CategoryDto category)
        {
            try
            {
               
                CategoryEntity cat = mapper.Map<CategoryEntity>(category);
                CategoryEntity c = categoryRepository.CreateCategory(cat);

                categoryRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetCategory", "Category", new { categoryID = c.CategoryID });
                CategoryDto categoryDto = mapper.Map<CategoryDto>(c);
                return Created(location, categoryDto);
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
        public ActionResult<CategoryDto> UpdateCategory(CategoryUpdateDto category)
        {

            try
            {
                CategoryEntity oldCategory = categoryRepository.GetCategoryByID(category.CategoryID);

                if (oldCategory == null)
                {
                    return NotFound();
                }
                CategoryEntity categoryEntity = mapper.Map<CategoryEntity>(category);

                oldCategory.Name = categoryEntity.Name;
                categoryRepository.SaveChanges();
                return Ok(mapper.Map<CategoryDto>(oldCategory));
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
        [HttpDelete("{categoryID}")]
        public IActionResult DeleteCategory(int categoryID)
        {
            try
            {
                CategoryEntity category = categoryRepository.GetCategoryByID(categoryID);
                if (category == null)
                {
                    return NotFound();
                }
                categoryRepository.DeleteCategory(categoryID);
                categoryRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetCategoryOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
