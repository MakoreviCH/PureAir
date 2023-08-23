using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace MonkeyFinder.View;

public partial class WorkspacePage : ContentPage
{
	public WorkspacePage(WorkspaceDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		Shell.SetTabBarIsVisible(this, false);
	}

	
}