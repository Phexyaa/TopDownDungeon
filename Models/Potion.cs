using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Enums;

namespace TopDownDungeon.Models;
internal class Potion:Consumable
{
    public PotionEffect Effect { get; set; }
    public bool IsHarmful { get; set; }

    public Potion()
    {
        Type = ConsumableType.Potion;
    }
}
