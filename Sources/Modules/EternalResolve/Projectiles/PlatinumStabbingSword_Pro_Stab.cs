using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class PlatinumStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(42, 42, 55);
			StabShade = 0.2f;
			StabDistance = 0.90f;
			StabEffectWidth = 0.4f;
		}
	}
}