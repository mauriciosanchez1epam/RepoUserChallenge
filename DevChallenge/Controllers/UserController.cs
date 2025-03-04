using Application.DTOs;
using Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DevChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<UserDTO> _validator;
        public UserController(ILogger<UserController> logger, IUserService userService, IValidator<UserDTO> validator)
        {
            _logger = logger;
            _userService = userService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Fetching data from service.");
            var data = await _userService.GetUsers();
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation("Fetching data from service.");
            var data = await _userService.GetUserById(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation("calling the validator.");
            var validationResult = await _validator.ValidateAsync(userDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            _logger.LogInformation("creating data from service.");
            if (await _userService.CreateUser(userDTO))
                return Created();
            else
            {
                return BadRequest("Please check that the email does not exist in the database.");
            }

        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"updating user with ID: {id}");
            var validationResult = await _validator.ValidateAsync(userDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var updated = await _userService.UpdateUser(id, userDTO);

            if (updated)
                return NoContent();
            else
            { return BadRequest("Please check that the email does not exist in the database."); }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation($"Deleting user with ID: {id}");
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
