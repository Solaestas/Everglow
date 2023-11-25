using Everglow.Myth.Misc.Dusts.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

public class DiamondBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Textures/SlingshotTrailWhite";
		TrailColor = Color.White;
		TrailColor.A = 0;
		dustType = ModContent.DustType<DiamondDust>();
	}
}
