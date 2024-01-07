using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Enums;
using TopDownDungeon.Models;

namespace TopDownDungeon.UI;
internal class Screen
{
    private char borderHorizontal = '=';
    private char borderVertical = '|';
    private char player = 'H';
    private char food = '@';
    private char potion = '&';
    private char encounter = '#';

    private int eventDisplayCharWidth = 15;
    internal ConsoleColor Foreground { get; set; } = ConsoleColor.Green;
    internal ConsoleColor Background { get; set; } = ConsoleColor.Black;
    internal ConsoleColor PlayerColor { get; set; } = ConsoleColor.Cyan;
    internal ConsoleColor FoodColor { get; set; } = ConsoleColor.Yellow;
    internal ConsoleColor PotionColor { get; set; } = ConsoleColor.Yellow;
    internal ConsoleColor EncounterColor { get; set; } = ConsoleColor.Red;

    internal MapPoint CursorPosition { get; set; } = new MapPoint(0, 0);
    private void SetCursorPosition(MapPoint point) => CursorPosition = point;

    private (int Width, int Height) CheckWindowSize()
    {
        return (Console.WindowWidth, Console.WindowHeight);
    }
    internal void ShowMessage(string message)
    {
        if (message.Length > (CheckWindowSize().Width - eventDisplayCharWidth))
        {
            message = message.Substring(0, message.Length - eventDisplayCharWidth);
        }


        Console.ForegroundColor = Foreground;

        Console.SetCursorPosition(0, 0);
        Console.Write(message);
        Console.Write("".PadRight(CheckWindowSize().Width - eventDisplayCharWidth));

        Console.ResetColor();
    }
    internal void DrawBorder()
    {
        Console.ForegroundColor = Foreground;

        Console.SetCursorPosition(0, 1);
        Console.Write("".PadRight(CheckWindowSize().Width, borderHorizontal));

        for (int i = 2; i < CheckWindowSize().Height; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(borderVertical);
        }
        for (int i = 2; i < CheckWindowSize().Height; i++)
        {
            Console.SetCursorPosition(CheckWindowSize().Width - 1, i);
            Console.Write(borderVertical);
        }

        Console.SetCursorPosition(0, CheckWindowSize().Height - 1);
        Console.Write("".PadRight(CheckWindowSize().Width, borderHorizontal));

        Console.ResetColor();
    }

    internal void DrawPlayer(int x, int y)
    {
        Console.ForegroundColor = PlayerColor;

        Console.SetCursorPosition(x, y);
        Console.Write(player);

        Console.ResetColor();
    }
    internal void DrawConsumable(List<Consumable> consumables)
    {
        foreach (var consumable in consumables)
        {
            for (int i = 0; i < foodLocations.Length; i++)
            {
                Console.SetCursorPosition(foodLocations[i].X, foodLocations[i].Y);

                var r = new Random().Next(1, 4);
                ConsumableType foodType = ConsumableType.Pie; ;
                switch (r)
                {
                    case 1:
                        foodType = ConsumableType.Pie; break;
                    case 2:
                        foodType = ConsumableType.Bread; break;
                    case 3:
                        foodType = ConsumableType.Meat; break;
                }
                switch (consumable.Type)
                {
                    case ConsumableType.Food:
                        Console.ForegroundColor = FoodColor;

                        Console.Write(food);
                        break;
                    case ConsumableType.Potion:
                        Console.ForegroundColor = PotionColor;
                        Console.Write(food);
                        break;
                    case null:
                        break;
                    default:
                        break;
                }

                Console.ResetColor();
            }
        }
    }
}
