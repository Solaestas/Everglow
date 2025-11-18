using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class TungstenStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			StabColor = new Color(48, 74, 49);
			base.SetDefaults();
			StabShade = 0.2f;
			StabDistance = 0.75f;
			StabEffectWidth = 0.4f;
		}
	}
}