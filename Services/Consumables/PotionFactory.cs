using TopDownDungeon.Enums;
using TopDownDungeon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Services.Consumables;
internal class PotionFactory
{
    internal static Potion Create(PotionEffect effect)
    {
        var potion = new Potion { Effect = effect };
        var helpValue = new Random().Next(0, 8);
        var hurtValue = helpValue * -1;

        switch (effect)
        {
            case PotionEffect.Heal:
                potion.EffectValue = helpValue;
                potion.IsHarmful = false;
                break;
            case PotionEffect.Harm:
                potion.EffectValue = hurtValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Poison:
                potion.EffectValue = hurtValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Invigorate:
                potion.EffectValue = helpValue;
                potion.IsHarmful = false;
                break;
            case PotionEffect.Drunken:
                potion.EffectValue = hurtValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Slow:
                potion.EffectValue = hurtValue;
                potion.IsHarmful = true;
                break;
            case PotionEffect.Speed:
                potion.EffectValue = helpValue;
                potion.IsHarmful = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(effect));
        }

        return potion;
    }

    public static List<Potion> Create(int quantity)
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
