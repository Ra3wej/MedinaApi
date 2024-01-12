namespace MedinaApi.DTO
{
    public class HashedPassword
    {
        public string PasswordHashed { get; set; }
        public string Salt { get; set; }
    }
}
