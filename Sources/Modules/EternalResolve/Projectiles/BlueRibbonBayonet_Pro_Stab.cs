using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class BlueRibbonBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(220, 220, 220);
			StabShade = 0.2f;
			StabDistance = 1.64f;
			StabEffectWidth = 0.4f;
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