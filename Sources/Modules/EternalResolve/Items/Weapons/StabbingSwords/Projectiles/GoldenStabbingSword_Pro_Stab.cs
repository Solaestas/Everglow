using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class GoldenStabbingSword_Pro_Stab : StabbingProjectile_Stab
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			StabColor = new Color(135, 76, 2);
			StabShade = 0.2f;
			StabDistance = 0.88f;
			StabEffectWidth = 0.4f;
		}
	}
}