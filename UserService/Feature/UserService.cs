using Contracts.Protos;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Database;

namespace UserService.Feature
{
    public class UserService (UserContext userDbContext, PasswortHandler passwordHandler)
    {
        public async Task<LoginMessageResponse> Login(string email, string password)
        {
            //Get user by email
            var user = await userDbContext.Users
                .Include(u => u.Class)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            //Check if user exists
            if (user == null)
            {
                return new LoginMessageResponse();
            }

            //verify password
            if (passwordHandler.VerifyPassword(user, user.PasswordHash, password))
            {
                return ConvertService.ConvertUserModelToLoginMessageResponse(user);
            }
            else
            {
                return new LoginMessageResponse();
            }
        }
        public async Task<List<UserMessage>> GetAllUsers()
        {
            try
            {
                var users = await userDbContext.Users
                    .Include(u => u.Class)
                    .OrderByDescending(p => p.GroupId)
                    .ToListAsync();

                return users.Select(ConvertService.ConvertUserModelToUserMessage).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return [];
            }
        }
        public async Task<bool> CreateNewUser(UserMessage userMessage)
        {
            try
            {
                if (await UserExists(userMessage.Email))
                    return false;

                var selectedClass = await GetClassByName(userMessage.Class.Name);
                if (selectedClass == null)
                    return false;

                var newUser = MapToUserModel(userMessage, selectedClass);

                var response = await userDbContext.Users.AddAsync(newUser);
                await userDbContext.SaveChangesAsync();
                if (response != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> UserExists(string email)
            => await userDbContext.Users.AnyAsync(u => u.Email == email);

        private async Task<ClassModel?> GetClassByName(string className)
        {
            return await userDbContext.Classes.FirstOrDefaultAsync(c => c.ClassName == className);
        }

        private UserModel MapToUserModel(UserMessage message, ClassModel selectedClass)
        {
            var user = new UserModel
            {
                Name = message.Name,
                Lastname = message.Lastname,
                Email = message.Email,
                GroupId = (int)message.Group,
                Class = selectedClass
            };

            user.PasswordHash = passwordHandler.HashPassword(user, message.Password);

            return user;
        }

        public async Task<UserMessage?> GetUserByEmail(string email)
        {
            try
            {
                var user = await userDbContext.Users
                    .Include(u => u.Class)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (user == null) return null;
                return ConvertService.ConvertUserModelToUserMessage(user);
            }
            catch (Exception ex)
            {
                return new UserMessage();
            }
        }   

        public async Task<bool> DeleteUser(string email)
        {
            try
            {
                var userToDelete = await userDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (userToDelete == null)
                {
                    return false;
                }

                userDbContext.Users.Remove(userToDelete);
                await userDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(UserMessage userMessage)
        {
            try
            {
                var userToUpdate = await userDbContext.Users
                    .Include(u => u.Class)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == userMessage.Email.ToLower());

                if (userToUpdate == null) return false;

                var selectedClass = await userDbContext.Classes
                    .FirstOrDefaultAsync(c => c.ClassName == userMessage.Class.Name);

                if (selectedClass == null) return false;

                userToUpdate.Name = userMessage.Name;
                userToUpdate.Lastname = userMessage.Lastname;
                userToUpdate.Email = userMessage.Email;
                userToUpdate.GroupId = (int)userMessage.Group;
                userToUpdate.Class = selectedClass;

                if (!string.IsNullOrWhiteSpace(userMessage.Password))
                {
                    userToUpdate.PasswordHash = passwordHandler.HashPassword(userToUpdate, userMessage.Password);
                }

                userDbContext.Users.Update(userToUpdate);
                await userDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}