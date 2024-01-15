using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedinaApi.Data;
using MedinaApi.Models;
using MedinaApi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ChronicDiasesController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;

        public ChronicDiasesController(Medina_Api_DbContext context)
        {
            _context = context;
        }

        // GET: api/ChronicDiases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChronicDiases>>> GetChronicDiases()
        {
            if (_context.ChronicDiases == null)
            {
                return NotFound();
            }
            return await _context.ChronicDiases.ToListAsync();
        }

        // GET: api/ChronicDiases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChronicDiases>> GetChronicDiases(int id)
        {
            if (_context.ChronicDiases == null)
            {
                return NotFound();
            }
            var chronicDiases = await _context.ChronicDiases.FindAsync(id);

            if (chronicDiases == null)
            {
                return NotFound();
            }

            return chronicDiases;
        }

        // PUT: api/ChronicDiases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutChronicDiases(UpdateChronicDiasesDto updateChronicDiasesDto)
        {

            try
            {
                var d = await _context.ChronicDiases.FirstOrDefaultAsync(c => c.Id == updateChronicDiasesDto.Id);
                if (d == null)
                {
                    return NotFound("not found");
                }
                d.DiasesName = updateChronicDiasesDto.DiasesName;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return Ok();
        }

        // POST: api/ChronicDiases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChronicDiases>> PostChronicDiases(AddChronicDiasesDto chronicDiases)
        {
            if (_context.ChronicDiases == null)
            {
                return Problem("Entity set 'Medina_Api_DbContext.ChronicDiases'  is null.");
            }
            _context.ChronicDiases.Add(new ChronicDiases
            {
                DiasesName = chronicDiases.DiasesName
            });
            await _context.SaveChangesAsync();

            return Ok("Created");
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<ChronicDiases>> Test()
        {
            var patient = new Patient()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                FamilyPhoneNumber = "123456789",
                Deceased = false,
                CountryCode = "964",
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "1",
                PassportId = "1",
                PhoneNumber = "123456789",
                SecondName = "2",
                ThirdName = "3",
            };
            var hospital = new Hospital
            {
                HospitalName = "1"
            };
            var dseases = new List<ChronicDiases>()
                        {
            new() {
                DiasesName="1"
            },new() {
                DiasesName="2"
            },new() {
                DiasesName="3"
            },new() {
                DiasesName="4"
            },new() {
                DiasesName="5"
            },new() {
                DiasesName="6"
            },new() {
                DiasesName="7"
            },new() {
                DiasesName="8"
            },new() {
                DiasesName="9"
            },
            };
            var doctor = new List<Doctors>
            {
                new Doctors()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                Deceased = false,
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "1",
                SecondName = "2",
                ThirdName = "3",
            },new Doctors()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                Deceased = false,
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "2",
                SecondName = "2",
                ThirdName = "3",
            },new Doctors()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                Deceased = false,
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "3",
                SecondName = "2",
                ThirdName = "3",
            },new Doctors()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                Deceased = false,
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "4",
                SecondName = "2",
                ThirdName = "3",
            },new Doctors()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                Deceased = false,
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "5",
                SecondName = "2",
                ThirdName = "3",
            },new Doctors()
            {
                BirthDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                GuidKey = Guid.NewGuid(),
                Deceased = false,
                FirstName = "1",
                Gender = 1,
                IsActive = true,
                NationalCardId = "6",
                SecondName = "2",
                ThirdName = "3",
            },
            };
            await _context.Patients.AddAsync(patient);
            await _context.Doctors.AddRangeAsync(doctor);
            await _context.Hospital.AddAsync(hospital);
            await _context.ChronicDiases.AddRangeAsync(dseases);
            await _context.SaveChangesAsync();

            await _context.PatientChronicDiases.AddRangeAsync(await _context.ChronicDiases.Select(c => new PatientChronicDiases
            {
                PatientId = patient.Id,
                ChronicDiaseId = c.Id,
            }).ToListAsync());
            await _context.PatientDoctorVisits.AddRangeAsync(await _context.Doctors.Select(c => new PatientDoctorVisits
            {
                PatientId = patient.Id,
                DoctorId = c.Id,
                HospitalId = hospital.Id,
            }).ToListAsync());
            return Ok("Created");
        }
        //[HttpPost("[action]")]
        //public async Task<ActionResult<ChronicDiases>> AddChronicDiasesToPatient(AddChronicDiasesDto chronicDiases)
        //{
        //    if (_context.ChronicDiases == null)
        //    {
        //        return Problem("Entity set 'Medina_Api_DbContext.ChronicDiases'  is null.");
        //    }
        //    _context.ChronicDiases.Add(new ChronicDiases
        //    {
        //        DiasesName = chronicDiases.DiasesName
        //    });
        //    await _context.SaveChangesAsync();

        //    return Ok("Created");
        //}
        //// DELETE: api/ChronicDiases/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteChronicDiases(int id)
        //{
        //    if (_context.ChronicDiases == null)
        //    {
        //        return NotFound();
        //    }
        //    var chronicDiases = await _context.ChronicDiases.FindAsync(id);
        //    if (chronicDiases == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ChronicDiases.Remove(chronicDiases);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
