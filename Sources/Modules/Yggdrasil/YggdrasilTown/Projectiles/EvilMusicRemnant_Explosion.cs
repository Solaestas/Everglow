using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Explosion : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = Projectile.height = 96;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 3;
		Projectile.tileCollide = false;
		Projectile.alpha = 255;
		Projectile.knockBack = 10f;
	}

	public override string Texture => ModAsset.EvilMusicRemnant_Minion_Mod;

	public override void AI()
	{
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		if (Main.expertMode)
		{
			if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
			{
				modifiers.FinalDamage /= 5;
			}
		}
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
		for (int i = 0; i < 40; i++)
		{
			float size = Main.rand.NextFloat(0.1f, 0.96f);
			var noteFlame = new EvilMusicRemnant_FlameDust
			{
				Velocity = new Vector2(0, Main.rand.NextFloat(0.3f, 1f)).RotatedByRandom(MathHelper.TwoPi) * 1.2f,
				Active = true,
				Visible = true,
				Position = Projectile.Center,
				MaxTime = Main.rand.Next(24, 36) * 6,
				Scale = 14f * size,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Frame = Main.rand.Next(3),
				ai = [Main.rand.NextFloat(-0.8f, 0.8f)],
			};
			Ins.VFXManager.Add(noteFlame);
		}
	}
}