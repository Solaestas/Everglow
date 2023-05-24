using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.DataStructures;
using Everglow.Yggdrasil.Common.Utils;
using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class SideHangingLantern_White : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileSolidTop[Type] = true;
		DustType = DustID.DynastyWood;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 2;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16
		};

		TileObjectData.newAlternate.Alternates = new List<TileObjectData>();
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinateWidth = 16;

		TileObjectData.newTile.AnchorBottom = new AnchorData(0, 0, 0);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
		TileObjectData.addAlternate(1);
		TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(135, 103, 90));
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
					if (!TileSpin.TileRotation.ContainsKey((i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18)))
						TileSpin.TileRotation.Add((i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
					else
					{
						float rot;
						float Omega;
						Omega = TileSpin.TileRotation[(i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18)].X;
						rot = TileSpin.TileRotation[(i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18)].Y;
						float mass = 24f;
						float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
						if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
							TileSpin.TileRotation[(i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18));
					}
				}
				if (Main.tile[i, j].WallType == 0)
				{
					if (!TileSpin.TileRotation.ContainsKey((i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18)))
						TileSpin.TileRotation.Add((i - tile.TileFrameY % 36 / 18, j - tile.TileFrameY % 54 / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
				}
			}
		}
	}
	public override void HitWire(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		int x = i - tile.TileFrameX / 18 % 2;
		int y = j - tile.TileFrameY / 18 % 3;
		for (int m = x; m < x + 2; m++)
		{
			for (int n = y; n < y + 3; n++)
			{
				if (!tile.HasTile)
					continue;
				if (tile.TileType == Type)
				{
					tile = Main.tile[m, n];
					if (tile.TileFrameY < 18 * 3)
					{
						tile = Main.tile[m, n];
						tile.TileFrameY += 54;
					}
					else
					{
						tile = Main.tile[m, n];
						tile.TileFrameY -= 54;
					}
				}
			}
		}
		if (!Wiring.running)
			return;
		for (int k = 0; k < 2; k++)
		{
			for (int l = 0; l < 3; l++)
			{
				Wiring.SkipWire(x + k, y + l);
			}
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D tPostTexture = ModAsset.SideHangingLantern_White_Post.Value;
		var rt = new Rectangle(i * 16, j * 16, 16, 16);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		rt.X -= (int)(Main.screenPosition.X - zero.X);
		rt.Y -= (int)(Main.screenPosition.Y - zero.Y);
		Tile tile = Main.tile[i, j];
		spriteBatch.samplerState = SamplerState.AnisotropicWrap;
		spriteBatch.Draw(tPostTexture, rt, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Lighting.GetColor(i, j));

		if (tile.TileFrameY % 54 == 0 && (tile.TileFrameX == 0 || tile.TileFrameX == 36))
		{
			var tileSpin = new TileSpin();
			tileSpin.Update(i, j);
			Texture2D tex = ModAsset.SideHangingLantern_White_Lantern.Value;
			int FrameX = 0;
			if (tile.TileFrameY == 54)
				FrameX = 26;
			tileSpin.DrawRotatedTile(i, j, tex, new Rectangle(FrameX, 0, 26, 36), new Vector2(13, 0), 16, 6);
			if (tile.TileFrameY == 0)
			{
				Lighting.AddLight(i, j, 0.8f, 0.6f, 0.4f);
				Lighting.AddLight(i, j + 1, 0.8f, 0.6f, 0.4f);
			}
		}
		return false;
	}
}
