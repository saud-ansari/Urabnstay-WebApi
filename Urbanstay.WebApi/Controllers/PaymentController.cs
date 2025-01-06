using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Urbanstay.WebApi.Models;
using Urbanstay.WebApi.ViewModels;

namespace Urbanstay.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly UrbanstayContext _appdbContext;

        public PaymentController()
        {
            _appdbContext = new UrbanstayContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var getpayment = _appdbContext.Payments.Select(x => new
            {
                x.PaymentId,
                x.BookingId,
                x.Amount,
                x.PaymentMethod,
                x.PaymentStatus,
                x.TransactionId,
                x.CreatedAt,
                x.UpdatedAt
            }).ToList();
            return Ok(getpayment);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PaymentModel _payment)
        {
            var payment = new Payment();
            payment.BookingId = _payment.BookingId;
            payment.Amount = _payment.Amount;
            payment.PaymentMethod = _payment.PaymentMethod;
            payment.PaymentStatus = "SuccessFull";
            payment.TransactionId = _payment.TransactionId;
            payment.CreatedAt = DateTime.Now;
            _appdbContext.Payments.Add(payment);
            if (_appdbContext.SaveChanges() > 0)

                return Ok("Done");
            else
                return BadRequest("Something went wrong");
        }
    }
}
