using Terraria.Localization;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles
{
	public class OldMoss : ModTile
	{
		public override void PostSetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<DragonScaleWood>()] = true;
			Main.tileMerge[Type][ModContent.TileType<YggdrasilDirt>()] = true;
			Main.tileMerge[Type][TileID.Stone] = true;
			Main.tileMerge[TileID.Stone][Type] = true;
			Main.ugBackTransition = 1000;
			DustType = DustID.BrownMoss;
			MinPick = 50;
			HitSound = SoundID.Dig;
			ItemDrop = ModContent.ItemType<Items.YggdrasilDirt>();
			AddMapEntry(new Color(81, 107, 18));
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
		}
		public override void RandomUpdate(int i, int j)
		{
		}
		public override void PlaceInWorld(int i, int j, Item item)
		{
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}
