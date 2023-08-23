using Microsoft.AspNetCore.Identity;

namespace PureAirBackend.Models
{
	public class Employee : IdentityUser
	{
		public string? First_Name { get; set; }
		public string? Last_Name { get; set; }
		public string PhoneNumber { get; set; }
		public string? JobTitle { get; set; }
		public EmployeePass EmployeePass { get; set; }

		
		
	}
}
