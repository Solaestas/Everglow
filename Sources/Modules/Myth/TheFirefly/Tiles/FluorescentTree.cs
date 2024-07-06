using Everglow.Myth.TheFirefly.Dusts;
using Everglow.Myth.TheFirefly.Gores;
using Terraria.Localization;
using Everglow.Commons.Physics.MassSpringSystem;

namespace Everglow.Myth.TheFirefly.Tiles;

public class FluorescentTree : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = true;
		Main.tileAxe[Type] = true;
		Main.tileNoAttach[Type] = false;

		TileID.Sets.IsATreeTrunk[Type] = true;
		var modTranslation = Language.GetOrRegister("Mods.Everglow.MapEntry.FluorescentTree");
		AddMapEntry(new Color(51, 26, 58), modTranslation);
		DustType = ModContent.DustType<FluorescentTreeDust>();
		AdjTiles = new int[] { Type };
	}

	public Dictionary<(int, int), List<Commons.Physics.MassSpringSystem.Rope>> HasRopeInCoord = new();

	public void InsertOneTreeRope(int i, int j, int style)
	{
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.GlowWood>());
	}

	public override bool CanDrop(int i, int j)
	{
		for (int x = 0; x < 6; x++)
		{
			Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
		}
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 2)
		{
			for (int x = 0; x < 12; x++)
			{
				Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
			}
			for (int x = 0; x < 64; x++)
			{
				Dust.NewDust(new Vector2(i * 16 - 40, j * 16 - 100), 96, 96, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
			}
		}
		if (tile.TileFrameY > 3)
		{
			Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
		}
		return false;
	}

	private void Shake(int i, int j, int frameY)
	{
		if (Main.rand.NextBool(7))
		{
			if (frameY == 2)
			{
				for (int x = 0; x < 12; x++)
				{
					Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
				}
			}
			if (frameY > 3)
			{
				Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
			}
		}
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		// TODO 处理手动的Drop调用
		int Dy = -1; // 向上破坏的自变化Y坐标
		if (!fail)
		{
			// 以下是破坏的特效,比如落叶
			if (Main.tile[i, j].TileFrameY < 4)
			{
				Tile tileLeft;
				Tile tileRight;
				tileLeft = Main.tile[i - 1, j];
				if (tileLeft.TileType == Type)
				{
					Shake(i - 1, j, tileLeft.TileFrameY);
					CanDrop(i - 1, j);
				}
				tileRight = Main.tile[i + 1, j];
				if (tileRight.TileType == Type)
				{
					Shake(i + 1, j, tileRight.TileFrameY);
					CanDrop(i + 1, j);
				}
				while (Main.tile[i, j + Dy].HasTile && Main.tile[i, j + Dy].TileType == Type && Dy > -100)
				{
					Shake(i, j + Dy, Main.tile[i, j + Dy].TileFrameY);
					CanDrop(i, j + Dy);

					tileLeft = Main.tile[i - 1, j + Dy];
					tileRight = Main.tile[i + 1, j + Dy];
					if (tileLeft.TileType == Type)
					{
						if (tileLeft.TileFrameY == 2)
						{
							break;
						}

						Shake(i - 1, j + Dy, tileLeft.TileFrameY);
						CanDrop(i - 1, j + Dy);
					}
					if (tileRight.TileType == Type)
					{
						if (tileRight.TileFrameY == 2)
						{
							break;
						}

						Shake(i + 1, j + Dy, tileRight.TileFrameY);
						CanDrop(i + 1, j + Dy);
					}
					Dy -= 1;
				}
				Dy = -1; // 向上破坏的自变化Y坐标
				tileLeft = Main.tile[i - 1, j];
				if (tileLeft.TileType == Type)
				{
					tileLeft.HasTile = false;
				}
				tileRight = Main.tile[i + 1, j];
				if (tileRight.TileType == Type)
				{
					tileRight.HasTile = false;
				}
				while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
				{
					Tile baseTile = Main.tile[i, j + Dy];

					baseTile.HasTile = false;

					tileLeft = Main.tile[i - 1, j + Dy];
					tileRight = Main.tile[i + 1, j + Dy];
					if (tileLeft.TileType == Type)
					{
						tileLeft.HasTile = false;
					}

					if (tileRight.TileType == Type)
					{
						tileRight.HasTile = false;
					}
					Dy -= 1;
				}
			}
		}
		else
		{
			Tile tileLeft;
			Tile tileRight;
			while (Main.tile[i, j + Dy].HasTile && Main.tile[i, j + Dy].TileType == Type && Dy > -100)
			{
				Shake(i, j + Dy, Main.tile[i, j + Dy].TileFrameY);
				tileLeft = Main.tile[i - 1, j + Dy];
				tileRight = Main.tile[i + 1, j + Dy];
				if (tileLeft.TileType == Type)
				{
					if (tileLeft.TileFrameY == 2)
					{
						break;
					}
					Shake(i - 1, j + Dy, tileLeft.TileFrameY);
				}
				if (tileRight.TileType == Type)
				{
					if (tileRight.TileFrameY == 2)
					{
						break;
					}
					Shake(i + 1, j + Dy, tileRight.TileFrameY);
				}
				Dy -= 1;
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D treeTexture = ModAsset.FluorescentTree.Value;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}

		Tile tile = Main.tile[i, j];
		int Width;
		int Height = 16;
		int TexCoordY;
		int OffsetY = 20;
		int OffsetX = 0;
		Color color = Lighting.GetColor(i, j);
		switch (tile.TileFrameY)
		{
			default:
				return false;
			case 0: // 树桩
				Width = 74;
				Height = 24;
				TexCoordY = 180;
				break;
			case 1: // 树干
				Width = 26;
				TexCoordY = 2;
				break;
			case 2: // 树冠
				Width = 150;
				Height = 132;
				TexCoordY = 46;
				OffsetY = 24;
				Lighting.AddLight(i, j, 0.4f, 0.4f, 0.4f);
				return false;
			case 3: // 粗树干
				Width = 50;
				Height = 24;
				TexCoordY = 20;
				OffsetY = 28;
				break;
			case 4: // 左树枝
				Width = 34;
				Height = 32;
				OffsetY = 32;
				OffsetX = -8;
				TexCoordY = 240;
				break;
			case 5: // 右树枝
				Width = 34;
				Height = 32;
				OffsetY = 32;
				OffsetX = 8;
				TexCoordY = 206;
				break;
		}
		var origin = new Vector2(Width / 2f, Height);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY, Width, Height), color, 0, origin, 1, SpriteEffects.None, 0);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY + 274, Width, Height), new Color(1f, 1f, 1f, 0), 0, origin, 1, SpriteEffects.None, 0);
		return false;
	}
}