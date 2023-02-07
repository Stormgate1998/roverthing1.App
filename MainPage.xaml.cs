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



namespace roverthing1;

public partial class MainPage : ContentPage
{

    public MainPage(JoinViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

}

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


