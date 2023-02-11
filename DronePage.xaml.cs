using roverthing1.ViewModels;

namespace roverthing1;

public partial class DronePage : ContentPage
{
	public DronePage(DroneViewModel model)
	{
		InitializeComponent();
		BindingContext= model;
	}
}