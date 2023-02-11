using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Formatting;
using roverthing1.Classes;
using MonkeyCache.FileStore;
using roverthing1.ViewModels;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsPageViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}