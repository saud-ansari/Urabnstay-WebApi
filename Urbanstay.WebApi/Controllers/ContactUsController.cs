using Urbanstay.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Urbanstay.WebApi.Services;
using System;

namespace Urbanstay.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly ContactUsServices _services;

        public ContactUsController(ContactUsServices contactUsServices)
        {
            _services = contactUsServices;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactModel contactModel)
        {
            try
            {
                await _services.ContactUs(contactModel);
                return Ok(new { success = true, message = "Your message has been sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal Server Error", detail = ex.Message });
            }
        }
    }

}
