using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaryApi.Data;
using VaryApi.DTO;
using VaryApi.Helpers;
using VaryApi.Models.Images;
using VaryApi.Services;

namespace VaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Vary_Api_DbContext _context;
        private readonly IFirebaseServices _FirebaseServices;
        public UsersController(Vary_Api_DbContext context, IFirebaseServices firebaseServices)
        {
            _context = context;
            _FirebaseServices = firebaseServices;
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetUser()
        {

            var users = await _context.Users.Include(u => u.UserImage).Select(u => new GetNormalUserDTO
            {
                IsActive = u.IsActive,
                Id = u.Id,
                ThirdName = u.ThirdName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                UserImage = UrlHelper.GetUrl(HttpContext, StaticDirectories.UserImages, u.UserImage.FileName ?? ""),
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                IsPhoneNumberConfirmed = u.IsPhoneNumberConfirmed,
                PhoneNumber = u.PhoneNumber
            }).ToListAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            var users = await _context.Users.Where(u => u.Id == id).Include(u => u.UserImage).Select(u => new GetNormalUserDTO
            {
                IsActive = u.IsActive,
                Id = u.Id,
                ThirdName = u.ThirdName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                UserImage = UrlHelper.GetUrl(HttpContext, StaticDirectories.UserImages, u.UserImage.FileName ?? ""),
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                IsPhoneNumberConfirmed = u.IsPhoneNumberConfirmed,
                PhoneNumber = u.PhoneNumber
            }).FirstOrDefaultAsync();
            return Ok(users);
        }

        [HttpPost("uploadUserProfile")]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> UploadUserPorifle([FromForm] UploadImageDTO uploadImage)
        {

            int userId = JwtHelper.GetUserId(HttpContext);
            var res = await FileHelper.Upload(HttpContext, StaticDirectories.UserImages);
            var user = await _context.Users.Include(d => d.UserImage).SingleOrDefaultAsync(d => d.Id == userId);

          using  var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {

                if (res.WasEmpty)
                {
                    return BadRequest("Please Upload A picture");
                }
                if (!res.Uploaded)
                {
                    return BadRequest("upload failed");
                }
                if (!res.WasEmpty)
                {
                    FileHelper.DeleteFile(user.UserImage.FileName, StaticDirectories.UserImages);

                    user.UserImage.FileName = res.Name;
                }

                await _context.UserImages.AddAsync(new UserImage
                {
                    UserId = userId,
                    FileName = res.Name
                });
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

            }
            catch (Exception)
            {
                FileHelper.DeleteFile(res.Name, StaticDirectories.UserImages);

                await dbTransaction.RollbackAsync();

                throw;
            }
            return Ok(UrlHelper.GetUrl(HttpContext, StaticDirectories.UserImages, res.Name));
        }

        [HttpDelete]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteUser()
        {
            int id = JwtHelper.GetUserId(HttpContext);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            user.IsActive = false;
            user.PhoneNumber += "----1";
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            } 
            user.IsActive = !user.IsActive;

            await _context.SaveChangesAsync();
            return Ok("Updated");
        }

        //public async Task<IActionResult> VerifyUser([FromBody] VerifyUserDTO verifyUser)
        //{
        //    int userId = JwtHelper.GetUserId(HttpContext);
        //    var user = await _context.Users.FindAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound("User Not Found");
        //    }
        //    if (user.IsPhoneNumberConfirmed)
        //    {
        //        return Conflict("Already Verfied");
        //    }
        //   bool verfied= await _FirebaseServices.PhoneNumberIsSignedIn("+964",user.PhoneNumber,verifyUser.FirebaseUserIdToken);

        //    if (verfied)
        //    {
        //        user.IsPhoneNumberConfirmed = true;
        //       await _context.SaveChangesAsync();
        //    return Ok("User verfied");

        //    }
        //    else
        //    {
        //        return BadRequest("cant verify phone number");
        //    }
        //}
    }
}
