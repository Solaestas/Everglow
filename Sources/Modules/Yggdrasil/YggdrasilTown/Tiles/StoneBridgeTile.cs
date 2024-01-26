using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.Common;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class StoneBridgeTile : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileSolid[Type] = true;
		Main.tileWaterDeath[Type] = false;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		DustType = DustID.Stone;
		MinPick = int.MaxValue;
		AddMapEntry(new Color(64, 64, 61));
	}
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

	}
	public void AddScene(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);
		if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
		{
			StoneBridge_fence sBF = new StoneBridge_fence { position = new Vector2(i, j - 3) * 16 - new Vector2(0, 14), Active = true, Visible = true, originTile = new Point(i, j), originType = ModContent.TileType<StoneBridgeTile>() };
			Ins.VFXManager.Add(sBF);
			StoneBridge_foreground sBF2 = new StoneBridge_foreground { position = new Vector2(i, j - 3) * 16 - new Vector2(0, 4), Active = true, Visible = true, originTile = new Point(i, j), originType = ModContent.TileType<StoneBridgeTile>() };
			Ins.VFXManager.Add(sBF2);
		}
	}
}