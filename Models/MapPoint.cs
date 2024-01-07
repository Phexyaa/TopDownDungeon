using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Models;
internal class MapPoint
{
    public int X { get; set; }
    public int Y { get; set; }

    public MapPoint(int x, int y)
    {
        X = x;
        Y = y;
    }
}
