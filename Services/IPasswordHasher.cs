using MedinaApi.DTO;

namespace MedinaApi.Services
{
    public interface IPasswordHasher
    {
        public HashedPassword HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash, string salt);

    }
}
