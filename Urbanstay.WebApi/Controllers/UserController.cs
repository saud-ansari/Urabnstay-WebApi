using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Urbanstay.WebApi.Models;
using Urbanstay.WebApi.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Urbanstay.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UrbanstayContext dbContext;

        public UserController()
        {
            dbContext = new UrbanstayContext();
        }

        [HttpGet]
        public List<UsersModel> Get()
        {
            var users = dbContext.Users
                .Where(x => x.IsActive)
                .Select(x => new UsersModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username,
                    MobileNo = x.MobileNo,
                    Email = x.Email,
                    Role = x.Role.Name,
                    ProfilePic = x.ProfilePic,
                    SysAdmin = (x.SysAdmin.HasValue && x.SysAdmin.Value ? "Yes" : "No"),
                    IsActive = x.IsActive,
                    AddedDate = x.AddedDate,
                    AddedBy = x.AddedBy.FirstName + " " + x.AddedBy.LastName,
                    ModifiedDate = x.ModifiedDate,
                    ModifiedBy = x.ModifiedBy.FirstName + "" + x.ModifiedBy.LastName
                }).ToList();
            return users;
        }

        [HttpGet("{id:int}")]
        public UserModel GetById(int id)
        {
            var user = dbContext.Users
                .Where(x => x.Id == id)
                .Select(x => new UserModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username,
                    MobileNo = x.MobileNo,
                    Email = x.Email,
                    RoleId = x.RoleId,
                    ProfilePic = x.ProfilePic,
                    SysAdmin = x.SysAdmin,
                    IsActive = x.IsActive
                }).FirstOrDefault();
            return user;
        }

        [HttpPost]
        public bool Post(UserModel userModel)
        {
            var user = dbContext.Users.Where(x => x.Username == userModel.Username).FirstOrDefault();

            if (user != null)
                return false;
            else
            {
                user = new User
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Username = userModel.Username,
                    Password = userModel.Password,
                    MobileNo = userModel.MobileNo,
                    Email = userModel.Email,
                    RoleId = userModel.RoleId,
                    ProfilePic = userModel.ProfilePic,
                    SysAdmin = userModel.SysAdmin,
                    IsActive = true,
                    AddedDate = DateTime.Now,
                    AddedById = 1
                };

                dbContext.Users.Add(user);
                return dbContext.SaveChanges() > 0;
            }
        }

        [HttpPut("{id:int}")]
        public bool Put(int id, UserModel userModel)
        {
            var user = dbContext.Users.Where(x => x.Id == id).FirstOrDefault();

            if (user == null)
                return false;
            else
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.MobileNo = userModel.MobileNo;
                user.Email = userModel.Email;
                user.RoleId = userModel.RoleId;
                user.ProfilePic = userModel.ProfilePic;
                user.SysAdmin = userModel.SysAdmin;
                user.IsActive = userModel.IsActive;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedById = 1;

                dbContext.Users.Update(user);
                return dbContext.SaveChanges() > 0;
            }
        }

        [HttpDelete("{id:int}")]
        public bool Delete(int id)
        {
            var user = dbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
                return false;
            else
            {
                user.IsActive = false;
                return dbContext.SaveChanges() > 0;
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

        [HttpPost("Reviews")]
        public IActionResult Post(ReviewResponse review)
        {
            var user = dbContext.Users.Find(review.ReviewerId);
            var property = dbContext.Properties.Find(review.PropertyId);

            if (user == null || property == null)
                return NotFound("User or Property not found");

            var newReview = new Review
            {
                PropertyId = review.PropertyId,
                ReviewerId = review.ReviewerId,
                Comment = review.Comment,
                CreatedAt = DateTime.Now
            };
            dbContext.Reviews.Add(newReview);
            var result = dbContext.SaveChanges() > 0;
            return Ok(result);  
        }
    }
}
