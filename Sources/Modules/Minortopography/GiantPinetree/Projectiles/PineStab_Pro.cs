using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Terraria.Audio;

namespace Everglow.Minortopography.GiantPinetree.Projectiles
{
	public class PineStab_Pro : StabbingProjectile
	{
		public override void SetCustomDefaults()
		{
			AttackColor = new Color(11, 84, 46);
			MaxDarkAttackUnitCount = 4;
			OldColorFactor = 0.5f;
			CurrentColorFactor = 0.4f;
			ShadeMultiplicative_Modifier = 0.52f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 0.63f;
			AttackEffectWidth = 0.25f;
			HitTileSparkColor = new Color(11, 84, 46, 30);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.Defense *= 3f;
			base.ModifyHitNPC(target, ref modifiers);
		}

		public override void HitTileSound(float scale)
		{
			SoundEngine.PlaySound(SoundID.Grass.WithVolume(1 - scale / 2.42f).WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), Projectile.Center);
			Projectile.soundDelay = SoundTimer;
		}
	}
}