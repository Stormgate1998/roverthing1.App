using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonkeyCache.FileStore;
using System.Xml.Linq;
using roverthing1.Classes;

namespace roverthing1.ViewModels;

public partial class FullMapViewModel : ObservableObject
{
    [ObservableProperty]
    private Dictionary<string, MapCell> cells;

    public RoverAPIService service;

    [ObservableProperty]
    private int translationx;

    [ObservableProperty]
    private int translationy;

    [ObservableProperty]
    private double scale;

    [ObservableProperty]
    private GraphicsDrawable drawable;
    public FullMapViewModel(RoverAPIService service)
    {
      this.service = service;
        Cells = service.map;
        Drawable = new GraphicsDrawable(Cells, service);
        Scale = 1;
        Translationx = 0;
        Translationy = 0;

    }

    [RelayCommand]
    private async Task ZoomToDrone()
    {
        Translationx = 790 + service.drone.row;
        Translationy = -1541 + (500 - service.drone.column);
        Scale = 5;

    }
    [RelayCommand]
    private async Task ZoomToRover()
    {
        Translationx = 790 + service.rover.row;
        Translationy = -1541 + (500 - service.rover.column);
        Scale = 5;
       
    }

    [RelayCommand]
    private async Task ZoomOut()
    {
        Translationx =0;
        Translationy = 0;
        Scale = 1;
    }


}

