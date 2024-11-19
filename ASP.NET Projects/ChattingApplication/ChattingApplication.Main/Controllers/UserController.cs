using ChattingApplication.DataAccess.Repository;
using ChattingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChattingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers().ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _userRepository.GetUser(u => u.Id == id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/User
        [HttpPost]
        public IActionResult AddUser([FromBody] Object user)
        {
            try
            {
                if (user == null)
                    return BadRequest("User object is null.");
                var userObject = JsonSerializer.Deserialize<User>(user.ToString());
                _userRepository.AddUser(userObject);
                return CreatedAtAction(nameof(GetUser), new { id = userObject.Id }, userObject);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null || updatedUser.Id != id)
                    return BadRequest("User object is null or IDs do not match.");

                var existingUser = _userRepository.GetUser(u => u.Id == id);
                if (existingUser == null)
                    return NotFound($"User with ID {id} not found.");

                _userRepository.UpdateUser(updatedUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _userRepository.GetUser(u => u.Id == id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                _userRepository.DeleteUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
