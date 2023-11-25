using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodMusicBox : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileObsidianKill[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.Origin = new Point16(0, 1);
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.DrawYOffset = 2;
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void MouseOver(int i, int j)
	{
		Player player = Main.LocalPlayer;
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<Items.Furnitures.GlowWoodMusicBox>();
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 2, 2);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if(closer)
		{
			Tile tile = Main.tile[i, j];
			if(tile.TileFrameX == 36 && tile.TileFrameY == 0)
			{
				if((int)(Main.timeForVisualEffects) % 25 == 0)
				{
					Gore.NewGore(new Vector2(i, j) * 16, new Vector2(Main.windSpeedCurrent, Main.rand.NextFloat(-0.5f, -0.4f)), Main.rand.Next(570, 573));
				}
			}
		}
		base.NearbyEffects(i, j, closer);
	}
}