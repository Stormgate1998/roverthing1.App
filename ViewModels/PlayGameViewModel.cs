using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonkeyCache.FileStore;
using roverthing1.Classes;
using static UIKit.UIGestureRecognizer;

namespace roverthing1.ViewModels
{
    public partial class PlayGameViewModel : ObservableObject
    {
        private readonly HttpClient client;
        private readonly INavigationService navigation;

        private readonly RoverAPIService service = new RoverAPIService();

        public PlayGameViewModel(INavigationService navigation)
        {
            this.navigation = navigation;

            client = new HttpClient
            {
                BaseAddress = new Uri("https://snow-rover.azurewebsites.net/")
            };
        }

        [ObservableProperty]
        private string orientation;

        [ObservableProperty]
        private string token1;

        private JoinObject joinObj;

        [ObservableProperty]
        private bool playingNow = false;


        [ObservableProperty]
        private RoverMove rover;

        [ObservableProperty]
        private DroneMove drone;

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
            string ready = "";
            while (ready != "Playing")
            {
                ready = await service.GetReady(Token1);
                if (!(await service.IsValid(Token1)))
                {
                    Preferences.Default.Set("token", "invalid");
                    await navigation.NavigateToAsync($"{nameof(MainPage)}");
                    PlayingNow = false;
                }
            }

            PlayingNow = true;
            joinObj = Barrel.Current.Get<JoinObject>(key: "JoinObject");
            Orientation = joinObj.orientation;
        }

        [RelayCommand]
        public async Task Forward()
        {
            try
            {
                Orientation = await service.MoveAWSD(Token1, "North");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Drone = service.GetDroneInfo();
        }

        [RelayCommand]
        public async Task Left()
        {
            try
            {
                Orientation = await service.MoveAWSD(Token1, "East");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Drone = service.GetDroneInfo();
        }

        [RelayCommand]
        public async Task Right()
        {
            try
            {
                Orientation = await service.MoveAWSD(Token1, "West");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Drone = service.GetDroneInfo();
        }

        [RelayCommand]
        public async Task Reverse()
        {
            try
            {
                Orientation = await service.MoveAWSD(Token1, "South");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Drone = service.GetDroneInfo();
        }


    }

    public class Navigation
    {
        public int row { get; set; }
        public int column { get; set; }
        public int batteryLevel { get; set; }
        public Neighbor[] neighbors { get; set; }
        public string message { get; set; }
        public string orientation { get; set; }
    }
}
