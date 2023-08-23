using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ATARK_Backend.Models;
using PureAirBackend.Configs;
using PureAirBackend.Models;
using PureAirBackend.Models.DTO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using PureAirBackend.Models;

namespace PureAirBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<Employee> _userManager;

		private readonly IConfiguration _configuration;

		private readonly RoleManager<IdentityRole> _roleManager;
		//private readonly JwtConfig _jwtConfig;

		public AuthenticationController(
			UserManager<Employee> userManager,
			RoleManager<IdentityRole> roleManager,
		// JwtConfig jwtConfig
		IConfiguration configuration
			)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_configuration = configuration;
			//	_jwtConfig = jwtConfig;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
		{
			if (ModelState.IsValid)
			{
				var userExist = await _userManager.FindByEmailAsync(requestDto.Email);
				if (userExist != null)
				{
					return BadRequest(new AuthResult()
					{
						Result = false,
						Errors = new List<string>()
						{
							"Email exists"
						}
					});
				}

				var newUser = new Employee()
				{
					Email = requestDto.Email,
					UserName = requestDto.Email,
					PhoneNumber = requestDto.PhoneNumber,
					First_Name = requestDto.First_Name,
					Last_Name = requestDto.Last_Name,
					JobTitle = requestDto.JobTitle,
				};

				var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);


				if (isCreated.Succeeded)
				{
					await _userManager.AddToRoleAsync(newUser, "Member");
					var token =await GenerateJwtToken(newUser);
					return Ok(new AuthResult()
					{
						Result = true,
						Token = token
					});
				}

				return BadRequest(new AuthResult()
				{
					Errors = new List<string>()
					{
						"Server error"
					},
					Result = false
				});
			}

			return BadRequest();
		}

		[Route("Login")]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
		{
			if (ModelState.IsValid)
			{
				var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
				if (existingUser == null)
				{
					return Ok(new AuthResult()
					{
						Result = false,
						Errors = new List<string>()
						{
							"User doesn't exists"
						}
					});
				}

				var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

				if(!isCorrect)
					return Ok(new AuthResult()
					{
						Result = false,
						Errors = new List<string>()
						{
							"Invalid credentials"
						}
					});

				var jwtToken = await GenerateJwtToken(existingUser);

				return Ok(new AuthResult()
				{
					Result = true,
					Token = jwtToken
				});
			}

			return BadRequest(new AuthResult()
			{
				Result = false,
				Errors = new List<string>()
				{
					"Invalid payload"
				}
			});
		}

		[Route("LoginAdmin")]
		[HttpPost]
		public async Task<IActionResult> LoginAdministrator([FromBody] UserLoginRequestDto loginRequest)
		{
			if (ModelState.IsValid)
			{
				var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
				if (existingUser == null)
				{
					return Ok(new AuthResult()
					{
						Result = false,
						Errors = new List<string>()
						{
							"User doesn't exist"
						}
					});
				}

				var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

				if (!isCorrect)
				{
					return Ok(new AuthResult()
					{
						Result = false,
						Errors = new List<string>()
						{
							"Invalid credentials"
						}
					});
				}

				var roles = await _userManager.GetRolesAsync(existingUser);

				if (!roles.Contains("Admin"))
				{
					return Ok(new AuthResult()
					{
						Result = false,
						Errors = new List<string>()
						{
							"Access denied. User is not an administrator"
						}
					});
				}

				var jwtToken = await GenerateJwtToken(existingUser);

				return Ok(new AuthResult()
				{
					Result = true,
					Token = jwtToken
				});
			}

			return BadRequest(new AuthResult()
			{
				Result = false,
				Errors = new List<string>()
				{
					"Invalid payload"
				}
			});
		}

		private async Task<List<Claim>> GetAllValidClaims(Employee user)
		{
			var _options = new IdentityOptions();

			var claims = new List<Claim>
			{
				new Claim("Id", user.Id),
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
			};

			var userClaims = await  _userManager.GetClaimsAsync(user);
			claims.AddRange(userClaims);

			var userRoles = await _userManager.GetRolesAsync(user);

			foreach (var userRole in userRoles)
			{
				

				var role = await _roleManager.FindByNameAsync(userRole);

				if (role != null)
				{
					claims.Add(new Claim(ClaimTypes.Role, userRole));
					var roleClaims = await _roleManager.GetClaimsAsync(role);
					foreach (var roleClaim in roleClaims)
					{
						claims.Add(roleClaim);
					}
				}
			}
			return claims;
		}
		private async Task<string> GenerateJwtToken(Employee user)
		{

			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
			
			var claims = await GetAllValidClaims(user);

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddMonths(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)

			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			return jwtTokenHandler.WriteToken(token);
		}
	}
}

