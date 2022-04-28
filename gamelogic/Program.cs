using System.Numerics;
using Newtonsoft.Json;
using gamelogic;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;


var v2 = new Vector2(1, 2);
var v22 = new Vector2(1, 3);
var v222 = new Vector2(1, 1);

var ship = new Ship(new Vector2[] {v2, v22, v222});
var ship2 = new Ship(new Vector2[] {v2, v22, v222});

var ships = new List<Ship> {ship,ship2};
var json = JsonConvert.SerializeObject(ships);


var player1 = new GameGrid();
var player2 = new GameGrid();
player1.PlaceShip(ship);

// var torpedo = new Vector2(1, 2);
//
// player1.RecieveHit(torpedo);
// player1.RecieveHit(v22);
// player1.RecieveHit(v222);
//
// Console.WriteLine(player1.Ships[0].IsSunk());
// Console.WriteLine(player1.AllSunk);

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5137/hubs/battleship")
    .WithAutomaticReconnect()
    .AddNewtonsoftJsonProtocol()
    .Build();

var connection2 = new HubConnectionBuilder()
    .WithUrl("http://localhost:5137/hubs/battleship")
    .WithAutomaticReconnect()
    .Build();

await connection.StartAsync();
await connection2.StartAsync();

connection.On<Vector2>("ReceiveShot", (vector2) =>
{
    Console.WriteLine($"con 1 Received hit at {vector2.X}, {vector2.Y}");
});
connection2.On<Vector2>("ReceiveShot", (vector2) =>
{
    Console.WriteLine($"con 2 Received hit at {vector2.X}, {vector2.Y}");
});

await connection.InvokeAsync("Register", "username1" + new Random().Next());
await connection2.InvokeAsync("Register", "username2" + new Random().Next());

var randint = new Random().Next();
await connection.InvokeAsync("CreateGameRoom", "RandomRoom" + randint);
await connection2.InvokeAsync("JoinGameRoom", "RandomRoom" + randint);

;

await connection.InvokeAsync("PlaceShips", ships);
await connection2.InvokeAsync("PlaceShips", ships);
var isHit = await connection.InvokeAsync<bool>("Shoot", v2);
Console.Out.WriteLine(isHit);

while(true);