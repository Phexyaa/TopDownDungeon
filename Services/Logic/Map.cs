using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownDungeon.Models;

namespace TopDownDungeon.Services.Logic;
internal class Map
{
    public List<MapPoint> FoodLocations { get; set; }
    public List<MapPoint> PotionLocations { get; set; }
    public List<MapPoint> EncounterLocations { get; set; }



    
}
