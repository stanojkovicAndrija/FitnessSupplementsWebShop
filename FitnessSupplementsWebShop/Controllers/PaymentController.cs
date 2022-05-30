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
    [Route("api/payments")]
    [Produces("application/json", "application/xml")] 
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public PaymentController(IPaymentRepository paymentRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.paymentRepository = paymentRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<PaymentDto>> GetPayment()
        {
            List<PaymentEntity> payments = paymentRepository.GetPayment();
            if (payments == null || payments.Count == 0)
                return NoContent();
            List<PaymentDto> paymentDto = mapper.Map<List<PaymentDto>>(payments);
            return Ok(paymentDto);
        }

        [Authorize(Roles = "admin,customer")]
        [HttpGet("{paymentID}")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<PaymentDto> GetPaymentByID(int paymentID)
        {
            PaymentEntity payment = paymentRepository.GetPaymentByID(paymentID);
            if (payment == null)
                return NotFound();
            PaymentDto paymentDto = mapper.Map<PaymentDto>(payment);
            return Ok(paymentDto);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [HttpHead]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<PaymentDto> CreatePayment([FromBody] PaymentDto payment)
        {
            try
            {
                PaymentEntity cat = mapper.Map<PaymentEntity>(payment);
                PaymentEntity c = paymentRepository.CreatePayment(cat);
                paymentRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetPayment", "Payment", new { paymentID = c.PaymentID });
                PaymentDto paymentDto = mapper.Map<PaymentDto>(c);
                return Created(location, paymentDto);
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
        public ActionResult<PaymentDto> UpdatePayment(PaymentDto payment)
        {

            try
            {
                PaymentEntity oldPayment = paymentRepository.GetPaymentByID(payment.PaymentID);

                if (oldPayment == null)
                {
                    return NotFound();
                }
                PaymentEntity paymentEntity = mapper.Map<PaymentEntity>(payment);

                oldPayment.PaymentMethod = paymentEntity.PaymentMethod;
                paymentRepository.SaveChanges();
                return Ok(mapper.Map<PaymentDto>(oldPayment));
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
        [HttpDelete("{paymentID}")]
        public IActionResult DeletePayment(int paymentID)
        {
            try
            {
                PaymentEntity payment = paymentRepository.GetPaymentByID(paymentID);
                if (payment == null)
                {
                    return NotFound();
                }
                paymentRepository.DeletePayment(paymentID);
                paymentRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetPaymentOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
