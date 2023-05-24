using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.DataStructures;
using Everglow.Yggdrasil.Common.Utils;
using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class ChinesePartitionLamp : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 4;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.CoordinateWidth = 48;
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
		TileObjectData.newTile.Origin = new Point16(0, 3);
		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = false;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1); // Facing right will use the second texture style
		TileObjectData.newTile.Origin = new Point16(0, 3);
		TileObjectData.addTile(Type);

		DustType = DustID.DynastyWood;
		AddMapEntry(new Color(135, 103, 90));
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 4, 48);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer)
		{
			var tile = Main.tile[i, j];
			if (tile.TileFrameY % 72 <= 36)
			{
				foreach (Player player in Main.player)
				{
					if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY % 72 / 18)))
							TileSpin.TileRotation.Add((i, j - tile.TileFrameY % 72 / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY % 72 / 18)].X;

							rot = TileSpin.TileRotation[(i, j - tile.TileFrameY % 72 / 18)].Y;
							float mass = 24f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i, j - tile.TileFrameY % 72 / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i, j - tile.TileFrameY % 72 / 18));
						}
					}
				}
				if (tile.WallType == 0)
				{
					if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY % 72 / 18)))
						TileSpin.TileRotation.Add((i, j - tile.TileFrameY % 72 / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
				}
			}
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D tPostTexture = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/ChinesePartitionLamp_Partition");
		var rt = new Rectangle(i * 16 - 16, j * 16, 48, 16);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		rt.X -= (int)(Main.screenPosition.X - zero.X);
		rt.Y -= (int)(Main.screenPosition.Y - zero.Y);
		Tile tile = Main.tile[i, j];

		if (tile.TileFrameY % 72 == 54)
			rt.Height = 18;
		spriteBatch.Draw(tPostTexture, rt, new Rectangle(tile.TileFrameX, tile.TileFrameY, 48, 16), Lighting.GetColor(i, j));

		if (tile.TileFrameY % 72 == 0 && (tile.TileFrameX == 0 || tile.TileFrameX == 48))
		{
			var tileSpin = new TileSpin();
			tileSpin.Update(i, j);
			Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/ChinesePartitionLamp_Lamp");
			float OffsetX = 1;
			if (tile.TileFrameY != 0)
				OffsetX = 15;
			tileSpin.DrawRotatedTile(i, j, tex, new Rectangle(tile.TileFrameX / 48 * 30, tile.TileFrameY / 72 * 32, 30, 32), new Vector2(15, 0), OffsetX, 2);
			if (tile.TileFrameX == 0)
			{
				Lighting.AddLight(i, j, 0.8f, 0.75f, 0.4f);
				Lighting.AddLight(i, j + 1, 0.8f, 0.75f, 0.4f);
			}
		}
		return false;
	}
}
