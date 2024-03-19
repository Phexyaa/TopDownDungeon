
using TopDownDungeon.Enums;
using System.Drawing;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TopDownDungeon.Models;
using System;
using TopDownDungeon.Logic;
using TopDownDungeon.UI;
using TopDownDungeon.Audio;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

builder.Services.AddSingleton<GameState>();
builder.Services.AddSingleton<Screen>();
builder.Services.AddSingleton<MapBuilder>();
builder.Services.AddTransient<AudioController>();
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

AudioController? _audio = host.Services.GetService<AudioController>();
if (_audio == null)
    throw new NullReferenceException("Sound effect service was null");



MapPoint winLocation = new MapPoint(1, 2);


Map map = _mapBuilder.CreateMap(_screen.GetWindowSize().Height, _screen.GetWindowSize().Width);






//Utility
ConsoleColor GetPlayerColor()
{
    if (_state.PlayerHealth > 0 && _state.PlayerHealth <= 100)
    {
        if (_state.PlayerHealth > 0 && _state.PlayerHealth <= 32)
            return ConsoleColor.Red;
        else if (_state.PlayerHealth > 32 && _state.PlayerHealth < 65)
            return ConsoleColor.Yellow;
        else
            return ConsoleColor.Green;
    }
    else
        throw new ArgumentOutOfRangeException(nameof(_state.PlayerHealth));
}


//Logic
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

    if (_screen.CheckMapBounds(newLocation))
    {
        DrainStamina(1);

        _audio.PlayWalkingEffect();
        _screen.DrawPlayer(newLocation, map.PlayerPosition);
        _screen.ShowMessage($"{newLocation.X},{newLocation.Y}");

        map.PlayerPosition = newLocation;

        CheckWinner();
        CheckForEncounter();
        //ChekForFood();
    }
    else
    {
        _audio.PlayAtBoundaryEffect();
        _screen.ShowMessage("At Bounds!");
    }
}
void CheckWinner()
{
    if (map.PlayerPosition.X == winLocation.X
        && map.PlayerPosition.Y == winLocation.Y)
    {
        _audio.PlayWinnerEffect();
        _screen.ShowMessage("WINNER!");
    }
}
void CheckForEncounter()
{
    foreach (var encounter in map.Encounters)
    {
        var x = encounter.Location;
        if (encounter.Location.X == map.PlayerPosition.X
            && encounter.Location.Y == map.PlayerPosition.Y)
        {
            _audio.PlayEncounterEffect();
            _screen.ShowMessage("Encountered enemy; Get ready to fight!");
        }
    }
}

void IncreaseStamina(int amount) => _state.PlayerStamina += amount;
void DrainStamina(int amount) => _state.PlayerStamina -= amount;
void IncreaseHP(int amount) => _state.PlayerHealth += amount;
void DecreaseHP(int amount) => _state.PlayerHealth -= amount;

//Main
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


