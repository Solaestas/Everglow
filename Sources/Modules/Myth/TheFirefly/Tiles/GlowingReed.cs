using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class GlowingReed : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.SettingsEnabled_TilesSwayInWind = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			32
		};
		TileObjectData.addTile(Type);
		TileID.Sets.SwaysInWindBasic[Type] = true;
		DustType = 191;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(11, 11, 160), modTranslation);
		HitSound = SoundID.Grass;
	}
	public override void RandomUpdate(int i, int j)
	{
		base.RandomUpdate(i, j);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return true;
	}
}