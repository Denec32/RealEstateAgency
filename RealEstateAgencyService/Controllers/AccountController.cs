using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;

namespace RealEstateAgencyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager; 
        }

        [HttpPost]
        public async Task<ActionResult<RegisterViewModel>> RegisterUser(RegisterViewModel model)
        {
            User user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                return Ok(model);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
