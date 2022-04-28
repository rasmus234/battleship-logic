using System.Numerics;
using gamelogic;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

var v2 = new Vector2(1, 2);
var v22 = new Vector2(1, 3);
var v222 = new Vector2(1, 1);

var ship = new Ship(new[] {v2, v22, v222});
var ship2 = new Ship(new[] {v2, v22, v222});

var ships = new List<Ship> {ship,ship2};


var player1 = new GameGrid();
var player2 = new GameGrid();

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5137/hubs/battleship")
    .WithAutomaticReconnect()
    .AddNewtonsoftJsonProtocol()
    .Build();

var connection2 = new HubConnectionBuilder()
    .WithUrl("http://localhost:5137/hubs/battleship")
    .WithAutomaticReconnect()
    .AddNewtonsoftJsonProtocol()
    .Build();

await connection.StartAsync();
await connection2.StartAsync();

connection.On<Vector2>("ReceiveShot", vector2 =>
{
    Console.WriteLine($"con 1 Received hit at {vector2.X}, {vector2.Y}");
});
connection2.On<Vector2>("ReceiveShot", vector2 =>
{
    Console.WriteLine($"con 2 Received hit at {vector2.X}, {vector2.Y}");
});

connection.On<Ship>("OpponentShipSunk", ship =>
{
    Console.WriteLine($"con 1 Opponent ship sunk");
});

connection2.On<Ship>("OpponentShipSunk", ship =>
{
    Console.WriteLine($"con 2 Opponent ship sunk");
});

await connection.InvokeAsync("Register", "username1" + new Random().Next());
await connection2.InvokeAsync("Register", "username2" + new Random().Next());

var randint = new Random().Next();
await connection.InvokeAsync("CreateGameRoom", "RandomRoom" + randint);
await connection2.InvokeAsync("JoinGameRoom", "RandomRoom" + randint);

await connection.InvokeAsync("PlaceShips", ships);
await connection2.InvokeAsync("PlaceShips", ships);
var isHit = await connection.InvokeAsync<bool>("Shoot", new Vector2(1, 2)); 
 await connection.InvokeAsync<bool>("Shoot", new Vector2(1, 3)); 
 await connection.InvokeAsync<bool>("Shoot", new Vector2(1, 1)); 
Console.Out.WriteLine(isHit);

while(true);