using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BlackFren : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			18
		};
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.addTile(Type);
		DustType = 191;
		AddMapEntry(new Color(11, 11, 11));
		HitSound = SoundID.Grass;
	}
}