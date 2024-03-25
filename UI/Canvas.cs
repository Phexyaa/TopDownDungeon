using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using TopDownDungeon.Enums;
using TopDownDungeon.Logic;
using TopDownDungeon.Models;
using TopDownDungeon.Utility;

namespace TopDownDungeon.UI;
internal class Canvas
{
    private readonly GameState _state;
    private readonly InterfaceHelper _defaults;
    private readonly TextPrompts _prompts;

    private readonly (int height, int width) _originalWindowSize;
    private int borderWidth = 1;
    private int topPadding = 1;
    private int bottomPadding = 2;
    private int bottomBufferRow => (GetWindowSize().Height - 1);

    public Canvas(GameState state, InterfaceHelper defaults, TextPrompts prompts)
    {
        _state = state;
        _state.SetSpawnPoint(new MapPoint(GetWindowSize().Width - 2, GetWindowSize().Height - 3));
        _defaults = defaults;
        _originalWindowSize = GetWindowSize();
        _prompts = prompts;

        Console.CursorVisible = false;
    }

    //Window Utility Methods
    internal (int Width, int Height) GetWindowSize()
    {
        return (Console.WindowWidth, Console.WindowHeight);
    }
    private bool CheckWindowSize()
    {
        if (GetWindowSize() == _originalWindowSize)
            return true;
        else
            return false;
    }
    private void ResetWindowSize()
    {
        Console.SetWindowSize(_originalWindowSize.width, _originalWindowSize.height);
    }

