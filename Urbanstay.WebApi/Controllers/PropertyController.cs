using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Urbanstay.WebApi.Models;
using Urbanstay.WebApi.ViewModels;
using static Urbanstay.WebApi.ViewModels.AddProperty;

namespace Urbanstay.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly UrbanstayContext _appdbContext;

        public PropertyController()
        {
            _appdbContext = new UrbanstayContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = _appdbContext.Properties.Where(x => x.IsActive).Select(x => new
            {
                x.PropertyId,
                x.HostId,
                HostName = x.Host.Username,
                x.Title,
                x.Description,
                x.ImagePath,
                x.ImagePath2,
                x.ImagePath3,
                x.ImagePath4,
                x.ImagePath5,
                x.IsActive,
                x.Address,
                x.City,
                x.Country,
                x.ZipCode,
                x.PropertyType,
                x.AvailabilityCalendar,
                x.PricePerNight,
                x.HouseRules,
                x.InstantBooking,
                x.CreatedAt,
                x.UpdatedAt,
                Review = x.Reviews.Select(r => new
                {
                    r.PropertyId,
                    r.Reviewer.Username,
                    r.Comment,
                    r.CreatedAt

                })
            }).ToList();
            return Ok(user);
        }

        [HttpGet("ById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _appdbContext.Properties.Where(x => x.PropertyId == id).Select(x => new
            {
                x.PropertyId,
                x.HostId,
                x.Title,
                x.Description,
                x.ImagePath,
                x.ImagePath2,
                x.ImagePath3,
                x.ImagePath4,
                x.ImagePath5,
                x.IsActive,
                x.Address,
                x.City,
                x.Country,
                x.State,
                x.ZipCode,
                x.PropertyType,
                x.AvailabilityCalendar,
                x.PricePerNight,
                x.HouseRules,
                x.InstantBooking,
                x.CreatedAt,
                x.UpdatedAt,
                Review = x.Reviews.Select(r => new
                {
                    r.PropertyId,
                    r.Reviewer.Username,
                    r.Comment,
                    r.CreatedAt

                })
            }).ToList();
            return Ok(user);
        }

        [HttpGet("ByHostId/{Id:int}")]
        public IActionResult GetByHostId(int Id)
        {
            var user = _appdbContext.Properties.Where(x => x.HostId == Id).Select(x => new
            {
                x.PropertyId,
                x.HostId,
                x.Title,
                x.Description,
                x.ImagePath,
                x.ImagePath2,
                x.ImagePath3,
                x.ImagePath4,
                x.ImagePath5,
                x.IsActive,
                x.Address,
                x.City,
                x.Country,
                x.State,
                x.ZipCode,
                x.PropertyType,
                x.AvailabilityCalendar,
                x.PricePerNight,
                x.HouseRules,
                x.InstantBooking,
                x.CreatedAt,
                x.UpdatedAt,
                Review = x.Reviews.Select(r => new
                {
                    r.PropertyId,
                    r.Reviewer.Username,
                    r.Comment,
                    r.CreatedAt

                })
            }).ToList();
            return Ok(user);
        }

        [HttpGet("SearchByDateAndCity")]
        public IActionResult SearchByDateAndCity(DateTime checkInDate, DateTime checkOutDate, string city)
        {
            // Filter properties based on city and availability
            var availableProperties = _appdbContext.Properties
                .Where(x => x.City.ToLower() == city.ToLower() && x.IsActive) // Filter by city and active properties
                .Where(x => !_appdbContext.Bookings.Any(b =>
                    b.PropertyId == x.PropertyId &&
                    b.Status == "confirmed" && // Check only confirmed bookings
                    (
                        (checkInDate >= b.CheckInDate && checkInDate < b.CheckOutDate) || // Start date overlaps
                        (checkOutDate > b.CheckInDate && checkOutDate <= b.CheckOutDate) || // End date overlaps
                        (checkInDate <= b.CheckInDate && checkOutDate >= b.CheckOutDate) // Envelops existing booking
                    )
                ))
                .Select(x => new
                {
                    x.PropertyId,
                    x.HostId,
                    x.Title,
                    x.Description,
                    x.ImagePath,
                    x.ImagePath2,
                    x.ImagePath3,
                    x.ImagePath4,
                    x.ImagePath5,
                    x.IsActive,
                    x.Address,
                    x.City,
                    x.Country,
                    x.State,
                    x.ZipCode,
                    x.PropertyType,
                    x.AvailabilityCalendar,
                    x.PricePerNight,
                    x.HouseRules,
                    x.InstantBooking,
                    x.CreatedAt,
                    x.UpdatedAt,
                    Review = x.Reviews.Select(r => new
                    {
                        r.PropertyId,
                        r.Reviewer.Username,
                        r.Comment,
                        r.CreatedAt
                    })
                })
                .ToList();

            return Ok(availableProperties);
        }

        [HttpPost]
        public IActionResult Post(AddProperty _addProperty)
        {
            var property = _appdbContext.Properties.Where(x => x.Title.ToLower() == _addProperty.Title.ToLower()).FirstOrDefault();
            if (property != null)
            {
                return Conflict(new { Message = "Already Added Property" });
            }
            else
            {
                property = new Property
                {
                    HostId = _addProperty.HostId,
                    Title = _addProperty.Title,
                    Description = _addProperty.Description,
                    IsActive = _addProperty.IsActive,
                    Address = _addProperty.Address,
                    City = _addProperty.City,
                    Country = _addProperty.Country,
                    State = _addProperty.State,
                    ZipCode = _addProperty.ZipCode,
                    PropertyType = _addProperty.PropertyType,
                    PricePerNight = _addProperty.PricePerNight,
                    AvailabilityCalendar = _addProperty.AvailabilityCalendar,
                    HouseRules = _addProperty.HouseRules,
                    InstantBooking = _addProperty.InstantBooking,
                    ImagePath = _addProperty.ImagePath,
                    ImagePath2 = _addProperty.ImagePath2,
                    ImagePath3 = _addProperty.ImagePath3,
                    ImagePath4 = _addProperty.ImagePath4,
                    ImagePath5 = _addProperty.ImagePath5,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
            }
            _appdbContext.Properties.Add(property);
            var result = _appdbContext.SaveChanges() > 0;
            return Ok(result);

        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, EditProperty _property)
        {
            var property = _appdbContext.Properties.Where(x => x.PropertyId == id ).FirstOrDefault();

            if (property == null)
            {
                return NotFound("Id Not Found");
            }
            else
            {
                property.HostId = _property.HostId;
                property.Title = _property.Title;
                property.Description = _property.Description;
                property.IsActive = _property.IsActive;
                property.Address = _property.Address;
                property.City = _property.City;
                property.Country = _property.Country;
                property.State = _property.State;
                property.ZipCode = _property.ZipCode;
                property.PropertyType = _property.PropertyType;
                property.PricePerNight = _property.PricePerNight;
                property.AvailabilityCalendar = _property.AvailabilityCalendar;
                property.HouseRules = _property.HouseRules;
                property.InstantBooking = _property.InstantBooking;
                property.ImagePath = _property.ImagePath;
                property.ImagePath2 = _property.ImagePath2;
                property.ImagePath3 = _property.ImagePath3;
                property.ImagePath4 = _property.ImagePath4;
                property.ImagePath5 = _property.ImagePath5;
                property.UpdatedAt = DateTime.Now;
            }

            _appdbContext.Properties.Update(property);
            if (_appdbContext.SaveChanges() > 0)
                return Ok(property);
            else
                return StatusCode(500, "A problem happened while handling your request.");
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var property = _appdbContext.Properties.FirstOrDefault(x => x.PropertyId == id && x.IsActive);
            if (property != null)
            {
                property.IsActive = false;
                _appdbContext.SaveChanges();
                return Ok("Done");
            }
            else
            {
                return BadRequest("No Id Found");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages(List<IFormFile> File)
        {
            if (File == null || File.Count != 5)
            {
                return BadRequest("Please upload exactly 5 images.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PropertyImg");
            var imagePaths = new List<string>();

            foreach (var file in File)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imagePaths.Add(file.FileName);
                }
            }

            return Ok(new { ImagePaths = imagePaths });
        }

    }
}
