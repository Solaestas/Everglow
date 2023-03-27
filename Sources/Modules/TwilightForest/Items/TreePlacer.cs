using Everglow.TwilightForest.Tiles;
using Terraria.ModLoader;

namespace Everglow.TwilightForest.Items;

internal class TreePlacer : ModItem
{
	//TODO:这是一个代码测试物品，使用后可能会引起程序崩坏
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 32;
		Item.useTime = 40;
		Item.useAnimation = 40;
	}
	public override void HoldItem(Player player)
	{
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			int i = (int)(Main.MouseWorld.X / 16);
			int j = (int)(Main.MouseWorld.Y / 16);
			WorldGen.PlaceTile(i,j,ModContent.TileType<Tiles.TwilightGrassBlock>());
			BuildTwilightTree(i, j, 30);
		}
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			int i = (int)(Main.MouseWorld.X / 16);
			int j = (int)(Main.MouseWorld.Y / 16);
			Main.NewText(Main.tile[i, j].TileType);
			Main.NewText(Main.tile[i, j].TileFrameX, Color.Green);
			Main.NewText(Main.tile[i, j].TileFrameY, Color.GreenYellow);
			if(Main.tile[i, j].type == TileID.Dirt || Main.tile[i, j].type == TileID.Grass)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<TwilightGrassBlock>();
			}
		}

	}
	public void BuildTwilightTree(int i, int j, int height = 10)
	{
		if (j < 30)
			return;
		int Height = Main.rand.Next(7, height);

		for (int g = 0; g < Height; g++)
		{
			Tile tile = Main.tile[i, j - g];
			Tile tileRight = Main.tile[i + 1, j - g];

			if (g == 0)
			{
				tile.TileType = (ushort)ModContent.TileType<TwilightTree>();
				tile.TileFrameY = 0;
				tile.TileFrameX = 0;
				tile.HasTile = true;

				tileRight.TileType = (ushort)ModContent.TileType<TwilightTree>();
				tileRight.TileFrameY = -1;
				tileRight.TileFrameX = 0;
				tileRight.HasTile = true;
				continue;
			}
			if (g == 1)
			{
				tile.TileType = (ushort)ModContent.TileType<TwilightTree>();
				tile.TileFrameY = -1;
				tile.TileFrameX = 0;
				tile.HasTile = true;

				tileRight.TileType = (ushort)ModContent.TileType<TwilightTree>();
				tileRight.TileFrameY = -1;
				tileRight.TileFrameX = 0;
				tileRight.HasTile = true;
				continue;
			}

			if (g == Height - 1)
			{
				tile.TileType = (ushort)ModContent.TileType<TwilightTree>();
				tile.TileFrameY = 3;
				tile.TileFrameX = 0;
				tile.HasTile = true;

				tileRight.TileType = (ushort)ModContent.TileType<TwilightTree>();
				tileRight.TileFrameY = -1;
				tileRight.TileFrameX = 0;
				tileRight.HasTile = true;
				continue;
			}
			tile.TileType = (ushort)ModContent.TileType<TwilightTree>();
			tile.TileFrameY = 1;
			tile.TileFrameX = (short)Main.rand.Next(4);
			tile.HasTile = true;

			tileRight.TileType = (ushort)ModContent.TileType<TwilightTree>();
			tileRight.TileFrameY = 2;
			tileRight.TileFrameX = (short)Main.rand.Next(4);
			tileRight.HasTile = true;
		}
	}
}
