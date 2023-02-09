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

namespace roverthing1;

public partial class FullMap : ContentPage
{
	public FullMap(FullMapViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }
}