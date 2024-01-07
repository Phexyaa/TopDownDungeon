using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;
using TopDownDungeon.Services.Consumables;
using TopDownDungeon.Services.Encounters;

namespace TopDownDungeon.Services.Logic;
internal class MapBuilder
{
    private int screenBorderWidth = 1;
    private int topPadding = 1;
    private int bottomPadding = 2;

    private List<MapPoint> UsedPoints = [];

    private MapPoint spawn => new(MapWidth - screenBorderWidth, MapHeight - bottomPadding);
    public int MapHeight { get; private set; } = 0;
    public int MapWidth { get; private set; } = 0;

    public Task<Map> CreateMap(int height, int width, int meals = 15, int potions = 7, int encounters = 42)
    {
        var map = new Map();
        List<Food> Meals = [];
        List<Potion> Potions = [];
        List<Encounter> Encounters = [];

        MapHeight = height;
        MapWidth = width;

        map.Meals = FoodFactory.Create(meals);
        map.Potions = PotionFactory.Create(potions);
        map.Encounters = EncounterFactory.Create(encounters);

        SetMapItemLocations();

        return Task.FromResult(map);
    }

    private void SetMapItemLocations()
    {
        foreach (var meal in Meals)
            meal.Location = GenerateRandomPoint();
        foreach (var potion in Potions)
            potion.Location = GenerateRandomPoint();
        foreach (var encounter in Encounters)
            encounter.Location = GenerateRandomPoint();
    }


    private MapPoint GenerateRandomPoint()
    {
        var randomX = new Random();
        var randomY = new Random();

        var point = new MapPoint(0, 0);
        do
        {
            point.X = randomX.Next(1, MapWidth);
            point.Y = randomY.Next(bottomPadding, MapHeight);
            if (!UsedPoints.Contains(point))
                UsedPoints.Add(point);
        }
        while (!CheckBounds(point) && !UsedPoints.Contains(point));

        return point;
    }
    private bool CheckBounds(MapPoint point)
    {
        var notSpawn = point != spawn;
        var checkX = point.X > 0 && point.X < (MapWidth - screenBorderWidth);
        var checkY = point.Y > 1 && point.Y < (MapHeight - topPadding);

        return notSpawn && checkX && checkY;
    }
}
