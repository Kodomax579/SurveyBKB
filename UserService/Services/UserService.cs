using Contracts.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserService.Data;
using UserService.Database;

namespace UserService.Service
{
    public class UserGrpcService : User.UserBase
    {
        private UserContext _context;

        public UserGrpcService(UserContext userContext)
        {
            _context = userContext;
        }

        public override async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
        {
            try
            {
                var response = new GetAllUsersResponse();

                var users = await _context.Users.Include(u => u.Class).ToListAsync();

                foreach (var user in users)
                {
                    response.Users.Add(new UserMessage
                    {
                        Name = user.Name,
                        Lastname = user.Lastname,
                        Email = user.Email,
                        Group = (UserGroup)user.GroupId,
                        Class = new ClassMessage
                        {
                            Name = user.Class.ClassName
                        },
                        Password = user.PasswordHash,
                        Username = user.Username,
                    });
                }

                return response;
            }
            catch(Exception ex)
            {
                return new GetAllUsersResponse();
            }
        }

        public override async Task<GetUserByPasswordAndEmailResponse> GetUserByPasswordAndEmail(GetUserByPasswordAndEmailRequest request, ServerCallContext context)
        {
            var response = new GetUserByPasswordAndEmailResponse();

            var user = await _context.Users.Include(u => u.Class).FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);
            if (user != null)
            {
                response.User = new UserMessage
                {
                    Name = user.Name,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Group = (UserGroup)user.GroupId,
                    Class = new ClassMessage
                    {
                        Name = user.Class.ClassName
                    },
                    Password = user.PasswordHash,
                    Username = user.Username,
                };
            }

            return response;
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var response = new CreateUserResponse();
            
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.User.Email || u.Username == request.User.Username);
                if (existingUser != null)
                {
                    response.Success = false;
                    return response;
                }

                var newUser = new Data.UserModel
                {
                    Name = request.User.Name,
                    Lastname = request.User.Lastname,
                    Email = request.User.Email,
                    GroupId = (int)request.User.Group,
                    PasswordHash = request.User.Password,
                    Username = request.User.Username,
                };
                var selectedClass = await _context.Classes.FirstOrDefaultAsync(c => c.ClassName == request.User.Class.Name);
                
                if(selectedClass == null)
                {
                    response.Success = false;
                    return response;
                }
            
                newUser.Class = selectedClass;

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                return response;
            }
        }

        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var response = new DeleteUserResponse();
            
            try
            {
                var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

                if (userToDelete != null)
                {
                    _context.Users.Remove(userToDelete);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                }
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                return response;
            }
        }

        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var response = new UpdateUserResponse();

            try
            {
                var UserToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

                if(UserToUpdate == null)
                {
                    response.Success = false;
                    return response;
                }

                var selectedClass = await _context.Classes.FirstOrDefaultAsync(c => c.ClassName == request.User.Class.Name);
                if(selectedClass == null)
                {
                    response.Success = false;
                    return response;
                }

                UserToUpdate.Username = request.User.Username;
                UserToUpdate.Name = request.User.Name;
                UserToUpdate.Email = request.User.Email;
                UserToUpdate.Lastname = request.User.Lastname;
                UserToUpdate.Class.ClassName = request.User.Class.Name;
                UserToUpdate.Class.Id = selectedClass.Id;
                UserToUpdate.GroupId = (int)request.User.Group;
                UserToUpdate.Username = request.User.Username;

                _context.Users.Update(UserToUpdate);
                await _context.SaveChangesAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                return response;
            }
        }

        public override async Task<GetAllClassesResponse> GetAllClasses(GetAllClassesRequest request, ServerCallContext context)
        {
            try
            {
                var response = new GetAllClassesResponse();

                var classes = await _context.Classes.ToListAsync();

                foreach (var classModel in classes)
                {
                    response.Classes.Add(new ClassMessage
                    {
                        Name = classModel.ClassName
                    });
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GetAllClassesResponse();
            }
        }

        public override async Task<CreateClassResponse> CreateClass(CreateClassRequest request, ServerCallContext context)
        {
            try
            {
                var response = new CreateClassResponse();

                var newClass = new ClassModel()
                {
                    ClassName = request.Class.Name,
                };

                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                return new CreateClassResponse { Success = false };
            }
        }
    }
}
