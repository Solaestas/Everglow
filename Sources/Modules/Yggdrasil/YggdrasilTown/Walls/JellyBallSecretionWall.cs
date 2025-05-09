using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class JellyBallSecretionWall : ModWall
{
	public override void SetStaticDefaults()
	{
		DustType = ModContent.DustType<JellyBallGel>();
		HitSound = SoundID.NPCHit1;
		AddMapEntry(new Color(12, 19, 80));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}