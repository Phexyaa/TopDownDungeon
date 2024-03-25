using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Enums;
using TopDownDungeon.Utility;

namespace TopDownDungeon.UI;
internal class TextPrompts
{
    private readonly InterfaceHelper _interfaceDefaults;
    public TextPrompts(InterfaceHelper defaults) => _interfaceDefaults = defaults;
    internal string SplashScreenMessage =>
        "Welcome to Top Down Dungeon.\n" +
        "The rules are simple; Just try to survive and reach the exit " +
        $"which is marked on the map with a {_interfaceDefaults.SpriteMap[MapSymbol.Exit]}.\n" +
        "Every step brings along with it a chance to run into an enemy lurking in the shadows, and the " +
        "weight of your will quickly drain your stamina leaving only your life force to keep you going.\n" +
        "You can reinvigorate yourself by eating one of the hearty meals " +
        $"that are marked with a {_interfaceDefaults.SpriteMap[MapSymbol.Food]}.\n" +
        "There are also potions spread out for you to find, but beware; The labels have worn away with time so " +
        "drink at your own risk. If you are feeling lucky these can be found " +
        $"by traveling to one of the {_interfaceDefaults.SpriteMap[MapSymbol.Potion]} symbols that are marked on the map.";

    internal string ControlMappings =
        "Control your hero with either the arrow keys or WASD.";
}
