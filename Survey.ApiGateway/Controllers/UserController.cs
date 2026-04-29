using Contracts.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Models;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly User.UserClient _grpcClient;
        
        public UserController(User.UserClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var grpcClient = new GetAllUsersRequest();

            var grpcResponse = _grpcClient.GetAllUsers(grpcClient);
            return Ok(new[] { "User1", "User2", "User3" });
        }

        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            // Placeholder for getting a user by ID
            return Ok($"User{id}");
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserModel model)
        {
            // Placeholder for creating a user
            return Ok($"User '{model}' created successfully.");
        }

        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel user)
        {
            // Placeholder for updating a user
            return Ok($"User '{id}' updated successfully.");
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            // Placeholder for deleting a user
            return Ok($"User '{id}' deleted successfully.");
        }
    }
}
