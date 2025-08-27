using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class CrutchBayonet_Pro_Stab : StabbingProjectile_Stab
	{
        public override void SetDefaults()
        {
            Color = new Color(155, 162, 164);
            base.SetDefaults();
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.05f;
			DrawWidth = 0.4f;
		}
		internal bool FirstHit = false;
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if(!FirstHit)
			{
				modifiers.CritDamage.Multiplicative *= 2.7f;
				FirstHit = true;
			}
			base.ModifyHitNPC(target, ref modifiers);
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