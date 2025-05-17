using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class DevilHeartIronBar : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileShine[Type] = 800;
		Main.tileSolid[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.addTile(Type);

		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(124, 17, 30), modTranslation);
	}

	public override bool CreateDust(int i, int j, ref int type)
	{
		type = ModContent.DustType<Dusts.DevilHeartIronDust>();
		return true;
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (Main.tile[i, j + 1].Slope != SlopeType.Solid || !Main.tile[i, j + 1].HasTile || Main.tile[i, j + 1].IsHalfBlock)
		{
			WorldGen.KillTile(i, j);
		}
	}
}