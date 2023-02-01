using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;


namespace roverthing1;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

public partial class JoinViewModel : ObservableObject
{

    private static HttpClient client;
    private void JoinGame(HttpClient _client)
    {
        client = _client;
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
        try
        {
            JoinObject response = await client.GetAsync($"http://snow-rover-pr-7.azurewebsites.net/Game/Join?gameId={Gameid}&name={Name}");
            token = response.token;
            
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }

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

    public static implicit operator JoinObject(HttpResponseMessage v)
    {
        throw new NotImplementedException();
    }
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
