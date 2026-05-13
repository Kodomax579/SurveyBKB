using Contracts.Protos;
using Microsoft.AspNetCore.Identity;
using UserService.Data;

namespace UserService.Feature
{
    public class PasswortHandler
    {
        private readonly PasswordHasher<UserModel> _hasher = new PasswordHasher<UserModel>();

        public string HashPassword(UserModel user, string plainTextPassword)
        {
            return _hasher.HashPassword(user, plainTextPassword);
        }

        public bool VerifyPassword(UserModel user, string storedHash, string inputPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, storedHash, inputPassword);

            return result switch
            {
                PasswordVerificationResult.Success => true,
                PasswordVerificationResult.SuccessRehashNeeded => true,
                PasswordVerificationResult.Failed => false,
                _ => false
            };
        }
    }
}
