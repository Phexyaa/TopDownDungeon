using TopDownDungeon.Models;

namespace TopDownDungeon.Logic;
internal class Map
{
    internal List<Food> Meals { get; set; }
    internal List<Potion> Potions { get; set; }
    internal List<Encounter> Encounters { get; set; }
    internal int Height { get; set; }
    internal int Width { get; set; }

    internal Map(List<Food> food, List<Potion> potions, List<Encounter> encounters, int height = 0, int width = 0)
    {
        Meals = food;
        Potions = potions;
        Encounters = encounters;
        Height = height;
        Width = width;
    }
}
