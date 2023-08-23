using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder.ViewModel
{
	public partial class LoginViewModel : BaseViewModel
	{
		[ObservableProperty] private string _email;

		[ObservableProperty] private string _password;

		private UserService _userService;
		private IConnectivity _connectivity;

		public LoginViewModel(UserService userService, IConnectivity connectivity)
		{
			_userService = userService;
			_connectivity = connectivity;
		}

		[RelayCommand]
		async Task Login()
		{
			if (IsBusy)
				return;
			if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
			{
				if (_connectivity.NetworkAccess != NetworkAccess.Internet)
				{
					await Shell.Current.DisplayAlert("No connectivity!",
						$"Please check internet and try again.", "OK");
					return;
				}

				IsBusy = true;
				try
				{
					await _userService.LoginUser(Email, Password);
					if (DeviceInfo.Platform == DevicePlatform.WinUI)
					{
						Shell.Current.Dispatcher.Dispatch(async () =>
						{
							await Shell.Current.GoToAsync($"//HomePage", true);
						});
					}
					else
					{
						await Shell.Current.GoToAsync($"//HomePage", true);

					}
				}
				catch (HttpRequestException e)
				{
					await Shell.Current.DisplayAlert("Error!", e.Message, "OK");
				}
				catch(Exception e)
				{
					await Shell.Current.DisplayAlert("Error!", e.Message, "OK");
				}
				IsBusy=false;
			}
		}
		[RelayCommand]
		async Task GoToRegister()
		{
			if (IsBusy)
				return;
			await Shell.Current.GoToAsync($"{nameof(RegisterPage)}", true);

		}
	}	
}
