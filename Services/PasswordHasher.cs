using MedinaApi.DTO;

namespace MedinaApi.Services
{

    public class PasswordHasher : IPasswordHasher
    {

        public bool VerifyPassword(string password, string passwordHash, string salt)
        {
            return BCrypt.Net.BCrypt.Verify(salt + password, passwordHash);

        }

        public HashedPassword HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            return new HashedPassword
            {
                PasswordHashed = BCrypt.Net.BCrypt.HashPassword(salt + password),
                Salt = salt,
            };

        }
    }
}
