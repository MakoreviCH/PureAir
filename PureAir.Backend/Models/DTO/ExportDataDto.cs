namespace PureAirBackend.Models.DTO
{
	public class ExportDataDto
	{
		public string? Temperature { get; set; }
		public string? Humidity { get; set; }
		public string? GasQuality { get; set; }
		public string Timestamp { get; set; }
	}
}
