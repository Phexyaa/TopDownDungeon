using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;

namespace TopDownDungeon.Services.Logic;
internal class GameState
{
    public MapPoint? Spawn { get; private set; }

    internal void SetSpawnPoint(MapPoint? spawn) => Spawn = spawn;
}
