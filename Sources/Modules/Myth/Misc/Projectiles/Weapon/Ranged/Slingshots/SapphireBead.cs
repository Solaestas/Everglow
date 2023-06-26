using Everglow.Myth.Misc.Dusts.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

public class SapphireBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Textures/SlingshotTrailBlue";
		TrailColor = Color.Blue;
		TrailColor.A = 0;
		dustType = ModContent.DustType<SapphireDust>();
	}
}
