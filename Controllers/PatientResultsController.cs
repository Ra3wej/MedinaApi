using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class PatientResultsController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;

        public PatientResultsController(Medina_Api_DbContext context)
        {
            _context = context;
        }

        // GET: api/PatientResults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPatientResultsDto>>> GetPatientResults()
        {
            var userId = JwtHelper.GetUserId(HttpContext);
            if (_context.PatientResults == null)
            {
                return NotFound();
            }
            var user = await _context.Patients.Where(c => c.GuidKey == userId).SingleOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            return await _context.PatientResults.Where(c => c.PatientId == user.Id).Select(c => new GetPatientResultsDto
            {
                FileUrl = c.FileUrl,
                PatientId = user.Id,
                Id = user.Id,
                CreatedAt = c.CreatedAt
            }).ToListAsync();
        }

        // GET: api/PatientResults/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PatientResults>> GetPatientResults(int id)
        //{
        //    if (_context.PatientResults == null)
        //    {
        //        return NotFound();
        //    }
        //    var patientResults = await _context.PatientResults.FindAsync(id);

        //    if (patientResults == null)
        //    {
        //        return NotFound();
        //    }

        //    return patientResults;
        //}

        // PUT: api/PatientResults/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPatientResults(PatientResults patientResults)
        //{
        //    if (id != patientResults.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(patientResults).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PatientResultsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/PatientResults
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PatientResults>> PostPatientResults(AddPatientResultsDto patientResults)
        {
            if (_context.PatientResults == null)
            {
                return Problem("Entity set 'Medina_Api_DbContext.PatientResults'  is null.");
            }
            _context.PatientResults.Add(new PatientResults
            {
                PatientId = patientResults.PatientId,
                FileUrl = patientResults.FileUrl,
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/PatientResults/5
    }
}
