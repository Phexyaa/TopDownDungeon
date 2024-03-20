namespace TopDownDungeon.Models;
internal class MapPoint
{
    public int X { get; set; }
    public int Y { get; set; }

    public MapPoint(int x = 0, int y = 0)
    {
        X = x;
        Y = y;
    }
    public MapPoint((int x, int y) point)
    {
        X = point.x;
        Y = point.y;
    }
}
