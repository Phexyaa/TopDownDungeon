
using TopDownDungeon.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TopDownDungeon.Models;
using TopDownDungeon.Logic;
using TopDownDungeon.UI;
using TopDownDungeon.Audio;
using TopDownDungeon.Utility;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

builder.Services.AddSingleton<GameState>();
builder.Services.AddSingleton<Screen>();

builder.Services.AddTransient<MapFactory>();
builder.Services.AddTransient<AudioController>();
builder.Services.AddTransient<BattleEngine>();
builder.Services.AddTransient<EncounterFactory>();
builder.Services.AddTransient<FoodFactory>();
builder.Services.AddTransient<PotionFactory>();

using IHost host = builder.Build();
host.Start();

Screen? _screen = host.Services.GetService<Screen>();
if (_screen == null)
    throw new NullReferenceException("Screen service was null");

MapFactory? _mapBuilder = host.Services.GetService<MapFactory>();
if (_mapBuilder == null)
    throw new NullReferenceException("Map builder service was null");

GameState? _state = host.Services.GetService<GameState>();
if (_state == null)
    throw new NullReferenceException("State machine was null");

AudioController? _audio = host.Services.GetService<AudioController>();
if (_audio == null)
    throw new NullReferenceException("Sound effect service was null");

BattleEngine? _battleEngine = host.Services.GetService<BattleEngine>();
if (_battleEngine == null)
    throw new NullReferenceException("Battle engine was null");



MapPoint winLocation = new MapPoint(1, 2);


Map map = CreateNewMap();








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
Map CreateNewMap()
{
    return _mapBuilder.CreateMap(_screen.GetWindowSize().Height, _screen.GetWindowSize().Width);
}

//Logic
void MovePlayer(MovementDirection direction)
{
    var newLocation = new MapPoint(_state.PlayerPosition.X, _state.PlayerPosition.Y); 

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

        var oldLocation = _state.PlayerPosition;
        _state.PlayerPosition = newLocation;
        _state.VisitedLocations.Add(newLocation);

        _screen.MovePlayerSprite(newLocation, oldLocation);
        _screen.ShowMapMessage($"{newLocation.X},{newLocation.Y}");


        CheckWinner();
        CheckForEncounter();
        //ChekForFood();

    }
    else
    {
        _audio.PlayAtBoundaryEffect();
        _screen.ShowMapMessage("At Bounds!");
    }
}
void CheckWinner()
{
    if (_state.PlayerPosition.X == winLocation.X
        && _state.PlayerPosition.Y == winLocation.Y)
    {
        _audio.PlayWinnerEffect();
        _screen.ShowMapMessage("WINNER!");
    }
}
void CheckForEncounter()
{
    foreach (var encounter in map.Encounters.ToList())
    {
        var x = encounter.Location;
        if (encounter.Location.X == _state.PlayerPosition.X
            && encounter.Location.Y == _state.PlayerPosition.Y)
        {
            _audio.PlayEncounterEffect();
            _screen.ShowMapMessage($"Encountered {encounter.Opponent.Name}. ; Get ready to fight!");
            HandleEncounter(encounter);
        }
    }
}

void HandleEncounter(Encounter encounter)
{
    var outcome = _battleEngine.StartEncounter(encounter, _state.PlayerHealth);

    if (outcome.Success)
    {
        _state.PlayerHealth = outcome.PlayerHP;
        map.Encounters.Remove(encounter);
        _screen.DrawMap(map);
    }
    else
    {
        map = CreateNewMap();
        _state.PlayerPosition.X = _state.Spawn.X;
        _state.PlayerPosition.Y = _state.Spawn.Y;

        _state.ResetState();
        _screen.DrawMap(map);
    }

}

void IncreaseStamina(int amount) => _state.PlayerStamina += amount;
void DrainStamina(int amount) => _state.PlayerStamina -= amount;
void IncreaseHP(int amount) => _state.PlayerHealth += amount;
void DecreaseHP(int amount) => _state.PlayerHealth -= amount;

//Main
_state.PlayerPosition.X = _state.Spawn.X;
_state.PlayerPosition.Y = _state.Spawn.Y;
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
}


