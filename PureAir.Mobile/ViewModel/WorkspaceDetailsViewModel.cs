using MonkeyFinder.Services;
using System.Globalization;
using MonkeyFinder.Resources.Languages;

namespace MonkeyFinder.ViewModel;

[QueryProperty(nameof(Workspace), "workspace")]
public partial class WorkspaceDetailsViewModel : BaseViewModel
{
	[ObservableProperty] 
	Workspace _workspace;

	[ObservableProperty]
	WorkspaceData _data;

	[ObservableProperty]
	string _decision;
	IMap map;
	IConnectivity _connectivity;
	private DatasService _dataService;
	private UserService _userService;
	public WorkspaceDetailsViewModel(IMap map, DatasService dataService, IConnectivity connectivity, UserService userService)
	{
		this.map = map;
		_dataService = dataService;
		_connectivity = connectivity;
		_userService = userService;
		Task.Run(async () => await GetDatasAsync());
	}

	[ObservableProperty]
	bool isRefreshing;


	[RelayCommand]
	async Task GetDatasAsync()
	{
		if (IsBusy)
			return;

		try
		{
			if (_connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				await Shell.Current.DisplayAlert("No connectivity!",
					$"Please check internet and try again.", "OK");
				return;
			}

			IsBusy = true;

			
			var decisionResponse = await _dataService.GetWorkspaceDisaster(Workspace.Id);

			if (decisionResponse.Decision == "low")
				Decision = Strings.Decision_Low;
			else if(decisionResponse.Decision=="medium")
				Decision = Strings.Decision_Medium;
			else if (decisionResponse.Decision == "high")
				Decision = Strings.Decision_High;
			else
				Decision = Strings.Decision_Error;

			Data = await _dataService.GetWorkspaceDatas(Workspace.Id);

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



}
public class InverseBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return !((bool)value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value;
		//throw new NotImplementedException();
	}
}
