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
    [Route("api/orders")]
    [Produces("application/json", "application/xml")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly IUsersRepository usersRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOrderitemRepository orderitemRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public OrdersController(IOrdersRepository ordersRepository,IUsersRepository usersRepository,IPaymentRepository paymentRepository,ICategoryRepository categoryRepository, LinkGenerator linkGenerator, IMapper mapper, IOrderitemRepository orderitemRepository)
        {
            this.ordersRepository = ordersRepository;
            this.usersRepository = usersRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.paymentRepository = paymentRepository;
            this.orderitemRepository = orderitemRepository;
        }
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<OrdersDto>> GetOrders()
        {
            List<OrdersEntity> orders = ordersRepository.GetOrder();
            if (orders == null || orders.Count == 0)
                return NoContent();
            List<OrdersDto> ordersDto = new List<OrdersDto>();

            foreach (OrdersEntity r in orders)
            {
                OrdersDto orderDto = mapper.Map<OrdersDto>(r);
                orderDto.Payment = mapper.Map<PaymentDto>(paymentRepository.GetPaymentByID(r.PaymentID));
                orderDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(r.UserID));
                ordersDto.Add(orderDto);
            }
            return Ok(ordersDto);
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet("{orderID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<OrdersDto> GetOrdersByID(int orderID)
        {
            OrdersEntity order = ordersRepository.GetOrderByID(orderID);
            if (order == null)
                return NotFound();
            OrdersDto orderDto = mapper.Map<OrdersDto>(order);
            orderDto.Payment = mapper.Map<PaymentDto>(paymentRepository.GetPaymentByID(order.PaymentID));
            orderDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(order.UserID));
            var response = mapper.Map<OrdersResponse>(orderDto);
            response.Products = mapper.Map<List<ProductDto>>(orderitemRepository.GetProductsByOrderID(response.OrderID));
            return Ok(response);
        }
        [Authorize(Roles = "admin,customer")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<OrdersDto> CreateOrders([FromBody] OrdersDto order)
        {
            try
            {
                OrdersEntity rew = mapper.Map<OrdersEntity>(order);
                OrdersEntity r = ordersRepository.CreateOrder(rew);
                ordersRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetOrders", "Orders", new { orderID = r.OrderID });
                OrdersDto orderDto = mapper.Map<OrdersDto>(r);
                orderDto.Payment = mapper.Map<PaymentDto>(paymentRepository.GetPaymentByID(order.PaymentID));
                orderDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(order.UserID));

                return Created(location, orderDto);
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
        public ActionResult<OrdersDto> UpdateOrders(OrdersDto order)
        {

            try
            {
                OrdersEntity oldOrder = ordersRepository.GetOrderByID(order.OrderID);

                if (oldOrder == null)
                {
                    return NotFound();
                }
                OrdersEntity orderEntity = mapper.Map<OrdersEntity>(order);

                oldOrder.OrderAddress = orderEntity.OrderAddress;
                oldOrder.City = orderEntity.City;
                oldOrder.NumberOfProducts = orderEntity.NumberOfProducts;
                oldOrder.Total = orderEntity.Total;
                oldOrder.UserID = orderEntity.UserID;
                oldOrder.PaymentID = orderEntity.PaymentID;
                ordersRepository.SaveChanges();
                OrdersDto orderDto = mapper.Map<OrdersDto>(oldOrder);
                orderDto.Payment = mapper.Map<PaymentDto>(paymentRepository.GetPaymentByID(order.PaymentID));
                orderDto.User = mapper.Map<UsersDto>(usersRepository.GetUserByID(order.UserID));
                return Ok(orderDto);
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
        [HttpDelete("{orderID}")]
        public IActionResult DeleteOrders(int orderID)
        {
            try
            {
                OrdersEntity order = ordersRepository.GetOrderByID(orderID);
                if (order == null)
                {
                    return NotFound();
                }
                ordersRepository.DeleteOrder(orderID);
                ordersRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetOrdersOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
