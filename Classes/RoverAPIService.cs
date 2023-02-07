using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static UIKit.UIGestureRecognizer;

namespace roverthing1.Classes
{
   
    public class RoverAPIService
    {
        private readonly HttpClient client;

        public RoverAPIService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://snow-rover.azurewebsites.net/")
            };
        }


        public async Task<JoinObject> JoinGame(string gameid, string name)
        {
            var response = await client.GetAsync($"Game/Join?gameId={gameid}&name={name}");
            var joinobj  = await response.Content.ReadAsAsync<JoinObject>();
            return joinobj;
        }


        public async Task<bool> IsValid(string token)
        {
            if(token == "invalid")
            {
                return false;
            }
            var response = await client.GetAsync($"Game/Status?token={token}");
            var tokenresponse = (int)response.StatusCode;

           return (tokenresponse == 200);

        }


        public async Task<string> Movedirection(string token, string direction)
        {
            if(await IsValid(token))
            {
                var nav = await client.GetFromJsonAsync<Navigation>($"Game/MovePerseverance?token={token}&direction={direction}");
                return nav.orientation;
            }
            else
            {
                return null;
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

}
