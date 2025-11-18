using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class IronStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			StabColor = new Color(50, 34, 32);
			base.SetDefaults();
			StabShade = 0.2f;
			StabDistance = 0.70f;
			StabEffectWidth = 0.4f;
		}
	}
}