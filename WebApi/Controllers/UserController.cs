using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController( IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetUserId")]
        public async Task<IActionResult> GetUserIdAsync(string username)
        {
            var userId = await _userRepository.GetUserIdAsync(username);
            if (userId == null)
            {
                return NotFound("User not found");
            }

            return Ok(userId);
        }
    }
}
