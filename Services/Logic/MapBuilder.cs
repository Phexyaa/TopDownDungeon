using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;
using TopDownDungeon.Services.Consumables;

namespace TopDownDungeon.Services.Logic;
internal class MapBuilder
{
    private int screenBorderWidth = 1;
    private int topPadding = 1;
    private int bottomPadding = 2;
    private MapPoint spawn => new(MapWidth - screenBorderWidth, MapHeight - bottomPadding);
    public static int MapHeight { get; set; } = 0;
    public static int MapWidth { get; set; } = 0;
    public static List<Food> Meals { get; set; } = [];
    public static List<Potion> Potions { get; set; } = [];

    public static Task<Map> CreateMap(int height, int width, int meals = 15, int potions = 7, int encounters = 42)
    {
        MapHeight = height;
        MapWidth = width;

        Meals = FoodGenerator.GenerateFood(meals);
        Potions = PotionGenerator.GeneratePotions(potions);

        return Task.FromResult(new Map());
    }



    private List<MapPoint> ()
    {

        for (int i = 0; i < foodCount; i++)
        {
            var point = GenerateRandomPoint();
            if (!foodLocations.Contains(point))
                foodLocations[i] = point;
        }
    }
    private List<MapPoint> DesignateEncounterPoints()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            var point = GenerateRandomPoint();
            if (!monsterLocations.Contains(point))
                if (!foodLocations.Contains(point))
                    monsterLocations[i] = point;
        }
    }
    private MapPoint GenerateRandomPoint()
    {
        var randomX = new Random();
        var randomY = new Random();

        var point = new MapPoint(0, 0);
        while (!CheckBounds(point))
        {
            point.X = randomX.Next(1, MapWidth);
            point.Y = randomY.Next(bottomPadding, MapHeight);
        }

        return point;
    }
    private bool CheckBounds(MapPoint point)
    {
        var notSpawn = point != spawn;
        var checkX = point.X > 0 && point.X < (MapWidth - screenBorderWidth);
        var checkY = point.Y > 1 && point.Y < (MapHeight - topPadding);

        return notSpawn && checkX && checkY;
    }




    private bool CheckForFood()
    {
        var result = false;
        for (int i = 0; i < foodLocations.Length; i++)
        {
            result = result || (foodLocations[i] == playerLocation);
        }
        return result;
    }
    private bool CheckForEnconter()
    {
        var result = false;
        for (int i = 0; i < monsterLocations.Length; i++)
        {
            result = result || (monsterLocations[i] == playerLocation);
        }
        return result;
    }
}
