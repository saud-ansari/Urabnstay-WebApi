using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Urbanstay.WebApi.Models;

namespace Urbanstay.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly UrbanstayContext _context;

        public DashboardController()
        {
            _context = new UrbanstayContext();
        }

        [HttpGet("/PropertyCountType")]
        public IActionResult GetPropertyTypeCounts()
        {
            var res = _context.Properties
                .GroupBy(x => x.PropertyType)
                .Select(g => new
            {
               PropertyType = g.Key,
               Count = g.Count()
            }).ToList();
            return Ok(res);
        }

        [HttpGet("/tenant")]
        public IActionResult GetTenanatCount()
        {
            var count = _context.Users.Where(x => x.RoleId == 4).Count();
            return Ok(count);
        }

        [HttpGet("/LandLordCount")]
        public IActionResult GetLandLordCount()
        {
            var count = _context.Users.Where(x => x.RoleId == 3).Count();
            return Ok(count);
        }

        [HttpGet("/PropertyCount")]
        public IActionResult GetPropertyCount()
        {
            var count = _context.Properties.Count();
            return Ok(count);
        }
    }
}
