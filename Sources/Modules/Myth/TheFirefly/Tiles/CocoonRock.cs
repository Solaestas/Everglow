using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class CocoonRock : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileMerge[ModContent.TileType<DarkCocoon>()][Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkCocoon>()] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
		TileObjectData.newTile.RandomStyleRange = 3;
		TileObjectData.addTile(Type);
		DustType = 191;
		ModTranslation modTranslation = base.CreateMapEntryName(null);
		AddMapEntry(new Color(25, 24, 25), modTranslation);
		HitSound = SoundID.Dig;
	}
}