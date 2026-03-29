using Everglow.Yggdrasil.KelpCurtain.Tiles.IsleOfBloom;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Tools.Developer;

public class ResetIsleOfBloom : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 30;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public Point OldMousePos = default;

	public override void HoldItem(Player player) => base.HoldItem(player);

	public override bool CanUseItem(Player player)
	{
		BuildIsleOfBloom(Main.MouseWorld);
		return false;
	}

	public void BuildIsleOfBloom(Vector2 worldPos)
	{
		Point tilePos = worldPos.ToTileCoordinates();
		YggdrasilWorldGeneration.KillRectangleAreaOfTile(tilePos.X - 150, tilePos.Y - 60, tilePos.X + 150, tilePos.Y + 150);

		for (int x = -130; x <= 130; x++)
		{
			for (int y = 0; y <= 120; y++)
			{
				var checkPoint = tilePos + new Point(x, y);
				var tile = TileUtils.SafeGetTile(checkPoint);
				float value0 = YggdrasilWorldGeneration.GetPerlinPixelG(x, y) * 12;
				if (y < 4 + value0)
				{
					tile.TileType = (ushort)ModContent.TileType<Tiles.OldMoss>();
				}
				else
				{
					tile.TileType = (ushort)ModContent.TileType<Tiles.MossProneSandSoil>();
				}
				float value1 = YggdrasilWorldGeneration.GetPerlinPixelR(x, y);
				if (y > value1 * 3 + value0)
				{
					tile.HasTile = true;
				}
				else
				{
					tile.HasTile = false;
				}
			}
		}

		// Middle Cave
		for (int y = -10; y <= 70; y += 2)
		{
			float radius = (60 - y) / 3f;
			radius = MathF.Max(radius, 10);
			TileUtils.PlaceCircleAreaOfBlockWithRandomNoise(tilePos + new Point(0, y), radius, ModContent.TileType<Tiles.OldMoss>(), 10, (int)TileUtils.TileChangeState.TileOnly);
			TileUtils.PlaceCircleAreaOfBlockWithRandomNoise(tilePos + new Point(0, y), radius - 3, -2, 10, (int)TileUtils.TileChangeState.Forceful);
		}

		// SubFloor Cave
		List<Vector2> Cave0_Bound = new List<Vector2>();
		List<Vector2> Cave0 = new List<Vector2>();
		int cave0Y = 75;
		int caveHeight = 8;
		for (int x = -130; x <= 130; x++)
		{
			float height = 130 - MathF.Abs(x);
			height = Math.Clamp(height, 0, caveHeight + 4);
			float value2 = TileUtils.GetPerlinPixelR(x * 2, cave0Y) * 4;
			height += value2;
			Cave0_Bound.Add(new Vector2(x, cave0Y - height));
			height -= 4;
			height = Math.Clamp(height, 0, caveHeight);
			value2 = TileUtils.GetPerlinPixelB(x * 2, cave0Y + 40) * 4;
			height += value2;
			Cave0.Add(new Vector2(x, cave0Y - height));
		}
		for (int x = 130; x >= -130; x--)
		{
			float height = 130 - MathF.Abs(x);
			height = Math.Clamp(height, 0, caveHeight + 4);
			float value2 = TileUtils.GetPerlinPixelB(x * 2, cave0Y + 30) * 4;
			height += value2;
			Cave0_Bound.Add(new Vector2(x, cave0Y + height));
			height -= 4;
			height = Math.Clamp(height, 0, caveHeight);
			value2 = TileUtils.GetPerlinPixelB(x * 2, cave0Y + 70) * 4;
			height += value2;
			Cave0.Add(new Vector2(x, cave0Y + height));
		}
		TileUtils.PlacePolygonAreaOfBlockWithOffset(Cave0_Bound, tilePos.ToVector2(), ModContent.TileType<Tiles.OldMoss>(), (int)TileUtils.TileChangeState.TileOnly);
		TileUtils.PlacePolygonAreaOfBlockWithOffset(Cave0, tilePos.ToVector2(), -2, (int)TileUtils.TileChangeState.Forceful);
		TileUtils.SmoothTile(tilePos.X - 150, tilePos.Y - 60, tilePos.X + 150, tilePos.Y + 150);

		// Bamboo
		for (int x = -130; x <= 130; x++)
		{
			if (MathF.Abs(x) > 30)
			{
				if (TileUtils.GenRand.NextBool(4))
				{
					int surfaceY = 0;
					for (int y = 0; y <= 40; y++)
					{
						var checkPoint = tilePos + new Point(x, y);
						var tile = TileUtils.SafeGetTile(checkPoint);
						if (tile.HasTile)
						{
							surfaceY = y - 1;
							break;
						}
					}
					float value2 = YggdrasilWorldGeneration.GetPerlinPixelG(x * 24, surfaceY) * 40;
					for (int j = 0; j <= 27 + value2; j++)
					{
						var checkPoint = tilePos + new Point(x, surfaceY - j);
						var tile = TileUtils.SafeGetTile(checkPoint);
						tile.TileType = (ushort)ModContent.TileType<IsleBamboo>();
						tile.HasTile = true;
					}
				}
			}
		}

		// Side peach
		for (int y = -10; y <= 40; y += 2)
		{
			if (TileUtils.GenRand.NextBool(4))
			{
				int checkX = 0;
				int direction = -1;
				if (TileUtils.GenRand.NextBool())
				{
					direction = 1;
				}
				for (int x = 0; x < 23; x++)
				{
					var tile = TileUtils.SafeGetTile(tilePos.X + x * direction, tilePos.Y + y);
					checkX = (x - 1) * direction;
					if (tile.HasTile)
					{
						break;
					}
				}
				if (MathF.Abs(checkX) < 21)
				{
					if (direction == -1)
					{
						if (YggdrasilWorldGeneration.CheckSpaceRight(tilePos.X + checkX, tilePos.Y + y) > 11)
						{
							TileUtils.PlaceFrameImportantTiles(tilePos.X + checkX, tilePos.Y + y, 10, 1, ModContent.TileType<IslePeachTree_side>(), 180);
						}
					}
					else
					{
						if (YggdrasilWorldGeneration.CheckSpaceLeft(tilePos.X + checkX, tilePos.Y + y) > 11)
						{
							TileUtils.PlaceFrameImportantTiles(tilePos.X + checkX - 10, tilePos.Y + y, 10, 1, ModContent.TileType<IslePeachTree_side>(), 0);
						}
					}
					break;
				}
			}
		}

		// Peach
		for (int x = -30; x <= 30; x++)
		{
			if (MathF.Abs(x) > 10)
			{
				if (TileUtils.GenRand.NextBool(6))
				{
					int surfaceY = 0;
					for (int y = 0; y <= 40; y++)
					{
						var checkPoint = tilePos + new Point(x, y);
						var tile = TileUtils.SafeGetTile(checkPoint);
						surfaceY = y - 1;
						if (tile.HasTile && tile.TileType != ModContent.TileType<IslePeachTree_side>())
						{
							break;
						}
					}
					if (surfaceY < 30)
					{
						float value2 = YggdrasilWorldGeneration.GetPerlinPixelG(x * 12, surfaceY) * 4;
						for (int j = 0; j <= 1 + value2; j++)
						{
							var checkPoint = tilePos + new Point(x, surfaceY - j);
							var tile = TileUtils.SafeGetTile(checkPoint);
							tile.TileType = (ushort)ModContent.TileType<IslePeachTree_medium>();
							tile.HasTile = true;
						}
						x += 5;
					}
				}
			}
		}

		// Small Peach
		for (int x = -60; x <= 60; x++)
		{
			if (TileUtils.GenRand.NextBool(16))
			{
				int surfaceY = 0;
				for (int y = 0; y <= 50; y++)
				{
					var checkPoint = tilePos + new Point(x, y);
					var tile = TileUtils.SafeGetTile(checkPoint);
					surfaceY = y;
					if (tile.HasTile && tile.TileType == ModContent.TileType<Tiles.OldMoss>())
					{
						break;
					}
				}
				if (surfaceY < 48)
				{
					TileUtils.PlaceFrameImportantTilesAbove(tilePos.X + x, tilePos.Y + surfaceY, 1, 2, ModContent.TileType<IslePeachTree_small>());
				}
			}
		}
	}
}