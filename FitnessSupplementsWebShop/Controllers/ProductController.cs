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
    [Route("api/products")]
    [Produces("application/json", "application/xml")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public ProductController(ICategoryRepository categoryRepository, IManufacturerRepository manufacturerRepository, IProductRepository productRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }
        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<ProductDto>> GetProduct([FromQuery] Params p)
        {
            if(p.PageIndex == 0)
                p.PageIndex = 1;
            var productsCount = productRepository.GetProductCount();
            var pageResult = 6f;
            var products = productRepository.GetProductByPage(p.PageIndex, (int)(pageResult),p.ManufacturerID,p.CategoryID,p.Sort,p.Search);
            var numberOfproducts = productRepository.GetNumberOfProducts();
            var response = new ProductResponse
            {
                Products = null,
                PageIndex = p.PageIndex,
                PageSize = (int)pageResult,
                Count = numberOfproducts
            };
            if (products == null || numberOfproducts == 0)
                return Ok(response);
            List<ProductDto> productsDto = new();
            foreach (ProductEntity r in products)
            {
                ProductDto productDto = mapper.Map<ProductDto>(r);
                productDto.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(r.ManufacturerID));
                productDto.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(r.CategoryID));
                productsDto.Add(productDto);
            }
             response = new ProductResponse
            {
                Products = productsDto,
                PageIndex = p.PageIndex,
                PageSize = (int)pageResult,
                Count = numberOfproducts
            };
            return Ok(response);
            
        }

        [AllowAnonymous]
        [HttpGet("{productID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ProductDto> GetProductByID(int productID)
        {
            ProductEntity product = productRepository.GetProductByID(productID);
            if (product == null)
                return NotFound();
            ProductDto productDto = mapper.Map<ProductDto>(product);
            productDto.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(product.ManufacturerID));
            productDto.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(product.CategoryID));
            return Ok(productDto);
        }
        //[AllowAnonymous]
        //[HttpGet]
        //[HttpHead]
        //public ActionResult<ProductDto> GetProductsBy([FromQuery]string category, [FromQuery]string manufacturer)
        //{
        //    List<ProductEntity> products = new List<ProductEntity>();
        //    if (category != null)
        //    {
        //         products = productRepository.GetProductsByCategory(category);
        //    }
        //    else if (category == null && manufacturer !=null)
        //    {
        //        products = productRepository.GetProductsByManufacturer(manufacturer);
        //    }
        //    else if(category !=null & manufacturer!=null)
        //    {
        //        products = productRepository.GetProductsByCategoryAndManufacturer(category, manufacturer);
        //    }
        //    else if (products == null)
        //    {
        //        return NotFound();
        //    }

        //    List<ProductDto> productsDto = new List<ProductDto>();

        //    foreach (ProductEntity r in products)
        //    {
        //        ProductDto productDto = mapper.Map<ProductDto>(r);
        //        productDto.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(r.ManufacturerID));
        //        productDto.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(r.CategoryID));
        //        productsDto.Add(productDto);
        //    }

        //    return Ok(productsDto);
        //}


        [Authorize(Roles = "admin")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ProductDto> CreateProduct([FromBody] ProductDto product)
        {
            try
            {
                ProductEntity rew = mapper.Map<ProductEntity>(product);
                ProductEntity r = productRepository.CreateProduct(rew);
                productRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetProduct", "Product", new { productID = r.ProductID });
                ProductDto productDto = mapper.Map<ProductDto>(r);
                productDto.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(product.ManufacturerID));
                productDto.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(product.CategoryID));

                return Created(location, productDto);
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
        public ActionResult<ProductDto> UpdateProduct(ProductDto product)
        {

            try
            {
                ProductEntity oldProduct = productRepository.GetProductByID(product.ProductID);

                if (oldProduct == null)
                {
                    return NotFound();
                }
                ProductEntity productEntity = mapper.Map<ProductEntity>(product);

                oldProduct.Name = productEntity.Name;
                oldProduct.Description = productEntity.Description;
                oldProduct.Price = productEntity.Price;
                oldProduct.Quantity = productEntity.Quantity;
                oldProduct.CategoryID = productEntity.CategoryID;
                oldProduct.ManufacturerID = productEntity.ManufacturerID;
                productRepository.SaveChanges();
                ProductDto productDto = mapper.Map<ProductDto>(oldProduct);
                productDto.Manufacturer = mapper.Map<ManufacturerDto>(manufacturerRepository.GetManufacturerByID(product.ManufacturerID));
                productDto.Category = mapper.Map<CategoryDto>(categoryRepository.GetCategoryByID(product.CategoryID)); 
                return Ok(productDto);
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
        [HttpDelete("{productID}")]
        public IActionResult DeleteProduct(int productID)
        {
            try
            {
                ProductEntity product = productRepository.GetProductByID(productID);
                if (product == null)
                {
                    return NotFound();
                }
                productRepository.DeleteProduct(productID);
                productRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetProductOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
