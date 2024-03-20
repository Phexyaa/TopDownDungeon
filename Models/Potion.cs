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
