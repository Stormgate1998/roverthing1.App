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