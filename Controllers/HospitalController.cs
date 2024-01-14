using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;

        public HospitalController(Medina_Api_DbContext context)
        {
            _context = context;
        }

        // GET: api/Hospital
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospital()
        {
            if (_context.Hospital == null)
            {
                return NotFound();
            }
            return await _context.Hospital.ToListAsync();
        }

        // GET: api/Hospital/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
            if (_context.Hospital == null)
            {
                return NotFound();
            }
            var Hospital = await _context.Hospital.FindAsync(id);

            if (Hospital == null)
            {
                return NotFound();
            }

            return Hospital;
        }

        // PUT: api/Hospital/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutHospital(UpdateHospitalDto updateHospitalDto)
        {

            try
            {
                var d = await _context.Hospital.FirstOrDefaultAsync(c => c.Id == updateHospitalDto.Id);
                if (d == null)
                {
                    return NotFound("not found");
                }
                d.HospitalName = updateHospitalDto.HospitalName;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return Ok();
        }

        // POST: api/Hospital
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hospital>> PostHospital(AddHospitalDto Hospital)
        {
            if (_context.Hospital == null)
            {
                return Problem("Entity set 'Medina_Api_DbContext.Hospital'  is null.");
            }
            _context.Hospital.Add(new Hospital
            {
                HospitalName = Hospital.HospitalName
            });
            await _context.SaveChangesAsync();

            return Ok("Created");
        }

    }
}
