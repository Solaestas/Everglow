using Everglow.Myth.MiscItems.Weapons.Slingshots.Dusts;

namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

public class RubyBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailRed";
		TrailColor = Color.Red;
		TrailColor.A = 0;
		dustType = ModContent.DustType<RubyDust>();
	}
}
