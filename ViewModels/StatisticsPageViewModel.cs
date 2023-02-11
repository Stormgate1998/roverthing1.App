using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonkeyCache.FileStore;
using roverthing1.Classes;

namespace roverthing1.ViewModels
{
    public partial class StatisticsPageViewModel : ObservableObject
    {
        private readonly HttpClient client;
        private readonly INavigationService navigation;

        private readonly RoverAPIService service;

        [ObservableProperty]
        private string token1;

        [ObservableProperty]
        private JoinObject joinObj;

        [ObservableProperty]
        private RoverMove rover = new RoverMove();

        [ObservableProperty]
        private DroneMove drone = new DroneMove();

        public StatisticsPageViewModel(INavigationService navigation, RoverAPIService service)
        {
            this.navigation = navigation;
            this.service = service;
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
            var rovercach = Barrel.Current.Get<RoverMove>(key: "Rover");
            if (rovercach != null)
            {
                Rover = rovercach;
            }

            var dronecach = Barrel.Current.Get<DroneMove>(key: "Drone");
            if (dronecach != null)
            {
                Drone = dronecach;
            }
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
}
