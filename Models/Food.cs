using TopDownDungeon.Enums;

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
