using TopDownDungeon.Data;
using TopDownDungeon.Models;

namespace TopDownDungeon.Utility;
internal class EncounterFactory
{
    private Encounter Create()
    {
        var randomIndex = new Random().Next(1, NpcData.DefaultNames.Count);
        var name = NpcData.DefaultNames[randomIndex];
        var encounter = new Encounter(new Npc(name, DecideTemperment()));

        return encounter;
    }

    private bool DecideTemperment()
    {
        var coinFilp = new Random().Next(0, 2);
        switch (coinFilp)
        {
            case 0:
                return false;
            case 1:
                return true;
            default:
                return true;
        }
    }

    internal List<Encounter> Create(int qty)
    {
        var encounters = new List<Encounter>();
        for (int i = 0; i < qty; i++)
        {
            encounters.Add(Create());
        }

        return encounters;
    }

}
