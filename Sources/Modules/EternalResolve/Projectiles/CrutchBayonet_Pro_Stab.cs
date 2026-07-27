using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class CrutchBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(155, 162, 164);
			StabShade = 0.2f;
			StabDistance = 1.05f;
			StabEffectWidth = 0.4f;
		}

		internal bool FirstHit = false;

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (!FirstHit)
			{
				modifiers.CritDamage.Multiplicative *= 2.7f;
				FirstHit = true;
			}
			base.ModifyHitNPC(target, ref modifiers);
		}

		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}

		public override void AI()
		{
			base.AI();
		}
	}
}