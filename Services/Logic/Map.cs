using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;

namespace TopDownDungeon.Services.Logic;
internal class Map
{
    internal List<Food> Meals { get; set; }
    internal List<Potion> Potions { get; set; }
    internal List<Encounter> Encounters { get; set; }
    internal int Height { get; set; }
    internal int Width { get; set; }
    internal int TESTES = 0;
    internal MapPoint PlayerPosition { get; set; } = new MapPoint();

    internal Map(List<Food> food, List<Potion> potions, List<Encounter> encounters, int height = 0, int width = 0)
    {
        Meals = food;
        Potions = potions;
        Encounters = encounters;
        Height = height;
        Width = width;
    }
}
