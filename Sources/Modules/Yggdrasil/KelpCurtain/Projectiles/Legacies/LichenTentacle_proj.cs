using Everglow.Commons.Weapons.Whips;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Legacies;

public class LichenTentacle_proj : WhipProjectile
{
	public override void SetDef()
	{
		DustType = ModContent.DustType<Dusts.LichenSlime>();
		WhipLength = 250;
		VerticalFrameCount = 5;
	}
}
