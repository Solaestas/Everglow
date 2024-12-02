using Everglow.Commons.Weapons;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Projectiles.DreamWeaver;

public class DreamWeaverII : TrailingProjectile
{
	public override void SetDefaults()
	{
		base.SetDefaults();
	}

	public override void SetDef()
	{
		TrailColor = new Color(0, 0.2f, 0.6f, 0f);
		TrailWidth = 8f;
		SelfLuminous = true;
		Projectile.extraUpdates = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
		TrailTexture = Commons.ModAsset.Trail_1.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
		ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10800;
	}

	private int breakTime = 200;

	public override void OnSpawn(IEntitySource source)
	{
		base.OnSpawn(source);
		if (Projectile.ai[0] == 3)
		{
			TrailWidth = 5f;
		}
	}

	public override void AI()
	{
		base.AI();
		Projectile.velocity.Y += 0.6f;
		if (Projectile.ai[0] != 3 && TimeTokill < 0)
		{
			for (int x = 0; x < 3; x++)
			{
				Vector2 basePos = Projectile.Center - new Vector2(4) - Projectile.velocity * x / 3f;
				var d0 = Dust.NewDustDirect(basePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0, 0, 0, default, Main.rand.NextFloat(0.4f, 1.5f));
				d0.noGravity = true;
				d0.velocity = Projectile.velocity * 0.2f;
			}
		}
		if (Projectile.ai[0] == 3 && TimeTokill < 0)
		{
			Vector2 basePos = Projectile.Center - new Vector2(4);
			var d0 = Dust.NewDustDirect(basePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0, 0, 0, default, Main.rand.NextFloat(0.3f, 0.6f));
			d0.noGravity = true;
			d0.velocity = Projectile.velocity * 0.2f;
		}
	}

	public override void KillMainStructure()
	{
		SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
		for (int x = 0; x < 5 * (4 - Projectile.ai[0]); x++)
		{
			Vector2 basePos = Projectile.Center - new Vector2(4);
			var d0 = Dust.NewDustDirect(basePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0, 0, 0, default, Main.rand.NextFloat(0.4f, 1.5f));
			d0.noGravity = true;
			d0.velocity = new Vector2(0, -Main.rand.NextFloat(0.5f, 2f)) * (4 - Projectile.ai[0]);
		}
		if (Projectile.ai[0] != 3)
		{
			for (int x = 0; x < 3; x++)
			{
				Vector2 velocity = new Vector2(0, -Main.rand.NextFloat(8f, 16f)).RotatedBy(Main.rand.NextFloat(-0.7f, 0.7f));
				var p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, Type, Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 3f);
				SoundEngine.PlaySound(SoundID.Item54);
			}
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -17), Type, Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 3f);
			foreach (var proj in Main.projectile)
			{
				if (proj != null && proj.active)
				{
					if (proj.owner == Projectile.owner && proj.type == ModContent.ProjectileType<DreamWeaverBall>())
					{
						DreamWeaverBall dWB = proj.ModProjectile as DreamWeaverBall;
						if (dWB != null)
						{
							dWB.HitPoint = Projectile.Center;
							dWB.PosLerpValue = 0;
						}
					}
				}
			}
		}

		Projectile.velocity = Projectile.oldVelocity;
		if (TimeTokill < 0)
		{
			Explosion();
		}
		TimeTokill = ProjectileID.Sets.TrailCacheLength[Projectile.type];
		float power = 3f;
		if (Projectile.ai[0] == 3)
		{
			power = 1f;
		}
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<DreamWeaver_hit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, power);
	}

	public override void DrawSelf()
	{
		var texMain = Commons.ModAsset.Drop.Value;
		float power = 0.6f;
		if (Projectile.ai[0] == 3)
		{
			power = 0.2f;
		}
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity * 0.1f, null, new Color(0, 0.6f, 1f, 0), Projectile.rotation, texMain.Size() / 2f, power, SpriteEffects.None, 0);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
	}
}