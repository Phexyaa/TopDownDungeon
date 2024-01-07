using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Models;
internal class Encounter : IMapItem
{
    public bool PreviouslyVisited { get; set; } = false;
    public BattleLog? Battle { get; set; }
    public Npc Opponent { get; set; }
    public MapPoint Location { get; set; } = new MapPoint();

    public Encounter(Npc npc) => Opponent = npc;
}
