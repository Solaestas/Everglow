using Everglow.TwilightForest.Common;
namespace Everglow.TwilightForest.Tiles;

public class TwilightGrassBlock : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileFrameImportant[Type] = false;
		Main.tileMergeDirt[Type] = true;
		TileID.Sets.Grass[Type] = true;
		TileID.Sets.ReplaceTileBreakDown[Type] = false;
		TileID.Sets.ReplaceTileBreakUp[Type] = false;

		Main.tileBlockLight[Type] = true;
		DustType = DustID.Dirt;
		ItemDrop = ItemID.DirtBlock;
		AddMapEntry(new Color(33, 140, 141));
	}
	public override void RandomUpdate(int i, int j)
	{
		//蔓延
		Tile ThisTile = Main.tile[i, j];
		if (Main.dayTime)
		{
			for (int x = -40; x < 41; x++)
			{
				for (int y = -40; y < 41; y++)
				{
					var tile = Main.tile[i + x, j + y];

					if (tile.TileType == ThisTile.TileType)
					{
						tile.TileType = TileID.Grass;
					}
				}
			}
		}
		else
		{
			for (int d = 0; d < 144; d++)
			{
				int x = Main.rand.Next(-22, 23);
				int y = Main.rand.Next(-22, 23);
				var tile = Main.tile[i + x, j + y];
				if (tile.TileType == TileID.Grass && tile.HasTile)
				{
					tile.TileType = ThisTile.TileType;
				}
			}
		}
		//用来扼杀被包裹住的的暮光草
		var UpTile = Main.tile[i, j - 1];
		var DownTile = Main.tile[i, j + 1];
		var LeftTile = Main.tile[i - 1, j];
		var RightTile = Main.tile[i + 1, j];
		if (IsDirt(UpTile) && IsDirt(DownTile) && IsDirt(LeftTile) && IsDirt(RightTile))
			ThisTile.TileType = TileID.Dirt;

		switch (Main.rand.Next(5))
		{
			case 0:
				//生成装饰块3*1
				for (int x = -1; x < 2; x++)
				{
					Tile nearTile = Main.tile[i + x, j - 1];
					if (nearTile.HasTile || nearTile.TileType != 0)
					{
						return;
					}
				}
				for (int x = -1; x < 2; x++)
				{
					Tile nearTile = Main.tile[i + x, j];
					if (nearTile.TileType != Type || !nearTile.HasTile)
					{
						return;
					}
				}
				TwilightForestUtils.PlaceFrameImportantTiles(i - 1, j - 1, 3, 1, ModContent.TileType<TwilightCrystal_Flat>());
				break;
			case 1:
				//生成装饰块3*3
				for (int x = -1; x < 2; x++)
				{
					for (int y = -1; y < -4; y--)
					{
						Tile nearTile = Main.tile[i + x, j + y];
						if (nearTile.HasTile || nearTile.TileType != 0)
						{
							Main.NewText("fail");
							return;
						}
					}
					for (int y = 0; y < -1; y--)
					{
						Tile nearTile = Main.tile[i + x, j + y];
						if (nearTile.TileType != Type || !nearTile.HasTile)
						{
							Main.NewText("fail");
							return;
						}
					}
				}
				TwilightForestUtils.PlaceFrameImportantTiles(i - 1, j - 3, 3, 3, ModContent.TileType<TwilightCrystal_Large>());
				break; 
			case 2:
				//生成装饰块2*2
				for (int x = 0; x < 2; x++)
				{
					for (int y = -1; y < -3; y--)
					{
						Tile nearTile = Main.tile[i + x, j + y];
						if (nearTile.HasTile || nearTile.TileType != 0)
						{
							Main.NewText("fail");
							return;
						}
					}
					for (int y = 0; y < -1; y--)
					{
						Tile nearTile = Main.tile[i + x, j + y];
						if (nearTile.TileType != Type || !nearTile.HasTile)
						{
							Main.NewText("fail");
							return;
						}
					}
				}
				TwilightForestUtils.PlaceFrameImportantTiles(i - 1, j - 2, 2, 2, ModContent.TileType<TwilightCrystal_small>(), Main.rand.Next(2) * 36);
				break;
			case 3:
				//生成装饰块4*3
				for (int x = -1; x < 3; x++)
				{
					for (int y = -1; y < -4; y--)
					{
						Tile nearTile = Main.tile[i + x, j + y];
						if (nearTile.HasTile || nearTile.TileType != 0)
						{
							return;
						}
					}
					for (int y = 0; y < -1; y--)
					{
						Tile nearTile = Main.tile[i + x, j + y];
						if (nearTile.TileType != Type || !nearTile.HasTile)
						{
							return;
						}
					}
				}
				TwilightForestUtils.PlaceFrameImportantTiles(i - 1, j - 3, 4, 3, ModContent.TileType<TwilightCrystalFlower>());
				break;
			case 4:
				//生成树木
				for (int x = -2; x < 4; x++)
				{
					var nearTile = Main.tile[i + x, j];
					if (nearTile.TileType != Type || !nearTile.HasTile)
					{
						return;
					}
				}
				for (int x = 0; x < 2; x++)
				{
					for (int y = -1; y > -40; y--)
					{
						var nearTile = Main.tile[i + x, j + y];
						if (nearTile.HasTile)
						{
							return;
						}
					}
				}
				BuildTwilightTree(i, j - 1, 36);
				break;
		}				
	}
	public void BuildTwilightTree(int i, int j, int height = 10)
	{
		if (j < height + 20)
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
	private bool IsDirt(Tile tile)
	{
		if (tile.TileType == TileID.Dirt && tile.HasTile)
			return true;
		return false;
	}
}