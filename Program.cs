
using TopDownDungeon.Enums;
using System.Drawing;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using TopDownDungeon.UI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TopDownDungeon.Services.Logic;
using TopDownDungeon.Models;
using System;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

builder.Services.AddSingleton<GameState>();
builder.Services.AddSingleton<Screen>();
builder.Services.AddSingleton<MapBuilder>();
using IHost host = builder.Build();

host.Start();

Screen? _screen = host.Services.GetService<Screen>();
if (_screen == null)
    throw new NullReferenceException("Screen service was null");

MapBuilder? _mapBuilder = host.Services.GetService<MapBuilder>();
if (_mapBuilder == null)
    throw new NullReferenceException("Map builder service was null");

GameState? _state = host.Services.GetService<GameState>();
if (_state == null)
    throw new NullReferenceException("State machine was null");


int playerFood = 10;
int playerHealth = 100;
int playerStamina = 20;
int playerSpeed = 1;
bool canMove = true;
MapPoint winLocation = new MapPoint(1, 2);

(int Height, int Width) currentWindowSize = _screen.GetWindowSize();
(int Height, int Width) lastKnownWindowSize = (0, 0);

Map map = _mapBuilder.CreateMap(currentWindowSize.Height, currentWindowSize.Width);






//Utility
ConsoleColor GetPlayerColor()
{
    if (playerHealth > 0 && playerHealth <= 100)
    {
        if (playerHealth > 0 && playerHealth <= 32)
            return ConsoleColor.Red;
        else if (playerHealth > 32 && playerHealth < 65)
            return ConsoleColor.Yellow;
        else
            return ConsoleColor.Green;
    }
    else
        throw new ArgumentOutOfRangeException(nameof(playerHealth));
}


//Logic
bool CheckWinner()
{
    return map.PlayerPosition == winLocation;
}


void MovePlayer(MovementDirection direction)
{
    var newLocation = new MapPoint(map.PlayerPosition.X, map.PlayerPosition.Y); 

    switch (direction)
    {
        case MovementDirection.North:
            newLocation.Y--;
            break;
        case MovementDirection.South:
            newLocation.Y++;
            break;
        case MovementDirection.East:
            newLocation.X++;
            break;
        case MovementDirection.West:
            newLocation.X--;
            break;
    }

    if (_screen.CheckBounds(newLocation))
    {
        DrainStamina(1);

        Console.SetCursorPosition(map.PlayerPosition.X, map.PlayerPosition.Y);
        Console.Write(" ");

        map.PlayerPosition = newLocation;

        _screen.DrawPlayer(newLocation);
        _screen.ShowMessage($"{newLocation.X},{newLocation.Y}");
    }
    else
    {
        Console.Beep();
        _screen.ShowMessage("At Bounds!");
    }
}
void IncreaseStamina(int amount) => playerStamina += amount;
void DrainStamina(int amount) => playerStamina -= amount;
void IncreaseHP(int amount) => playerHealth += amount;
void DecreaseHP(int amount) => playerHealth -= amount;

//Main
Console.CursorVisible = false;
Console.SetCursorPosition(_state.Spawn.X, _state.Spawn.Y);
map.PlayerPosition.X = _state.Spawn.X;
map.PlayerPosition.Y = _state.Spawn.Y;
_screen.DrawMap(map);

while (true)
{
    var input = Console.ReadKey(true);
    switch (input.Key)
    {
        case ConsoleKey.Escape:
            break;
        case ConsoleKey.Spacebar:
            break;
        case ConsoleKey.LeftArrow:
            MovePlayer(MovementDirection.West);
            break;
        case ConsoleKey.UpArrow:
            MovePlayer(MovementDirection.North);
            break;
        case ConsoleKey.RightArrow:
            MovePlayer(MovementDirection.East);
            break;
        case ConsoleKey.DownArrow:
            MovePlayer(MovementDirection.South);
            break;
        case ConsoleKey.A:
            MovePlayer(MovementDirection.West);
            break;
        case ConsoleKey.D:
            MovePlayer(MovementDirection.East);
            break;
        case ConsoleKey.S:
            MovePlayer(MovementDirection.South);
            break;
        case ConsoleKey.W:
            MovePlayer(MovementDirection.North);
            break;
    }

    if (CheckWinner())
    {
        Console.Beep();
        _screen.ShowMessage("WINNER!");
    }
    //if (CheckForFood())
    //{
    //    ConsumeFood(food);
    //}
    //if (CheckForMonster())
    //{
    //    Console.Beep();
    //    Console.Beep();
    //    Console.Beep();
    //    _screen.ShowMessage("MONSTER!");
    //}
    //CheckForFood(currentPosition);
    //CheckFormonster(currentPosition);
}


