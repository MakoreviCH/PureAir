using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using MonkeyFinder.Model.Dto;

namespace MonkeyFinder.Services
{
	public class DatasService
	{
		HttpClient _httpClient;
		private IConfiguration _config;
		public DatasService(IConfiguration config)
		{
			_config = config;
			_httpClient = new HttpClient(GetInsecureHandler());
			_httpClient.BaseAddress = new Uri(_config.GetRequiredSection("Settings").GetRequiredSection("BaseAddress").Value);
			
		}

		private WorkspaceData lastData;

		public async Task<WorkspaceData> GetWorkspaceDatas(int workspaceId)

		{
			_httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", Preferences.Default.Get<string>("JWT", null));

			var response = await _httpClient.GetAsync("api/WorkspaceDatas/info/last/" + workspaceId);
			if (response.IsSuccessStatusCode)
			{


				var settings = new JsonSerializerSettings
				{
					DateFormatString = "yyyy-MM-ddTH:mm:ss.fffZ",
					DateTimeZoneHandling = DateTimeZoneHandling.Utc
				};
				lastData = JsonConvert.DeserializeObject<WorkspaceData>(await response.Content.ReadAsStringAsync(), settings);

			}
			return lastData;
		}


		public async Task<DecisionDto> GetWorkspaceDisaster(int? workspaceId)

		{
			_httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", Preferences.Default.Get<string>("JWT", null));

			var response = await _httpClient.GetAsync("api/WorkspaceDatas/decision/" + workspaceId);
			if (response.IsSuccessStatusCode)
			{


				var settings = new JsonSerializerSettings
				{
					DateFormatString = "yyyy-MM-ddTH:mm:ss.fffZ",
					DateTimeZoneHandling = DateTimeZoneHandling.Utc
				};
				var decision = JsonConvert.DeserializeObject<DecisionDto>(await response.Content.ReadAsStringAsync(), settings);
				return decision;
			}
			return null;
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

	}
}
