using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
	internal class MothWorld : SubWorldModule.Subworld
	{
		//public override int Width => 800;

		//public override int Height => 600;

		//public override bool NormalUpdates => true;

		//public override List<GenPass> Tasks => new() { new MothLand.MothLandGenPass() };

		//public override void OnLoad()
		//{
		//    SubWorldModule.SubworldSystem.hideUnderworld = true;
		//    Main.worldSurface = 1;
		//    Main.rockLayer = 2;
		//    WorldGen.waterLine = Main.maxTilesY;
		//}

		//public override void OnEnter()
		//{
		//    //TODO: 我希望玩家进入子世界之后能出现在传送门附近
		//    //for(int x = 20;x < Width - 20;x++)
		//    //{
		//    //    for (int y = 20; y < Height - 20; y++)
		//    //    {
		//    //        if(Main.tile[x, y].TileType == (ushort)(ModContent.TileType<Tiles.MothWorldDoor>()))
		//    //        {
		//    //            Main.LocalPlayer.position = new Vector2(x * 16 - 120, y * 16);
		//    //            break;
		//    //        }
		//    //    }
		//    //}
		//    base.OnEnter();
		//}
		public override SaveSetting HowSaveWorld { get; init; } = SaveSetting.PerWorld;
		public override int Width { get; init; } = 800;
		public override int Height { get; init; } = 600;
		public override List<GenPass> Tasks { get; init; } = new() { new MothLand.MothLandGenPass() };
		public override bool NormalTime => true;
		public override bool HideUnderworld => true;
		public override void OnLoad()
		{
			Main.worldSurface = 20;
			Main.rockLayer = 150;
			GenVars.waterLine = Main.maxTilesY;
		}
	}
}