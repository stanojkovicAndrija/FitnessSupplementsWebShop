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
    [Route("api/reviews")]
    [Produces("application/json", "application/xml")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IProductRepository productRepository;
        private readonly IUsersRepository usersRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public ReviewController(ICategoryRepository categoryRepository,IManufacturerRepository manufacturerRepository,IReviewRepository reviewRepository, LinkGenerator linkGenerator, IMapper mapper, IUsersRepository usersRepository, IProductRepository productRepository)
        {
            this.reviewRepository = reviewRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.usersRepository = usersRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<ReviewDto>> GetReview()
        {
            List<ReviewEntity> reviews = reviewRepository.GetReview();
            if (reviews == null || reviews.Count == 0)
                return NoContent();
            List<ReviewDto> reviewsDto = new List<ReviewDto>();

            foreach (ReviewEntity r in reviews)
            {
                ReviewDto reviewDto = mapper.Map<ReviewDto>(r);
                reviewDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(r.ProductID));
                reviewDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(r.UserID));
                reviewDto.Product.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(r.Product.CategoryID));
                reviewDto.Product.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(r.Product.ManufacturerID));

                reviewsDto.Add(reviewDto);
            }    
            return Ok(reviewsDto);
        }

        [AllowAnonymous]
        [HttpGet("{reviewID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ReviewDto> GetReviewByID(int reviewID)
        {
            ReviewEntity review = reviewRepository.GetReviewByID(reviewID);
            if (review == null)
                return NotFound();
            ReviewDto reviewDto = mapper.Map<ReviewDto>(review);
            reviewDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(review.ProductID));
            reviewDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(review.UserID));
            reviewDto.Product.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(review.Product.CategoryID));
            reviewDto.Product.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(review.Product.ManufacturerID));
            return Ok(reviewDto);
        }

        [Authorize(Roles = "admin,customer")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ReviewDto> CreateReview([FromBody] ReviewDto review)
        {
            try
            {
                ReviewEntity rew = mapper.Map<ReviewEntity>(review);
                ReviewEntity r = reviewRepository.CreateReview(rew);
                reviewRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetReview", "Review", new { reviewID = r.ReviewID });
                ReviewDto reviewDto = mapper.Map<ReviewDto>(r);
                reviewDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(review.UserID));
                reviewDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(review.ProductID));

                return Created(location, reviewDto);
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
        public ActionResult<ReviewDto> UpdateReview(ReviewDto review)
        {

            try
            {
                ReviewEntity oldReview = reviewRepository.GetReviewByID(review.ReviewID);

                if (oldReview == null)
                {
                    return NotFound();
                }
                ReviewEntity reviewEntity = mapper.Map<ReviewEntity>(review);

                oldReview.Comment = reviewEntity.Comment;
                oldReview.Rating = reviewEntity.Rating;
                oldReview.UserID = reviewEntity.UserID;
                oldReview.User = reviewEntity.User;
                oldReview.ProductID = reviewEntity.ProductID;
                reviewRepository.SaveChanges();
                ReviewDto reviewDto = mapper.Map<ReviewDto>(oldReview);
                reviewDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(oldReview.ProductID));
                return Ok(reviewDto);
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
        [HttpDelete("{reviewID}")]
        public IActionResult DeleteReview(int reviewID)
        {
            try
            {
                ReviewEntity review = reviewRepository.GetReviewByID(reviewID);
                if (review == null)
                {
                    return NotFound();
                }
                reviewRepository.DeleteReview(reviewID);
                reviewRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetReviewOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
