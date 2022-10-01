using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.YggdrasilModule
{
    internal class YggdrasilWorld : SubWorldModule.Subworld
    {
        public override int Width => 1000;

        public override int Height => 12000;

        //public override bool ShouldSave => true;
        public override List<GenPass> Tasks => new() { new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass()};
        public override void OnLoad()
        {
            SubWorldModule.SubworldSystem.hideUnderworld = true;
            Main.worldSurface = Main.maxTilesY - 2;
            Main.rockLayer = Main.maxTilesY - 1;
            WorldGen.waterLine = Main.maxTilesY;
        }
    }
}
