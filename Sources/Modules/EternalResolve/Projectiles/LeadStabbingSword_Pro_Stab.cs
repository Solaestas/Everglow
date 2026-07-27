using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class LeadStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(28, 37, 63);
			StabShade = 0.2f;
			StabDistance = 0.72f;
			StabEffectWidth = 0.4f;
		}
	}
}