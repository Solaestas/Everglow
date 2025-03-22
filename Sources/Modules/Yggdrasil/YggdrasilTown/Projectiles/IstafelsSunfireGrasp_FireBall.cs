using Everglow.Commons.DataStructures;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;
using Everglow.Yggdrasil.YggdrasilTown.VFXs.IstafelsEffects;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class IstafelsSunfireGrasp_FireBall : TrailingProjectile, IWarpProjectile_warpStyle2
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

		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
		TrailColor = new Color(0.7f, 0.1f, 0f, 0);
		TrailTexture = Commons.ModAsset.Trail_8.Value;
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

		Lighting.AddLight(Projectile.Center, 1.8f, 0.8f, 0f);
		if (Actived)
		{
			Projectile.rotation += Projectile.velocity.X * 0.01f;

			if (TargetWhoAmI < 0)
			{
				TargetWhoAmI = ProjectileUtils.FindTarget(Projectile.Center, 300);
				return;
			}

			// Generate dusts
			if (Main.rand.NextBool(6))
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
				var spark = new FireSpark_MetalStabDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center,
					maxTime = Main.rand.Next(1, 25),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(10f, 27.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) },
				};
				Ins.VFXManager.Add(spark);
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
			float speedMax = movement.Length() * 0.1f + owner.velocity.Length();
			if (movement.Length() > 10f)
			{
				Projectile.velocity = movement.NormalizeSafe() * speedMax * 0.05f + Projectile.velocity * 0.95f;
			}

			if (Projectile.velocity.Length() > speedMax)
			{
				Projectile.velocity = Projectile.velocity.NormalizeSafe() * speedMax;
			}
			Projectile.velocity *= 0.9f;

			if (BuildUpProgress > 0.5f)
			{
				// Generate dusts
				// This lava drop look also well.
				// But I have a better idea.
				if (Main.rand.NextBool(26))
				{
					// var randomInHitBox = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height * 0.5f, Projectile.height));
					// var generatePos = Vector2.Lerp(randomInHitBox, Projectile.Center, 0.75f);
					// Gore gore = Gore.NewGoreDirect(generatePos, Projectile.velocity, GoreID.LavaDrip);
					// gore.frame = 7;
					// gore.scale = Main.rand.NextFloat(0.4f, 1.2f);
					// gore.velocity.X *= 0.1f;
					// gore.velocity.Y = 0;
					// gore.velocity += Projectile.velocity;
					// gore.position -= gore.AABBRectangle.Size() * 0.5f;
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<IstafelsSunfireDrop_glimmer>());
					dust.customData = Projectile;
					dust.velocity = new Vector2(0, 1).RotatedByRandom(0.4f);
				}

				// Shoot projectile to target
				
			}
			if (Timer % 120 == 0)
			{
				var target = ProjectileUtils.FindTarget(Projectile.Center, 2000);
				if (target >= 0 && Collision.CanHit(Main.npc[target], Projectile))
				{
					var direction = (Main.npc[target].Center - Projectile.Center).NormalizeSafe();
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, direction * SubProjectileVelocity, ModContent.ProjectileType<IstafelsSunfireGrasp_Sub_FireBall>(), 50, 0.4f, Projectile.owner);
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
		var drawPos = Projectile.Center - Main.screenPosition;
		var drawScale = MathHelper.Lerp(InitialScale, 1f, BuildUpProgress);
		var drawSize = Projectile.width / 2 * drawScale;
		var blackBall = Commons.ModAsset.blackBall.Value;
		Main.spriteBatch.Draw(blackBall, drawPos, null, Color.White, 0, blackBall.Size() * 0.5f, drawScale * 0.4f, SpriteEffects.None, 0);

		DrawTrail();

		var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		var effect = ModAsset.IstafelsSunfireGrasp_FireBall.Value;
		effect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * 0.05f);
		effect.CurrentTechnique.Passes[0].Apply();

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

	public override void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
		{
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);
		}

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = 1;
			float timeValue = (float)-Main.timeForVisualEffects * 0.005f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			float lengthFade = 1f;
			if (i > SmoothTrail.Count - 20f)
			{
				lengthFade = (SmoothTrail.Count - i - 1) / 20f;
			}
			if (i < 3f)
			{
				lengthFade *= i / 3f;
			}
			drawC *= lengthFade;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue + 0.6f, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue + 0.6f, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue + 0.8f, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue + 0.8f, 0.5f, width)));
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		if (bars2.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		}

		if (bars3.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
		ShakerManager.AddShaker(Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.Pi), 60, 12f, 200, 0.9f, 0.8f, 600);
	}

	public new void DrawWarp(VFXBatch spriteBatch)
	{
		var drawPos = Projectile.Center - Main.screenPosition;
		var fade = 1f;
		var drawScale = MathHelper.Lerp(InitialScale, 1f, BuildUpProgress);
		var drawSize = MathF.Sqrt(Projectile.width / 2 * drawScale) * 0.27f;
		var timeValue = (float)Main.timeForVisualEffects * 0.003f;
		float resolutionCircle = 50;
		List<Vertex2D> bars = new List<Vertex2D>();
		List<Vertex2D> bars2 = new List<Vertex2D>();
		for (int i = 0; i <= resolutionCircle; i++)
		{
			var radiusInner = new Vector2(0, 10 * drawSize).RotatedBy(i / resolutionCircle * MathHelper.TwoPi);
			var radiusMiddle = new Vector2(0, 24 * drawSize).RotatedBy(i / resolutionCircle * MathHelper.TwoPi) - Projectile.velocity;
			var radiusOuter = new Vector2(0, 40 * drawSize).RotatedBy(i / resolutionCircle * MathHelper.TwoPi) - Projectile.velocity * 2;

			float rot = MathHelper.PiOver2;

			Vector2 dirInner = new Vector2(1, 0).RotatedBy(i / resolutionCircle * MathHelper.TwoPi + rot);
			Vector2 dirMiddle = (dirInner - Projectile.velocity.NormalizeSafe() * 0.15f).NormalizeSafe();
			Vector2 dirOuter = (dirMiddle - Projectile.velocity.NormalizeSafe() * 0.15f).NormalizeSafe();

			Color colorInner = new Color(-dirInner.X * 0.5f + 0.5f, -dirInner.Y * 0.5f + 0.5f, 0, 0);
			Color colorMiddle = new Color(-dirMiddle.X * 0.5f + 0.5f, -dirMiddle.Y * 0.5f + 0.5f, fade * 0.5f * Main.GameViewMatrix.Zoom.X, 0);
			Color colorOuter = new Color(-dirOuter.X * 0.5f + 0.5f, -dirOuter.Y * 0.5f + 0.5f, 0, 0);

			bars.Add(drawPos + radiusMiddle, colorMiddle, new Vector3(timeValue, i / 50f, 1));
			bars.Add(drawPos + radiusInner, colorInner, new Vector3(timeValue + 0.15f, i / 50f, 1));

			bars2.Add(drawPos + radiusMiddle, colorMiddle, new Vector3(timeValue, i / 50f, 1));
			bars2.Add(drawPos + radiusOuter, colorOuter, new Vector3(timeValue - 0.15f, i / 50f, 1));

			// bars.Add(drawPos + radiusMiddle, colorMiddle, new Vector3(i / 50f - timeValue, 0, timeValue));
			// bars.Add(drawPos + radiusInner, colorInner, new Vector3(i / 50f - timeValue, 0, timeValue + 0.3f));

			// bars2.Add(drawPos + radiusOuter, colorOuter, new Vector3(i / 50f - timeValue, 0, timeValue - 0.3f));
			// bars2.Add(drawPos + radiusMiddle, colorMiddle, new Vector3(i / 50f - timeValue, 0, timeValue));
		}
		if (bars.Count > 2)
		{
			Main.graphics.graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.graphicsDevice.RasterizerState = RasterizerState.CullNone;
			spriteBatch.Draw(Commons.ModAsset.Noise_melting_H.Value, bars, PrimitiveType.TriangleStrip);
		}
		if (bars2.Count > 2)
		{
			Main.graphics.graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.graphicsDevice.RasterizerState = RasterizerState.CullNone;
			spriteBatch.Draw(Commons.ModAsset.Noise_melting_H.Value, bars2, PrimitiveType.TriangleStrip);
		}
	}
}