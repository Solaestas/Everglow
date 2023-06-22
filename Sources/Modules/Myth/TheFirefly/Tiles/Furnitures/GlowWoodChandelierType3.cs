using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodChandelierType3 : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
		TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
		TileID.Sets.IsValidSpawnPoint[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<BlueGlow>();
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.addTile(Type);
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 3, 3);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
		{
			r = 0.1f;
			g = 0.9f;
			b = 1f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
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
					if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)))
						TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
					else
					{
						float rot;
						float Omega;
						Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)].X;
						rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)].Y;
						if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
							TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18));
					}
				}
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
		{
			var tileSpin = new TileSpin();
			tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18);
			Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodChandelierType3");
			tileSpin.DrawRotatedChandelier(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18, tex, 8, -2);
		}
		//Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		//if (Main.drawToScreen)
		//{
		//    zero = Vector2.Zero;
		//}

		//Rectangle rc = Main.LocalPlayer.Hitbox;
		//rc.X -= (int)(Main.screenPosition.X - zero.X);
		//rc.Y -= (int)(Main.screenPosition.Y - zero.Y);

		//Rectangle rc2 = new Rectangle(i * 16, j * 16, 16, 16);
		//rc2.X -= (int)(Main.screenPosition.X - zero.X);
		//rc2.Y -= (int)(Main.screenPosition.Y - zero.Y);
		//spriteBatch.Draw(TextureAssets.MagicPixel.Value, rc2, new Color(0.5f, 0, 0, 0));

		//spriteBatch.Draw(TextureAssets.MagicPixel.Value,rc,new Color(0.5f,0,0,0));
		return false;
	}
}