using Terraria.IO;
using Terraria.WorldBuilding;

namespace Everglow.SubSpace;

public class WoodenBoxRoomGenPass : GenPass
{
	public WoodenBoxRoomGenPass() : base("Wooden Box", 500)
	{
	}

	public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
	{
		Main.statusText = "Test";
		BuildWoodenRoom();
	}
	public static void BuildWoodenRoom() 
	{
		for(int x = 20;x < Main.maxTilesX - 20;x++)
		{
			for (int y = 20; y < Main.maxTilesY - 20; y++)
			{
				Tile tile = Main.tile[x,y];
				tile.wall = WallID.Wood;
				if(!(Math.Abs(x - Main.maxTilesX / 2) < 10 && Math.Abs(y - Main.maxTilesY / 2) < 10))
				{
					tile.TileType = TileID.WoodBlock;
					tile.HasTile = true;
				}
			}
		}
	}
}