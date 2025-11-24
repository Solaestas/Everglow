using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class SilverStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(50, 54, 55);
			StabShade = 0.2f;
			StabDistance = 0.75f;
			StabEffectWidth = 0.4f;
		}
	}
}