using TopDownDungeon.Enums;
using TopDownDungeon.Models;

namespace TopDownDungeon.Utility;
internal class PotionFactory
{
    internal Potion Create(PotionEffect effect)
    {
        var potion = new Potion { Effect = effect };
        var effectValue = new Random().Next(0, 8);

        switch (effect)
        {
            case PotionEffect.Heal:
                potion.EffectValue = effectValue;
                potion.IsHarmful = false;
                break;
            case PotionEffect.Harm:
                potion.EffectValue = effectValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Poison:
                potion.EffectValue = effectValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Invigorate:
                potion.EffectValue = effectValue;
                potion.IsHarmful = false;
                break;
            case PotionEffect.Drunken:
                potion.EffectValue = effectValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Slow:
                potion.EffectValue = effectValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Speed:
                potion.EffectValue = effectValue;
                potion.IsHarmful = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(effect));
        }

        return potion;
    }

    public List<Potion> Create(int quantity)
    {
        var potions = new List<Potion>();
        var effects = Enum.GetNames(typeof(PotionEffect)).ToList();
        var random = new Random();

        for (int i = 0; i < quantity; i++)
        {
            int index = random.Next(0, effects.Count);

            if (!Enum.TryParse<PotionEffect>(effects[index], out var effect))
                throw new ArgumentOutOfRangeException(nameof(effects));
            else
                potions.Add(Create(effect));
        }
        return potions;
    }
}
