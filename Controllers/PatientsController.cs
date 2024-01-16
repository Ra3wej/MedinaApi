using MedinaApi.Data;
using MedinaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Helpers;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;
        public PatientsController(Medina_Api_DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<GetpatientDetailDto>>> GetUser()
        {

            var users = await _context.Patients.Select(u => new GetpatientDetailDto
            {
                IsActive = u.IsActive,
                Id = u.GuidKey,
                ThirdName = u.ThirdName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                CreatedAt = u.CreatedAt,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                PhoneNumber = u.PhoneNumber,
                CountryCode = u.CountryCode,
                Deceased = u.Deceased,
                NationalCardId = u.NationalCardId,
                PassportId = u.PassportId
            }).ToListAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<GetpatientDetailDto>> GetUser(Guid id)
        {
            var users = await _context.Patients.Where(u => u.GuidKey == id).Select(u => new GetpatientDetailDto
            {
                IsActive = u.IsActive,
                Id = u.GuidKey,
                ThirdName = u.ThirdName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                CreatedAt = u.CreatedAt,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                PhoneNumber = u.PhoneNumber,
                CountryCode = u.CountryCode,
                Deceased = u.Deceased,
                NationalCardId = u.NationalCardId,
                PassportId = u.PassportId
            }).FirstOrDefaultAsync();
            if (users == null)
            {
                return NotFound("User not found");
            }
            return Ok(users);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<GetpatientDetailDto>>> SearchUsers(string key)
        {
           var users = await _context.Patients.Where(c => c.PhoneNumber.Contains(key)
            || (c.NationalCardId??"").Contains(key) || (c.PassportId??"").Contains(key)
            || c.PhoneNumber.Contains(key))
                .Select(u => new GetpatientDetailDto
                {
                    IsActive = u.IsActive,
                    Id = u.GuidKey,
                    ThirdName = u.ThirdName,
                    FirstName = u.FirstName,
                    SecondName = u.SecondName,
                    CreatedAt = u.CreatedAt,
                    BirthDate = u.BirthDate,
                    Gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    CountryCode = u.CountryCode,
                    Deceased = u.Deceased,
                    NationalCardId = u.NationalCardId,
                    PassportId = u.PassportId
                }).ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 409)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 400)]
        public async Task<ActionResult<Guid>> AddPatient([FromBody] AddPatientDto usersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (usersDTO.FirstName == null || usersDTO.SecondName == null || usersDTO.ThirdName == null || usersDTO.PhoneNumber == null)
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
                return Conflict("Patient exists");
            }


            var NewUser = new Patient
            {
                NationalCardId = usersDTO.NationalCardId,
                CountryCode = usersDTO.CountryCode,
                FamilyPhoneNumber = usersDTO.FamilyPhoneNumber,
                FirstName = usersDTO.FirstName,
                PassportId = usersDTO.PassportId,
                PhoneNumber = usersDTO.PhoneNumber,
                ThirdName = usersDTO.ThirdName,
                SecondName = usersDTO.SecondName,
                BirthDate = usersDTO.BirthDate,
                Gender = usersDTO.Gender,
            };

            await _context.Patients.AddAsync(NewUser);
            await _context.SaveChangesAsync();

            return Ok(NewUser.GuidKey);
        }




        [HttpPut("[action]")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 404)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> UpdatePatientInfo([FromBody] UpdateUserDTO updateUser)
        {
            var user = await _context.Patients.SingleOrDefaultAsync(u => u.GuidKey == updateUser.Id);

            if (user == null)
            {
                return NotFound("Patient not found");
            }
            user.BirthDate = updateUser.BirthDate;
            user.FamilyPhoneNumber = updateUser.FamilyPhoneNumber;
            user.FirstName = updateUser.FirstName;
            user.PhoneNumber = updateUser.PhoneNumber;
            user.CountryCode = updateUser.CountryCode;
            user.Gender = updateUser.Gender;
            user.Deceased = updateUser.Deceased;
            user.PassportId = updateUser.PassportId;
            user.IsActive = updateUser.IsActive;
            user.SecondName = updateUser.SecondName;
            user.ThirdName = updateUser.ThirdName;
            await _context.SaveChangesAsync();

            return Ok("Updated");
        }

    }
}
