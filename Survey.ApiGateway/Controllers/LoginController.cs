using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Feature.Email;
using Survey.ApiGateway.Feature.User;
using Survey.ApiGateway.Feature.User.DTO;
using Survey.ApiGateway.Models;
using Survey.ApiGateway.Models.DTO;
using Survey.ApiGateway.Services;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(LoginService loginService, AuthService authService, EmailService emailService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            try
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
            catch (Exception ex)
            {
                // HIER IST DER TRICK: Wir schicken die echte Fehlermeldung an Swagger zurück!
                return StatusCode(500, $"Backend-Absturz: {ex.Message} -> {ex.InnerException?.Message}");
            }
        }

        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return BadRequest("Email should not be null or empty.");    
            }

            emailService.CreatePasswordResetEmail(email);

            return Ok(email);
        }
    }
}
