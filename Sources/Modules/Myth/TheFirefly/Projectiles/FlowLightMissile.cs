using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Myth.TheFirefly.Projectiles;

internal class FlowLightMissile : ModProjectile
{
	public override string Texture => "Everglow/Myth/TheFirefly/Projectiles/FlowLightMissile";

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 350;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override Color? GetAlpha(Color lightColor)
	{
		return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}

	private bool release = true;
	private Vector2 oldPo = Vector2.Zero;
	private int energy = 0;

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		if (Projectile.timeLeft % 5 == 0)
			player.statMana--;
		energy += 2;
		Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
		MouseToPlayer = Vector2.Normalize(MouseToPlayer);
		if (player.controlUseItem && release)
		{
			Projectile.ai[0] *= 0.9f;
			Projectile.ai[1] -= 1f;
			Projectile.rotation = (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) + Math.PI * 0.25);
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer).RotatedBy(Projectile.ai[0] / 0.8d) * (8f - Projectile.ai[0] * 8) + new Vector2(0, 0);
			oldPo = Projectile.Center;
			Projectile.Center = oldPo;
			Projectile.velocity *= 0;
		}
		if (!player.controlUseItem && release)
		{
			if (Projectile.ai[1] > 0)
			{
				Projectile.ai[0] *= 0.9f;
				Projectile.ai[1] -= 1f;
				Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer).RotatedBy(Projectile.ai[0] / 4d) * (8f - Projectile.ai[0] * 4);
			}
			else
			{
				Shoot();
			}
		}
		if (energy >= 450 || player.statMana <= 0)
			Shoot();
		if (energy >= 180)
		{
			//player.velocity += MouseToPlayer;
			//if (player.velocity.Length() > 20f)
			//	player.velocity *= 20f / player.velocity.Length();
			Vector2 HitPoint = player.Center + MouseToPlayer * 105;
			if (Collision.SolidCollision(HitPoint, 0, 0))
				HitToAnything();
			foreach (var target in Main.npc)
			{
				if (target.active)
				{
					if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy())
					{
						if (Rectangle.Intersect(target.Hitbox, new Rectangle((int)HitPoint.X, (int)HitPoint.Y, 0, 0)) != new Rectangle(0, 0, 0, 0))
							HitToAnything();
					}
				}
			}
		}
		if (Projectile.Center.X < player.MountedCenter.X)
			player.direction = -1;
		else
		{
			player.direction = 1;
		}
	}
	private void HitToAnything()
	{
		Player player = Main.player[Projectile.owner];
		ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();

		Projectile.velocity = Projectile.oldVelocity;
		Projectile.friendly = false;
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<MissileExplosion>(), (int)(Projectile.damage * (energy + 150) / 600f), Projectile.knockBack * 0.4f, Projectile.owner, energy / 5f);
		Projectile.Kill();
	}
	private void Shoot()
	{
		SoundEngine.PlaySound(SoundID.Item72.WithVolumeScale(0.2f), Projectile.Center);
		Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
		v0 = Vector2.Normalize(v0);
		Player player = Main.player[Projectile.owner];

		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v0 * 62, v0 * (energy + 20) / 9f, ModContent.ProjectileType<MissileProj>(), (int)(Projectile.damage * (energy + 150) / 200f), Projectile.knockBack, player.whoAmI, energy, 0);

		Vector2 newVelocity = v0;
		newVelocity *= 1f - Main.rand.NextFloat(0.3f);
		newVelocity *= Math.Clamp(energy / 18f, 0.2f, 10f);
		Vector2 basePos = Projectile.Center + newVelocity * 3.7f + v0 * 62;

		for (int j = 0; j < energy * 2; j++)
		{
			Vector2 v = newVelocity / 27f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			int num20 = Dust.NewDust(basePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v1.X, v1.Y, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * 0.4f);
			Main.dust[num20].noGravity = true;
		}
		for (int j = 0; j < energy * 2; j++)
		{
			Vector2 v = newVelocity / 54f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			float Scale = Main.rand.NextFloat(3.7f, 5.1f);
			int num21 = Dust.NewDust(basePos + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v1.X, v1.Y, 100, default, Scale);
			Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
		}
		for (int j = 0; j < 16; j++)
		{
			Vector2 v = newVelocity / 54f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			v1 *= 0.2f;
			float Scale = Main.rand.NextFloat(3.7f, 5.1f);
			int num21 = Dust.NewDust(basePos + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<MothSmog>(), v1.X, v1.Y, 100, default, Scale);
			Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
		}
		//player.velocity -= newVelocity;

		Projectile.Kill();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		if (!release)
			return;
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		Vector2 v0 = Projectile.Center - player.MountedCenter;
		if (Main.mouseLeft)
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));

		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Texture2D texMainG = ModAsset.FlowLightMissileGlow.Value;

		Projectile.frame = (int)(energy % 45 / 5f);
		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == -1)
			se = SpriteEffects.FlipVertically;
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + MathF.PI * 0.36f * player.direction, texMain.Size() / 2f, 1f, se, 0);
		Main.spriteBatch.Draw(texMainG, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, new Color(energy, energy, energy, 0), Projectile.rotation - (float)(Math.PI * 0.25) + MathF.PI * 0.36f * player.direction, texMain.Size() / 2f, 1f, se, 0);
	}
}