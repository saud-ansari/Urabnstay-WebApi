using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Urbanstay.WebApi.Models;
using Urbanstay.WebApi.Services;
using Urbanstay.WebApi.ViewModels;

namespace Urbanstay.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UrbanstayContext _appdbContext;
        private readonly IConfiguration _configuration;

        public BookingController(IConfiguration configuration)
        {
            _appdbContext = new UrbanstayContext();
            _configuration = configuration;
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
                GuestEmail = x.Guest.Email,
                GuestID = x.Guest.Id,
                HostName = x.Host.FirstName + " " + x.Host.LastName,
                HostEmail = x.Host.Email,
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
                GuestID = x.GuestId,
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
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, string status, [FromQuery] string fromemail,[FromQuery] string toName,[FromQuery] string toemail)
            {
            var order = _appdbContext.Bookings.Include(b => b.Property).FirstOrDefault(x => x.BookingId == bookingId);

            if (order == null)
            {
                return NotFound("Booking not found.");
            }

            // Validate status
            var validStatuses = new[] { "Confirmed", "Cancelled" };
            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid status value. Allowed values: Confirmed, Cancelled.");
            }

            // Update the booking status
            order.Status = status;
            order.UpdatedAt = DateTime.Now;

            // Prepare email text based on status
            var emailText = status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase)
                ? $"Your booking has been successfully confirmed for '{order.Property.Title}'!"
                : $"Unfortunately, your booking has been cancelled for '{order.Property.Title}'";

            var emailService = new EmailServices(_configuration);
            await emailService.SendEmail(
                fromName: "UrbanStay",
                fromemail:fromemail,
                toName: toName,
                toemail: toemail,
                subject: "Booking Status Update",
                body: emailText
            );

            _appdbContext.SaveChanges();
            return Ok($"Booking status updated to '{status}' and email sent to the user.");
        }


    }
}
