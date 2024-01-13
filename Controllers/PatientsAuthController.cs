using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Helpers;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsAuthController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;

        private readonly IConfiguration _configuration;
        public PatientsAuthController(Medina_Api_DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("[Action]")]
        // [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 400)]
        [ProducesResponseType(typeof(ActionResult<string>),statusCode: 404)]
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
            if (usersDTO.NationalCardId != null)
            {
                user = user.Where(u => u.PhoneNumber == usersDTO.PhoneNumber && u.NationalCardId == usersDTO.NationalCardId);
            }
            else if (usersDTO.PassportId != null)
            {
                user = user.Where(u => u.PhoneNumber == usersDTO.PhoneNumber && u.PassportId == usersDTO.PassportId);
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
                Token = JwtHelper.GenerateToken(id: res.GuidKey, StaticTokenRole.User, _configuration.GetValue<string>("Jwt:Key"), IsnormalUser: true, IsSuperUser: false),
            });
        }

      
    }
}
