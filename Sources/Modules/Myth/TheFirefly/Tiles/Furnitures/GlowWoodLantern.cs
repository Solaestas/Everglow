using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodLantern : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		AdjTiles = new int[] { TileID.HangingLanterns };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newTile.AnchorBottom = default;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile & AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
		TileObjectData.newAlternate.DrawYOffset = -2;
		TileObjectData.addAlternate(1);

		TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
		TileObjectData.newAlternate.DrawYOffset = -8;
		TileObjectData.addAlternate(0);

		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(251, 235, 127), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 2);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 18)
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
					if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18)))
						TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
					else
					{
						float rot;
						float omega;
						omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)].X;
						rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)].Y;
						float mass = 15f;
						float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
						if (Math.Abs(omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
							TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)] = new Vector2(omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 18));
					}
				}
			}
			if (tile.WallType == 0)
			{
				if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18)))
					TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 0)
		{
			var tileSpin = new TileSpin();
			tileSpin.Update(i, j - tile.TileFrameY / 18);
			Texture2D tex = ModAsset.GlowWoodLanternDraw.Value;
			var tileUp = Main.tile[i, j - 1];
			tileSpin.DrawRotatedLamp(i, j - tile.TileFrameY / 18, tex, 8, TileObjectData.GetTileData(tile).DrawYOffset, 16);
		}
		return false;
	}
}