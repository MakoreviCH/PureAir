using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Model.Dto
{
	public class RegisterInfoDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Phone_Number { get; set; }
		public string? Last_Name { get; set; }
		public string? First_Name { get; set; }
	}
}
