using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Maui.Graphics.Color;

namespace roverthing1.Classes
{

    public class RoverAPIService
    {
        private readonly HttpClient client;

        private JoinObject joinobject;

        public Dictionary<string, MapCell> map;

        public Queue<PerseverenceMove> dronemoves = new Queue<PerseverenceMove>();
        public RoverAPIService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://snow-rover.azurewebsites.net/")
            };
        }

        public RoverMove rover = new RoverMove();
        public DroneMove drone = new DroneMove();


        public async Task<JoinObject> JoinGame(string gameid, string name)
        {
            var response = await client.GetAsync($"Game/Join?gameId={gameid}&name={name}");
            joinobject = await response.Content.ReadAsAsync<JoinObject>();
            map = CreateMap(joinobject.lowResolutionMap);
            UpdateMapCellDifficulty(map, joinobject.neighbors);
            return joinobject;
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
                rover = await client.GetFromJsonAsync<RoverMove>($"Game/MovePerseverance?token={token}&direction={direction}");
                UpdateMap(rover);
                return rover.orientation;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> MoveAWSD(string token, string direction)
        {
            while (rover.orientation != direction)
            {
                await Movedirection(token, "Left");
            }

            await Movedirection(token, "Forward");
            return rover.orientation;
        }

        //returns the game status 
        public async Task<string> GetReady(string token)
        {
            var response = await client.GetAsync($"Game/Status?token={token}");
            var parsed = response.Content.ToString().Trim();
            return parsed;
        }
        //give it a token, a number 1 or 2, and a direction 1-8. Diagonals are possible. It will move Perseverence that direction
        public async Task MovePerseverence(string token, int moveamount, int direction)
        {
            int responsecode;
            if (moveamount > 2)
            {
                moveamount = 2;
            }
            switch (direction)
            {
                case 1://diagonal up and left
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row - moveamount}&destinationColumn={drone.column + moveamount}");
                    responsecode = drone.StatusCode;
                    break;
                case 2://diagonal up
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row}&destinationColumn={drone.column + moveamount}");
                    responsecode = drone.StatusCode;
                    break;
                case 3://diagonal up and right
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row + moveamount}&destinationColumn={drone.column + moveamount}");
                    responsecode = drone.StatusCode;
                    break;
                case 4://left
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row - moveamount}&destinationColumn={drone.column}");
                    responsecode = drone.StatusCode;
                    break;
                case 5://right
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row + moveamount}&destinationColumn={drone.column}");
                    responsecode = drone.StatusCode; 
                    break;
                case 6://down and left
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row - moveamount}&destinationColumn={drone.column - moveamount}");
                    responsecode = drone.StatusCode; 
                    break;
                case 7://down
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row}&destinationColumn={drone.column - moveamount}");
                    responsecode = drone.StatusCode; 
                    break;
                case 8://down and right
                    drone = await client.GetFromJsonAsync<DroneMove>($"Game/MoveIngenuity?token={token}&destinationRow={drone.row + moveamount}&destinationColumn={drone.column - moveamount}");
                    responsecode = drone.StatusCode;
                    break;
                default:
                    Console.WriteLine("Invalid direction");
                    break;
            }
            UpdateMap(drone);

        }
        //adds to the queue of moves for perseverence
        public async Task PersevereQueueAdd(string token, int moveamount, int direction)
        {
            PerseverenceMove move = new PerseverenceMove(token, moveamount, direction);
            dronemoves.Enqueue(move);
        }
        //takes things in the queue and runs them.
        public async Task PersevereQueueRemove()
        {
            var waittimer = 0;
            while (dronemoves.Count > 0)
            {
                waittimer++;
                waittimer = waittimer % 10;
                PerseverenceMove move = dronemoves.Dequeue();
               
                    await MovePerseverence(move.token, move.moveamount, move.direction);

                if(waittimer == 9)
                {
                    //System.Threading.Thread.Sleep(500);
                }
            }
        }


        public DroneMove GetDroneInfo()
        {
            return drone;
        }

        public RoverMove GetRoverInfo()
        {
            return rover;
        }

        public Dictionary<string, MapCell> CreateMap(Lowresolutionmap[] maps)
        {
            int maxRow = 0;
            int maxColumn = 0;
            foreach (var map in maps)
            {
                if (map.upperRightRow > maxRow)
                    maxRow = map.upperRightRow;
                if (map.upperRightColumn > maxColumn)
                    maxColumn = map.upperRightColumn;
            }
            Dictionary<string, MapCell> mapDict = new Dictionary<string, MapCell>();
            foreach (var map in maps)
            {
                for (int i = map.lowerLeftRow; i <= map.upperRightRow; i++)
                {
                    for (int j = map.lowerLeftColumn; j <= map.upperRightColumn; j++)
                    {
                        MapCell cell = new MapCell();
                        cell.difficulty = map.averageDifficulty;
                        cell.isdiscovered = false;
                        mapDict.Add(i + "," + j, cell);
                    }
                }
            }

            return mapDict;

        }



        public void UpdateMapCellDifficulty(Dictionary<string, MapCell> mapDict, Neighbor[] neighbors)
        {
            MapCell cell;
            foreach (var neighbor in neighbors)
            {
                cell = mapDict[$"{neighbor.row},{neighbor.column}"];
                cell.difficulty = neighbor.difficulty;
                cell.isdiscovered = true;
                mapDict[$"{neighbor.row},{neighbor.column}"] = cell;

            }
        }

        public void UpdateMap(RoverMove move)
        {
            UpdateMapCellDifficulty(map, move.neighbors);
           
        }

        public void UpdateMap(DroneMove move)
        {
            UpdateMapCellDifficulty(map, move.neighbors);
        }


        

        


    }

    public class PerseverenceMove
    {
        public string token;
        public int moveamount;
        public int direction;
        public PerseverenceMove(string token, int moveamount, int direction)
        {
            this.moveamount = moveamount;
            this.direction = direction;
            this.token = token;
        }
    }

    public struct MapCell
    {
        public int difficulty;
        public bool isdiscovered;
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
        public int StatusCode { get; set; }
    }


    public class DroneMove
    {
        public int row { get; set; }
        public int column { get; set; }
        public int batteryLevel { get; set; }
        public Neighbor[] neighbors { get; set; }
        public string message { get; set; }
        public int StatusCode { get; set; }
    }

    public class GraphicsDrawable : IDrawable
    {
        private Dictionary<string, MapCell> cells;
        private RoverAPIService service;

        public GraphicsDrawable(Dictionary<string, MapCell> cells, RoverAPIService service)
        {
            this.cells = cells;
            this.service = service;
        }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var interval = 20;
            var keys = cells.Keys.ToList();
            foreach (var key in keys)
            {
                string[] parts = key.Split(',');
                int row = int.Parse(parts[0]);
                int column = int.Parse(parts[1]);

                MapCell cell = cells[key];
                if (cell.isdiscovered)
                {
                    int colorValue = (int)Math.Floor((double)cell.difficulty / interval);
                    int red = colorValue * interval;
                    int green = 255 - (colorValue * interval);
                    int blue = 0;
                    canvas.StrokeColor = Color.FromRgb(red, green, blue);
                    canvas.FillColor = Color.FromRgb(red, green, blue);
                }else
                {
                    int colorValue = (int)Math.Floor((double)cell.difficulty / interval);
                    int red = 255 - (colorValue * interval);
                    int green = red;
                    int blue = red;
                    canvas.StrokeColor = Color.FromRgb(red, green, blue);
                    canvas.FillColor = Color.FromRgb(red, green, blue);
                }
                canvas.StrokeSize = 1;
                canvas.DrawRectangle(row, 500-column, 1, 1);
            }
            canvas.StrokeColor = Color.FromRgb(0, 0, 255);
            canvas.FillColor = Color.FromRgb(0, 0, 255);
            canvas.DrawRectangle(service.drone.row, 500 - service.drone.column, 1, 1);
            canvas.StrokeColor = Color.FromRgb(0, 0, 0);
            canvas.FillColor = Color.FromRgb(0, 0, 0);
            canvas.DrawRectangle(service.rover.row, 500 - service.rover.column, 1, 1);
        }

    }


}
