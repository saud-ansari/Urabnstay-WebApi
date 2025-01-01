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
                x.Property.ImagePath,
                x.Property.ImagePath2,
                x.Property.ImagePath3,
                x.Property.ImagePath4,
                x.Property.ImagePath5,
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

        [HttpGet("byId/{bookId:int}")]
        public IActionResult GetByBookingId(int bookId)
        {
            var booking = _appdbContext.Bookings.Where(x => x.BookingId == bookId).Select(x => new
            {
                x.BookingId,
                propertyName = x.Property.Title,
                GuestName = x.Guest.FirstName + " " + x.Guest.LastName,
                HostName = x.Host.FirstName + " " + x.Host.LastName,
                x.Property.ImagePath,
                x.Property.ImagePath2,
                x.Property.ImagePath3,
                x.Property.ImagePath4,
                x.Property.ImagePath5,
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
                    p.PaymentStatus,
                    p.PaymentMethod,
                    p.TransactionId,
                }),
                x.Guest.Username,
                x.Guest.Email,
                x.Guest.MobileNo
            }).ToList();
            return Ok (booking);
        }

        [HttpGet("{hostid:int}")]
        public IActionResult GetById(int hostid)
        {
            var user = _appdbContext.Bookings.Where(x => x.HostId == hostid).Select(x => new
            {
                x.BookingId,
                propertyName = x.Property.Title,
                GuestName = x.Guest.FirstName + " " + x.Guest.LastName,
                x.Property.ImagePath,
                x.Property.ImagePath2,
                x.Property.ImagePath3,
                x.Property.ImagePath4,
                x.Property.ImagePath5,
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
                    p.PaymentStatus,
                    p.PaymentMethod,
                    p.TransactionId,
                }),
                x.Guest.Username,
                x.Guest.Email,
                x.Guest.MobileNo               
            }).ToList();
            return Ok(user);
        }

        [HttpGet("guest/{guestId:int}")]
        public IActionResult GetByGuestId(int guestId)
        {
            var booking = _appdbContext.Bookings.Where(x=> x.GuestId == guestId).Select(x=> new
            {
                x.BookingId,
                propertyName = x.Property.Title,
                GuestName = x.Guest.FirstName + " " + x.Guest.LastName,
                HostName = x.Host.FirstName + " " + x.Host.LastName,
                x.Property.ImagePath,
                x.Property.ImagePath2,
                x.Property.ImagePath3,
                x.Property.ImagePath4,
                x.Property.ImagePath5,
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
                    p.PaymentStatus,
                    p.PaymentMethod,
                    p.TransactionId,
                }),
                x.Guest.Username,
                x.Guest.Email,
                x.Guest.MobileNo
            }).ToList();
            return Ok(booking);
        }

        [HttpPost]
        public IActionResult Post(AddOrder _order)
        {
            // Check for overlapping bookings
            var conflictingBooking = _appdbContext.Bookings.Any(b =>
                b.PropertyId == _order.PropertyId &&
                b.Status == "confirmed" && // Check only confirmed bookings
                (
                    (_order.CheckInDate >= b.CheckInDate && _order.CheckInDate < b.CheckOutDate) || // Start date overlaps
                    (_order.CheckOutDate > b.CheckInDate && _order.CheckOutDate <= b.CheckOutDate) || // End date overlaps
                    (_order.CheckInDate <= b.CheckInDate && _order.CheckOutDate >= b.CheckOutDate) // Envelops existing booking
                )
            );

            if (conflictingBooking)
            {
                return BadRequest("The property is already booked for the selected dates.");
            }

            // Proceed with booking
            var order = new Booking
            {
                PropertyId = _order.PropertyId,
                GuestId = _order.GuestId,
                HostId = _order.HostId,
                CheckInDate = _order.CheckInDate,
                CheckOutDate = _order.CheckOutDate,
                NumberOfGuests = _order.NumberOfGuests,
                TotalPrice = _order.TotalPrice,
                Status = "pending",
                CreatedAt = DateTime.Now
            };

            _appdbContext.Bookings.Add(order);
            if (_appdbContext.SaveChanges() > 0)
            {
                return Ok("Booking created successfully.");
            }
            else
            {
                return BadRequest("Something went wrong while creating the booking.");
            }
        }


        [HttpPost("{bookingId}/{status}")]
        public IActionResult Post(int bookingId,string status)
        {
            var order = _appdbContext.Bookings.FirstOrDefault(x=> x.BookingId == bookingId );
            if(order != null)
            {
                order.Status = status;
                if(status.ToLower() == "Confirmed,Cancelled".ToString().ToLower())
                 order.UpdatedAt = DateTime.Now;                    
            }
            var result = _appdbContext.SaveChanges() > 0;
            return Ok(result);
        }
    }
}
