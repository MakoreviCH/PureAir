namespace PureAirBackend.Models.DTO
{
    public class WorkspaceViewDto
    {
        public int Id { get; set; }
        public string WorkspaceName { get; set; }
        public double? TemperatureThreshold { get; set; }
        public double? HumidityThreshold { get; set; }
        public double? GasThreshold { get; set; }
    }
}
