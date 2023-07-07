using Everglow.Myth.Misc.Dusts.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

public class AmethystBead : GemAmmo
{
	public override void SetDef()
	{
		TrailTexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Textures/SlingshotTrailPurple";
		TrailColor = Color.Purple;
		TrailColor.A = 0;
		dustType = ModContent.DustType<AmethystDust>();
	}
}