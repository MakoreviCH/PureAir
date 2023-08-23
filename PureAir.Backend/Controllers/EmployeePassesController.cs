using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PureAirBackend;
using PureAirBackend.Models;
using PureAirBackend.Models.DTO;

namespace PureAirBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePassesController : ControllerBase
    {
        private readonly BackendContext _context;

        public EmployeePassesController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/EmployeePasses
		[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeePass>>> GetEmployeePasses()
        {
          if (_context.EmployeePasses == null)
          {
              return NotFound();
          }
            return await _context.EmployeePasses.ToListAsync();
        }

        // GET: api/EmployeePasses/5
		[Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "GetEmployeePass")]
		public async Task<ActionResult<EmployeePass>> GetEmployeePass(string id)
        {
          if (_context.EmployeePasses == null)
          {
              return NotFound();
          }
            var employeePass = await _context.EmployeePasses.FindAsync(id);

            if (employeePass == null)
            {
                return NotFound();
            }

            return employeePass;
        }
		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info/{userId}")]
		public async Task<ActionResult<PassInfoDto>> GetPassInfo(string userId)
		{
			if (_context.EmployeePasses == null)
			{
				return NotFound();
			}

			var employeePass = await _context.EmployeePasses.Where(ep=>ep.EmployeeId==userId).Select(ep => new PassInfoDto()
			{
                WorkspaceId = ep.WorkspaceId,
                PassId = ep.Id,
                Timestamp = ep.Timestamp
			}).FirstOrDefaultAsync();

			if (employeePass == null)
			{
				return NotFound();
			}

			return employeePass;
		}

		// PUT: api/EmployeePasses/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeePass(string id, EmployeePassInfoDto employeePass)
        {
			if (_context.EmployeePasses == null)
			{
				return BadRequest();
			}
			var foundPass = await _context.EmployeePasses.FindAsync(id);
			
			if (foundPass == null)
			{
				return NotFound();
			}

			foundPass.EmployeeId = employeePass.EmployeeId;

            if (foundPass.WorkspaceId != employeePass.WorkspaceId)
            {
                foundPass.WorkspaceId = employeePass.WorkspaceId;
                foundPass.Timestamp = DateTime.UtcNow;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeePassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("update")]
		public async Task<IActionResult> UpdateInfo(string id, int workspace, bool isEnter)
        {

			var foundPass = await _context.EmployeePasses.FindAsync(id);
			if (foundPass == null)
			{
				return NotFound();
			}

            if(isEnter)
	            foundPass.WorkspaceId = workspace;
            else
	            foundPass.WorkspaceId = null;
            foundPass.Timestamp = DateTime.UtcNow;

			try
	        {
		        await _context.SaveChangesAsync();
	        }
	        catch (DbUpdateConcurrencyException)
	        {
		        if (!EmployeePassExists(id))
		        {
			        return NotFound();
		        }
		        else
		        {
			        throw;
		        }
	        }

	        return NoContent();
        }

		// POST: api/EmployeePasses
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public async Task<ActionResult<EmployeePass>> PostEmployeePass(EmployeePassCreateDto employeePass)
        {
          if (_context.EmployeePasses == null)
          {
              return Problem("Entity set 'BackendContext.EmployeePasses'  is null.");
          }

          var newPass = new EmployeePass()
          {
              Id = employeePass.Id,
              EmployeeId = employeePass.EmployeeId,
              
          };
            _context.EmployeePasses.Add(newPass);
            await _context.SaveChangesAsync();

            return Ok();
		}

        // DELETE: api/EmployeePasses/5
		[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeePass(string id)
        {
            if (_context.EmployeePasses == null)
            {
                return NotFound();
            }
            var employeePass = await _context.EmployeePasses.FindAsync(id);
            if (employeePass == null)
            {
                return NotFound();
            }

            _context.EmployeePasses.Remove(employeePass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeePassExists(string id)
        {
            return (_context.EmployeePasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
