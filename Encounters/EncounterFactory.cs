using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Data;
using TopDownDungeon.Models;

namespace TopDownDungeon.Encounters;
internal class EncounterFactory
{
    private static Encounter Create()
    {
        var random = new Random().Next(1, NpcData.DefaultNames.Count);
        var name = NpcData.DefaultNames[random];
        var encounter = new Encounter(new Npc(name));

        return encounter;
    }
    internal static List<Encounter> Create(int qty)
    {
        var encounters = new List<Encounter>();
        for (int i = 0; i < qty; i++)
        {
            encounters.Add(Create());
        }

        return encounters;
    }

}
