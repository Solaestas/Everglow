using Everglow.SpellAndSkull.Projectiles;

namespace Everglow.SpellAndSkull.Projectiles.CursedFlames;

internal class CursedFlamesBook : MagicBookProjectile
{
	public override void SetDef()
	{
		DustType = DustID.CursedTorch;
		ItemType = ItemID.CursedFlames;
		ProjType = ModContent.ProjectileType<CursedFlamesII>();
		MulDamage = 1f;
		MulVelocity = 0.3f;
		effectColor = new Color(105, 255, 0, 0);
	}
}