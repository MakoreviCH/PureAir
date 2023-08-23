using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
				fonts.AddFont("Inter-Regular.ttf", "InterRegular");
				fonts.AddFont("Inter-Medium.ttf", "InterMedium");
			});
#if DEBUG
			//	builder.Logging.AddDebug();
#endif

		var a = Assembly.GetExecutingAssembly();
		using var stream = a.GetManifestResourceStream("MonkeyFinder.appsettings.json");

		var config = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();

		builder.Configuration.AddConfiguration(config);

		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
		builder.Services.AddSingleton<IMap>(Map.Default);

		builder.Services.AddTransient<DatasService>();
		builder.Services.AddTransient<WorkspaceService>();
		builder.Services.AddTransient<WorkspacesViewModel>();
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<UserService>();
		builder.Services.AddTransient<UserProfilePage>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<WorkspaceDetailsViewModel>();
		builder.Services.AddTransient<WorkspacePage>();
		builder.Services.AddTransient<UserProfileViewModel>();
		ChangeCulture(Preferences.Get("lang", "en-US"));
		//Preferences.Default.Set("JWT", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImI4MDc5Yjc5LTUyMWItNDk0OS04NDhhLTUxMjEyYmFmMGFhMSIsInN1YiI6InJvc3R5c2xhdkBudXJlLnVhIiwiZW1haWwiOiJyb3N0eXNsYXZAbnVyZS51YSIsImp0aSI6IjlmNDg3NDhiLTUzZmQtNDEwMi1hODRhLWI2NzYwOTNhNTNiNSIsImlhdCI6MTY3MTQ2MTAxNCwicm9sZSI6IkFkbWluIiwibmJmIjoxNjcxNDYxMDE0LCJleHAiOjE2NzQxMzk0MTR9.ZAHJ06iq31Md21vD0uhyNKo9u2-5XAi7f8NtwU8rz1Y");
		return builder.Build();
	}
	static CultureInfo[] _cultures = new[]
	{
		new CultureInfo("en-US"),
		new CultureInfo("uk-UA")
	};
	public static void ChangeCulture(string value)
	{
		CultureInfo info = new(value);
		if (CultureInfo.CurrentCulture != info)
		{
			Thread.CurrentThread.CurrentCulture = info;
			Thread.CurrentThread.CurrentUICulture = info;
			CultureInfo.DefaultThreadCurrentCulture = info;
			CultureInfo.DefaultThreadCurrentUICulture = info;
			Preferences.Default.Set("lang", value);
		}
	}


}
