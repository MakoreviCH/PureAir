using MonkeyFinder.Model.Dto;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder.ViewModel;

public partial class RegisterViewModel : BaseViewModel
{
	UserService _userService;
	public RegisterViewModel(UserService userService)
	{
		_userService = userService;
	}

	[ObservableProperty] private string _email;
	[ObservableProperty] private string _password;
	[ObservableProperty] private string _firstName;
	[ObservableProperty] private string _lastName;
	[ObservableProperty] private string _phone;

	[RelayCommand]
	async Task Register()
	{
	}
}