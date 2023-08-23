namespace PureAirBackend.Models
{
	public class WorkspaceData
	{
		public int Id { get; set; }
		public int WorkspaceId { get; set; }
		public Workspace Workspace { get; set; }
		public double? Temperature { get; set; }
		public double? Humidity { get; set; }
		public double? GasQuality { get; set; }
		public DateTime Timestamp { get; set; }
		
	}
}
