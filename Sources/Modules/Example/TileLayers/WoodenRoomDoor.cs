using SixLabors.ImageSharp.ColorSpaces;
using Terraria.ObjectData;

namespace Everglow.Example.TileLayers;

public class WoodenRoomDoor : LayerDeeperTriggerTile
{
	public override void SSD()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = 4;
		TileObjectData.newTile.Width = 4;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16
		};
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(15, 11, 9));
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		Player player = Main.LocalPlayer;
		int layer = TileLayerSystem.PlayerZoneLayer[player.whoAmI];
		for (int x = -20; x < 21;x++)
		{
			for(int y = -12; y < 4; y++)
			{
				var myTile = new TileClone();
				myTile.I = i + x;
				myTile.J = j + y;
				myTile.TileType = WallID.Wood;
				myTile.HasTile = false;
				myTile.Wall = WallID.Wood;
				if(x == -20 || x== 20||y == -12 || y == 3)
				{
					myTile.HasTile = true;
					myTile.TileType = TileID.WoodBlock;
				}
				TileLayerSystem.LayerTile[(i + x, j + y, layer - 1)] = myTile;
			}
		}
		for (int x = -6; x < 7; x++)
		{
			for (int y = -28; y < 4; y++)
			{
				var myTile = new TileClone();
				myTile.I = i + x;
				myTile.J = j + y;
				myTile.TileType = WallID.Wood;
				myTile.HasTile = false;
				myTile.Wall = WallID.Wood;
				if (x == -6 || x == 6 || y == -28 || y == 3)
				{
					myTile.HasTile = true;
					myTile.TileType = TileID.WoodBlock;
				}
				TileLayerSystem.LayerTile[(i + x, j + y, layer - 1)] = myTile;
			}
		}
	}
}