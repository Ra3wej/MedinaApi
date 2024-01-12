using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Helpers;
using MedinaApi.Models;
using MedinaApi.Services;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardUsersAuthController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public DashboardUsersAuthController(Medina_Api_DbContext context, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("[Action]")]
        // [Authorize]
        [AllowAnonymous]
        public IActionResult LogIn([FromBody] DashboardUserLogInDTO usersDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (usersDTO.UserName == null || usersDTO.Password == null)
            {
                return BadRequest("username or password not provided");
            }

            var user = _context.DashboardUsers.SingleOrDefault(u => u.UserName == usersDTO.UserName && u.IsActive);

            if (user == null)
            {
                return NotFound("user does not exist");

            }

            if (_passwordHasher.VerifyPassword(usersDTO.Password, user.Password, user.Salt))
            {
                return Ok(new
                {
                    Username = user.UserName,
                    Token = JwtHelper.GenerateToken(id: user.Id, StaticTokenRole.Admin, _configuration.GetValue<string>("Jwt:Key"), IsSuperUser: user.IsSuperUser),
                });
            }
            else
            {
              return BadRequest("username or password is wrong");
            }
            //    if (username==null) { 
            //    return NotFound();
            //    }
            //    return Ok(new
            //{   a=username,
            //    V = 123,
            //    d = JwtManager.GenerateToken(username: "ad"),
            //});
        }

        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SignUp([FromBody] DashboardUserSignUpDTO usersDTO)
        {
            if (!HttpContext.IsSuperUser())
            {
                return BadRequest("Needs to be a super User");
            }

            if (usersDTO.UserName == null || usersDTO.EmailAddress == null || usersDTO.Password == null || usersDTO.ConPassword == null)
            {
                return BadRequest("one or more field is not provided");
            }

            var user = _context.DashboardUsers.SingleOrDefault(u => u.EmailAddress == usersDTO.EmailAddress);

            if (user != null)
            {
                return BadRequest(new
                {
                    message = "email or phone number is already taken"
                });
            }

            if (usersDTO.Password != usersDTO.ConPassword)
            {
                return BadRequest("passwords dont match.");
            }
            var pass = _passwordHasher.HashPassword(usersDTO.Password);
            var NewUser = new DashboardUser
            {
                EmailAddress = usersDTO.EmailAddress,
                UserName = usersDTO.UserName,
                Password = pass.PasswordHashed,
                Salt = pass.Salt,
            };

            await _context.DashboardUsers.AddAsync(NewUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                userName = usersDTO.UserName,
                Token = JwtHelper.GenerateToken(id: NewUser.Id, StaticTokenRole.Admin, _configuration.GetValue<string>("Jwt:Key"), IsSuperUser: NewUser.IsSuperUser),
                ImageUrl = "https://i.pinimg.com/564x/55/14/32/5514321d87da3de324cbf95350c6aade.jpg"
            });
        }

        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ResetPasswordForUsers([FromBody] DashboardUserResetPasswordDto usersDTO)
        {

            if (!HttpContext.IsSuperUser())
            {
                return BadRequest("Needs to be a super User");
            }
            var user = _context.DashboardUsers.SingleOrDefault(u => u.UserName == usersDTO.UserName);

            if (user == null)
            {
                return NotFound("user does not exist");
            }

            var pass = _passwordHasher.HashPassword("123456789");
            user.Password = pass.PasswordHashed;
            user.Salt = pass.Salt;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePasswordForUser([FromBody] DashboardUserChangePasswordDto usersDTO)
        {

            var user =await _context.DashboardUsers.FindAsync(HttpContext.GetUserId());

            if (user == null)
            {
                return NotFound("user does not exist");
            }
            if (_passwordHasher.VerifyPassword(usersDTO.OldPassword, user.Password, user.Salt))
            {
                if (usersDTO.ConPassword == usersDTO.NewPassword)
                {
                    var pass = _passwordHasher.HashPassword(usersDTO.NewPassword);
                    user.Password = pass.PasswordHashed;
                    user.Salt = pass.Salt;
                }
                else
                {
                    return BadRequest("Passwords don`t match");
                }
            }
            else
            {
                return BadRequest("Old password is wrong");
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        //[HttpPost("[action]")]
        //public async Task<IActionResult> UploadDashboardUserImage([FromForm] IFormFile file) {

        //    var userId = JwtHelper.GetUserId(HttpContext);
        //    var res =await FileHelper.Upload(HttpContext, Statics.DashboardUserImages);

        //    try
        //    {
        //        var dashboardImage = new DashboardUserImage
        //        {
        //            DashboardUserId = userId,
        //            FileName = res.Name
        //        };
        //       await _context.DashboardUserImages.AddAsync(dashboardImage);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        FileHelper.DeleteFile(res.Name,Statics.DashboardUserImages);

        //        throw;
        //    }
        //    return Ok("Uploaded.");
        //}

    }
}
