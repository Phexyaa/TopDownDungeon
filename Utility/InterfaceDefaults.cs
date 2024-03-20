using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Enums;

namespace TopDownDungeon.Utility;
internal class InterfaceDefaults
{
    internal Dictionary<MapSymbol, char> SpriteMape = new Dictionary<MapSymbol, char>()
    {
        { MapSymbol.BorderHorizontal, '=' },
        { MapSymbol.BorderVertical, '|' },
        { MapSymbol.Player, 'H' },
        { MapSymbol.Food, '@' },
        { MapSymbol.Potion, '&' },
        { MapSymbol.NewEncounter, '!' },
        { MapSymbol.Exit, 'W' },
        { MapSymbol.PreviousLocation, '+' },
    };

    internal Dictionary<GameColor, ConsoleColor> DisplayColors = new Dictionary<GameColor, ConsoleColor>()
    {
        { GameColor.MessageColor, ConsoleColor.White },
        { GameColor.Foreground, ConsoleColor.Green },
        { GameColor.Background, ConsoleColor.Black },
        { GameColor.PlayerColor, ConsoleColor.Cyan },
        { GameColor.FoodColor, ConsoleColor.Yellow },
        { GameColor.PotionColor, ConsoleColor.Yellow },
        { GameColor.EncounterColor, ConsoleColor.Red },
        { GameColor.PreviousLocationColor, ConsoleColor.Gray },
        { GameColor.ExitMarker, ConsoleColor.Magenta },
    };
}

