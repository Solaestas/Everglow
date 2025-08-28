using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.LightningMechanism;

public class UnderwaterLightningMechanism : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFlame[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		DustType = ModContent.DustType<WaterErodedGreenBrickDust>();

		// Placement - Standard Chandelier Setup Below
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
		TileObjectData.newTile.Origin = new Point16(1, 4);
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleHorizontal = false;
		TileObjectData.newTile.StyleLineSkip = 2;
		TileObjectData.newTile.DrawYOffset = 0;
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(101, 107, 121));
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 3, 3);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		r = 0f;
		g = 0f;
		b = 0f;
	}

	public override void AnimateTile(ref int frame, ref int frameCounter)
	{
		if (++frameCounter >= 4)
		{
			frameCounter = 0;
			frame = ++frame % 10;
		}
	}

	public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
	{
		frameYOffset = Main.tileFrame[type] * 72;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		//TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}
}