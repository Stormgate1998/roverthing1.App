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
    public partial class DroneViewModel : ObservableObject
    {
        private readonly INavigationService navigation;

        private readonly RoverAPIService service;

        public DroneViewModel(INavigationService navigation, RoverAPIService service)
        {
            this.navigation = navigation;
            this.service = service;
        }

        [ObservableProperty]
        private DroneMove drone = new DroneMove();

        [ObservableProperty]
        private int droneMoveMag;

        [RelayCommand]
        public async Task Start()
        {
            Token1 = Preferences.Default.Get("token", "invalid");

            //this should redirect if the response code is bad.
            if (!(await service.IsValid(Token1)))
            {
                Preferences.Default.Set("token", "invalid");
                await navigation.NavigateToAsync($"{nameof(MainPage)}");
                PlayingNow = false;
            }
            DroneMoveMag = 1;

            var dronecach = Barrel.Current.Get<DroneMove>(key: "Drone");
            if (dronecach != null)
            {
                Drone = dronecach;
            }
            else
            {
                Drone.row = JoinObj.startingRow;
                Drone.column = JoinObj.startingColumn;
            }
        }

        [RelayCommand]
        public async Task MoveDronePathUL(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 1);
        }
        [RelayCommand]
        public async Task MoveDronePathU(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 2);
        }
        [RelayCommand]
        public async Task MoveDronePathUR(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 3);
        }
        [RelayCommand]
        public async Task MoveDronePathL(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 4);
        }
        [RelayCommand]
        public async Task MoveDronePathR(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 5);
        }
        [RelayCommand]
        public async Task MoveDronePathDL(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 6);
        }
        [RelayCommand]
        public async Task MoveDronePathD(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 7);
        }
        [RelayCommand]
        public async Task MoveDronePathDR(int magnitude)
        {
            await service.PersevereQueueAdd(Token1, magnitude, 8);
        }

        [RelayCommand]
        public async Task MoveDroneExecute()
        {
            await service.PersevereQueueRemove();
            Drone = service.drone;
            Barrel.Current.Add(key: "Drone", data: Drone, expireIn: TimeSpan.FromHours(1));
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
