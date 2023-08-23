namespace PureAirBackend.Models.DTO
{
	public class WorkspaceCreateDto
	{
		public string WorkspaceName { get; set; }
		public double? TemperatureThreshold { get; set; }
		public double? HumidityThreshold { get; set; }
		public double? GasThreshold { get; set; }
	}
}
