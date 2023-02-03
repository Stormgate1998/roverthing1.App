
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;
namespace roverthing1;

public partial class PlayGame : ContentPage
{
    public PlayGame()
    {
        InitializeComponent();
    }
}

[QueryProperty(nameof(Orientation), "orientation")]
[QueryProperty(nameof(Token), "token")]
public partial class PlayGameViewModel : ObservableObject
{
    private readonly HttpClient client;

    public PlayGameViewModel(HttpClient client)
    {
        this.client = client;
    }

    [ObservableProperty]
    private string orientation;

    [ObservableProperty]
    private string token;


    [RelayCommand]
    public async Task Forward()
    {
        try
        {
            var nav = await client.GetFromJsonAsync<Navigation>($"http://snow-rover-pr-7.azurewebsites.net/Game/MovePerseverance?token={Token}&direction=Forward");
            Orientation = nav.orientation;
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
            var nav = await client.GetFromJsonAsync<Navigation>($"http://snow-rover-pr-7.azurewebsites.net/Game/MovePerseverance?token={Token}&direction=Left");
            Orientation = nav.orientation;
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
            var nav = await client.GetFromJsonAsync<Navigation>($"http://snow-rover-pr-7.azurewebsites.net/Game/MovePerseverance?token={Token}&direction=Right");
            Orientation = nav.orientation;
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
            var nav = await client.GetFromJsonAsync<Navigation>($"http://snow-rover-pr-7.azurewebsites.net/Game/MovePerseverance?token={Token}&direction=Reverse");
            Orientation = nav.orientation;
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