using Contracts.Protos;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Database;

namespace UserService.Feature
{
    public class UserService (UserContext userDbContext)
    {
        public async Task<List<UserMessage>> GetAllUser()
        {
            try
            {
                var userList = new List<UserMessage>();

                var users = await userDbContext.Users
                    .Include(u => u.Class)
                    .OrderBy(p => p.Username)
                    .ToListAsync();

                foreach (var user in users)
                {
                    userList.Add(ConvertService.ConvertUserModelToUserMessage(user));
                }

                return userList;
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        public async Task<UserMessage> GetUserById(int id)
        {
            try
            {
                var user = await userDbContext.Users.Include(u => u.Class).FirstOrDefaultAsync(u => u.Id == id);

                if (user != null)
                {
                    return ConvertService.ConvertUserModelToUserMessage(user);
                }
                return new(); ;
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        public async Task<UserMessage> Login(string email, string password)
        {
            try
            {
                var user = await userDbContext.Users.Include(u => u.Class).FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
                if (user != null)
                {
                    return ConvertService.ConvertUserModelToUserMessage(user);
                }
                return new();
            }
            catch(Exception ex)
            {
                return new();
            }
        }

        public async Task<bool> CreateNewUser(UserMessage userMessage)
        {
            try
            {
                var existingUser = await userDbContext.Users.FirstOrDefaultAsync(u => u.Email == userMessage.Email || u.Username == userMessage.Username);
                if (existingUser != null)
                {
                    return false;
                }

                var newUser = ConvertService.ConvertUserModelToUserMessage(userMessage);
                var selectedClass = await userDbContext.Classes.FirstOrDefaultAsync(c => c.ClassName == newUser.Class.ClassName);

                if (selectedClass == null)
                {
                    return false;
                }

                newUser.Class = selectedClass;

                await userDbContext.Users.AddAsync(newUser);
                await userDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                var userToDelete = await userDbContext
                    .Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

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

        public async Task<bool> UpdateUser(UserMessage userMessage, int userId)
        {
            try
            {
                var userToUpdate = await userDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var selectedClass = await userDbContext.Classes.FirstOrDefaultAsync(p => p.ClassName == userToUpdate.Class.ClassName);

                if (userToUpdate == null || selectedClass == null)
                {
                    return false;
                }

                userToUpdate.Username = userMessage.Username;
                userToUpdate.Name = userMessage.Name;
                userToUpdate.Email = userMessage.Email;
                userToUpdate.Lastname = userMessage.Lastname;
                userToUpdate.Class.ClassName = userMessage.Class.Name;
                userToUpdate.Class.Id = selectedClass.Id;
                userToUpdate.GroupId = (int)userMessage.Group;
                userToUpdate.Username = userMessage.Username;

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