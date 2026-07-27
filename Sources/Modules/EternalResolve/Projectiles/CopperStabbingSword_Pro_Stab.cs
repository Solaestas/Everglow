using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class CopperStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(80, 34, 5);
			StabShade = 2f;
			StabDistance = 0.70f;
			StabEffectWidth = 0.4f;
		}
	}
}