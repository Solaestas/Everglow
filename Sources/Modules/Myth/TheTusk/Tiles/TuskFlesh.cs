namespace Everglow.Myth.TheTusk.Tiles
{
	public class TuskFlesh : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][TileID.BoneBlock] = true;
			Main.tileMerge[TileID.BoneBlock][Type] = true;
			Main.tileMerge[Type][ModContent.TileType<BloodMossStone>()] = true;
			Main.tileMerge[ModContent.TileType<BloodMossStone>()][Type] = true;
			Main.tileBlockLight[Type] = true;
			MinPick = 60;
			DustType = DustID.Blood;
			ItemDrop = ModContent.ItemType<Items.TuskFlesh>();
			AddMapEntry(new Color(219, 41, 47));
			HitSound = SoundID.Grass;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{

		}
	}
}
