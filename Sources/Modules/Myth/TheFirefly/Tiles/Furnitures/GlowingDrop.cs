using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowingDrop : ModTile
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
		TileID.Sets.CanBeSleptIn[Type] = true;
		TileID.Sets.InteractibleByNPCs[Type] = true;
		TileID.Sets.IsValidSpawnPoint[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<BlueGlow>();
		AdjTiles = new int[] { TileID.Chandeliers };

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
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
			g = 0.5f;
			b = 1.2f;
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
			bool windSway = tile.wall <= 0;
			if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
			{
				if(windSway && !Main.gamePaused)
				{
					if (tile.WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
					}
				}
				foreach (Player player in Main.player)
				{
					if (player.Hitbox.Intersects(new Rectangle(i * 16 + 11, j * 16 + 32, 10, 12)))
					{
						TileSpin.ActivateTileSpin((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18), player.velocity.X * 0.1f);
					}

					if (player.Hitbox.Intersects(new Rectangle(i * 16 + 21, j * 16 + 12, 10, 12)))
					{
						TileSpin.ActivateTileSpin((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18), player.velocity.X * 0.1f);
					}

					if (player.Hitbox.Intersects(new Rectangle(i * 16 - 1, j * 16 + 18, 10, 12)))
					{
						TileSpin.ActivateTileSpin((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1), player.velocity.X * 0.1f);
					}

					if (player.Hitbox.Intersects(new Rectangle(i * 16 - 13, j * 16 + 26, 10, 12)))
					{
						TileSpin.ActivateTileSpin((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18), player.velocity.X * 0.1f);
					}
				}
			}
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		int Adx = 0;
		if (tile.TileFrameX > 54)
			Adx = 54;
		if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
		{
			var tileSpin = new TileSpin();
			tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18);
			Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowingDrop");
			tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18, tex, new Rectangle(30 + Adx, (int)((Math.Sin(i + j) * 100 + 100) % 22), 10, 48), new Vector2(5, 0), 16, -2, 1);

			tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18);
			tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18, tex, new Rectangle(40 + Adx, (int)((Math.Sin(i + j) * 100 + 100) % 14), 10, 48), new Vector2(5, 0), 10, -2, 1);

			tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1);
			tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1, tex, new Rectangle(18 + Adx, (int)((Math.Sin(i + j) * 100 + 100) % 14), 10, 48), new Vector2(5, 0), 4, -18, 1);

			tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18);
			tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18, tex, new Rectangle(6 + Adx, (int)((Math.Sin(i + j) * 100 + 100) % 26), 10, 48), new Vector2(5, 0), 8, -2, 1);
		}

		return false;
	}
}