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
    public class BookingController : ControllerBase
    {
        private readonly UrbanstayContext _appdbContext;

        public BookingController()
        {
            _appdbContext = new UrbanstayContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var order = _appdbContext.Bookings.Select(x=> new
            {
                x.BookingId,
                propertyName = x.Property.Title,
                GuestName = x.Guest.FirstName + " " + x.Guest.LastName,
                HostName = x.Host.FirstName + " " + x.Host.LastName,
                x.CheckInDate,
                x.CheckOutDate,
                x.NumberOfGuests,
                x.TotalPrice,
                x.Status,
                x.CreatedAt,
                x.UpdatedAt,
                Payment = x.Payments.Select(p => new
                {
                    p.Amount,
                    p.PaymentStatus
                })
            }).ToList();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post(AddOrder _order)
        {
            var order = new Booking();
            order.PropertyId = _order.PropertyId;
            order.GuestId = _order.GuestId;
            order.HostId = _order.HostId;
            order.CheckInDate = _order.CheckInDate;
            order.CheckOutDate = _order.CheckOutDate;
            order.NumberOfGuests = _order.NumberOfGuests;
            order.TotalPrice = _order.TotalPrice;
            order.Status = _order.Status;
            order.CreatedAt = DateTime.Now;            
        
            _appdbContext.Bookings.Add(order);
            if (_appdbContext.SaveChanges() > 0)
            {
                return Ok("Done");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPut("{bookingId}/{status}")]
        public IActionResult Post(int bookingId,string status)
        {
            var order = _appdbContext.Bookings.FirstOrDefault(x=> x.BookingId == bookingId );
            if(order != null)
            {
                order.Status = status;
                if(status.ToLower() == "Confirmed".ToString().ToLower())
                 order.UpdatedAt = DateTime.Now;                    
            }
            var result = _appdbContext.SaveChanges() > 0;
            return Ok(result);
        }
    }
}
