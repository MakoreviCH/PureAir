using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MonkeyFinder.Services
{
	public class WorkspaceService
	{
		HttpClient _httpClient;
		private IConfiguration _config;
		public WorkspaceService(IConfiguration config)
		{
			_config = config;
			_httpClient = new HttpClient(GetInsecureHandler());
			_httpClient.BaseAddress = new Uri(_config.GetRequiredSection("Settings").GetRequiredSection("BaseAddress").Value);
		}

		List<Workspace> workspaceList;

		public async Task<List<Workspace>> GetStores()
		{
			//	if(Preferences.Default.ContainsKey("JWT"))
			_httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", Preferences.Default.Get<string>("JWT", null));

			var response = await _httpClient.GetAsync("http://10.0.2.2:7031/api/Workspaces/Info");
			if (response.IsSuccessStatusCode)
			{
				workspaceList = await response.Content.ReadFromJsonAsync<List<Workspace>>();
			}
			return workspaceList;
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
			return handler;
		}
	}
}
