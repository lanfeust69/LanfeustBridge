using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using LanfeustBridge.Models;
using LanfeustBridge.Services;

namespace LanfeustBridge.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        IUserStore<User> _userStore;
        SignInManager<User> _signInManager;

        public UserController(IUserStore<User> userStore, SignInManager<User> signInManager)
        {
            _userStore = userStore;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCurrent()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            // also handle the case where the account has been deleted (but there is still a valid cookie)
            User user = null;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null) // should probably always be true...
                user = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            if (user == null)
            {
                // prevent the Identity UI from thinking the user is authenticated
                await _signInManager.SignOutAsync();
                return Unauthorized();
            }

            return Ok(user.UserName);
        }
    }
}
