using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.TileEffect;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class VampireMatCave_HangingSign : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.Height = 14;
		TileObjectData.newTile.Width = 5;
		TileObjectData.newTile.CoordinateHeights = new int[14];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 2);
		TileObjectData.newTile.Origin = new Point16(2, 0);
		TileObjectData.addTile(Type);
		DustType = ModContent.DustType<VampireMatCave_HangingSign_Dust>();
		AddMapEntry(new Color(61, 34, 48));
	}
}