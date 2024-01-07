using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;

namespace TopDownDungeon.Services.Logic;
internal class Map
{
    public List<Food>? Meals { get; set; }
    public List<Potion>? Potions { get; set; }
    public List<Encounter>? Encounters { get; set; }    
}
