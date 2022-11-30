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
        //public override int Width => 1200;

        //public override int Height => 12000;

        ////public override bool ShouldSave => true;

        //public override bool NormalUpdates => true;

        //public override List<GenPass> Tasks => new() { new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass()};
        //public override void OnLoad()
        //{
        //    SubWorldModule.SubworldSystem.hideUnderworld = true;
        //    Main.worldSurface = Main.maxTilesY - 2;
        //    Main.rockLayer = Main.maxTilesY - 1;
        //    WorldGen.waterLine = Main.maxTilesY;
        //}
        public override SaveSetting HowSaveWorld { get; init; } = SaveSetting.PerWorld;
        public override int Width { get; init; } = 1200;
        public override int Height { get; init; } = 12000;
        public override List<GenPass> Tasks { get; init; } = new() { new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass() };
        public override bool NormalTime => false;
        public override bool HideUnderworld => true;
        public override void ModifyPlayerBasicGravity(Player player, ref float basicgravity, ref float maxFallSpeed)
        {
            base.ModifyPlayerBasicGravity(player, ref basicgravity, ref maxFallSpeed);
        }
        public override void OnLoad()
        {
            Main.worldSurface = Main.maxTilesY - 2;
            Main.rockLayer = Main.maxTilesY - 1;
            WorldGen.waterLine = Main.maxTilesY;
        }
    }
}

