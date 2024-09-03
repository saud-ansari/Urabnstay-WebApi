using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
                x.Title,
                x.Description,
                x.ImagePath,
                x.IsActive,
                x.Address,
                x.City,
                x.Country,
                x.ZipCode,
                x.PropertyType,
                x.AvailabilityCalendar,
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

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _appdbContext.Properties.Where(x =>x.PropertyId == id).Select(x => new
            {
                x.PropertyId,
                x.HostId,
                x.Title,
                x.Description,
                x.ImagePath,
                x.IsActive,
                x.Address,
                x.City,
                x.Country,
                x.ZipCode,
                x.PropertyType,
                x.AvailabilityCalendar,
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
                property = new Property();
                property.HostId = _addProperty.HostId;
                property.Title = _addProperty.Title;
                property.Description = _addProperty.Description;
                property.IsActive = _addProperty.IsActive;
                property.Address = _addProperty.Address;
                property.City = _addProperty.City;
                property.Country = _addProperty.Country;
                property.State = _addProperty.State;
                property.ZipCode = _addProperty.ZipCode;
                property.PropertyType = _addProperty.PropertyType;
                property.PricePerNight = _addProperty.PricePerNight;
                property.AvailabilityCalendar = _addProperty.AvailabilityCalendar;
                property.HouseRules = _addProperty.HouseRules;
                property.InstantBooking = _addProperty.InstantBooking;
                property.ImagePath = _addProperty.ImagePath;
                property.CreatedAt = DateTime.Now;
                property.UpdatedAt = null;
            }
            _appdbContext.Properties.Add(property);
            var result = _appdbContext.SaveChanges() > 0;
            return Ok(result);

        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, EditProperty _property)
        {
            var property = _appdbContext.Properties.Where(x => x.PropertyId == id && x.IsActive).FirstOrDefault();

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
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfilePic");
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { ImagePath = file.FileName });
        }   
    }
}
