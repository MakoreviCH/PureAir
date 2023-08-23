using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;
using MonkeyFinder.Model.Dto;
using MonkeyFinder.Resources.Languages;
using MonkeyFinder.Services;
using MonkeyFinder.View;


namespace MonkeyFinder.ViewModel
{
	public partial class UserProfileViewModel : BaseViewModel
	{

		public UserService _userService;
		public WorkspaceService _workspaceService;
		DatasService _datasService;
		private IConnectivity _connectivity;

		public UserProfileViewModel(UserService userService, WorkspaceService workspaceService, DatasService datasService, IConnectivity connectivity)
		{

			_userService = userService;
			_workspaceService = workspaceService;
			_datasService = datasService;
			_connectivity = connectivity;
			_user = UserService.CurrentUser;
			_userName = _user.FirstName + " " + _user.LastName;
			Title = _userName;
		}
		[ObservableProperty]
		User _user;

		[ObservableProperty]
		UserPass _pass;

		[ObservableProperty]
		bool isRefreshing;

		[ObservableProperty]
		string _userName;
		[ObservableProperty]
		string _decision;


		[RelayCommand]
		async Task SignOut()
		{
			if (IsBusy)
				return;
			await _userService.SignOut();
			Application.Current.MainPage = new AppShell();
			
		}
		[RelayCommand]
		async Task GetPassAsync()
		{
			if (IsBusy)
				return;

			try
			{
				
				IsBusy = true;


				try
				{
					Pass = await _userService.GetPassInfo(User.UserId);

					DecisionDto decisionResponse= new ();

					if (Pass.WorkspaceId != null)
						decisionResponse = await _datasService.GetWorkspaceDisaster(Pass.WorkspaceId);
					if (decisionResponse.Decision == "low")
						Decision = Strings.Decision_Low;
					else if (decisionResponse.Decision == "medium")
						Decision = Strings.Decision_Medium;
					else if (decisionResponse.Decision == "high")
						Decision = Strings.Decision_High;
					else
						Decision = Strings.Decision_Error;

				}
				catch (HttpRequestException e)
				{
					Console.WriteLine(e);
					throw;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
				


			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Unable to get monkeys: {ex.Message}");

				await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");

			}
			finally
			{
				IsBusy = false;
				IsRefreshing = false;
			}

		}

		[ObservableProperty]
		bool _isToggled = Preferences.Get("lang", "en-US") == "uk-UA";


		bool SwitchIsToggled
		{
			get => _isToggled;
			set	
			{
				_isToggled = value;

			}
		}
	}
}
