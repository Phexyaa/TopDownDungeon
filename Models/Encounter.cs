using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Models;
internal class Encounter
{
    public MapPoint? Location { get; set; }
    public bool PreviouslyVisited { get; set; } = false;
    public BattleLog? Battle { get; set; }
    public Npc Opponent { get; set; }

    public Encounter(Npc npc) => Opponent = npc;
}
