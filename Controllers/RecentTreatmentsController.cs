using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedinaApi.Data;
using MedinaApi.Models;
using MedinaApi.Helpers;
using MedinaApi.DTO;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentTreatmentsController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;

        public RecentTreatmentsController(Medina_Api_DbContext context)
        {
            _context = context;
        }

        // GET: api/RecentTreatments
        [HttpGet]
        public async Task<IActionResult> GetPatientRecentTreatments()
        {
            if (_context.RecentTreatments == null)
            {
                return NotFound();
            }
            var userId = JwtHelper.GetUserId(HttpContext);
            return Ok(await _context.RecentTreatments.Where(c => c.Patient.GuidKey == userId)
                .Include(c => c.Hospital)
                .Include(c => c.Doctor)
                .Include(c => c.ChronicDiases)
                .Select(c => new
                {
                    PatientName = c.Patient.FirstName,
                    DoctorName = c.Doctor.FirstName,
                    c.Hospital.HospitalName,
                    c.ChronicDiases.DiasesName,
                    c.CreatedAt
                }).ToListAsync());
        }

        // PUT: api/RecentTreatments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutRecentTreatments(UpdateRecentTreatmentsDTOs update)
        {
            var recent = await _context.RecentTreatments.FindAsync(update.Id);
            if (recent == null)
            {
                return NotFound();
            }
            recent.HospitalId = update.HsopitalId;
            recent.DoctorId = update.DoctorId;
            recent.PatientId = update.PatientId;
            recent.ChronicDiasesId = update.ChronicDiasesId;
            recent.CreatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/RecentTreatments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecentTreatments>> PostRecentTreatments(AddRecentTreatmentsDTOs recentTreatments)
        {
            if (_context.RecentTreatments == null)
            {
                return Problem("Entity set 'Medina_Api_DbContext.RecentTreatments'  is null.");
            }
            _context.RecentTreatments.Add(new RecentTreatments
            {
                DoctorId = recentTreatments.DoctorId,
                HospitalId = recentTreatments.HsopitalId,
                PatientId = recentTreatments.PatientId,
                ChronicDiasesId = recentTreatments.ChronicDiasesId,
                CreatedAt = DateTime.Now,
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        //// DELETE: api/RecentTreatments/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRecentTreatments(int id)
        //{
        //    if (_context.RecentTreatments == null)
        //    {
        //        return NotFound();
        //    }
        //    var recentTreatments = await _context.RecentTreatments.FindAsync(id);
        //    if (recentTreatments == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.RecentTreatments.Remove(recentTreatments);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
