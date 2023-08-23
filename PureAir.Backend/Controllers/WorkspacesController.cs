using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PureAirBackend;
using PureAirBackend.Models.DTO;
using Workspace = PureAirBackend.Models.Workspace;

namespace PureAirBackend.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class WorkspacesController : ControllerBase
	{
		private readonly BackendContext _context;

		public WorkspacesController(BackendContext context)
		{
			_context = context;
		}

		// GET: api/Workspaces
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Workspace>>> GetWorkspaces()
		{
			if (_context.Workspaces == null)
			{
				return NotFound();
			}
			return await _context.Workspaces.ToListAsync();
		}
		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info")]
		public async Task<ActionResult<IEnumerable<WorkspaceViewDto>>> GetWorkspacesInfo()
		{
			if (_context.Workspaces == null)
			{
				return NotFound();
			}
			return await _context.Workspaces.Select(workspace => new WorkspaceViewDto()
			{
				Id = workspace.Id,
				WorkspaceName = workspace.WorkspaceName,
				TemperatureThreshold = workspace.TemperatureThreshold,
				HumidityThreshold = workspace.HumidityThreshold,
				GasThreshold = workspace.GasThreshold
			}).ToListAsync();
		}
		// GET: api/Workspaces/5
		[Authorize(Roles = "Admin")]
		[HttpGet("{id}", Name = "GetWorkspace")]
		public async Task<ActionResult<Workspace>> GetWorkspace(int id)
		{
			if (_context.Workspaces == null)
			{
				return NotFound();
			}
			var workspace = await _context.Workspaces.FindAsync(id);

			if (workspace == null)
			{
				return NotFound();
			}

			return workspace;
		}
		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info/{id}")]
		public async Task<ActionResult<WorkspaceInfoDto>> GetWorkspaceInfo(int id)
		{
			if (_context.Workspaces == null)
			{
				return NotFound();
			}
			var workspace = await _context.Workspaces.Where(ws => ws.Id == id).Select(workspace => new WorkspaceInfoDto()
			{
				WorkspaceName = workspace.WorkspaceName,
				TemperatureThreshold = workspace.TemperatureThreshold,
				HumidityThreshold = workspace.HumidityThreshold,
				GasThreshold = workspace.GasThreshold

			}).FirstOrDefaultAsync();

			if (workspace == null)
			{
				return NotFound();
			}

			return workspace;
		}

		// PUT: api/Workspaces/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutWorkspace(int id, WorkspaceInfoDto workspace)
		{
			if (_context.Workspaces == null)
			{
				return BadRequest();
			}
			var foundWorkspace = await _context.Workspaces.FindAsync(id);
			if (foundWorkspace == null)
			{
				return NotFound();
			}

			foundWorkspace.WorkspaceName = workspace.WorkspaceName;
			foundWorkspace.TemperatureThreshold = workspace.TemperatureThreshold;
			foundWorkspace.HumidityThreshold = workspace.HumidityThreshold;
			foundWorkspace.GasThreshold = workspace.GasThreshold;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!WorkspaceExists(id))
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

		// POST: api/Workspaces
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<Workspace>> PostWorkspace(WorkspaceCreateDto workspace)
		{

			if (_context.Workspaces == null)
			{
				return Problem("Entity set 'BackendContext.Workspaces'  is null.");
			}

			if (!ModelState.IsValid) return BadRequest();
			Workspace newWorkspace = new Workspace()
			{
				WorkspaceName = workspace.WorkspaceName,
				TemperatureThreshold = workspace.TemperatureThreshold,
				HumidityThreshold = workspace.HumidityThreshold,
				GasThreshold = workspace.GasThreshold

			};
			_context.Workspaces.Add(newWorkspace);
			await _context.SaveChangesAsync();
			return Ok();
		}

		// DELETE: api/Workspaces/5
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteWorkspace(int id)
		{
			if (_context.Workspaces == null)
			{
				return NotFound();
			}
			var workspace = await _context.Workspaces.FindAsync(id);
			if (workspace == null)
			{
				return NotFound();
			}

			_context.Workspaces.Remove(workspace);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool WorkspaceExists(int id)
		{
			return (_context.Workspaces?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
