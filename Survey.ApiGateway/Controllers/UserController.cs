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
        public async Task<IActionResult> GetAllUsers()
        {
            var grpcRequest = new GetAllUsersRequest();

            var grpcResponse = await _grpcClient.GetAllUsersAsync(grpcRequest);
            return Ok(grpcResponse.Users);
        }

        [HttpGet("GetUserByPasswordAndEmail")]
        public async Task<IActionResult> GetUserByPasswordAndEmail(string email, string password)
        {
            var grpcRequest = new GetUserByPasswordAndEmailRequest() { Email = email, Password = password };

            var grpcResponse = await _grpcClient.GetUserByPasswordAndEmailAsync(grpcRequest);
            return Ok(grpcResponse.User);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            var grpcRequest = new CreateUserRequest()
            {
                User = new UserMessage()
                {
                    Name = model.Name,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Password = model.Password,
                    Class = new ClassMessage()
                    {
                        Name = model.Class.ClassName,
                    },
                    Username = model.Username,
                    Group = (UserGroup)model.Group
                }
            };
            var grpcResponse = await _grpcClient.CreateUserAsync(grpcRequest);

            return Ok(grpcResponse.Success);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserModel user)
        {
            var grpcRequest = new UpdateUserRequest()
            {
                User = new UserMessage()
                {
                    Name = user.Name,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Password = user.Password,
                    Class = new ClassMessage()
                    {
                        Name = user.Class.ClassName,
                    },
                    Username = user.Username,
                    Group = (UserGroup)user.Group
                },
                Id = id
            };
            var grpcResponse = await _grpcClient.UpdateUserAsync(grpcRequest); 
            return Ok(grpcResponse.Success);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var grpcRequest = new DeleteUserRequest() { Id = id };
            var grpcResponse = await _grpcClient.DeleteUserAsync(grpcRequest);
            return Ok(grpcResponse.Success);
        }

        [HttpGet("GetAllClasses")]
        public async Task<IActionResult> GetAllClasses()
        {
            var grpcRequest = new GetAllClassesRequest();
            var grpcResponse = await _grpcClient.GetAllClassesAsync(grpcRequest);
            return Ok(grpcResponse.Classes);
        }

        [HttpPost("CreateClass")]
        public async Task<IActionResult> GetClassById([FromBody] ClassModel model)
        {
            var grpcRequest = new CreateClassRequest()
            {
                Class = new ClassMessage()
                {
                    Name = model.ClassName
                }
            };
            var grpcResponse = await _grpcClient.CreateClassAsync(grpcRequest);
            return Ok(grpcResponse.Success);
        }
    }
}
