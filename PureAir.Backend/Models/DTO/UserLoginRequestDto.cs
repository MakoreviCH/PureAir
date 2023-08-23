using Microsoft.Build.Framework;

namespace PureAirBackend.Models.DTO
{
	public class UserLoginRequestDto
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
