using System.Runtime.CompilerServices;

namespace PureAirBackend.Models
{
	public class EmployeePass
	{
		public string Id { get; set; }
		public string EmployeeId { get; set; }
		public Employee Employee { get; set; }
		public DateTime? Timestamp { get; set; }
		public int? WorkspaceId { get; set; }
		public Workspace? Workspace { get; set; }
	}
}
