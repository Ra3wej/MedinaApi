using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobilePatientsController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;
        public MobilePatientsController(Medina_Api_DbContext context)
        {
            _context = context;
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> GetPatientFamilyMembers()
        {
            var userId = JwtHelper.GetUserId(HttpContext);
            if (userId == null)
            {
                return Unauthorized("token not valid");
            }
            var user = await _context.Patients.SingleOrDefaultAsync(c => c.GuidKey == userId);

            if(user == null)
            {
                return NotFound("user not found");
            }
            if(user.FamilyPhoneNumber == null)
            {
                return NotFound("user does not have family members phoneNumber.");
            }
            var FamilyMembers = await _context.Patients.Where(c => c.FamilyPhoneNumber == user.FamilyPhoneNumber).Select(c => new
            {
                c.GuidKey,
                FullName = c.FirstName + c.SecondName + c.ThirdName,
            }).ToListAsync();

            return Ok(FamilyMembers);   
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<UpdateChronicDiasesDto>>> GetPatientChronicDiases()
        {
            var userId = JwtHelper.GetUserId(HttpContext);
            if (userId == null)
            {
                return Unauthorized("token not valid");
            }
            var user = await _context.Patients.Include(c=>c.PatientChronicDiases).ThenInclude(c=>c.ChronicDiase).SingleOrDefaultAsync(c => c.GuidKey == userId);

            if (user == null)
            {
                return NotFound("user not found");
            }

            return Ok(user.PatientChronicDiases.Select(c=>new UpdateChronicDiasesDto
            {
                DiasesName=c.ChronicDiase.DiasesName,
                Id=c.ChronicDiaseId
            }).ToList());
        }

    }
}
