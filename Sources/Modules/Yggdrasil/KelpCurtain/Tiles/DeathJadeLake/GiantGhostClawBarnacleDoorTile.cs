using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class GiantGhostClawBarnacleDoorTile : ModTile
{
	public override void SetStaticDefaults()
	{
		DustType = ModContent.DustType<SharpBarnacle_Dust>();
		MinPick = int.MaxValue;

		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileSolid[Type] = true;
		Main.tileWaterDeath[Type] = false;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = false;

		TileObjectData.newTile.WaterPlacement = Terraria.Enums.LiquidPlacement.Allowed;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Width = 19;
		TileObjectData.newTile.Height = 23;
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinateHeights = new int[23];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new(0, 12);
		TileObjectData.newTile.AnchorTop = new AnchorData(0, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(0, 0, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(101, 99, 107));
	}

	public override string Texture => ModAsset.GiantGhostClawBarnacleCollideTile_Mod;

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return false;
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		base.PostDraw(i, j, spriteBatch);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		tile.LiquidType = LiquidID.Water;
		tile.LiquidAmount = 255;
		base.NearbyEffects(i, j, closer);
	}
}