using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class YggdrasilCyathea : ModTile
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

		AddMapEntry(new Color(71, 86, 59));
		DustType = ModContent.DustType<YggdrasilCyatheaTrunkDust>();
		AdjTiles = new int[] { Type };
	}

	private Vector2[] basePositions = new Vector2[16];

	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.YggdrasilCyatheaWood>());
	}
	public override bool CanDrop(int i, int j)
	{
		for (int x = 0; x < 6; x++)
		{
			Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
		}
		//var tile = Main.tile[i, j];
		//if (tile.TileFrameY == 2)
		//{
		//	for (int x = 0; x < 12; x++)
		//	{
		//		Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
		//	}
		//	for (int x = 0; x < 64; x++)
		//	{
		//		Dust.NewDust(new Vector2(i * 16 - 40, j * 16 - 100), 96, 96, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
		//	}
		//}
		//if (tile.TileFrameY > 3)
		//	Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));

		return false;
	}

	private void Shake(int i, int j, int frameY)
	{
		//if (Main.rand.NextBool(7))
		//{
		//	if (frameY == 2)
		//	{
		//		for (int x = 0; x < 12; x++)
		//		{
		//			Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
		//		}
		//	}
		//	if (frameY > 3)
		//		Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
		//}
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		// TODO 处理手动的Drop调用
		int Dy = -1;//向上破坏的自变化Y坐标
		if (!fail)
		{
			//以下是破坏的特效,比如落叶
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
							break;
						Shake(i - 1, j + Dy, tileLeft.TileFrameY);
						CanDrop(i - 1, j + Dy);
					}
					if (tileRight.TileType == Type)
					{
						if (tileRight.TileFrameY == 2)
							break;
						Shake(i + 1, j + Dy, tileRight.TileFrameY);
						CanDrop(i + 1, j + Dy);
					}
					Dy -= 1;
				}

				Dy = -1;//向上破坏的自变化Y坐标
				tileLeft = Main.tile[i - 1, j];
				if (tileLeft.TileType == Type)
					tileLeft.HasTile = false;
				tileRight = Main.tile[i + 1, j];
				if (tileRight.TileType == Type)
					tileRight.HasTile = false;
				while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
				{
					Tile baseTile = Main.tile[i, j + Dy];

					baseTile.HasTile = false;

					tileLeft = Main.tile[i - 1, j + Dy];
					tileRight = Main.tile[i + 1, j + Dy];
					if (tileLeft.TileType == Type)
						tileLeft.HasTile = false;
					if (tileRight.TileType == Type)
						tileRight.HasTile = false;
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
						break;
					Shake(i - 1, j + Dy, tileLeft.TileFrameY);
				}
				if (tileRight.TileType == Type)
				{
					if (tileRight.TileFrameY == 2)
						break;
					Shake(i + 1, j + Dy, tileRight.TileFrameY);
				}
				Dy -= 1;
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D treeTexture = ModAsset.YggdrasilCyathea.Value;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Tile tile = Main.tile[i, j];
		int Width;
		int Height = 16;
		int TexCoordY;
		int OffsetY = 20;
		int OffsetX = 0;
		float Rot = 0;
		Color color = Lighting.GetColor(i, j);
		switch (tile.TileFrameY)
		{
			default:
				return false;

			case 0:  //树桩
				Width = 24;
				Height = 20;
				TexCoordY = 180;
				break;

			case 1:  //树干
				Width = 26;
				TexCoordY = 2;
				break;

			case 2:  //树冠
				Width = 150;
				Height = 132;
				TexCoordY = 46;
				float Wind = Main.windSpeedCurrent / 15f;
				Rot = Wind + (float)Math.Sin(j + Main.timeForVisualEffects / 30f) * Wind * 0.3f;
				OffsetY = 24;
				break;
		}
		var origin = new Vector2(Width / 2f, Height);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY, Width, Height), color, Rot, origin, 1, SpriteEffects.None, 0);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY + 274, Width, Height), new Color(1f, 1f, 1f, 0), Rot, origin, 1, SpriteEffects.None, 0);

		return false;
	}
}