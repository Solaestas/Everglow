using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class TungstenStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(48, 74, 49);
			StabShade = 0.2f;
			StabDistance = 0.75f;
			StabEffectWidth = 0.4f;
		}
	}
}