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
using OfficeOpenXml;
using System.IO;
using PureAirBackend.Models;
using PureAirBackend.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PureAirBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WorkspaceDatasController : ControllerBase
	{
		private readonly BackendContext _context;

		public WorkspaceDatasController(BackendContext context)
		{
			_context = context;
		}

		// GET: api/WorkspaceDatas
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<WorkspaceData>>> GetDatas()
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}
			return await _context.WorkspaceDatas.ToListAsync();
		}
		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info")]
		public async Task<ActionResult<IEnumerable<WorkspaceDataViewDto>>> GetDatasInfo()
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}

			return await _context.WorkspaceDatas.Join(_context.Workspaces, data => data.WorkspaceId,
				workspace => workspace.Id, (data, workspace) => new WorkspaceDataViewDto()
				{
					Id = data.Id,
					WorkspaceName = workspace.WorkspaceName,
					Temperature = data.Temperature,
					Humidity = data.Humidity,
					GasQuality = data.GasQuality,
					Timestamp = data.Timestamp

				}).ToListAsync();

		}

		// GET: api/WorkspaceDatas/5
		[Authorize(Roles = "Admin")]
		[HttpGet("{id}", Name = "GetWorkspaceData")]
		public async Task<ActionResult<WorkspaceData>> GetWorkspaceData(int id)
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}
			var workspaceData = await _context.WorkspaceDatas.FindAsync(id);

			if (workspaceData == null)
			{
				return NotFound();
			}

			return workspaceData;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("workspace/{id}")]
		public async Task<ActionResult<List<WorkspaceData>>> GetDataByWorkspace(int id)
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}
			var workspaceData = await _context.WorkspaceDatas.Where(data => data.WorkspaceId == id).ToListAsync();

			if (workspaceData == null)
			{
				return NotFound();
			}

			return workspaceData;
		}

		enum Time
		{
			hour = 1,
			day = 24,
			week = 168,
			month = 720
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("workspace/{id}/{date}")]
		public async Task<ActionResult<List<WorkspaceData>>> GetDataByWorkspaceDate(int id, string date)
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}

			Time choiceTime;
			if (Enum.TryParse(date, out choiceTime))
			{
				int hours = (int)choiceTime;

				DateTime timeNow = DateTime.UtcNow;
				var workspaceData = await _context.WorkspaceDatas.Where(data => data.WorkspaceId == id &&
					data.Timestamp <= timeNow && data.Timestamp >= timeNow.AddHours(-hours)).ToListAsync();

				if (workspaceData == null)
				{
					return NotFound();
				}

				return workspaceData;
			}
			else
				return NotFound();
		}

		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info/{id}")]
		public async Task<ActionResult<WorkspaceDataViewDto>> GetWorkspaceDataInfo(int id)
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}

			var workspaceData = await _context.WorkspaceDatas.Join(_context.Workspaces, data => data.WorkspaceId,
				workspace => workspace.Id, (data, workspace) => new WorkspaceDataViewDto()
				{
					Id = data.Id,
					WorkspaceName = workspace.WorkspaceName,
					Temperature = data.Temperature,
					Humidity = data.Humidity,
					GasQuality = data.GasQuality,
					Timestamp = data.Timestamp

				}).Where(wd => wd.Id == id).FirstOrDefaultAsync();


			if (workspaceData == null)
			{
				return NotFound();
			}

			return workspaceData;
		}

		[Authorize(Roles = "Member,Admin")]
		[HttpGet("info/last/{workspaceId}")]
		public async Task<ActionResult<WorkspaceDataSingleDto>> GetLastDataInfo(int workspaceId)
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}


			var workspaceData = await _context.WorkspaceDatas.OrderByDescending(wd => wd.Timestamp).Where(wd => wd.WorkspaceId == workspaceId).Join(
				_context.Workspaces, data => data.WorkspaceId,
				workspace => workspace.Id, (data, workspace) => new WorkspaceDataSingleDto()
				{
					Id = data.Id,
					Temperature = data.Temperature,
					Humidity = data.Humidity,
					GasQuality = data.GasQuality,
					Timestamp = data.Timestamp

				}).FirstOrDefaultAsync();

			if (workspaceData == null)
			{
				return NotFound();
			}

			return workspaceData;
		}

		// PUT: api/WorkspaceDatas/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutWorkspaceData(int id, WorkspaceDataInfoDto workspaceData)
		{
			if (_context.WorkspaceDatas == null)
			{
				return BadRequest();
			}

			var foundWorkspace = await _context.WorkspaceDatas.FindAsync(id);
			if (foundWorkspace == null)
			{
				return NotFound();
			}

			foundWorkspace.WorkspaceId = workspaceData.WorkspaceId;
			foundWorkspace.Temperature = workspaceData.Temperature;
			foundWorkspace.Humidity = workspaceData.Humidity;
			foundWorkspace.GasQuality = workspaceData.GasQuality;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!WorkspaceDataExists(id))
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

		// POST: api/WorkspaceDatas
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<WorkspaceData>> PostWorkspaceData(WorkspaceDataInfoDto workspaceData)
		{
			if (_context.WorkspaceDatas == null)
			{
				return Problem("Entity set 'BackendContext.WorkspaceDatas'  is null.");
			}

			var newData = new WorkspaceData()
			{
				WorkspaceId = workspaceData.WorkspaceId,
				GasQuality = workspaceData.GasQuality,
				Temperature = workspaceData.Temperature,
				Humidity = workspaceData.Humidity,
				Timestamp = DateTime.Now
			};
			_context.WorkspaceDatas.Add(newData);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/WorkspaceDatas/5
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteWorkspaceData(int id)
		{
			if (_context.WorkspaceDatas == null)
			{
				return NotFound();
			}
			var workspaceData = await _context.WorkspaceDatas.FindAsync(id);
			if (workspaceData == null)
			{
				return NotFound();
			}

			_context.WorkspaceDatas.Remove(workspaceData);
			await _context.SaveChangesAsync();

			return NoContent();
		}



		[HttpGet("decision/{workspaceId}")]
		public async Task<ActionResult<DecisionDto>> MakeDecision(int workspaceId)
		{
			var workspaceData = await _context.WorkspaceDatas.OrderByDescending(wd => wd.Timestamp).FirstOrDefaultAsync(wd => wd.WorkspaceId == workspaceId);
			if (workspaceData == null)
			{
				return NotFound();
			}

			double? airQuality = workspaceData.GasQuality;
			double? temperature = workspaceData.Temperature;
			double? humidity = workspaceData.Humidity;

			var workspace = await _context.Workspaces.FirstOrDefaultAsync(w => w.Id == workspaceData.WorkspaceId);
			if (workspace == null)
			{
				return NotFound();
			}

			double? gasQualityThreshold = workspace.GasThreshold;
			double? temperatureThreshold = workspace.TemperatureThreshold;
			double? humidityThreshold = workspace.HumidityThreshold;

			double? disasterProbability = CalculateDisasterProbability(airQuality, temperature, humidity, gasQualityThreshold, temperatureThreshold, humidityThreshold);

			string decision;

			if (disasterProbability == null)
			{
				return new DecisionDto()
				{
					Decision = "error",
					Probability = -1
				};
			}
			if (disasterProbability >= 70)
			{
				decision = "high";
			}
			else if (disasterProbability >= 30)
			{
				decision = "medium";
			}
			else
			{
				decision = "low";
			}

			return new DecisionDto()
			{
				Decision = decision,
				Probability = disasterProbability
			};
		}
		private double? CalculateDisasterProbability(double? airQuality, double? temperature, double? humidity,
			double? airQualityThreshold, double? temperatureThreshold, double? humidityThreshold)
		{
			double airQualityWeight = 0.6;
			double temperatureWeight = 0.25;
			double humidityWeight = 0.15;


			double? airQualityFactor = airQuality / airQualityThreshold / 1.5; 
			double? temperatureFactor = temperature / temperatureThreshold / 1.5; 
			double? humidityFactor = humidity / humidityThreshold / 1.5; 

			// Normalization of data 
			double? normalizedAirQuality = airQualityFactor >= 1.0 ? 1.0 : airQualityFactor;
			double? normalizedTemperature = temperatureFactor >= 1.0 ? 1.0 : temperatureFactor;
			double? normalizedHumidity = humidityFactor >= 1.0 ? 1.0 : humidityFactor;

			// Calculate the weighted sum with adjusted factors
			double? weightedSum = (airQualityWeight * normalizedAirQuality) + (temperatureWeight * normalizedTemperature) + (humidityWeight * normalizedHumidity);

			// Calculate the probability as a percentage (ranging from 0 to 100)
			double? probability = weightedSum * 100.0;

			// Return the probability
			return probability;
		}
		private bool WorkspaceDataExists(int id)
		{
			return (_context.WorkspaceDatas?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
