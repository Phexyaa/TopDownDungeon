using TopDownDungeon.Models;
using TopDownDungeon.Enums;

namespace TopDownDungeon.Services.Consumables;

internal class FoodFactory
{
    private static Food Create(FoodType foodType)
    {
        var food = new Food("Nothing");
        switch (foodType)
        {
            case FoodType.Bread:
                food.Name = "Bread";
                food.EffectValue = 3;
                break;
            case FoodType.Meat:
                food.Name = "Meat";
                food.EffectValue = 5;
                break;
            case FoodType.Pie:
                food.Name = "Pie";
                food.EffectValue = 2;
                break;
            case FoodType.Pemican:
                food.Name = "Pemican";
                food.EffectValue = 7;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(foodType));
        }

        return food;
    }

    public static List<Food> Create(int quantity)
    {
        var foods = new List<Food>();
        var foodNames = Enum.GetNames(typeof(FoodType)).ToList();
        var random = new Random();

        for (int i = 0; i < quantity; i++)
        {
            int index = random.Next(0, foodNames.Count);

            if (!Enum.TryParse<FoodType>(foodNames[index], out var foodType))
                throw new ArgumentOutOfRangeException(nameof(foodNames));
            else
                foods.Add(Create(foodType));
        }
        return foods;
    }
}