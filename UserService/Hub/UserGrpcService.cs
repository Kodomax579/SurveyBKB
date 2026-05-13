using Contracts.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserService.Data;
using UserService.Database;
using UserService.Feature;

namespace UserService.Service
{
    public class UserGrpcService (Feature.UserService userService, Feature.ClassService classService) : User.UserBase
    {

        public override async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
        {
            var response = new GetAllUsersResponse();

            response.Users.AddRange(await userService.GetAllUsers());

            return response;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var response = new LoginResponse();

            response.User = await userService.Login(request.Email, request.Password);

            return response;
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var response = new CreateUserResponse();

            response.Success = await userService.CreateNewUser(request.User);

            return response;
        }

        public override async Task<GetUserByEmailResponse> GetUserByEmail(GetUserByEmailRequest request, ServerCallContext context)
        {
            var response = new GetUserByEmailResponse();

            response.User = await userService.GetUserByEmail(request.Email);

            return response;
        }

        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var response = new DeleteUserResponse();

            response.Success = await userService.DeleteUser(request.Email);

            return response;
        }

        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var response = new UpdateUserResponse();

            response.Success = await userService.UpdateUser(request.User);

            return response;
        }

        public override async Task<GetAllClassesResponse> GetAllClasses(GetAllClassesRequest request, ServerCallContext context)
        {
            var response = new GetAllClassesResponse();

            response.Classes.AddRange(await classService.GetAllClasses());

            return response;
        }

        public override async Task<CreateClassResponse> CreateClass(CreateClassRequest request, ServerCallContext context)
        {
            var response = new CreateClassResponse();

            response.Success = await classService.CreateClass(request.Class);

            return response;
        }
    }
}
