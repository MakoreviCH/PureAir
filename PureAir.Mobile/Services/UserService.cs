using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonkeyFinder.Model.Dto;


namespace MonkeyFinder.Services
{
	public class UserService
	{
		HttpClient _httpClient;

		public static User CurrentUser;
		private IConfiguration _config;
		public UserService(IConfiguration config)
		{
			_config = config;
			_httpClient = new HttpClient(GetInsecureHandler());
			_httpClient.BaseAddress = new Uri(_config.GetRequiredSection("Settings").GetRequiredSection("BaseAddress").Value);

		}

		public async Task<User> LoginUser(string email, string password)
		{

			// _httpClient.DefaultRequestHeaders.Authorization
			//    = new AuthenticationHeaderValue("Bearer", Preferences.Default.Get<string>("JWT", null));
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Authentication/Login");

			requestMessage.Content = JsonContent.Create(new { Email = email, Password = password });

			HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<AuthInfo>();
				if (result.Errors.IsNullOrEmpty())
				{
					var jwt = result.Token;
					Preferences.Default.Set("JWT", jwt);

					var handler = new JwtSecurityTokenHandler();
					var token = handler.ReadJwtToken(jwt);
					string userId = token.Claims.First(claim => claim.Type == "Id").Value;

					CurrentUser = await GetUserInfo(userId);
					return CurrentUser;
				}
				else
				{
					foreach (var error in result.Errors)
					{
						throw new HttpRequestException(error);
					}
				}
			}
			else
				throw new Exception(response.ReasonPhrase);

			return null;
		}

		

		public async Task<User> GetUserInfo(string userId)
		{

			if (Preferences.Default.ContainsKey("JWT"))
			{

				_httpClient.DefaultRequestHeaders.Authorization
					= new AuthenticationHeaderValue("Bearer", Preferences.Default.Get<string>("JWT", null));
				var response = await _httpClient.GetAsync("api/Employees/info/" + userId);

				if (response.IsSuccessStatusCode)
				{
					var User = await response.Content.ReadFromJsonAsync<User>();
					return User;
				}
				else if (response.StatusCode == HttpStatusCode.Unauthorized)
					throw new SecurityTokenExpiredException("Token is expired");
				else if (response.StatusCode == HttpStatusCode.NotFound)
					throw new InvalidCredentialException("User is not found");
				else
					throw new HttpRequestException(response.StatusCode.ToString());
			}
			else
				throw new SecurityTokenException("JWT token can't be found, you need to login");

		}

		public async Task<UserPass> GetPassInfo(string userId)
		{

			if (Preferences.Default.ContainsKey("JWT"))
			{

				_httpClient.DefaultRequestHeaders.Authorization
					= new AuthenticationHeaderValue("Bearer", Preferences.Default.Get<string>("JWT", null));
				var response = await _httpClient.GetAsync("api/EmployeePasses/info/" + userId);

				if (response.IsSuccessStatusCode)
				{
					var Pass = await response.Content.ReadFromJsonAsync<UserPass>();
					return Pass;
				}
				else if (response.StatusCode == HttpStatusCode.Unauthorized)
					throw new SecurityTokenExpiredException("Token is expired");
				else if (response.StatusCode == HttpStatusCode.NotFound)
					throw new InvalidCredentialException("Pass is not found");
				else
					throw new HttpRequestException(response.StatusCode.ToString());
			}
			else
				throw new SecurityTokenException("JWT token can't be found, you need to login");

		}

		public static HttpClientHandler GetInsecureHandler()
		{
			HttpClientHandler handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
			{
				if (cert.Issuer.Equals("CN=localhost"))
					return true;
				return errors == System.Net.Security.SslPolicyErrors.None;
			};
			handler.UseProxy = false;
			return handler;
		}

		public async Task SignOut()
		{
			CurrentUser = null;
			Preferences.Set("JWT", null);
		}
	}
}
