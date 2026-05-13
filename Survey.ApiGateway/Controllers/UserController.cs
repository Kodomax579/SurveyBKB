using Contracts.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
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
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var grpcRequest = new GetAllUsersRequest();

            var grpcResponse = await _grpcClient.GetAllUsersAsync(grpcRequest);
            return Ok(grpcResponse.Users);
        }


        [HttpPost("CreateUser")]
        [Authorize]
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
                    Group = (UserGroup)model.Group
                }
            };
            var grpcResponse = await _grpcClient.CreateUserAsync(grpcRequest);

            return Ok(grpcResponse.Success);
        }

        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
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
                    Group = (UserGroup)user.Group
                },
            };
            var grpcResponse = await _grpcClient.UpdateUserAsync(grpcRequest); 
            return Ok(grpcResponse.Success);
        }

        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var grpcRequest = new DeleteUserRequest() { Email = email };
            var grpcResponse = await _grpcClient.DeleteUserAsync(grpcRequest);
            return Ok(grpcResponse.Success);
        }

        [HttpGet("GetAllClasses")]
        [Authorize]
        public async Task<IActionResult> GetAllClasses()
        {
            var grpcRequest = new GetAllClassesRequest();
            var grpcResponse = await _grpcClient.GetAllClassesAsync(grpcRequest);
            return Ok(grpcResponse.Classes);
        }

        [HttpPost("CreateClass")]
        [Authorize]
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
