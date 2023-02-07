//KEPT FOR REFERENCE
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria.WorldBuilding;

//namespace Everglow.Sources.Modules.YggdrasilModule
//{
//    internal class YggdrasilWorld : Subworld
//    {
//        //public override int Width => 1200;

//        //public override int Height => 12000;

//        ////public override bool ShouldSave => true;

//        //public override bool NormalUpdates => true;

//        //public override List<GenPass> Tasks => new() { new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass()};
//        //public override void OnLoad()
//        //{
//        //    SubWorldModule.SubworldSystem.hideUnderworld = true;
//        //    Main.worldSurface = Main.maxTilesY - 2;
//        //    Main.rockLayer = Main.maxTilesY - 1;
//        //    WorldGen.waterLine = Main.maxTilesY;
//        //}
//        public override int Width { get; } = 1200;
//        public override int Height { get; } = 12000;
//        public override List<GenPass> Tasks { get; } = new() { new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass() };
//        public override bool NormalUpdates => false;
//        public override bool ShouldSave => true;
//        public override void OnLoad()
//        {
//            Main.worldSurface = Main.maxTilesY - 2;
//            Main.rockLayer = Main.maxTilesY - 1;
//            WorldGen.waterLine = Main.maxTilesY;
//        }
//    }
//}

