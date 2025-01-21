using health_pal_backend.DTOs;
using health_pal_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace health_pal_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegDTO dto)
        {
            try
            {
                var token = await _userService.RegisterAsync(dto);
                _logger.LogInformation($"User registered successfully: {dto.Email}");
                return Ok(new { Token = token, Message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var token = await _userService.LoginAsync(dto);
            _logger.LogInformation($"User logged in successfully: {dto.Email}");
            return Ok(new { Token = token, Message = "User logged in successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            _logger.LogInformation("Users fetched successfully");
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            _logger.LogInformation($"User with id {id} fetched successfully");
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO dto)
        {
            await _userService.UpdateUserAsync(id, dto);
            _logger.LogInformation($"User with id {id} updated successfully");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            _logger.LogInformation($"User with id {id} deleted successfully");
            return NoContent();
        }
    }
}
