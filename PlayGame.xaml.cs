
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;
using roverthing1.Classes;
using MonkeyCache.FileStore;

namespace roverthing1;

public partial class PlayGame : ContentPage
{
    public PlayGame(PlayGameViewModel model)
    {
        InitializeComponent();
        BindingContext= model;
    }
}

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
    private string token;

    private JoinObject joinObj;

    [ObservableProperty]
    private bool playingNow = false;

    public async Task Start()
    {
        Token = Preferences.Default.Get("token", "invalid");
        
        //this should redirect if the response code is bad.
        if (!(await service.IsValid(Token)))
        {
            Preferences.Default.Set("token", "invalid");
            await navigation.NavigateToAsync($"{nameof(MainPage)}");
            PlayingNow = false;
        }
        string ready = "";
        while(ready != "Playing")
        {
            ready = await service.GetReady(Token);
            if(!(await service.IsValid(Token)))
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
            Orientation = await service.MoveAWSD(Token, "North");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [RelayCommand]
    public async Task Left()
    {
        try
        {
            Orientation = await service.MoveAWSD(Token, "East");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
       
    }

    [RelayCommand]
    public async Task Right()
    {
        try
        {
            Orientation = await service.MoveAWSD(Token, "West");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [RelayCommand]
    public async Task Reverse()
    {
        try
        {
            Orientation = await service.MoveAWSD(Token, "South");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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