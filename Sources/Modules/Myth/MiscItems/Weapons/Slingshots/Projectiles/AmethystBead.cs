using Everglow.Myth.MiscItems.Weapons.Slingshots.Dusts;

namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

public class AmethystBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailPurple";
		TrailColor = Color.Purple;
		TrailColor.A = 0;
		dustType = ModContent.DustType<AmethystDust>();
	}
}