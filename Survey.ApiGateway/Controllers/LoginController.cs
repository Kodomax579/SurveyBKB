using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Feature.User;
using Survey.ApiGateway.Feature.User.DTO;
using Survey.ApiGateway.Models;
using Survey.ApiGateway.Models.DTO;
using Survey.ApiGateway.Services;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController (LoginService loginService, AuthService authService): ControllerBase
    {
        [HttpPost]
        [AllowAnonymous] 
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(false);
            }

            var userModel = await loginService.Login(request.Email, request.Password);

            if (userModel == null)
            {
                return Unauthorized("Ungültige E-Mail oder falsches Passwort.");
            }

            var token = authService.CreateToken(userModel);

            return Ok(new
            {
                User = userModel,
                Token = token
            });
        }
    }
}
