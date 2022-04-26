using System.Numerics;
using gamelogic;
using Microsoft.AspNetCore.SignalR.Client;


var v2 = new Vector2(1, 2);
var v22 = new Vector2(1, 3);
var v222 = new Vector2(1, 1);
var ship = new Ship(new Vector2[] {v2, v22, v222});

var player1 = new GameGrid();
var player2 = new GameGrid();
player1.PlaceShip(ship);

var torpedo = new Vector2(1, 2);

player1.RecieveHit(torpedo);
player1.RecieveHit(v22);
player1.RecieveHit(v222);

Console.WriteLine(player1.Ships[0].IsSunk());
Console.WriteLine(player1.AllSunk);

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5137/hubs/battleship")
    .WithAutomaticReconnect()
    .Build();

await connection.StartAsync();

connection.On<int,int>("ReceiveShot", (x,y) =>
{
    Console.WriteLine($"Received hit at {x}, {y}");
});
await connection.InvokeAsync("Register", "username1" + new Random().Next());

while(true);