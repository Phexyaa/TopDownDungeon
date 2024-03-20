using TopDownDungeon.Models;

namespace TopDownDungeon.Logic;
internal class GameState
{
    internal MapPoint Spawn { get; private set; } = new MapPoint();
    internal MapPoint PlayerPosition { get; set; } = new MapPoint();
    internal List<MapPoint> VisitedLocations { get; set; } = new List<MapPoint>();

    internal int PlayerHealth = 100;
    internal int PlayerFood = 10;
    internal int PlayerStamina = 20;
    internal int PlayerSpeed = 1;
    internal bool CanMove = true;
    internal void SetSpawnPoint(MapPoint spawn) => Spawn = spawn;
    internal void ResetState()
    {
        Spawn = new MapPoint();
        PlayerPosition = new MapPoint();
        VisitedLocations.Clear();
    }
}
