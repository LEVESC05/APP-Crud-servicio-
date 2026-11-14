using Microsoft.AspNetCore.Identity;

namespace AppCrud.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }
        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                PasswordVerificationResult result =
                    _passwordHasher.VerifyHashedPassword(
                        null!,
                        passwordHash,
                        password);

                return result == PasswordVerificationResult.Success ||
                    result == PasswordVerificationResult.SuccessRehashNeeded;
            }
            catch (FormatException)
            {
                return password == passwordHash;
            }
        }
    }
}
