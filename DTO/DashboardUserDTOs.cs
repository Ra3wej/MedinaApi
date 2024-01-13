namespace MedinaApi.DTO
{
    public class DashboardUserDTOs
    {
    }
    public class GetDashboardUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; } = true;

    }
    public class DashboardUserLogInDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class DashboardUserLogInReturnDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    public class DashboardUserSignUpDTO
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConPassword { get; set; }
    }
    public class ResetDashboardUserPasswordDTO
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }

    }

    public class DashboardUserResetPasswordDto
    {
        public string UserName { get; set; }
    }
    public class DashboardUserChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConPassword { get; set; }
    }
    }
