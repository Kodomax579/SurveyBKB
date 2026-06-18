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
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController(LoginService loginService, AuthService authService, EmailService emailService, UserService userService) : ControllerBase
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
        public async Task<IActionResult> ForgotPassword([FromQuery] string email) // <- HIER [FromQuery] nutzen!
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email should not be null or empty.");
            }

            var result = await emailService.CreatePasswordResetEmail(email);

            if(result)
            {
                return Ok(email);
            }
            return BadRequest();
        }

        [HttpPost("/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    return BadRequest("Ungültige Daten übergeben.");
                }

                var erfolg = await userService.ResetPasswordByEmail(request.Email, request.NewPassword);

                if (erfolg)
                {
                    return Ok(new { Message = "Das Passwort wurde erfolgreich zurückgesetzt." });
                }

                return BadRequest("Passwort konnte nicht zurückgesetzt werden. Benutzer existiert eventuell nicht.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fehler beim Zurücksetzen: {ex.Message}");
            }
        }
    }
}
