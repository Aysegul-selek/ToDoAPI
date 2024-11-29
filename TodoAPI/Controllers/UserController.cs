﻿using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Business.Concrete;
using ToDoAPI.Entities.DTOs;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users); // Verileri OK (200) ile döndürüyoruz
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Eğer user bulunamazsa 404 döndür
            }
            return Ok(user); // Bulunursa 200 ile döndür
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User verisi geçersiz.");
            }

            var createdUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser); // 201 döner
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest("User ID'leri eşleşmiyor.");
            }

            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null)
            {
                return NotFound(); // Eğer güncellenecek user bulunamazsa 404 döner
            }

            return NoContent(); // Başarıyla güncellendi, 204 döner
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound(); // Silinecek user bulunamazsa 404 döner
            }

            return NoContent(); // Başarıyla silindi, 204 döner
        }
    }
}
