using TopDownDungeon.Logic;
using TopDownDungeon.Models;
using TopDownDungeon.UI;

namespace TopDownDungeon.Utility;
internal class MapFactory
{
    private List<MapPoint> UsedPoints = [];
    private MapPoint spawn = new MapPoint();
    private readonly Screen _screen;
    private readonly EncounterFactory _encounterFactory;
    private readonly FoodFactory _foodFactory;
    private readonly PotionFactory _potionFactory;

    public MapFactory(Screen screen, EncounterFactory encounterFactory, FoodFactory foodFactory, PotionFactory potionFactory)
    {
        _screen = screen;
        _encounterFactory = encounterFactory;
        _foodFactory = foodFactory;
        _potionFactory = potionFactory;
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
        meals = _foodFactory.Create(quantity);
        foreach (var meal in meals)
            meal.Location = GenerateRandomPoint();
        return meals;
    }
    private List<Potion> SpawnMapItem(out List<Potion> potions, int quantity)
    {
        potions = _potionFactory.Create(quantity);
        foreach (var potion in potions)
            potion.Location = GenerateRandomPoint();
        return potions;
    }
    private List<Encounter> SpawnMapItem(out List<Encounter> encounters, int quantity)
    {
        encounters = _encounterFactory.Create(quantity);
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
        while (!_screen.CheckMapBounds(point)
            || !_screen.CheckForSpawnOverlap(point)
            || !UsedPoints.Contains(point))
        {
            point.X = randomX.Next(1, screenDimensions.Width - 1);
            point.Y = randomY.Next(1, screenDimensions.Height - 1);
            if (!UsedPoints.Contains(point))
                UsedPoints.Add(point);
        };

        return point;
    }
}
