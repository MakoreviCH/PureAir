using MonkeyFinder.Services;
using MonkeyFinder.View;
using System.Collections.Generic;
using System.Globalization;

namespace MonkeyFinder.ViewModel;

public partial class WorkspacesViewModel : BaseViewModel
{
	public ObservableCollection<Workspace> Workspaces { get; } = new();
	WorkspaceService _workspaceService;
	DatasService _datasService;
	UserService _userService;
	IConnectivity connectivity;
	IGeolocation geolocation;
	public WorkspacesViewModel(IConnectivity connectivity, IGeolocation geolocationStoreService, WorkspaceService workspaceService,
		UserService userService, DatasService datasService)
	{
		Title = "Workspaces";
		_workspaceService = workspaceService;
		_userService = userService;
		_datasService = datasService;
		this.connectivity = connectivity;
		Task.Run(async () => await GetWorkspacesAsync());
	}

	[RelayCommand]
	async Task GoToDetails(Workspace workspace)
	{
		if (workspace == null)
			return;


		await Shell.Current.GoToAsync(nameof(WorkspacePage), true, new Dictionary<string, object>()
		{
			["workspace"] = workspace
		});

	}

	[ObservableProperty]
	bool isRefreshing;

	[RelayCommand]
	async Task GetWorkspacesAsync()
	{
		if (IsBusy)
			return;

		try
		{
			if (connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				await Shell.Current.DisplayAlert("No connectivity!",
					$"Please check internet and try again.", "OK");
				return;
			}

			IsBusy = true;

			var workspaces= new List<Workspace>();
			try
			{
				workspaces = await _workspaceService.GetStores();
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
			if (Workspaces.Count != 0)
				Workspaces.Clear();

			foreach (var workspace in workspaces)
				Workspaces.Add(workspace);

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

	[RelayCommand]
	async Task GoToProfile()
	{
		if (IsBusy)
			return;

		try
		{
			if (connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				await Shell.Current.DisplayAlert("No connectivity!",
					$"Please check internet and try again.", "OK");
				return;
			}

			await Shell.Current.GoToAsync(nameof(UserProfilePage), true);
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Unable to get user: {ex.Message}");

			await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");

		}
	}

}


