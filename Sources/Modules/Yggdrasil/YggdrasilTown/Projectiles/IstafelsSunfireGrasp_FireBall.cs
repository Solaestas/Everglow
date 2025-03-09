using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class IstafelsSunfireGrasp_FireBall : TrailingProjectile
{
	public override string Texture => Commons.ModAsset.Point_Mod;

	public const int ProjectileVelocity = 4;
	public const int SubProjectileVelocity = 12;
	private const float InitialScale = 0.5f;
	public const int TimeLeftMax = 600;

	public const int BuffDuration = 960;
	public const int ContactDamage = 25;
	public const int BuildUpMax = 200;

	/// <summary>
	/// If the projectile has been actived by <see cref="IstafelsSunfireGrasp"/>.
	/// </summary>
	public bool Actived { get; set; } = false;

	/// <summary>
	/// The build-up value of projectile, capped at <see cref="BuildUpMax"/>.
	/// </summary>
	public int BuildUp { get; set; } = 0;

	/// <summary>
	/// The build-up value of projectile, bewteen 0 and 1.
	/// </summary>
	public float BuildUpProgress => Math.Clamp(BuildUp / (float)BuildUpMax, 0f, 1f);

	/// <summary>
	/// Chase target.
	/// </summary>
	public int TargetWhoAmI
	{
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();

		Projectile.width = 40;
		Projectile.height = 40;

		Projectile.DamageType = DamageClass.Default;

		Projectile.friendly = false;
		Projectile.hostile = false;

		Projectile.timeLeft = TimeLeftMax;

		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;

		Projectile.localNPCHitCooldown = 10;
		Projectile.usesLocalNPCImmunity = true;

		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		TrailColor = new Color(1f, 0.3f, 0f, 0);
		SelfLuminous = false;
		TargetWhoAmI = -1;
	}

	public override bool? CanCutTiles() => true;

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		ShootScoria();
		return true;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		// Let the projectile deals true damage, minus base damage 1
		modifiers.FinalDamage.Flat += ContactDamage - 1;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<Charred>(), BuffDuration);
		ShootScoria();
	}

	/// <summary>
	/// Explode then shoot scoria. Only called when killed by tile colliding and npc colliding.
	/// </summary>
	private void ShootScoria()
	{
		// Shoot explosion
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IstafelsSunfireGrasp_Explosion>(), 200, Projectile.knockBack, Projectile.owner);

		// Shoot scoria
		var scoriaProjCount = Main.rand.Next(8, 10);
		for (int i = 0; i < scoriaProjCount; i++)
		{
			var velocity = Projectile.velocity * 0.6f + Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 4f;
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<IstafelsSunfireGrasp_Scoria>(), 1, 1.1f, Projectile.owner);
		}
	}

	public override void AI()
	{
		// Trailing proj ai
		Timer++;

		Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0f);
		if (Actived)
		{
			Projectile.rotation += Projectile.velocity.X * 0.04f;

			if (TargetWhoAmI < 0)
			{
				TargetWhoAmI = ProjectileUtils.FindTarget(Projectile.Center, 300);
				return;
			}

			// Generate dusts
			if (Main.rand.NextBool(6))
			{
				Dust.NewDustDirect(
					Projectile.position + new Vector2(Projectile.width, Projectile.height) * (1 - BuildUpProgress) / 2,
					(int)(Projectile.width * BuildUpProgress),
					(int)(Projectile.height * BuildUpProgress),
					DustID.RedTorch,
					Projectile.velocity.X,
					Projectile.velocity.Y,
					Scale: 1.2f);
			}

			// Chase target
			var target = Main.npc[TargetWhoAmI];
			if (target.active)
			{
				var distance = target.Center - Projectile.Center;
				Projectile.velocity = distance.NormalizeSafe() * ProjectileVelocity;
			}
			else
			{
				TargetWhoAmI = -1;
			}
		}
		else
		{
			var owner = Main.player[Projectile.owner];

			// Update life time
			if (owner.HasBuff<IstafelsSunfireGraspFireBallBuff>())
			{
				owner.AddBuff(ModContent.BuffType<IstafelsSunfireGraspFireBallBuff>(), 2);
				Projectile.timeLeft = TimeLeftMax;
			}
			else
			{
				Projectile.Kill();
				return;
			}

			// Chase player
			var destination = owner.Center + new Vector2(-30 * owner.direction, -50 * owner.gravDir);
			var movement = destination - Projectile.Center;
			if (movement.Length() < ProjectileVelocity * 3)
			{
				Projectile.velocity = movement;
			}
			else
			{
				Projectile.velocity = movement.NormalizeSafe() * ProjectileVelocity * 3;
			}

			if (BuildUpProgress > 0.5f)
			{
				// Generate dusts
				if (Main.rand.NextBool(6))
				{
					Dust.NewDustDirect(
						Projectile.position + new Vector2(Projectile.width, Projectile.height) * (1 - BuildUpProgress) / 2,
						(int)(Projectile.width * BuildUpProgress),
						(int)(Projectile.height * BuildUpProgress),
						DustID.RedTorch,
						Projectile.velocity.X,
						Projectile.velocity.Y,
						Scale: 1.2f);
				}

				// Shoot projectile to target
				if (Timer % 120 == 0)
				{
					var target = ProjectileUtils.FindTarget(Projectile.Center, 2000);
					if (target >= 0)
					{
						var direction = (Main.npc[target].Center - Projectile.Center).NormalizeSafe();
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, direction * SubProjectileVelocity, ProjectileID.ImpFireball, 50, 0.4f, Projectile.owner);
					}
				}
			}
		}
	}

	/// <summary>
	/// Active projectile
	/// </summary>
	/// <param name="target"></param>
	public void Active(NPC target)
	{
		Actived = true;

		// Shoot projectile towards the target
		TargetWhoAmI = target.whoAmI;
		Projectile.netUpdate = true;
		Projectile.velocity = (target.Center - Projectile.Center).NormalizeSafe() * ProjectileVelocity;

		// Update misc infos
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();

		var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		var effect = ModAsset.IstafelsSunfireGrasp_FireBall.Value;
		effect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * 0.05f);
		effect.CurrentTechnique.Passes[0].Apply();

		var drawPos = Projectile.Center - Main.screenPosition;
		var drawScale = MathHelper.Lerp(InitialScale, 1f, BuildUpProgress);
		var drawSize = Projectile.width / 2 * drawScale;
		var vertices = new List<Vertex2D>
		{
			{ drawPos + new Vector2(-1, -1).RotatedBy(Projectile.rotation) * drawSize, Color.White, new(0, 0, 0) },
			{ drawPos + new Vector2(1, -1).RotatedBy(Projectile.rotation) * drawSize, Color.White, new(1, 0, 0) },
			{ drawPos + new Vector2(-1, 1).RotatedBy(Projectile.rotation) * drawSize, Color.White, new(0, 1, 0) },
			{ drawPos + new Vector2(1, 1).RotatedBy(Projectile.rotation) * drawSize, Color.White, new(1, 1, 0) },
		};

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		return false;
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);

		// Draw dusts on kill
		for (int i = 0; i < 40; i++)
		{
			var offset = new Vector2(Main.rand.NextFloat(16), 0).RotatedBy(Main.rand.NextFloat() * MathHelper.TwoPi);
			Dust.NewDust(Projectile.Center + offset, 4, 4, DustID.LavaMoss, Scale: 1.4f);
		}
		for (int i = 0; i < 20; i++)
		{
			Dust.NewDust(Projectile.Center, Projectile.width / 2, Projectile.height / 2, DustID.Torch, Scale: 1.2f);
		}
	}
}