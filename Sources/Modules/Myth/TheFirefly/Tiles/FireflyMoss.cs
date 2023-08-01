using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly.Tiles;

public class FireflyMoss : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = true;
		Main.tileMoss[Type] = true;
		Main.tileCut[Type] = true;
		AddMapEntry(new Color(51, 107, 204));
		DustType = ModContent.DustType<FluorescentTreeDust>();
	}
	public override bool CreateDust(int i, int j, ref int type)
	{
		Dust d = Dust.NewDustDirect(new Vector2(i, j) * 16, 16, 16, DustType,0,0,0,default, Main.rand.NextFloat(0.3f, 0.6f));
		return false;
	}
	public override void MouseOver(int i, int j)
	{
		Player player = Main.LocalPlayer;
		if(player.HeldItem.type == ItemID.PaintScraper || player.HeldItem.type == ItemID.SpectrePaintScraper)
		{
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = player.HeldItem.type;
			if(player.controlUseItem)
			{
				WorldGen.KillTile(i, j);
			}
		}
		base.MouseOver(i, j);
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		Player player = Main.LocalPlayer;
		if (player.HeldItem.type == ItemID.PaintScraper || player.HeldItem.type == ItemID.SpectrePaintScraper)
		{
			if(Main.rand.NextBool(10))
			{
				yield return new Item(ModContent.ItemType<Items.FireflyMoss>());
			}
		}
		else
		{
			yield break;
		}
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer)
		{
			var tile = Main.tile[i, j];
			foreach (Player player in Main.player)
			{
				if(player != null)
				{
					if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
					{
						float referenceValue = player.velocity.X;
						if(tile.TileFrameY >= 54)
						{
							referenceValue = -player.velocity.X;
						}
						if (tile.TileFrameY >= 108)
						{
							referenceValue = player.velocity.Y;
						}
						if (tile.TileFrameY >= 162)
						{
							referenceValue = -player.velocity.Y;
						}
						if (!TileSpin.TileRotation.ContainsKey((i, j)))
							TileSpin.TileRotation.Add((i, j), new Vector2(Math.Clamp(referenceValue * 0.02f, -1, 1) * 0.2f));
						else
						{
							float rot;
							float omega;
							omega = TileSpin.TileRotation[(i, j)].X;
							rot = TileSpin.TileRotation[(i, j)].Y;
							if (Math.Abs(omega) < 0.4f && Math.Abs(rot) < 0.4f)
								TileSpin.TileRotation[(i, j)] = new Vector2(omega + Math.Clamp(referenceValue * 0.02f, -1, 1) * 2f, rot + omega + Math.Clamp(referenceValue * 0.02f, -1, 1) * 2f);
							if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i, j));
						}
					}
					if (Main.tile[i, j].WallType == 0)
					{
						float referenceValue = Main.windSpeedCurrent;
						if (tile.TileFrameY >= 54)
						{
							referenceValue = -Main.windSpeedCurrent;
						}
						if (tile.TileFrameY >= 108)
						{
							referenceValue = Main.windSpeedCurrent;
						}
						if (tile.TileFrameY >= 162)
						{
							referenceValue = -Main.windSpeedCurrent;
						}
						if (!TileSpin.TileRotation.ContainsKey((i, j)))
							TileSpin.TileRotation.Add((i, j), new Vector2(Math.Clamp(referenceValue, -1, 1) * (0.3f + MathUtils.Sin(i + (float)Main.time / 24f) * 0.2f)));
						else
						{
							float rot;
							float omega;
							omega = TileSpin.TileRotation[(i, j)].X;
							rot = TileSpin.TileRotation[(i, j)].Y;
							if (Math.Abs(omega) < 4f && Math.Abs(rot) < 4f)
								TileSpin.TileRotation[(i, j)] = new Vector2(omega * 0.999f + Math.Clamp(referenceValue, -1, 1) * (0.3f + MathUtils.Sin(i + (float)Main.time / 24f) * 0.1f) * 0.002f, rot * 0.999f + Math.Clamp(referenceValue, -1, 1) * (0.3f + MathUtils.Sin(i + (float)Main.time / 24f) * 0.1f) * 0.002f);
						}
					}
				}
			}
		}
	}
	
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];

		var tileUp = Main.tile[i, j - 1];
		var tileDown = Main.tile[i, j + 1];
		var tileLeft = Main.tile[i - 1, j];
		var tileRight = Main.tile[i + 1, j];
		int offsetX = 0;
		int offsetY = 0;
		if(CheckDarkCocoonMoss(tileUp))
		{
			offsetY -= 2;
		}
		if (CheckDarkCocoonMoss(tileDown))
		{
			offsetY += 2;
		}
		if (CheckDarkCocoonMoss(tileLeft))
		{
			offsetX -= 2;
		}
		if (CheckDarkCocoonMoss(tileRight))
		{
			offsetX += 2;
		}
		if(offsetX == 0 && offsetY == 0)
		{
			WorldGen.KillTile(i, j, false, false, true);
		}
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;

		Texture2D texMain = ModAsset.Tiles_FireflyMoss.Value;
		Texture2D texGlow = ModAsset.FireflyMossGlow.Value;

		var tsp = new TileSpin();
		tsp.Update(i, j);
		tsp.DrawRotatedTile(spriteBatch, i, j, texMain, new Rectangle(tile.TileFrameX, tile.TileFrameY, 20, 16), new Vector2(10 + offsetX * 4, 8 + offsetY * 4), offsetX * 5.5f + 8, offsetY * 5.5f + 8, false);
		tsp.DrawRotatedTile(spriteBatch, i, j, texGlow, new Rectangle(tile.TileFrameX, tile.TileFrameY, 20, 16), new Vector2(10 + offsetX * 4, 8 + offsetY * 4), offsetX * 5.5f + 8, offsetY * 5.5f + 8, true, new Color(55, 55, 55, 0));

		base.PostDraw(i, j, spriteBatch);
		return false;
	}
	private bool CheckDarkCocoonMoss(Tile tile)
	{
		if(tile.HasTile)
		{
			if (tile.TileType == ModContent.TileType<DarkCocoonMoss>())
			{
				return true;
			}
		}
		return false;
	}
}