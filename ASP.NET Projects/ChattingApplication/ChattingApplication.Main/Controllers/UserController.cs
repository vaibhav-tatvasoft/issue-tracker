﻿using ChattingApplication.DataAccess.Repository;
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
        //[HttpGet]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    try
        //    {
        //        var users = _userRepository.GetAllUsers().ToList();
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userRepository.GetUser(u => u.id == id);
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
        public async Task<IActionResult> AddUser([FromBody] Object user)
        {
            try
            {
                if (user == null)
                    return BadRequest("User object is null.");

                User userObject = JsonSerializer.Deserialize<User>(user.ToString());
                var result = await _userRepository.GetUser(u => u.email == userObject.email);
                if(result == null)
                {
                    userObject.id = Guid.NewGuid().ToString();
                    _userRepository.AddUser(userObject);
                    return CreatedAtAction(nameof(GetUser), new { id = userObject.id }, userObject);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null || updatedUser.id != id)
                    return BadRequest("User object is null or IDs do not match.");

                var existingUser = await _userRepository.GetUser(u => u.id == id);
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userRepository.GetUser(u => u.id == id);
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
