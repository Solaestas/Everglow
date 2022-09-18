using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    internal class MothWorld : SubWorldModule.Subworld
    {
        public override int Width => 800;

        public override int Height => 600;

        public override List<GenPass> Tasks => new() { new MothLand.MothLandGenPass() };
        public override void OnLoad()
        {
            SubWorldModule.SubworldSystem.hideUnderworld = true;
            Main.worldSurface = Main.maxTilesY - 2;
            Main.rockLayer = Main.maxTilesY - 1;
            WorldGen.waterLine = Main.maxTilesY;
        }
    }
}
