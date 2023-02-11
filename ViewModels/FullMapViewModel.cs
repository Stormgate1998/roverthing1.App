using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonkeyCache.FileStore;
using roverthing1.Classes;

namespace roverthing1.ViewModels;

public partial class FullMapViewModel : ObservableObject
{
    private readonly INavigationService navigation;

    [ObservableProperty]
    private string token1;

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
    public FullMapViewModel(RoverAPIService service, INavigationService navigation)
    {
        this.navigation = navigation;
      this.service = service;
        Cells = service.map;
        Drawable = new GraphicsDrawable(Cells, service);
        Scale = 1;
        Translationx = 0;
        Translationy = 0;

    }

    [RelayCommand]
    public async Task Start()
    {
        Token1 = Preferences.Default.Get("token", "invalid");

        //this should redirect if the response code is bad.
        if (!(await service.IsValid(Token1)))
        {
            Preferences.Default.Set("token", "invalid");
            await navigation.NavigateToAsync($"{nameof(MainPage)}");
        }
    }

    [RelayCommand]
    private async Task ZoomToDrone()
    {
        Translationy = 790 + service.drone.row;
        Translationx = -1541 + (500 - service.drone.column);
        //Scale = 5;

    }
    [RelayCommand]
    private async Task ZoomToRover()
    {
        Translationy = 790 + service.rover.row;
        Translationx = -1541 + (500 - service.rover.column);
        Scale = 5;
       
    }

    [RelayCommand]
    private async Task ZoomOut()
    {
        Translationx =0;
        Translationy = 0;
        Scale = 1;
    }
    [RelayCommand]
    public async Task NavigateToRover()
    {
        await navigation.NavigateToAsync($"{nameof(PlayGame)}");
    }
    [RelayCommand]
    public async Task NavigateToDrone()
    {
        await navigation.NavigateToAsync($"{nameof(DronePage)}");
    }

    [RelayCommand]
    public async Task NavigateToStats()
    {
        await navigation.NavigateToAsync($"{nameof(StatisticsPage)}");
    }
    [RelayCommand]
    public async Task NavigateToMap()
    {
        await navigation.NavigateToAsync($"{nameof(FullMap)}");
    }

}

