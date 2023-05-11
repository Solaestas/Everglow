using Everglow.Minortopography.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class SungloChandelier : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		TileID.Sets.HasOutlines[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = DustID.BrownMoss;
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.addTile(Type);
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.SungloChandelier>());
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.4f;
		g = 0.9f;
		b = 0f;
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		var tile = Main.tile[i, j];
		if (closer)
		{
			foreach (Player player in Main.player)
			{
				if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
				{
					int frameY = tile.TileFrameY % 50;
					if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - frameY / 18)))
						TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - frameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
					else
					{
						float rot;
						float Omega;
						Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - frameY / 18)].X;
						rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - frameY / 18)].Y;
						if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
							TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - frameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j - frameY / 18));
					}
				}
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX % 54 == 18 && tile.TileFrameY % 50 == 0)
		{
			tile.TileFrameY = (short)((int)Main.timeForVisualEffects / 5 % 8 * 50);
			var tileSpin = new TileSpin();
			tileSpin.Update(i, j);
			Texture2D tex = ModAsset.TilesAndWalls_SungloChandelier.Value;
			tileSpin.DrawRotatedChandelier(i, j, tex, 8, -2);
		}
		return false;
	}
}