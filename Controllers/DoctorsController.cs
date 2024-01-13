using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;
        public DoctorsController(Medina_Api_DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<GetDoctorDetailDto>>> GetDoctors()
        {

            var users = await _context.Doctors.Select(u => new GetDoctorDetailDto
            {
                IsActive = u.IsActive,
                Id = u.GuidKey,
                ThirdName = u.ThirdName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                CreatedAt = u.CreatedAt,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                Deceased = u.Deceased,
                NationalCardId = u.NationalCardId,
                PassportId = u.PassportId
            }).ToListAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<GetDoctorDetailDto>> GetUser(Guid id)
        {
            var users = await _context.Doctors.Where(u => u.GuidKey == id).Select(u => new GetDoctorDetailDto
            {
                IsActive = u.IsActive,
                Id = u.GuidKey,
                ThirdName = u.ThirdName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                CreatedAt = u.CreatedAt,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
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



        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 409)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 400)]
        public async Task<ActionResult<Guid>> AddDoctor([FromBody] AddDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (doctorDto.FirstName == null || doctorDto.SecondName == null || doctorDto.ThirdName == null)
            {
                return BadRequest("one or more field is not provided");
            }

            var res = _context.Doctors.AsQueryable();
            if (doctorDto.NationalCardId != null)
            {
                res = res.Where(u => u.NationalCardId == doctorDto.NationalCardId);
            }
            else if (doctorDto.PassportId != null)
            {
                res = res.Where(u => u.PassportId == doctorDto.PassportId);
            }
            var user = await res.SingleOrDefaultAsync();

            if (user != null)
            {
                return Conflict("Doctor exists");
            }


            var NewUser = new Doctors
            {
                NationalCardId = doctorDto.NationalCardId,

                FirstName = doctorDto.FirstName,
                PassportId = doctorDto.PassportId,
                ThirdName = doctorDto.ThirdName,
                SecondName = doctorDto.SecondName,
                BirthDate = doctorDto.BirthDate,
                Gender = doctorDto.Gender,
            };

            await _context.Doctors.AddAsync(NewUser);
            await _context.SaveChangesAsync();

            return Ok(NewUser.GuidKey);
        }


        [HttpPut("[action]")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(typeof(ActionResult<string>), statusCode: 404)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> UpdateDoctorInfo([FromBody] UpdateDoctorDto updateDoctor)
        {
            var user = await _context.Doctors.SingleOrDefaultAsync(u => u.GuidKey == updateDoctor.Id);

            if (user == null)
            {
                return NotFound("Doctor not found");
            }
            user.BirthDate = updateDoctor.BirthDate;
            user.FirstName = updateDoctor.FirstName;
            user.Gender = updateDoctor.Gender;
            user.Deceased = updateDoctor.Deceased;
            user.PassportId = updateDoctor.PassportId;
            user.IsActive = updateDoctor.IsActive;
            user.SecondName = updateDoctor.SecondName;
            user.ThirdName = updateDoctor.ThirdName;
            await _context.SaveChangesAsync();
            return Ok("Updated");
        }


    }
}
