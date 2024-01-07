using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Models;
internal class Npc
{
    public string Name { get; }
    public string? Description { get; set; }
    public int HealthPoints => new Random().Next(15, 70);

    public Npc(string name) => Name = name;
}
