using Everglow.Myth;
using Everglow.Myth.Common;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.MagnetSphere;

public class MagnetSphereII : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 3000;
		Projectile.alpha = 0;
		Projectile.penetrate = 18;
		Projectile.scale = 1f;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 24;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0, 0.46f * Projectile.scale, 0.4f * Projectile.scale);
		Projectile.velocity *= 0.999f;
		Projectile.scale = 0.6f + (float)Math.Sin(Main.timeForVisualEffects / 1.8f + Projectile.ai[0]) * 0.45f;
		Projectile.timeLeft -= player.ownedProjectileCounts[Projectile.type];
		if (Main.rand.NextBool(8))
		{
			foreach (NPC target in Main.npc)
			{
				if (target.active)
				{
					if (!target.friendly && !target.dontTakeDamage && target.CanBeChasedBy())
					{
						Vector2 v = target.Center - Projectile.Center;
						if (v.Length() < 400)
						{
							if (Main.rand.NextBool(6))
							{
								ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
								Gsplayer.FlyCamPosition = new Vector2(0, 11).RotatedByRandom(6.283);
								int HitType = ModContent.ProjectileType<MagnetSphereLighting>();
								var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Vector2.One, HitType, Projectile.damage * 1, Projectile.knockBack, Projectile.owner, Projectile.whoAmI, Projectile.rotation + Main.rand.NextFloat(6.283f));
								p.CritChance = Projectile.CritChance;
								SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, target.Center);
								Projectile.penetrate--;
								if (Projectile.penetrate < 0)
									Projectile.Kill();
							}
						}
					}
				}
			}
		}
	}
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[0] = 0;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D Light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/MagnetSphere/MagnetSphereII");
		Texture2D Light2 = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/MagnetSphere/Projectile_254");
		Texture2D Shade = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterBolt/NewWaterBoltShade");

		var c0 = new Color(0, 199, 129, 0);


		Main.spriteBatch.Draw(Shade, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, Shade.Size() / 2f, 1.08f * Projectile.scale, SpriteEffects.None, 0);


		Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, 0.8f * Projectile.scale, SpriteEffects.None, 0);
		var rt = new Rectangle(0, 44 * (int)(Main.timeForVisualEffects / 6f % 5), 38, 44);
		Main.spriteBatch.Draw(Light2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rt, new Color(95, 95, 95, 55), Projectile.rotation, rt.Size() / 2f, Projectile.scale * 0.2f + 1.2f, SpriteEffects.None, 0);
		return false;
	}



	public override void Kill(int timeLeft)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 22).RotatedByRandom(6.283);
		SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
		for (int d = 0; d < 14; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Electric, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283) * 3;
		}
		int HitType = ModContent.ProjectileType<MagnetSphereHit>();
		var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, (int)(Projectile.damage * 3f), Projectile.knockBack, Projectile.owner, 24, Projectile.rotation + Main.rand.NextFloat(6.283f));
		p.CritChance = Projectile.CritChance;
	}

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
		Spark();
	}
	public override void OnHitPvp(Player target, int damage, bool crit)
	{
		Spark();
	}
	private void Spark()
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 22).RotatedByRandom(6.283);
		SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);
		for (int d = 0; d < 10; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Electric, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
		}
		Projectile.penetrate -= 5;
		if (Projectile.penetrate < 0)
			Projectile.Kill();
		int HitType = ModContent.ProjectileType<MagnetSphereHit>();
		var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, (int)(Projectile.damage * 2f), Projectile.knockBack, Projectile.owner, 18, Projectile.rotation + Main.rand.NextFloat(6.283f));
		p.CritChance = Projectile.CritChance;
		Projectile.damage = (int)(Projectile.damage * 1.2);
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Spark();
		if (Projectile.velocity.X != oldVelocity.X)
			Projectile.velocity.X = -oldVelocity.X;
		if (Projectile.velocity.Y != oldVelocity.Y)
			Projectile.velocity.Y = -oldVelocity.Y;
		Projectile.velocity *= 0.98f;
		Projectile.penetrate -= 5;
		if (Projectile.penetrate < 0)
			Projectile.Kill();
		return false;
	}
}