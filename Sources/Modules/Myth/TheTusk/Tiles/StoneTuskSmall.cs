using Everglow.Myth.TheTusk.Items;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheTusk.Tiles;

public class StoneTuskSmall : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			18
		};
		TileObjectData.newTile.CoordinateWidth = 24;
		TileObjectData.addTile(Type);
		DustType = 191;
		ModTranslation modTranslation = base.CreateMapEntryName(null);
		AddMapEntry(new Color(112, 83, 67), modTranslation);
		modTranslation.SetDefault("Tusk Fossil");
		modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "化石牙");
		HitSound = SoundID.DD2_SkeletonHurt;
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
	}
	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		for (int x = 0; x < 2; x++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 60f)).RotatedByRandom(3.14159);
			Item.NewItem(null, i * 16 + (int)v.X, j * 16 + (int)v.Y, 16, 32, ModContent.ItemType<StoneTusk>());
		}
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		short num = (short)Main.rand.Next(0, 12);
		Main.tile[i, j].TileFrameX = (short)(num * 24);
		Main.tile[i, j + 1].TileFrameX = (short)(num * 24);
	}
}
