namespace TopDownDungeon.Models;
internal class Encounter : IMapItem
{
    public bool PreviouslyVisited { get; set; } = false;
    public Npc Opponent { get; set; }
    public MapPoint Location { get; set; } = new MapPoint();

    public Encounter(Npc npc)
    {
        Opponent = npc;
    }
}
