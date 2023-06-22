namespace Everglow.Myth.TheFirefly.Tiles;

public class BlackVine : ModTile//TODO:Need to copy some code from vanilla.This is a kind of vine,it will swaying in the wind or dragged by moving-players.
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		DustType = 191;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(11, 11, 11), modTranslation);
		HitSound = SoundID.Grass;
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
	}

	public override void RandomUpdate(int i, int j)
	{
		base.RandomUpdate(i, j);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return true;
	}
}