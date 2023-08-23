using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.Audio;

namespace Everglow.Minortopography.GiantPinetree.Projectiles
{
    public class PineStab_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(11, 84, 46);
			TradeLength = 4;
			TradeShade = 0.5f;
			Shade = 0.4f;
			FadeTradeShade = 0.52f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.63f;
			DrawWidth = 0.25f;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.Defense *= 3f;
			base.ModifyHitNPC(target, ref modifiers);
		}
		public override void HitTileSound(float scale)
		{
			SoundStyle ss = SoundID.Grass;
			SoundEngine.PlaySound(ss.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)).WithVolume(0.3f), Projectile.Center);
			Projectile.soundDelay = SoundTimer;
		}
	}
}