    //Message and Heads Up Display Methods
    internal void ShowMessage(string message)
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.MessageColor];
        Console.WriteLine(message);
        Console.ResetColor();
    }
    internal void ShowModalMessage(string message)
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.MessageColor];
        ShowMessage(message);
        ShowMessage("Press 'Enter' to continue");
        Console.ReadLine();
        Console.ResetColor();
    }
    internal void ShowMapMessage(string message)
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.MessageColor];
        Console.SetCursorPosition(0, bottomBufferRow);
        Console.Write(message.PadRight(GetWindowSize().Width));
        Console.ResetColor();
    }
    internal void UpdateHud()
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.MessageColor];
        Console.SetCursorPosition(0, 0);
        Console.Write($"Health: {_state.PlayerHealth}, Stamina: {_state.PlayerStamina}, Location: {_state.PlayerLocation.X},{_state.PlayerLocation.Y}"
            .PadRight(GetWindowSize().Width));
        Console.ResetColor();
    }
    internal void ShowStartMenu()
    {
        Console.Clear();
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.MessageColor];
        DrawTopBorder();
        Console.WriteLine(_prompts.SplashScreenMessage);
        Console.WriteLine(_prompts.ControlMappings);
        Console.WriteLine("Press any key to continue...");
        DrawBottomBorder();
        Console.ResetColor();
        Console.ReadLine();
    }

    //Map Utility Methods
    internal bool CheckMapBounds(MapPoint point)
    {
        var windowSize = GetWindowSize();
        var checkX = point.X > 0 && point.X < windowSize.Width - borderWidth;
        var checkY = point.Y > topPadding && point.Y < windowSize.Height - bottomPadding;

        return checkX && checkY;
    }
    internal bool CheckForSpawnOverlap(MapPoint point)
    {
        return (point.X != _state.Spawn.X && point.Y != _state.Spawn.Y);
    }
    internal void ClearScreen() => Console.Clear();
    internal void DrawPlayerSprite(MapPoint location)
    {
        if (!CheckWindowSize()) { ResetWindowSize(); }

        Console.ForegroundColor = _defaults.DisplayColors[GameColor.PlayerColor];

        Console.SetCursorPosition(location.X, location.Y);
        Console.Write(_defaults.SpriteMap[MapSymbol.Player]);

        Console.ResetColor();
    }
    internal void MovePlayerSprite(MapPoint newLocation, MapPoint oldLocation)
    {
        Console.SetCursorPosition(oldLocation.X, oldLocation.Y);

        DrawPlayerSprite(newLocation);
        MarkVisitedLocation(oldLocation);

        Console.ResetColor();
    }

    //Screen Painting Methods
    private void DrawTopBorder(bool mapBorder = false)
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.Border];

        if (mapBorder)
            Console.SetCursorPosition(0, 1);
        else
            Console.SetCursorPosition(0, 0);

        Console.Write("".PadRight(GetWindowSize().Width, _defaults.SpriteMap[MapSymbol.BorderHorizontal]));
        Console.ResetColor();
    }
    private void DrawBottomBorder(bool mapBorder = false)
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.Border];

        if (mapBorder)
            Console.SetCursorPosition(0, GetWindowSize().Height - 2);

        Console.Write("".PadRight(GetWindowSize().Width, _defaults.SpriteMap[MapSymbol.BorderHorizontal]));
        Console.ResetColor();
    }
    private void DrawSideBorders()
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.Border];
        for (int i = 2; i < (GetWindowSize().Height - 2); i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(_defaults.SpriteMap[MapSymbol.BorderVertical]);
        }
        for (int i = 2; i < (GetWindowSize().Height - 2); i++)
        {
            Console.SetCursorPosition(GetWindowSize().Width - 1, i);
            Console.Write(_defaults.SpriteMap[MapSymbol.BorderVertical]);
        }
        Console.ResetColor();
    }
    private void DrawMapBorder()
    {
        DrawTopBorder(true);
        DrawSideBorders();
        DrawBottomBorder(true);
    }
    private void DrawMapItems(List<Food> meals)
    {
        foreach (var meal in meals)
        {
            for (int i = 0; i < meals.Count; i++)
            {
                Console.SetCursorPosition(meal.Location.X, meal.Location.Y);
                Console.ForegroundColor = _defaults.DisplayColors[GameColor.FoodColor];
                Console.Write(_defaults.SpriteMap[MapSymbol.Food]);

                Console.ResetColor();
            }
        }
    }
    private void DrawMapItems(List<Potion> potions)
    {
        foreach (var potion in potions)
        {
            for (int i = 0; i < potions.Count; i++)
            {
                Console.SetCursorPosition(potion.Location.X, potion.Location.Y);
                Console.ForegroundColor = _defaults.DisplayColors[GameColor.PotionColor];
                Console.Write(_defaults.SpriteMap[MapSymbol.Potion]);

                Console.ResetColor();
            }
        }
    }
    private void DrawMapItems(List<Encounter> encounters)
    {
        foreach (var encounter in encounters)
        {
            Console.SetCursorPosition(encounter.Location.X, encounter.Location.Y);

            Console.ForegroundColor = _defaults.DisplayColors[GameColor.EncounterColor];

            if (encounter.PreviouslyVisited)
                Console.Write(_defaults.SpriteMap[MapSymbol.PreviousEncounter]);
            else
                Console.Write(_defaults.SpriteMap[MapSymbol.NewEncounter]);

            Console.ResetColor();

        }
    }

    private void DrawWinLocation()
    {
        Console.ForegroundColor = _defaults.DisplayColors[GameColor.ExitMarker];
        Console.SetCursorPosition(1, 2);
        Console.Write(_defaults.SpriteMap[MapSymbol.Exit]);
        Console.ResetColor();
    }

    private void MarkVisitedLocation(List<MapPoint> visitedLocations)
    {
        foreach (var point in visitedLocations)
            MarkVisitedLocation(point);
    }
    private void MarkVisitedLocation(MapPoint point)
    {
        if (point.X == _state.PlayerLocation.X && point.Y == _state.PlayerLocation.Y)
        {
            return;
        }
        else
        {
            Console.SetCursorPosition(point.X, point.Y);

            Console.ForegroundColor = _defaults.DisplayColors[GameColor.PreviousLocationColor];
            Console.Write(_defaults.SpriteMap[MapSymbol.PreviousLocation]);
        }
    }
    internal void DrawMap(Map map)
    {
        Console.Clear();

        Console.SetCursorPosition(_state.Spawn.X, _state.Spawn.Y);

        DrawMapBorder();
        DrawPlayerSprite(_state.PlayerLocation);
#if DEBUG
        DrawMapItems(map.Encounters);
#endif
        DrawMapItems(map.Meals);
        DrawMapItems(map.Potions);
        DrawWinLocation();
        UpdateHud();
        MarkVisitedLocation(_state.VisitedLocations);
    }
}
