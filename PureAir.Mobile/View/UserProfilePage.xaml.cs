using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switch = Microsoft.Maui.Controls.Switch;

namespace MonkeyFinder.View;

public partial class UserProfilePage : ContentPage
{
	public UserProfilePage(UserProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private void Switch_OnToggled(object sender, ToggledEventArgs e)
	{

		if (e.Value)
		{
			MauiProgram.ChangeCulture("uk-UA");

		}
		else
		{
			MauiProgram.ChangeCulture("en-US");
		}

		if ((sender as Switch).IsLoaded)
		{
			Shell.Current.DisplayAlert("Info",
				$"Restart", "OK");
			Application.Current.MainPage = new AppShell();
		}

	}
}