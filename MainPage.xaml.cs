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


namespace roverthing1;

public partial class MainPage : ContentPage
{

    public MainPage(JoinViewModel model)
    {
        InitializeComponent();
        BindingContext= model;
    }

}

public partial class JoinViewModel : ObservableObject
{
    private readonly INavigationService navigation;

    private readonly HttpClient client;
    public JoinViewModel(INavigationService navigation)
    {
        this.navigation= navigation;
        client = new HttpClient
        {
            BaseAddress = new Uri("https://snow-rover.azurewebsites.net/")
        };
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
       var response = await client.GetAsync($"Game/Join?gameId={Gameid}&name={Name}");
        var joinObject = await response.Content.ReadAsAsync<JoinObject>();
        token = joinObject.token;

        await navigation.NavigateToAsync($"{nameof(PlayGame)}?orientation={joinObject.orientation}&token={token}");

    }

}


public class JoinObject
{
    public string token { get; set; }
    public int startingRow { get; set; }
    public int startingColumn { get; set; }
    public int targetRow { get; set; }
    public int targetColumn { get; set; }
    public Neighbor[] neighbors { get; set; }
    public Lowresolutionmap[] lowResolutionMap { get; set; }
    public string orientation { get; set; }

    
}

public class Neighbor
{
    public int row { get; set; }
    public int column { get; set; }
    public int difficulty { get; set; }
}

public class Lowresolutionmap
{
    public int lowerLeftRow { get; set; }
    public int lowerLeftColumn { get; set; }
    public int upperRightRow { get; set; }
    public int upperRightColumn { get; set; }
    public int averageDifficulty { get; set; }
}
