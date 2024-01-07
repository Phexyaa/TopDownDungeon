using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Enums;
using TopDownDungeon.Models;
using TopDownDungeon.Services.Consumables;
using TopDownDungeon.Services.Encounters;

namespace TopDownDungeon.Services.Logic;
internal class MapBuilder
{
    private int screenBorderWidth = 1;
    private int topPadding = 1;
    private int bottomPadding = 2;
    private int screenWidth = 0;
    private int screenHeight = 0;

    private List<MapPoint> UsedPoints = [];
    private MapPoint spawn = new MapPoint();

    public Task<Map> CreateMap(int height, int width, int meals = 15, int potions = 7, int encounters = 42)
    {
        spawn = new(width - screenBorderWidth, height - bottomPadding);
        screenHeight = height;
        screenWidth = width;

        List<Food> _meals = [];
        List<Potion> _potions = [];
        List<Encounter> _encounters = [];

        SpawnMapItem(out _meals, meals);
        SpawnMapItem(out _potions, potions);
        SpawnMapItem(out _encounters, encounters);

        var map = new Map(_meals, _potions, _encounters, height, width);

        return Task.FromResult(map);
    }
    private List<Food> SpawnMapItem(out List<Food> meals, int quantity)
    {
        meals = FoodFactory.Create(quantity);
        foreach (var meal in meals)
            meal.Location = GenerateRandomPoint();
        return meals;
    }
    private List<Potion> SpawnMapItem(out List<Potion> potions, int quantity)
    {
        potions = PotionFactory.Create(quantity);
        foreach (var potion in potions)
            potion.Location = GenerateRandomPoint();
        return potions;
    }
    private List<Encounter> SpawnMapItem(out List<Encounter> encounters, int quantity)
    {
        encounters = EncounterFactory.Create(quantity);
        foreach (var encounter in encounters)
            encounter.Location = GenerateRandomPoint();
        return encounters;
    }


    private MapPoint GenerateRandomPoint()
    {
        var randomX = new Random();
        var randomY = new Random();

        var point = new MapPoint(0, 0);
        do
        {
            point.X = randomX.Next(1, screenWidth);
            point.Y = randomY.Next(bottomPadding, screenHeight);
            if (!UsedPoints.Contains(point))
                UsedPoints.Add(point);
        }
        while (!CheckBounds(point) && !UsedPoints.Contains(point));

        return point;
    }
    private bool CheckBounds(MapPoint point)
    {
        var notSpawn = point != spawn;
        var checkX = point.X > 0 && point.X < (screenWidth - screenBorderWidth);
        var checkY = point.Y > 1 && point.Y < (screenHeight - topPadding);

        return notSpawn && checkX && checkY;
    }
}
