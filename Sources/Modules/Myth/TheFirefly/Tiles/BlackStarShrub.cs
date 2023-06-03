using Everglow.Myth.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BlackStarShrub : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			20
		};
		TileObjectData.newTile.CoordinateWidth = 72;
		TileObjectData.addTile(Type);
		DustType = 191;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(84, 172, 255), modTranslation);
		HitSound = SoundID.Grass;
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer)
		{
			var tile = Main.tile[i, j];
			foreach (Player player in Main.player)
			{
				if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
				{
					if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16 + 2)))
						TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16 + 2), new Vector2(+Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
					else
					{
						float rot;
						float omega;
						omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 2)].X;
						rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 2)].Y;
						if (Math.Abs(omega) < 0.04f && Math.Abs(rot) < 0.04f)
							TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 2)] = new Vector2(omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16 + 2));
					}

					if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16 + 1)))
						TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16 + 1), new Vector2(+Math.Clamp(player.velocity.X, -0.3f, 0.3f) * 0.2f));
					else
					{
						float rot;
						float omega;
						omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)].X;
						rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)].Y;
						if (Math.Abs(omega) < 0.04f && Math.Abs(rot) < 0.04f)
							TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)] = new Vector2(omega + Math.Clamp(player.velocity.X, -0.3f, 0.3f) * 0.2f, rot + omega + Math.Clamp(player.velocity.X, -0.3f, 0.3f) * 0.2f);
						if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16 + 1));
					}

					if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16)))
						TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16), new Vector2(+Math.Clamp(player.velocity.X, -0.15f, 0.15f) * 0.2f));
					else
					{
						float rot;
						float omega;
						omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)].X;
						rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)].Y;
						if (Math.Abs(omega) < 0.04f && Math.Abs(rot) < 0.04f)
							TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)] = new Vector2(omega + Math.Clamp(player.velocity.X, -0.15f, 0.15f) * 0.2f, rot + omega + Math.Clamp(player.velocity.X, -0.15f, 0.15f) * 0.2f);
						if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16));
					}
				}
			}
		}
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		short num = (short)Main.rand.Next(0, 6);
		Main.tile[i, j].TileFrameX = (short)(num * 72);
		Main.tile[i, j + 1].TileFrameX = (short)(num * 72);
		Main.tile[i, j + 2].TileFrameX = (short)(num * 72);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 32)
		{
			var tileSpin = new TileSpin();
			Texture2D tex = ModAsset.BlackStarShrubDraw.Value;
			tileSpin.Update(i, j - tile.TileFrameY / 16 + 2);
			tileSpin.Update(i, j - tile.TileFrameY / 16 + 1);
			tileSpin.UpdateBlackShrub(i, j - tile.TileFrameY / 16, 0.85f, 0.13f, new Vector2(0, -60), 0, 54, 16, 32);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 0, 72, 56), new Vector2(36, 56), 8, 54, 1);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 58, 72, 56), new Vector2(36, 56), 8, 54, 1.2f);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 2, tex, new Rectangle(tile.TileFrameX, 114, 72, 56), new Vector2(36, 56), 8, 22, 1.04f);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 2, tex, new Rectangle(tile.TileFrameX, 170, 72, 56), new Vector2(36, 56), 8, 22, 0.75f);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 2, tex, new Rectangle(tile.TileFrameX, 226, 72, 56), new Vector2(36, 56), 8, 22, 0.49f);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 2, tex, new Rectangle(tile.TileFrameX, 450, 72, 56), new Vector2(36, 56), 8, 22, 1.04f, true, new Color(0.27f, 0.27f, 0.27f, 0));
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 2, tex, new Rectangle(tile.TileFrameX, 506, 72, 56), new Vector2(36, 56), 8, 22, 0.75f, true, new Color(0.27f, 0.27f, 0.27f, 0));
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 2, tex, new Rectangle(tile.TileFrameX, 562, 72, 56), new Vector2(36, 56), 8, 22, 0.49f, true, new Color(0.27f, 0.27f, 0.27f, 0));
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 1, tex, new Rectangle(tile.TileFrameX, 282, 72, 56), new Vector2(36, 56), 8, 38, 0.94f);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 1, tex, new Rectangle(tile.TileFrameX, 338, 72, 56), new Vector2(36, 56), 8, 38, 0.24f);
			tileSpin.DrawRotatedTile(spriteBatch, i, j - tile.TileFrameY / 16 + 1, tex, new Rectangle(tile.TileFrameX, 394, 72, 56), new Vector2(36, 56), 8, 38, 0.13f);
		}
		return false;
	}
}