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
    [Route("api/orderItems")]
    [Produces("application/json", "application/xml")]
    public class OrderitemController : ControllerBase
    {
        private readonly IOrderitemRepository orderitemRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public OrderitemController(IOrdersRepository ordersRepository, IOrderitemRepository orderitemRepository, LinkGenerator linkGenerator, IMapper mapper, IProductRepository productRepository)
        {
            this.orderitemRepository = orderitemRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.ordersRepository = ordersRepository;
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<OrderitemDto>> GetOrderitem()
        {
            List<OrderitemEntity> orderItems = orderitemRepository.GetOrderItem();
            if (orderItems == null || orderItems.Count == 0)
                return NoContent();
            List<OrderitemDto> orderItemsDto = new List<OrderitemDto>();

            //List<OrderitemDto> orderItemsDto = mapper.Map<List<OrderitemDto>>(orderItems);
            foreach (OrderitemEntity o in orderItems)
            {
                OrderitemDto orderItem = mapper.Map<OrderitemDto>(o);
                orderItem.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(o.ProductID));
                orderItem.Order = mapper.Map<OrdersDto>(ordersRepository.GetOrderByID(o.OrderID));
                orderItemsDto.Add(orderItem);
            }
            return Ok(orderItemsDto);
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet("{orderItemID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<OrderitemDto> GetOrderitemByID(int orderItemID)
        {
            OrderitemEntity orderItem = orderitemRepository.GetOrderItemByID(orderItemID);
            if (orderItem == null)
                return NotFound();
           
            OrderitemDto orderItemDto = mapper.Map<OrderitemDto>(orderItem);
            orderItemDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(orderItem.ProductID));
            orderItemDto.Order = mapper.Map<OrdersDto>(ordersRepository.GetOrderByID(orderItem.OrderID));
            return Ok(orderItemDto);
        }
        [Authorize(Roles = "admin,customer")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<OrderitemDto> CreateOrderitem([FromBody] OrderitemDto orderItem)
        {
            try
            {
                OrderitemEntity rew = mapper.Map<OrderitemEntity>(orderItem);               
                OrderitemEntity r = orderitemRepository.CreateOrderItem(rew);
                orderitemRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetOrderItem", "Orderitem", new { orderItemID = r.OrderItemID });
                OrderitemDto orderItemDto = mapper.Map<OrderitemDto>(r);
                orderItemDto.Order = mapper.Map<OrdersDto>(ordersRepository.GetOrderByID(orderItem.OrderID));
                orderItemDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(orderItem.ProductID));

                return Ok(orderItemDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        [Authorize(Roles = "admin,customer")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        [HttpHead]
        public ActionResult<OrderitemDto> UpdateOrderitem(OrderitemDto orderItem)
        {

            try
            {
                OrderitemEntity oldOrderitem = orderitemRepository.GetOrderItemByID(orderItem.OrderItemID);

                if (oldOrderitem == null)
                {
                    return NotFound();
                }
                OrderitemEntity orderItemEntity = mapper.Map<OrderitemEntity>(orderItem);

                oldOrderitem.OrderID = orderItemEntity.OrderID;
                oldOrderitem.ProductID = orderItemEntity.ProductID;
                oldOrderitem.Quantity = orderItemEntity.Quantity;
                orderitemRepository.SaveChanges();
                OrderitemDto orderItemDto = mapper.Map<OrderitemDto>(oldOrderitem);
                orderItemDto.Order = mapper.Map<OrdersDto>(ordersRepository.GetOrderByID(orderItem.OrderID));
                orderItemDto.Product = mapper.Map<ProductDto>(productRepository.GetProductByID(oldOrderitem.ProductID));
                return Ok(orderItemDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }

        [Authorize(Roles = "admin,customer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{orderItemID}")]
        public IActionResult DeleteOrderitem(int orderItemID)
        {
            try
            {
                OrderitemEntity orderItem = orderitemRepository.GetOrderItemByID(orderItemID);
                if (orderItem == null)
                {
                    return NotFound();
                }
                orderitemRepository.DeleteOrderItem(orderItemID);
                orderitemRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetOrderitemOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
