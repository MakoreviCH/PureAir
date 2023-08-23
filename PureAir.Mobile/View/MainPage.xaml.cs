namespace MonkeyFinder.View;

public partial class MainPage : ContentPage
{
	public MainPage(WorkspacesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		Shell.SetTabBarIsVisible(this, true);

	}
}

