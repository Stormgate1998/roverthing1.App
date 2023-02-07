﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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



        public async Task<string> MoveAWSD(string token, string direction)
        {
            string returnedDirection = "";

            switch (direction)
            {
                case "North":
                    direction = "Reverse";
                    break;
                case "South":
                    direction = "Forwards";
                    break;
                case "East":
                    direction = "Left";
                    break;
                case "West":
                    direction = "Right";
                    break;
                default:
                    break;
            }

            do
            {
                returnedDirection = await Movedirection(token, direction);
            } while (returnedDirection != direction);
            await Movedirection(token, direction);
            return returnedDirection;
        }


        public async Task<string> GetReady(string token)
        {

            var response = await client.GetAsync($"Game/Status?token={token}");
            var parsed = response.Content.ToString().Trim();
            return parsed;
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
