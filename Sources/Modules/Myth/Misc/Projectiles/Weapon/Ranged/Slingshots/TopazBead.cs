using Everglow.Myth.Misc.Dusts.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

public class TopazBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Textures/SlingshotTrailYellow";
		TrailColor = Color.Yellow;
		TrailColor.A = 0;
		dustType = ModContent.DustType<TopazDust>();
	}
}
