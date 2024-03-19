using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;

namespace TopDownDungeon.Logic;
internal class GameState
{
    internal MapPoint Spawn { get; private set; } = new MapPoint();
    internal int PlayerHealth = 100;
    internal int PlayerFood = 10;
    internal int PlayerStamina = 20;
    internal int PlayerSpeed = 1;
    internal bool CanMove = true;
    internal void SetSpawnPoint(MapPoint spawn) => Spawn = spawn;
}
