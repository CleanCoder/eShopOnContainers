using Domain;
using Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Controllers
{
    [Authorize]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IUserService _userService;

        public AdminController(ICurrentUserService currentUser, IUserService userService)
        {
            _currentUser = currentUser;
            _userService = userService;
        }

        /// <summary>
        /// Get Users List
        /// </summary>
        /// <returns>Status 200 OK</returns>
       // [Authorize(Policy = Permissions.Users.View)]
        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var accessToken = await this.HttpContext.GetTokenAsync("access_token");

            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        /// <summary>
        /// Get User Roles By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Users.View)]
        [HttpGet("users/{id}/roles")]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            var userRoles = await _userService.GetRolesAsync(id);
            return Ok(userRoles);
        }
    }
}
