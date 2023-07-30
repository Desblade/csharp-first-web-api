using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            try
            {
                if (user.Email == null || user.NickName == null || user.Comments == null)
                { 
                    return BadRequest("Incorrect values");
                }

                User? existingUser = await _userRepository.GetUserByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest("This user already existed");
                }

                user.CreateData = DateTime.Now;

                User createdUser = await _userRepository.CreateUserAsync(user);

                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the user: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve users {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<User>> Get([FromQuery] string email)
        {
            try
            {
                User? user = await _userRepository.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return NotFound("User isn't found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get user {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<User>> Update([FromQuery] int id, [FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data isn't found");
                }

                User? existintUser = await _userRepository.GetUserByIdAsync(id);

                if (existintUser == null)
                {
                    return NotFound("User isn't found");
                }

                if (user.Email != null)
                {
                    existintUser.Email = user.Email;
                }

                if (user.NickName != null)
                {
                    existintUser.NickName = user.NickName;
                }

                if (user.Comments != null)
                {
                    existintUser.Comments = user.Comments;
                }

                User? updatedUser = await _userRepository.UpdateUserAsync(existintUser);

                return Ok(updatedUser);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Failed to update user {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult<User>> Delete([FromQuery] int id)
        {
            User? user = await _userRepository.DeleteUserAsync(id);

            if (user == null)
            {
                return NotFound("User isn't found");
            }

            return Ok();
        }
    }
}
