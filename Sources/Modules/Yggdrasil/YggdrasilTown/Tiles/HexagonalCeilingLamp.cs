using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;
using Everglow.Yggdrasil.Common.Utils;
using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class HexagonalCeilingLamp : ModTile
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

		DustType = DustID.Pearlwood;
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.addTile(Type);

		// Etc
		var name = CreateMapEntryName();
		AddMapEntry(new Color(216, 172, 125), name);
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
			r = 0.8f;
			g = 0.75f;
			b = 0.4f;
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
						float mass = 26f;
						float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
						if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
							TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18));
					}
				}
				if (Main.tile[i, j].WallType == 0)
				{
					if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)))
						TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
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
			Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/HexagonalCeilingLamp");
			tileSpin.DrawRotatedChandelier(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18, tex, 8, -2);
		}
		return false;
	}
}
