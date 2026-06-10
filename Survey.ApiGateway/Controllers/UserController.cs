using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Feature.User; 
using Survey.ApiGateway.Feature.User.Models; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService userService, IWebHostEnvironment env) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await userService.GetAllUsers());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            if (model == null)
            { 
                return BadRequest();
            }

            var createdUser = await userService.CreateUser(model);
            if (createdUser == null)
            {
                return BadRequest(false);
            }

            return Ok(createdUser);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("{id}/upload-profile-image")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Keine Datei angegeben.");

            var uploadsDir = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var saved = await userService.UploadProfileImage(id, $"/uploads/{fileName}");
            if (saved == null) return NotFound("Benutzer nicht gefunden oder konnte nicht gespeichert werden.");

            return Ok(new { ImageLink = saved });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var updatedUser = await userService.UpdateUser(id, user);
            if (updatedUser == null)
            {
                return NotFound("Benutzer nicht gefunden.");
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await userService.DeleteUser(id);
            if (!success) return NotFound();

            return Ok(true);
        }


        [HttpGet("classes")]
        [Authorize]
        public async Task<IActionResult> GetAllClasses()
        {
            return Ok(await userService.GetAllClasses());
        }

        [HttpPost("classes")]
        [Authorize]
        public async Task<IActionResult> CreateClass([FromBody] ClassModel model)
        {
            var createdClass = await userService.CreateClass(model);
            if (createdClass == null)
            {
                return BadRequest("Klasse existiert bereits.");
            }

            return Ok(createdClass);
        }
    }
}