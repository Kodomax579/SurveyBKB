using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.User.Models;
using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Feature.User
{
    public class LoginService(SurveyDbContext surveyDbContext)
    {
        private readonly PasswordHasher<UserModel> _passwordHasher = new();

        public async Task<UserDTO?> Login(string email, string password)
        {
            var user = await surveyDbContext.Users
                .Include(u => u.Class)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                return null;
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                ImageLink = user.ImageLink,
                Class = new DTO.ClassDTO
                {
                    Classname = user.Class.ClassName
                },
                Group = user.Group,
            };
        }
    }
}