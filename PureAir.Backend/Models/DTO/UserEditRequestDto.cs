namespace PureAirBackend.Models.DTO
{
	public class UserEditRequestDto
	{
		public string PhoneNumber { get; set; }
		public string First_Name { get; set; }
		public string Last_Name { get; set; }
		public string? JobTitle { get; set; }
	}
}
