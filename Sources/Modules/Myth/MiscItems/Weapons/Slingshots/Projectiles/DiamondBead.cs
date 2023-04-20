using Everglow.Myth.MiscItems.Weapons.Slingshots.Dusts;

namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

public class DiamondBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailWhite";
		TrailColor = Color.White;
		TrailColor.A = 0;
		dustType = ModContent.DustType<DiamondDust>();
	}
}
