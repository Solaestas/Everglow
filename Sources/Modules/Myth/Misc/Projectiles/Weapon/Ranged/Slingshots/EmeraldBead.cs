using Everglow.Myth.Misc.Dusts.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

public class EmeraldBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Textures/SlingshotTrailGreen";
		TrailColor = Color.Green;
		TrailColor.A = 0;
		dustType = ModContent.DustType<EmeraldDust>();
	}
}
