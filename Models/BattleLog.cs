using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Models;
internal class BattleLog
{
    public string? OpponentName { get; set; }
    public int PlayerStart { get; set; }
    public int PlayerFinish { get; set; }
}
