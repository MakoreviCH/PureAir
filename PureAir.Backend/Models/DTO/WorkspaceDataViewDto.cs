namespace PureAirBackend.Models.DTO
{
	public class WorkspaceDataViewDto
	{
		public int Id { get; set; }
		public string WorkspaceName { get; set; }
		public double? Temperature { get; set; }
		public double? Humidity { get; set; }
		public double? GasQuality { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
