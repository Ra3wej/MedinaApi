using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VaryApi.Data;
using VaryApi.DTO;
using VaryApi.Models;
using VaryApi.Services;
using VaryApi.Helpers;
using VaryApi.Models.Images;
using MedinaApi.Data;
using MedinaApi.Helpers;

namespace VaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersAuthController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;

        private readonly IConfiguration _configuration;
        public UsersAuthController(Medina_Api_DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("[Action]")]
        // [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<ActionResult<PatientLogInResponse>> LogIn([FromBody] UserLogInDto usersDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (usersDTO.PhoneNumber == null || (usersDTO.NationalCardId == null && usersDTO.PassportId == null))
            {
                return BadRequest("email or password not provided");
            }

            var user = _context.Patients.AsQueryable();
            if(usersDTO.NationalCardId != null)
            {
                user=user.Where(u => u.PhoneNumber == usersDTO.PhoneNumber&&u.NationalCardId==usersDTO.NationalCardId);
            }else if(usersDTO.PassportId != null)
            {
                user=user.Where(u => u.PhoneNumber == usersDTO.PhoneNumber&&u.PassportId==usersDTO.PassportId);
            }
            var res = await user.SingleOrDefaultAsync();

            if (res == null)
            {
                return NotFound("Patient does not exist");
            }
            if (!res.IsActive)
            {
                return NotFound("Patient record has been deactivated");

            }
            return Ok(new PatientLogInResponse
            {
                Id = res.Id,
                FirstName = res.FirstName,
                SecondName = res.SecondName,
                ThirdName = res.ThirdName,
                PhoneNumber = res.PhoneNumber,
                Token = JwtHelper.GenerateToken(id: res.GuidKey, StaticTokenRole.User, _configuration.GetValue<string>("Jwt:Key"),IsnormalUser:true,IsSuperUser:false),
            });
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
        [Authorize(Roles ="admin")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 409)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> AddPatient([FromBody] AddPatientDto usersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (usersDTO.FirstName == null || usersDTO.SecondName == null || usersDTO.ThirdName == null || usersDTO.PhoneNumber == null || usersDTO.Password == null || usersDTO.ConPassword == null)
            {
                return BadRequest("one or more field is not provided");
            }

            var res = _context.Patients.AsQueryable();
            if (usersDTO.NationalCardId != null)
            {
                res = res.Where(u => u.PhoneNumber == usersDTO.PhoneNumber && u.NationalCardId == usersDTO.NationalCardId);
            }
            else if (usersDTO.PassportId != null)
            {
                res = res.Where(u => u.PhoneNumber == usersDTO.PhoneNumber && u.PassportId == usersDTO.PassportId);
            }
            var user = await res.SingleOrDefaultAsync();

            if (user != null)
            {
                return Conflict("Patient does not exist");
            }

            if (res != null)
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
            var NewUser = new User
            {
                FirstName = usersDTO.FirstName,
                PhoneNumber = usersDTO.PhoneNumber,
                ThirdName = usersDTO.ThirdName,
                SecondName = usersDTO.SecondName,
                BirthDate = usersDTO.BirthDate,
                PasswordSalt = pass.Salt,
                IsPhoneNumberConfirmed = false,
                Gender = usersDTO.Gender,
                Password = pass.PasswordHashed,


            };

            await _context.Users.AddAsync(NewUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Id = res?.Id,
                FirstName = usersDTO.FirstName,
                SecondName = usersDTO.SecondName,
                ThirdName = usersDTO.ThirdName,
                IsPhoneVerified = false,
                Token = JwtHelper.GenerateToken(id: NewUser.Id, StaticTokenRole.User, _configuration.GetValue<string>("Jwt:Key")),

            });
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> ConfirmPhoneNumber(ConfirmUserPhoneNumberDTO confirmUserPhone)
        {
            int userId = JwtHelper.GetUserId(HttpContext);
            var user = await _context.Users.FindAsync(userId);
            if (confirmUserPhone.PhoneNumber[0] == '0')
            {
                confirmUserPhone.PhoneNumber = confirmUserPhone.PhoneNumber[1..];
            }

            if (user == null || user.PhoneNumber != confirmUserPhone.PhoneNumber)
            {
                return BadRequest("Unauthorized attemp");
            }
            bool confirmed = await FirebaseServices.PhoneNumberIsSignedIn("+964", confirmUserPhone.PhoneNumber, confirmUserPhone.FirebaseUserIdToken);
            if (confirmed)
            {
                user.IsPhoneNumberConfirmed = true;
                user.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok("user Phone Number Confirmed.");

            }
            else
            {
                return BadRequest("Cant Verify Phone number");
            }

        }

        [HttpPost("[action]")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> ChangePhoneNumber([FromBody] string NewPhoneNumber)
        {
            if (await _context.Users.AnyAsync(u => u.PhoneNumber == NewPhoneNumber))
            {
                return BadRequest("Phone Number Already Exists");
            }
            int userId = JwtHelper.GetUserId(HttpContext);
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Ok("User not found");
            }
            if (user.IsPhoneNumberConfirmed)
            {
                return Forbid("Phone number cant be updated for this user");
            }
            user.PhoneNumber = NewPhoneNumber;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("Pone number updated");
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UploadUserImage([FromForm] IFormFile file)
        {

            var userId = JwtHelper.GetUserId(HttpContext);
            var res = await FileHelper.Upload(HttpContext, StaticDirectories.UserImages);

            try
            {
                var dashboardImage = new UserImage
                {
                    UserId = userId,
                    FileName = res.Name
                };
                await _context.UserImages.AddAsync(dashboardImage);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                FileHelper.DeleteFile(res.Name, StaticDirectories.UserImages);
                return BadRequest();
                throw;
            }
            return Ok("Uploaded.");
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDTO forgetDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == forgetDto.PhoneNumber);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (!user.IsActive)
            {
                return NotFound();
            }
            if (forgetDto.PhoneNumber[0] == '0')
            {
                forgetDto.PhoneNumber = forgetDto.PhoneNumber[1..];
            }

            if ( user.PhoneNumber != forgetDto.PhoneNumber)
            {
                return BadRequest("Unauthorized attemp");
            }
            bool confirmed = await FirebaseServices.PhoneNumberIsSignedIn("+964", forgetDto.PhoneNumber, forgetDto.FirebaseUserIdToken);
            if (confirmed)
            {
                if (forgetDto.ConfirmNewPassword == forgetDto.NewPassword)
                {
                    var pass = _passwordHasher.HashPassword(forgetDto.NewPassword);

                    user.Password = pass.PasswordHashed;
                    user.PasswordSalt = pass.Salt;
                    user.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok("Password updated");
                }

                return BadRequest("password dont match.");

            }

            return BadRequest("Cant Verify Phone number");

        }

    }
}
