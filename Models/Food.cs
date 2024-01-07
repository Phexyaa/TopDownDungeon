using TopDownDungeon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Models;
internal class Food : Consumable
{
    public string Name { get; set; }

    public Food(string name)
    {
        Type = ConsumableType.Food;
        Name = name;
        EffectValue = 5;
    }
}
