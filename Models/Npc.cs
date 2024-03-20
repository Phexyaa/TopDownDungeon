namespace TopDownDungeon.Models;
internal class Npc
{
    public string Name { get; }
    public bool IsAggressive { get; set; } = true;
    public int HealthPoints => new Random().Next(15, 100);

    public Npc(string name, bool temperment)
    {
        Name = name;
        IsAggressive = temperment;
    }
}
