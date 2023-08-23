namespace PureAirBackend.Models.DTO
{
	public class WorkspaceInfoDto
	{
		public string WorkspaceName { get; set; }
		public double? TemperatureThreshold { get; set; }
		public double? HumidityThreshold { get; set; }
		public double? GasThreshold { get; set; }
	}
}
