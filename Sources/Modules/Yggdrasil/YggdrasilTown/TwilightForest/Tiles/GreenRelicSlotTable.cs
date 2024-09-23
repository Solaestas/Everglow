using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

public class GreenRelicSlotTable : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileLavaDeath[Type] = false;

		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 3;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18,
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(70, 99, 99));
		DustType = ModContent.DustType<GreenRelicBrick_dust>();
		MinPick = 9999;
	}

	public override bool RightClick(int i, int j)
	{
		if (Main.netMode == NetmodeID.Server)
		{
			return false;
		}
		Player player = Main.LocalPlayer;
		foreach (var item in player.inventory)
		{
			if(item.type == ModContent.ItemType<CelticKeyStone>())
			{
				item.stack--;
				if (item.stack <= 0)
				{
					item.active = false;
					return false;
				}
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(i, j);
				Point topLeftPoint = new Point(i - tile.TileFrameX / 18, j - tile.TileFrameY / 18);
				YggdrasilWorldGeneration.KillRectangleAreaOfTile(topLeftPoint.X - 1, topLeftPoint.Y, topLeftPoint.X + 3, topLeftPoint.Y + 42);
				YggdrasilWorldGeneration.SmoothTile(topLeftPoint.X - 2, topLeftPoint.Y, topLeftPoint.X + 4, topLeftPoint.Y + 42);
			}
		}
		return false;
	}

	public override void MouseOver(int i, int j)
	{
		if(Main.netMode == NetmodeID.Server)
		{
			return;
		}
		Player player = Main.LocalPlayer;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<CelticKeyStone>();
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return base.PreDraw(i, j, spriteBatch);
	}
}