using Everglow.Commons.CustomTiles;
using Everglow.Yggdrasil.KelpCurtain.CustomTiles;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.IsleOfBloom;

public class BlackAwningBoatSign : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 3;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18,
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.addTile(Type);
		DustType = ModContent.DustType<AlgaeExtractor_Dust>();
		AddMapEntry(new Color(131, 158, 154));
	}

	public override bool RightClick(int i, int j)
	{
		ColliderManager.Instance.Add<BlackAwningBoat>(new Vector2(i - 3, j) * 16 - new Vector2(46, 20));
		return base.RightClick(i, j);
	}

	public override bool CanExplode(int i, int j) => false;

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		noBreak = true;
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}
}