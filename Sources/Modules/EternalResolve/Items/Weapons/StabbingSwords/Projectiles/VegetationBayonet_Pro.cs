namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class VegetationBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(66, 137, 58);
			TradeLength = 4;
			TradeShade = 0.7f;
			Shade = 0.3f;
			FadeTradeShade = 0.3f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.buffImmune[BuffID.OnFire] = false;
			target.AddBuff(BuffID.Poisoned, 240);
		}
	}
}