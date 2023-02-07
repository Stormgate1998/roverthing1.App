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

namespace roverthing1.ViewModels
{
    public partial class JoinViewModel : ObservableObject
    {
        private readonly INavigationService navigation;


        private RoverAPIService service = new RoverAPIService();
        public JoinViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
        }


        public async Task Start()
        {
            token = Preferences.Default.Get("token", "invalid");

            if (await service.IsValid(token))
            {
                await navigation.NavigateToAsync($"{nameof(roverthing1.PlayGame)}");
            }
            else
            {
                Preferences.Default.Set("token", "invalid");
            }

        }

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string gameid;


        public string token;
        public JoinViewModel() { }


        [RelayCommand]
        public async Task EnterData()
        {
            JoinObject joinObject = await service.JoinGame(Gameid, Name);
            token = joinObject.token;
            Barrel.Current.Add(key: "JoinObject", data: joinObject, expireIn: TimeSpan.FromHours(1));
            Preferences.Default.Set("token", token);
            await navigation.NavigateToAsync($"{nameof(roverthing1.PlayGame)}");

        }

    }


}
