using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Enums;
using TopDownDungeon.Models;
using TopDownDungeon.Services.Logic;

namespace TopDownDungeon.UI;
internal class Screen
{
    private void SetCursorPosition(MapPoint point) => CursorPosition = point;
    private Dictionary<MapSymbol, char> _charMap = new Dictionary<MapSymbol, char>()
    {
        { MapSymbol.BorderHorizontal, '=' },
        { MapSymbol.BorderVertical, '|' },
        { MapSymbol.Player, 'H' },
        { MapSymbol.Food, '@' },
        { MapSymbol.Potion, '&' },
        { MapSymbol.NewEncounter, '!' },
        { MapSymbol.PreviousEncounter, '§' },
    };
    private int eventDisplayCharWidth = 15;
    private readonly GameState _state;

    internal int borderWidth { get; set; } = 1;
    internal int topPadding { get; set; } = 1;
    internal int bottomPadding = 2;
    internal ConsoleColor Foreground { get; set; } = ConsoleColor.Green;
    internal ConsoleColor Background { get; set; } = ConsoleColor.Black;
    internal ConsoleColor PlayerColor { get; set; } = ConsoleColor.Cyan;
    internal ConsoleColor FoodColor { get; set; } = ConsoleColor.Yellow;
    internal ConsoleColor PotionColor { get; set; } = ConsoleColor.Yellow;
    internal ConsoleColor EncounterColor { get; set; } = ConsoleColor.Red;
    internal MapPoint CursorPosition { get; set; } = new MapPoint(0, 0);

    public Screen(GameState state)
    {
        _state = state;
        _state.SetSpawnPoint(new MapPoint(GetWindowSize().Width - 2, GetWindowSize().Height - bottomPadding));
    }

    internal bool CheckBounds(MapPoint point)
    {
        var screenSize = GetWindowSize();
        var notSpawn = point != _state.Spawn;
        var checkX = point.X > 0 && point.X < (screenSize.Width - 2);
        var checkY = point.Y > topPadding && point.Y < (screenSize.Height - bottomPadding);

        return notSpawn && checkX && checkY;
    }
    internal (int Width, int Height) GetWindowSize()
    {
        return (Console.WindowWidth, Console.WindowHeight);
    }
    internal void ShowMessage(string message)
    {
        if (message.Length > (GetWindowSize().Width - eventDisplayCharWidth))
        {
            message = message.Substring(0, message.Length - eventDisplayCharWidth);
        }


        Console.ForegroundColor = Foreground;

        Console.SetCursorPosition(0, 0);
        Console.Write(message);
        Console.Write("".PadRight(GetWindowSize().Width - eventDisplayCharWidth));

        Console.ResetColor();
    }
    internal void DrawPlayer(MapPoint point)
    {
        Console.ForegroundColor = PlayerColor;

        Console.SetCursorPosition(point.X, point.Y);
        Console.Write(_charMap[MapSymbol.Player]);

        Console.ResetColor();
    }
    private void DrawBorder()
    {
        Console.ForegroundColor = Foreground;

        Console.SetCursorPosition(0, 1);
        Console.Write("".PadRight(GetWindowSize().Width, _charMap[MapSymbol.BorderHorizontal]));

        for (int i = 2; i < GetWindowSize().Height; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(_charMap[MapSymbol.BorderVertical]);
        }
        for (int i = 2; i < GetWindowSize().Height; i++)
        {
            Console.SetCursorPosition(GetWindowSize().Width - 1, i);
            Console.Write(_charMap[MapSymbol.BorderVertical]);
        }

        Console.SetCursorPosition(0, GetWindowSize().Height - 1);
        Console.Write("".PadRight(GetWindowSize().Width, _charMap[MapSymbol.BorderHorizontal]));

        Console.ResetColor();
    }
    private void DrawMapItems(List<Food> meals)
    {
        foreach (var meal in meals)
        {
            for (int i = 0; i < meals.Count; i++)
            {
                Console.SetCursorPosition(meal.Location.X, meal.Location.Y);
                Console.ForegroundColor = FoodColor;
                Console.Write(_charMap[MapSymbol.Food]);

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
                Console.ForegroundColor = PotionColor;
                Console.Write(_charMap[MapSymbol.Potion]);

                Console.ResetColor();
            }
        }
    }
    private void DrawMapItems(List<Encounter> encounters)
    {
        foreach (var encounter in encounters)
        {
            Console.SetCursorPosition(encounter.Location.X, encounter.Location.Y);

            Console.ForegroundColor = EncounterColor;

            if (encounter.PreviouslyVisited)
                Console.Write(_charMap[MapSymbol.PreviousEncounter]);
            else
                Console.Write(_charMap[MapSymbol.NewEncounter]);

            Console.ResetColor();

        }
    }
    internal void ShowPosition(MapPoint point)
    {
        Console.ForegroundColor = Foreground;

        Console.SetCursorPosition(GetWindowSize().Width - 15, 0);
        Console.Write($"({point.X}, {point.Y})");

        Console.ResetColor();
    }
    internal void DrawMap(Map map)
    {
        Console.Clear();
        DrawBorder();
        DrawPlayer(map.PlayerPosition);
        DrawMapItems(map.Meals);
        DrawMapItems(map.Potions);
        DrawMapItems(map.Encounters);
        //throw new NotImplementedException();
    }
}
