namespace PureAirBackend.Models
{
	public class Workspace
	{
		public int Id { get; set; }
		public string WorkspaceName { get; set; }
		public double? TemperatureThreshold { get; set; }
		public double? HumidityThreshold { get; set; }
		public double? GasThreshold { get; set; }
		public List<WorkspaceData> WorkspaceData { get; set; }
		public List<EmployeePass> EmployeePasses { get; set; }

	}
}
