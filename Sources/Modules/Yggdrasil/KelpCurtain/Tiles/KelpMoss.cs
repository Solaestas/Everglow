using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.GameContent.Drawing;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class KelpMoss : ModTile, ITileFluentlyDrawn
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		TileID.Sets.IsVine[Type] = true;
		TileID.Sets.VineThreads[Type] = true;
		DustType = ModContent.DustType<KelpMoss_dust>();
		Main.tileCut[Type] = true;
		AddMapEntry(new Color(36, 86, 19));
		HitSound = SoundID.Grass;
	}

	public override void PlaceInWorld(int i, int j, Item item)
	{
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield break;
	}

	public override void RandomUpdate(int i, int j)
	{
		int deltaY = 0;
		while (Main.tile[i, j - 1 - deltaY].TileType == Type)
		{
			deltaY++;
			if (deltaY > j - 1)
			{
				break;
			}
		}
		if (deltaY > 15 + Math.Sin(i + j) * 3)
		{
			return;
		}
		if (Main.rand.NextBool(Math.Max(1, deltaY * deltaY - 40)))
		{
			var tileBelow = Main.tile[i, j + 1];
			if (!tileBelow.HasTile)
			{
				tileBelow.TileType = Type;
				tileBelow.HasTile = true;

				// 与原版一致，漆与涂料继承
				tileBelow.CopyPaintAndCoating(Main.tile[i, j]);

				// frame调整与联机同步
				WorldGen.SquareTileFrame(i, j + 1);
				if (Main.netMode is NetmodeID.Server)
				{
					NetMessage.SendTileSquare(-1, i, j + 1);
				}
			}
		}
		base.RandomUpdate(i, j);
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		//Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
		if (Main.tile[i, j - 1].TileType != Type)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		DrawVineStrip(screenPosition, Vector2.zeroVector, pos.X, pos.Y, tileDrawing);
	}

	/// <summary>
	/// Vanilla function.
	/// </summary>
	/// <param name="tileDrawing"></param>
	/// <param name="screenPosition"></param>
	/// <param name="offSet"></param>
	/// <param name="x"></param>
	/// <param name="startY"></param>
	private void DrawVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY, TileDrawing tileDrawing)
	{
		int num = 0;
		int num2 = 0;
		Vector2 value = new Vector2(x * 16 + 8, startY * 16 - 2);
		float amount = Math.Abs(Main.WindForVisuals) / 1.2f;
		amount = MathHelper.Lerp(0.2f, 1f, amount);
		float num3 = -0.08f * amount;
		float windCycle = tileDrawing.GetWindCycle(x, startY, tileDrawing._vineWindCounter);
		float num4 = 0f;
		float num5 = 0f;
		for (int i = startY; i < Main.maxTilesY - 10; i++)
		{
			Tile tile = Main.tile[x, i];
			bool flag = tile != null;
			if (flag)
			{
				ushort type = tile.type;
				bool flag2 = !tile.active() || !TileID.Sets.VineThreads[type];
				if (flag2)
				{
					break;
				}
				num++;
				bool flag3 = num2 >= 5;
				if (flag3)
				{
					num3 += 0.0075f * amount;
				}
				bool flag4 = num2 >= 2;
				if (flag4)
				{
					num3 += 0.0025f;
				}
				bool flag5 = WallID.Sets.AllowsWind[tile.wall] && i < Main.worldSurface;
				if (flag5)
				{
					num2++;
				}
				float windGridPush = tileDrawing.GetWindGridPush(x, i, 20, 0.01f);
				num4 = (windGridPush != 0f || num5 == 0f) ? (num4 - windGridPush) : (num4 * -0.78f);
				num5 = windGridPush;
				short tileFrameX = tile.frameX;
				short tileFrameY = tile.frameY;
				Color color = Lighting.GetColor(x, i);
				//color *= 2.4f;
				int tileWidth;
				int tileHeight;
				int tileTop;
				int halfBrickHeight;
				int addFrX;
				int addFrY;
				SpriteEffects tileSpriteEffect;
				Texture2D texture2D;
				Rectangle rectangle;
				Color color2;
				tileDrawing.GetTileDrawData(x, i, tile, type, ref tileFrameX, ref tileFrameY, out tileWidth, out tileHeight, out tileTop, out halfBrickHeight, out addFrX, out addFrY, out tileSpriteEffect, out texture2D, out rectangle, out color2);
				Vector2 position = new Vector2((float)(-(float)((int)screenPosition.X)), (float)(-(float)((int)screenPosition.Y))) + offSet + value;
				bool flag6 = tile.color() == 31;
				if (flag6)
				{
					color = Color.White;
				}
				float num6 = num2 * num3 * windCycle + num4;
				Texture2D tileDrawTexture = ModAsset.KelpMoss_II.Value;
				bool flag7 = tileDrawTexture == null;
				if (flag7)
				{
					break;
				}
				color.A = 0;
				Main.spriteBatch.Draw(tileDrawTexture, position, new Rectangle?(new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight)), color, num6, new Vector2(tileWidth / 2, halfBrickHeight - tileTop), 1f, tileSpriteEffect, 0f);
				value += (num6 + 1.5707964f).ToRotationVector2() * 16f;
			}
		}
	}

	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
	{
		offsetY = -2;
	}

	public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
	{
		if (i % 2 == 0)
		{
			spriteEffects = SpriteEffects.FlipHorizontally;
		}
	}
}