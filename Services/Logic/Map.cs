using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;

namespace TopDownDungeon.Services.Logic;
internal class Map
{
    public List<Food> Meals { get; set; }
    public List<Potion> Potions { get; set; }
    public List<Encounter> Encounters { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public Map(List<Food> food, List<Potion> potions, List<Encounter> encounters, int height = 0, int width = 0)
    {
        Meals = food;
        Potions = potions;
        Encounters = encounters;
        Height = height;
        Width = width;
    }
}
