namespace PureAirBackend.Models.DTO
{
	public class WorkspaceDataInfoDto
	{
		public int WorkspaceId { get; set; }
		public double? Temperature { get; set; }
		public double? Humidity { get; set; }
		public double? GasQuality { get; set; }
		
	}
}
