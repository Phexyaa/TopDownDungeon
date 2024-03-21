using TopDownDungeon.Models;

namespace TopDownDungeon.Logic;
internal class GameState
{
    internal MapPoint Spawn { get; private set; } = new MapPoint();
    internal MapPoint PlayerLocation { get; set; } = new MapPoint();
    internal List<MapPoint> VisitedLocations { get; set; } = new List<MapPoint>();

    internal int PlayerHealth { get; set; }
    internal int PlayerStamina { get; set; }
    internal int PlayerSpeed { get; set; }
    internal bool CanMove { get; set; }
    internal void SetSpawnPoint(MapPoint spawn) => Spawn = spawn;
}
