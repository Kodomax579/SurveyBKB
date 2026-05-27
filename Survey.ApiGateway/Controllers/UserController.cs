using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Feature.User; 
using Survey.ApiGateway.Feature.User.Models; 

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService userService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await userService.GetAllUsers());
        }

        [HttpPost]
        [Authorize]
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