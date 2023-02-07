using CoreImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
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

        public RoverMove rover;
        public DroneMove drone;


        public async Task<JoinObject> JoinGame(string gameid, string name)
        {
            var response = await client.GetAsync($"Game/Join?gameId={gameid}&name={name}");
            var joinobj = await response.Content.ReadAsAsync<JoinObject>();
            return joinobj;
        }


        public async Task<bool> IsValid(string token)
        {
            if (token == "invalid")
            {
                return false;
            }
            var response = await client.GetAsync($"Game/Status?token={token}");
            var tokenresponse = (int)response.StatusCode;

            return (tokenresponse == 200);

        }


        public async Task<string> Movedirection(string token, string direction)
        {
            if (await IsValid(token))
            {
                var nav = await client.GetFromJsonAsync<RoverMove>($"Game/MovePerseverance?token={token}&direction={direction}");
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

        public async void MovePerseverence(string token, int moveamount, int direction, DroneMove drone)
        {
            if(moveamount > 2)
            {
                moveamount = 2;
            }
            switch (direction)
            {
                case 1://diagonal up and left
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row - moveamount}&destinationColumn={drone.column + moveamount}");
                    break;
                case 2://diagonal up
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row}&destinationColumn={drone.column + moveamount}");
                    break;
                case 3://diagonal up and right
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row + moveamount}&destinationColumn={drone.column + moveamount}");
                    break;
                case 4://left
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row - moveamount}&destinationColumn={drone.column}");
                    break;
                case 5://right
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row + moveamount}&destinationColumn={drone.column}");
                    break;
                case 6://down and left
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row - moveamount}&destinationColumn={drone.column - moveamount}");
                    break;
                case 7://down
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row}&destinationColumn={drone.column - moveamount}");
                    break;
                case 8://down and right
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row + moveamount}&destinationColumn={drone.column - moveamount}");
                    break;
                default:
                    Console.WriteLine("Invalid direction");
                    break;
            }

        }

        public void MapUpdate(RoverMove rover)
        {
            
        }

        public void MapUpdate(DroneMove drone)
        {

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


    public class RoverMove
    {
        public int row { get; set; }
        public int column { get; set; }
        public int batteryLevel { get; set; }
        public Neighbor[] neighbors { get; set; }
        public string message { get; set; }
        public string orientation { get; set; }
    }


    public class DroneMove
    {
        public int row { get; set; }
        public int column { get; set; }
        public int batteryLevel { get; set; }
        public Neighbor[] neighbors { get; set; }
        public string message { get; set; }
    }

}
