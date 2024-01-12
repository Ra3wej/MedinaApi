using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedinaApi.Data;
using MedinaApi.DTO;
using MedinaApi.Helpers;
using MedinaApi.Models;
using MedinaApi.Services;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardUsersController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;
        private readonly IPasswordHasher _passwordHasher;


        public DashboardUsersController(Medina_Api_DbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: api/DashboardUsers
        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<IEnumerable<GetDashboardUserDTO>>> GetDashboardUsers()
        {
            return await _context.DashboardUsers.Select(d => new GetDashboardUserDTO
            {
                IsActive = d.IsActive,
                EmailAddress = d.EmailAddress,
                Id = d.Id,
                UserName = d.UserName,
            }).ToListAsync();
        }

        // GET: api/DashboardUsers/5
        [HttpGet("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<GetDashboardUserDTO>> GetDashboardUser(int id)
        {
            var dashboardUser = await _context.DashboardUsers.FindAsync(id);

            if (dashboardUser == null)
            {
                return NotFound();
            }

            return new GetDashboardUserDTO
            {
                IsActive = dashboardUser.IsActive,
                EmailAddress = dashboardUser.EmailAddress,
                Id = dashboardUser.Id,
                UserName = dashboardUser.UserName,
            };
        }

        // PUT: api/DashboardUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> PutDashboardUser(int id)
        {
            try
            {
                var dashboardUserDTO = await _context.DashboardUsers.FindAsync(id);
                if (dashboardUserDTO == null)
                {
                    return NotFound("User not found");
                }
                dashboardUserDTO.IsActive = !dashboardUserDTO.IsActive;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DashboardUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        // POST: api/DashboardUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<DashboardUser>> PostDashboardUser(DashboardUser dashboardUser)
        //{
        //    _context.DashboardUsers.Add(dashboardUser);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetDashboardUser", new { id = dashboardUser.Id }, dashboardUser);
        //}

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> ResetDashboardUSerPassword(ResetDashboardUserPasswordDTO resetPassword)
        {

            var user = await _context.DashboardUsers.FindAsync(resetPassword.UserId);
            if (user == null)
            {
                return NotFound("User not Found");
            }
            var pass = _passwordHasher.HashPassword(resetPassword.NewPassword);
            user.Password = pass.PasswordHashed;
            user.Salt = pass.Salt;
            await _context.SaveChangesAsync();
            return Ok("Password Reseted");
        }
        // DELETE: api/DashboardUsers/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteDashboardUser(int id)
        {
            var dashboardUser = await _context.DashboardUsers.FindAsync(id);
            if (dashboardUser == null)
            {
                return NotFound();
            }

            dashboardUser.IsActive = !dashboardUser.IsActive;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool DashboardUserExists(int id)
        {
            return _context.DashboardUsers.Any(e => e.Id == id);
        }


    }
}
