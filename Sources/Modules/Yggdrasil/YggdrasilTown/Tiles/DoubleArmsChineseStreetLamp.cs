using Terraria.ObjectData;
using Everglow.Yggdrasil.Common.Utils;
using Everglow.Yggdrasil.Common;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class DoubleArmsChineseStreetLamp : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			18
		};
		TileObjectData.newTile.CoordinateWidth = 54;
		TileObjectData.addTile(Type);
		DustType = DustID.DynastyWood;
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer)
		{
			var tile = Main.tile[i, j];
			if (tile.TileFrameY <= 36 && tile.TileFrameY >= 18)
			{
				foreach (Player player in Main.player)
				{
					if (player.Hitbox.Intersects(new Rectangle(i * 16 - 16, j * 16, 16, 16)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18)))
							TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)].X;
							rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)].Y;
							float mass = 8f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 18));
						}
					}
					if (Main.tile[i - 1, j].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18)))
							TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.6f, 0));
					}
					if (player.Hitbox.Intersects(new Rectangle(i * 16 + 16, j * 16, 16, 16)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18 + 1)))
							TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18 + 1), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18 + 1)].X;
							rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18 + 1)].Y;
							float mass = 8f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i, j - tile.TileFrameY / 18 + 1)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 18 + 1));
						}
					}
					if (Main.tile[i + 1, j].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18 + 1)))
							TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18 + 1), new Vector2(Main.windSpeedCurrent * 0.6f, 0));
					}
				}
			}
		}
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 5, 54);
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D tPostTexture = ModAsset.DoubleArmsChineseStreetLamp_Post.Value;
		var rt = new Rectangle(i * 16 - 19, j * 16, 54, 16);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		rt.X -= (int)(Main.screenPosition.X - zero.X);
		rt.Y -= (int)(Main.screenPosition.Y - zero.Y);
		if (Main.tile[i, j].TileFrameY == 72)
			rt.Height = 19;
		Tile tile = Main.tile[i, j];

		spriteBatch.Draw(tPostTexture, rt, new Rectangle(tile.TileFrameX, tile.TileFrameY, 54, 16), Lighting.GetColor(i, j));

		if (tile.TileFrameY == 0 && (tile.TileFrameX == 0 || tile.TileFrameX == 54))
		{
			var tileSpin = new TileSpin();

			Texture2D tex = ModAsset.LargeLantern_Lantern.Value;
			float OffsetX = -8;
			tileSpin.Update(i, j);
			tileSpin.DrawThreeLanternsString(i, j, tex, new Rectangle(tile.TileFrameX / 54 * 18 + 2, 2, 14, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 4, 12, 10, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 6, 22, 6, 8), new Vector2(7, 5), new Vector2(5, 5), new Vector2(3, 4), OffsetX, 23);
			tileSpin.Update(i, j + 1);
			tileSpin.DrawThreeLanternsString(i, j + 1, tex, new Rectangle(tile.TileFrameX / 54 * 18 + 2, 2, 14, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 4, 12, 10, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 6, 22, 6, 8), new Vector2(7, 5), new Vector2(5, 5), new Vector2(3, 4), OffsetX + 32, 7);

			if (tile.TileFrameX == 0)
			{
				Texture2D texGlow = ModAsset.LargeLantern_Lantern_glow.Value;
				tileSpin.DrawThreeLanternsString(i, j, texGlow, new Rectangle(tile.TileFrameX / 54 * 18 + 2, 2, 14, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 4, 12, 10, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 6, 22, 6, 8), new Vector2(7, 5), new Vector2(5, 5), new Vector2(3, 4), OffsetX, 23,1, true, new Color(255, 255, 255, 0));
				tileSpin.DrawThreeLanternsString(i, j + 1, texGlow, new Rectangle(tile.TileFrameX / 54 * 18 + 2, 2, 14, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 4, 12, 10, 10), new Rectangle(tile.TileFrameX / 54 * 18 + 6, 22, 6, 8), new Vector2(7, 5), new Vector2(5, 5), new Vector2(3, 4), OffsetX + 32, 7,1, true, new Color(255, 255, 255, 0));
				Lighting.AddLight(i, j, 1f, 0.45f, 0.15f);
				Lighting.AddLight(i, j + 1, 1f, 0.45f, 0.15f);
			}
		}
		return false;
	}
}
