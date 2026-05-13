using Contracts.Protos;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Models;
using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        private readonly User.UserClient _grpcClient;
        private readonly Services.AuthService _authService;

        public LoginController(User.UserClient grpcClient, Services.AuthService authService)
        {
            _grpcClient = grpcClient;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string email, string password)
        {
            var grpcRequest = new LoginRequest() { Email = email, Password = password };

            var grpcResponse = await _grpcClient.LoginAsync(grpcRequest);

            if (string.IsNullOrEmpty(grpcResponse.User.Email))
            {
                return Unauthorized();
            }

            var model = new UserDTO()
            {
                Email = grpcResponse.User.Email,
                Name = grpcResponse.User.Name,
                Group = grpcResponse.User.Group,
                Lastname = grpcResponse.User.Lastname,
                Class = new ClassModel()
                {
                    ClassName = grpcResponse.User.Class.Name
                }
            };

            var token = _authService.CreateToken(model);
            Response.Headers.Append("Authorization", $"Bearer {token}");

            return Ok(model);
        }
    }
}
