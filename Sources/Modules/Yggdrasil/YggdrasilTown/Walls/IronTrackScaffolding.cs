namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class IronTrackScaffolding : ModWall
{
	public override void SetStaticDefaults()
	{
		DustType = DustID.Torch;
		AddMapEntry(new Color(3, 0, 0));
		HitSound = SoundID.NPCHit4;
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
