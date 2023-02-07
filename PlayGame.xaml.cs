
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;
using roverthing1.Classes;

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


    public async Task Start()
    {
        Token = Preferences.Default.Get("token", "invalid");
        int tokenresponse = 0;
        
        //this should redirect if the response code is bad.
        if (await service.IsValid(Token))
        {
            Preferences.Default.Set("token", "invalid");
            await navigation.NavigateToAsync($"{nameof(MainPage)}");

        }
    }


    [RelayCommand]
    public async Task Forward()
    {
        try
        {
            Orientation = await service.Movedirection(Token, "Reverse");
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
            Orientation = await service.Movedirection(Token, "Left");
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
            Orientation = await service.Movedirection(Token, "Right");
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
            Orientation = await service.Movedirection(Token, "Forward");
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