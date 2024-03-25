using TopDownDungeon.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TopDownDungeon.Models;
using TopDownDungeon.Logic;
using TopDownDungeon.UI;
using TopDownDungeon.Audio;
using TopDownDungeon.Utility;
using System.Diagnostics.Metrics;
using System.ComponentModel.DataAnnotations;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

builder.Services.AddSingleton<GameState>();
builder.Services.AddSingleton<Canvas>();

builder.Services.AddTransient<MapFactory>();
builder.Services.AddTransient<AudioController>();
builder.Services.AddTransient<BattleEngine>();
builder.Services.AddTransient<EncounterFactory>();
builder.Services.AddTransient<FoodFactory>();
builder.Services.AddTransient<PotionFactory>();
builder.Services.AddTransient<InterfaceHelper>();
builder.Services.AddTransient<TextPrompts>();

using IHost host = builder.Build();
host.Start();

//Services
Canvas? _canvas = host.Services.GetService<Canvas>();
if (_canvas == null)
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

//Game Defaults
const int PlayerHealth = 100;
const int PlayerStamina = 20;
const int PlayerSpeed = 1;
const bool CanMove = true;

//Variables
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
    return _mapBuilder.CreateMap(_canvas.GetWindowSize().Height, _canvas.GetWindowSize().Width);
}

//Logic
void MovePlayer(MovementDirection direction)
{
    var newLocation = new MapPoint(_state.PlayerLocation.X, _state.PlayerLocation.Y);

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

    if (_canvas.CheckMapBounds(newLocation))
    {
        DrainStamina(1);

        _audio.PlayWalkingEffect();

        var oldLocation = _state.PlayerLocation;
        _state.PlayerLocation = newLocation;
        _state.VisitedLocations.Add(newLocation);

        _canvas.MovePlayerSprite(newLocation, oldLocation);

        ExamineNewLocation();
    }
    else
    {
        _audio.PlayAtBoundaryEffect();
        _canvas.ShowMapMessage("At Bounds!");
    }

    _canvas.UpdateHud();
}

void ExamineNewLocation()
{
    if (CheckForWinState())
        HandleWinState();

    var encounterCheck = CheckForEncounter();
    if (encounterCheck.IsEncounter && encounterCheck.Encounter != null)
        HandleEncounter(encounterCheck.Encounter);

    var foodcheck = CheckForFood();
    if (foodcheck.IsFood && foodcheck.Food != null)
    {
        EatFood(foodcheck.Food);
    }

    var potionCheck = CheckForPotion();
    if (potionCheck.IsPotion && potionCheck.Potion != null)
    {
        DrinkPotion(potionCheck.Potion);
    }
}

bool CheckForWinState()
{
    return (_state.PlayerLocation.X == winLocation.X
        && _state.PlayerLocation.Y == winLocation.Y);
}
void HandleWinState()
{
    _audio.PlayWinnerEffect();
    _canvas.ShowMapMessage("WINNER!");
}
(bool IsEncounter, Encounter? Encounter) CheckForEncounter()
{
    bool isEncounter = false;
    Encounter? foundEncounter = null;
    foreach (var encounter in map.Encounters)
    {
        isEncounter = (encounter.Location.X == _state.PlayerLocation.X && encounter.Location.Y == _state.PlayerLocation.Y);
        if (isEncounter)
        {
            foundEncounter = encounter;
            break;
        }
    }
    return (isEncounter, foundEncounter);
}
(bool IsFood, Food? Food) CheckForFood()
{
    bool isFood = false;
    Food? foundFood = null;
    foreach (var food in map.Meals)
    {
        isFood = (food.Location.X == _state.PlayerLocation.X && food.Location.Y == _state.PlayerLocation.Y);
        if (isFood)
        {
            foundFood = food;
            break;
        }
    }
    return (isFood, foundFood);
}
void EatFood(Food food)
{
    _canvas.ShowMapMessage($"Found {food} increasing HP and stamina by {food.EffectValue}");
    _state.PlayerHealth += food.EffectValue;
    _state.PlayerStamina += food.EffectValue;
}
(bool IsPotion, Potion? Potion) CheckForPotion()
{
    bool isPotion = false;
    Potion? foundPotion = null;
    foreach (var potion in map.Potions)
    {
        isPotion = (potion.Location.X == _state.PlayerLocation.X && potion.Location.Y == _state.PlayerLocation.Y);
        if (isPotion)
            { foundPotion = potion;
            break;
        }
    }
    return (isPotion, foundPotion);
}
void DrinkPotion(Potion potion)
{
    _canvas.ShowMapMessage($"Found {potion.Effect} potion, bottoms up!");
    switch (potion.Effect)
    {
        case PotionEffect.Heal:
            _state.PlayerHealth += potion.EffectValue;
            break;
        case PotionEffect.Harm:
            _state.PlayerHealth -= potion.EffectValue;
            break;
        case PotionEffect.Poison:
            break;
        case PotionEffect.Invigorate:
            _state.PlayerStamina += potion.EffectValue;
            break;
        case PotionEffect.Drunken:
            _state.PlayerStamina -= potion.EffectValue;
            break;
        case PotionEffect.Slow:
            break;
        case PotionEffect.Speed:
            break;
        default:
            break;
    }
}
void HandleEncounter(Encounter encounter)
{
    _audio.PlayEncounterEffect();
    _canvas.ShowMapMessage($"Encountered {encounter.Opponent.Name}. ; Get ready to fight!");
    var outcome = _battleEngine.StartEncounter(encounter, _state.PlayerHealth);

    if (outcome.Success)
    {
        _state.PlayerHealth = outcome.PlayerHP;
        map.Encounters.Remove(encounter);
        _canvas.DrawMap(map);
    }
    else
    {
        map = CreateNewMap();
        _state.PlayerLocation.X = _state.Spawn.X;
        _state.PlayerLocation.Y = _state.Spawn.Y;

        ResetState();
        _canvas.DrawMap(map);
    }

}
void DrainStamina(int amount) => _state.PlayerStamina -= amount;
void ResetState()
{
    _state.PlayerHealth = PlayerHealth;
    _state.PlayerStamina = PlayerStamina;
    _state.PlayerSpeed = PlayerSpeed;
    _state.CanMove = CanMove;
    _state.VisitedLocations.Clear();
}

//Main
_state.PlayerLocation.X = _state.Spawn.X;
_state.PlayerLocation.Y = _state.Spawn.Y;
ResetState();
_canvas.ShowStartMenu();
_canvas.DrawMap(map);

//Core Loop
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
