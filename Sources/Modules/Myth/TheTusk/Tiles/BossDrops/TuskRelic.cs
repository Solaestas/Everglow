using Terraria.ObjectData;

namespace Everglow.Myth.TheTusk.Tiles.BossDrops;

public class TuskRelic : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileShine2[Type] = true; // Modifies the draw color slightly.
		Main.tileShine[Type] = 300; // How often tiny dust appear off this tile. Larger is less frequently
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = false;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(100, 100, 100));

		DustType = DustID.Gold;
		AdjTiles = new int[] { Type };
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(233, 207, 94), modTranslation);
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}

		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX != 18 || !(tile.TileFrameY == 54 || tile.TileFrameX == 126))
		{
			return;
		}

		for (int x = 0; x < 2; x++)
		{
			var point = new Point(i, j);

			if (tile != null && tile.HasTile)
			{
				Texture2D value = ModAsset.Tiles_TuskRelic.Value;
				int frameY = tile.TileFrameX / 54;
				bool flag = tile.TileFrameY / 72 != 0;
				int horizontalFrames = 1;
				int verticalFrames = 27;
				Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, 0, frameY, 0, 0);
				Vector2 origin = rectangle.Size() / 2f;
				Vector2 value2 = point.ToWorldCoordinates(0f, 0f);
				float num3 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 5f));
				Vector2 value3 = value2 + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
				Color color = Lighting.GetColor(point.X, point.Y);
				SpriteEffects effects = flag ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Main.spriteBatch.Draw(value, value3 - Main.screenPosition + zero + new Vector2(8, 16), null, color, 0f, value.Size() / 2f, 1f, effects, 0f);
				float scale = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 2f)) * 0.3f + 0.7f;
				Color color2 = color;
				color2.A = 0;
				color2 = color2 * 0.1f * scale;
				for (float num4 = 0f; num4 < 1f; num4 += 0.166666672f)
				{
					Main.spriteBatch.Draw(value, value3 - Main.screenPosition + (6.28318548f * num4).ToRotationVector2() * (6f + num3 * 2f) + zero + new Vector2(8, 16), null, color2, 0f, value.Size() / 2f, 1f, effects, 0f);
				}
			}
		}

		// base.PostDraw(i, j, spriteBatch);
	}
}