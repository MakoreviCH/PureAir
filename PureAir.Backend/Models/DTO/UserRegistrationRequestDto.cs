using Microsoft.Build.Framework;

namespace PureAirBackend.Models.DTO
{
	public class UserRegistrationRequestDto
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		public string? First_Name { get; set; }
		public string? Last_Name { get; set; }
		public string? JobTitle { get; set; }
	}
}
