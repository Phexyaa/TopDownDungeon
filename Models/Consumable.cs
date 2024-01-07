using System.Data.Common;
using TopDownDungeon.Enums;

namespace TopDownDungeon.Models;

internal abstract class Consumable
{
    internal ConsumableType? Type { get; set; } = null;
    public string? Description { get; set; }
    public int EffectValue { get; set; }

    public MapPoint? Point { get; set; };

    public Consumable()
    {
        Description = GeneratePlaceholder(Type);
    }

    private string? GeneratePlaceholder(ConsumableType? type)
    {
        switch (type)
        {
            case ConsumableType.Food:
                return "Some delicious food.";
            case ConsumableType.Potion:
                return "Wonder what this could be? Certainly one way to find out";
            case null:
                throw new ArgumentNullException(nameof(type));
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}