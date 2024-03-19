using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Consumables;
using TopDownDungeon.Encounters;
using TopDownDungeon.Enums;
using TopDownDungeon.Models;
using TopDownDungeon.UI;

namespace TopDownDungeon.Logic;
internal class MapBuilder
{
    private List<MapPoint> UsedPoints = [];
    private MapPoint spawn = new MapPoint();
    private readonly Screen _screen;

    public MapBuilder(Screen screen)
    {
        _screen = screen;
    }

    internal Map CreateMap(int height, int width, int meals = 15, int potions = 7, int encounters = 42)
    {
        List<Food> _meals = [];
        List<Potion> _potions = [];
        List<Encounter> _encounters = [];

        SpawnMapItem(out _meals, meals);
        SpawnMapItem(out _potions, potions);
        SpawnMapItem(out _encounters, encounters);

        var map = new Map(_meals, _potions, _encounters, height, width);

        return map;
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
        var screenDimensions = _screen.GetWindowSize();
        do
        {
            point.X = randomX.Next(1, screenDimensions.Width - 1);
            point.Y = randomY.Next(_screen.bottomPadding, screenDimensions.Height - 1);
            if (!UsedPoints.Contains(point))
                UsedPoints.Add(point);
        }
        while (!_screen.CheckMapBounds(point) && !UsedPoints.Contains(point));

        return point;
    }
}
