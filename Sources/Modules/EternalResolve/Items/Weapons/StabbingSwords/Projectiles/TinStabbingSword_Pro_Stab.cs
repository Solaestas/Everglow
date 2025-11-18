using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class TinStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			StabColor = new Color(65, 44, 30);
			StabShade = 0.2f;
			StabDistance = 0.70f;
			StabEffectWidth = 0.4f;
		}
	}
}