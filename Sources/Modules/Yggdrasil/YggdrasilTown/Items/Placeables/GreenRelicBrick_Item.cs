using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;
using Everglow.Yggdrasil.YggdrasilTown.Walls.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class GreenRelicBrick_Item : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<ForestCastle_Scene>());
		Item.width = 16;
		Item.height = 16;
	}

	public override void HoldItem(Player player)
	{
		// Green brick runner.
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			Point point = Main.MouseWorld.ToTileCoordinates();
			var checkTiles = YggdrasilWorldGeneration.BFSContinueTile(point, true, 1024);
			foreach (var tile in checkTiles)
			{
				if (tile.TileType == TileID.GreenDungeonBrick)
				{
					tile.TileType = (ushort)ModContent.TileType<GreenRelicBrick>();
				}

				//if (tile.WallType == WallID.GreenDungeonSlab)
				//{
				//	tile.WallType = (ushort)ModContent.WallType<GreenRelicWall>();
				//}
				//if (tile.WallType == WallID.GreenDungeon)
				//{
				//	tile.WallType = (ushort)ModContent.WallType<GreenRelicWall_Style2>();
				//}
				tile.wall = 0;
				//tile.wall = (ushort)ModContent.WallType<GreenRelicWall_Style2>();
			}
		}
	}
}