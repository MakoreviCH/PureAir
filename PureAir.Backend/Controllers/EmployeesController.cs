using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PureAirBackend;
using PureAirBackend.Models;
using PureAirBackend.Models.DTO;

namespace PureAirBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly BackendContext _context;

		public EmployeesController(BackendContext context)
		{
			_context = context;
		}

		// GET: api/Employees
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
		{
			if (_context.Employees == null)
			{
				return NotFound();
			}
			return await _context.Employees.ToListAsync();
		}


		// GET: api/Employees/5
		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info/{id}")]
		public async Task<ActionResult<UserInfoDto>> GetEmployeeInfo(string id)
		{
			if (_context.Employees == null)
			{
				return NotFound();
			}
			var employee = await _context.Employees.Where(st => st.Id == id).Select(user => new UserInfoDto()
			{
				UserId = user.Id,
				FirstName = user.First_Name,
				LastName = user.Last_Name,
				JobTitle = user.JobTitle,
				Phone = user.PhoneNumber,
				Email = user.Email
			}).FirstOrDefaultAsync();

			return employee;
		}
		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		public async Task<ActionResult<Employee>> GetEmployee(string id)
		{
			if (_context.Employees == null)
			{
				return NotFound();
			}
			var employee = await _context.Employees.FindAsync(id);

			if (employee == null)
			{
				return NotFound();
			}

			return employee;
		}

		// PUT: api/Employees/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutEmployee(string id, UserEditRequestDto employee)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var foundUser = await _context.Employees.FindAsync(id);
			if (foundUser == null)
			{
				return BadRequest();
			}

			foundUser.PhoneNumber = employee.PhoneNumber;
			foundUser.First_Name = employee.First_Name;
			foundUser.Last_Name = employee.Last_Name;
			foundUser.JobTitle = employee.JobTitle;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!EmployeeExists(id))
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


		// DELETE: api/Employees/5
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEmployee(string id)
		{
			if (_context.Employees == null)
			{
				return NotFound();
			}
			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}

			_context.Employees.Remove(employee);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool EmployeeExists(string id)
		{
			return (_context.EmployeePasses?.Any(e => e.Id.Equals(id))).GetValueOrDefault();
		}
	}
}
