using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.User.Models;

namespace Survey.ApiGateway.Feature.User
{
    public class UserService(SurveyDbContext dbContext)
    {
        private readonly PasswordHasher<UserModel> _passwordHasher = new();

        public async Task<ClassModel?> CreateClass(ClassModel newClass)
        {
            var exists = await dbContext.Classes.AnyAsync(c => c.ClassName.ToLower() == newClass.ClassName.ToLower());
            if (exists)
            {
                return null;
            }

            await dbContext.Classes.AddAsync(newClass);
            await dbContext.SaveChangesAsync();
            return newClass;
        }

        public async Task<List<ClassModel>> GetAllClasses()
        {
            return await dbContext.Classes.OrderBy(c => c.ClassName).ToListAsync();
        }

        public async Task<UserModel?> CreateUser(UserModel user)
        {
            var emailExists = await dbContext.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());
            if (emailExists)
            {
                return null;
            }

            var classExists = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassName.ToLower() == user.Class.ClassName.ToLower());
            if (classExists == null)
            {
                return null;
            }
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            user.Class = classExists;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            user.Password = string.Empty;
            return user;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await dbContext.Users
                .Include(u => u.Class) 
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Email = u.Email,
                    ClassId = u.ClassId,
                    Class = u.Class,
                    Group = u.Group,
                    Password = ""
                })
                .ToListAsync();
        }

        public async Task<UserModel?> GetUserById(int id)
        {
            var user = await dbContext.Users
                .Include(u => u.Class)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                user.Password = string.Empty;
            }

            return user;
        }

        public async Task<UserModel?> UpdateUser(int id, UserModel userUpdate)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser == null)
            { 
                return null; 
            }

            existingUser.Firstname = userUpdate.Firstname;
            existingUser.Lastname = userUpdate.Lastname;
            existingUser.Group = userUpdate.Group;

            if (existingUser.ClassId != userUpdate.ClassId)
            {
                var classExists = await dbContext.Classes.AnyAsync(c => c.Id == userUpdate.ClassId);
                if (classExists)
                {
                    existingUser.ClassId = userUpdate.ClassId;
                }
            }

            if (!string.IsNullOrWhiteSpace(userUpdate.Password))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, userUpdate.Password);
            }

            try
            {
                await dbContext.SaveChangesAsync();
                existingUser.Password = string.Empty;
                return existingUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null) return false;

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null) return false; // User existiert nicht

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, currentPassword);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return false;
            }

            user.Password = _passwordHasher.HashPassword(user, newPassword);

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AdminResetPassword(int userId, string newPassword)
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null) return false; 

            user.Password = _passwordHasher.HashPassword(user, newPassword);

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}