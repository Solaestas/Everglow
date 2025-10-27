using Everglow.SubSpace;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class ExitUnion : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileSolid[Type] = true;
		Main.tileWaterDeath[Type] = false;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		DustType = DustID.Stone;
		MinPick = int.MaxValue;
		AddMapEntry(new Color(0, 0, 0));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}

	public override bool RightClick(int i, int j)
	{
		RoomManager.ExitALevelOfRoom();
		return base.RightClick(i, j);
	}

	public override void MouseOver(int i, int j)
	{
		Main.instance.MouseText("Exit Union.");
	}
}