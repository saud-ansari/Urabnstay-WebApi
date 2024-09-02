using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Urbanstay.WebApi.Models;
using Urbanstay.WebApi.ViewModels;


namespace Urbanstay.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UrbanstayContext dbContext;

        public AuthController()
        {
            dbContext = new UrbanstayContext();
        }

        [HttpPost("login")]
        public LoginResponseModel Post(LoginModel loginModel)
        {
            var user = dbContext.Users
                .Include(x => x.Role)
                .Where(x => x.Username == loginModel.Username && x.Password == loginModel.Password && x.IsActive)
                .FirstOrDefault();

            if (user == null)
                return null;
            else
            {
                return new LoginResponseModel
                {
                    Id = user.Id,
                    UserName = user.Username,
                    UserRole = user.Role.Name,
                    ProfilePic = user.ProfilePic
                };
            }
        }
    }
}
