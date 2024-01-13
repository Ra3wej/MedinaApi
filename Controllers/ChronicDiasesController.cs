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
    [Authorize(Roles ="admin")]
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
                var d=await _context.ChronicDiases.FirstOrDefaultAsync(c=>c.Id==updateChronicDiasesDto.Id);
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
                DiasesName=chronicDiases.DiasesName
            });
            await _context.SaveChangesAsync();

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
