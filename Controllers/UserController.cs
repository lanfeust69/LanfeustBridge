using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanfeustBridge.Controllers
{
    using LanfeustBridge.Models;

    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            // also handle the case where the account has been deleted (but there is still a valid cookie)
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // prevent the Identity UI from thinking the user is authenticated
                await _signInManager.SignOutAsync();
                return Unauthorized();
            }

            return Ok(new { user.Email, Name = user.DisplayName, user.Roles });
        }

        [HttpGet]
        public Task<IActionResult> GetAll()
        {
            IActionResult result = Ok(_userManager.Users.Select(user => new { user.Email, Name = user.DisplayName }));
            return Task.FromResult(result);
        }
    }
}
