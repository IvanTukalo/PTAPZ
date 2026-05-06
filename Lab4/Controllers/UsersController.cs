using Lab4.DTOs;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) { _userService = userService; }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var newUser = await _userService.RegisterUserAsync(dto);
            return Ok(newUser);
        }
    }
}