using Everglow.Myth.Misc.Dusts;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

internal class ComingGhost : MeleeProj
{
	public override void SetDef()
	{
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 30;
		Projectile.extraUpdates = 1;
		Projectile.scale = 1f;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
		Projectile.DamageType = DamageClass.Melee;

		Projectile.width = 80;
		Projectile.height = 80;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		longHandle = false;
		MaxAttackType = 3;
		MaxSlashTrailLength = 20;
		shaderType = Commons.MEAC.Enums.MeleeTrailShaderType.ArcBladeTransparentedByZ;
		AutoEnd = false;
	}

	private int hasHit = 0;

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		hasHit++;
		if (hasHit > 2)
		{
			return;
		}

		Player player = Main.player[Projectile.owner];
		Vector2 v = new Vector2(0, 6).RotatedByRandom(Math.PI * 2) * 5f;
		Projectile.NewProjectile(null, target.Center - v * 3, v, ModContent.ProjectileType<GhostHit>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(-0.05f, 0.05f));
	}

	public override string TrailShapeTex()
	{
		return Commons.ModAsset.Melee_Mod;
	}

	public override string TrailColorTex()
	{
		return "Everglow/Myth/Misc/Projectiles/Weapon/Melee/ComingGhost_meleeColor";
	}

	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.15f;
	}

	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}

	public override void End()
	{
		Player player = Main.player[Projectile.owner];
		player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
		player.fullRotation = 0;
		player.legRotation = 0;
		player.legPosition = Vector2.Zero;
		Projectile.Kill();
		player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}

	public override void OnSpawn(IEntitySource source)
	{
		hasHit = 0;
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		base.AI();
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		UseTrail = true;
		float timeMul = 1 / player.meleeSpeed;
		if (CurrantAttackType == 0)
		{
			if (Timer < 14 * timeMul)// 前摇
			{
				UseTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				MainAxisDirection += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
			if (Timer == (int)(14 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 14 * timeMul && Timer < 35 * timeMul)
			{
				IsAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(110, Projectile.rotation, -1.2f, -0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((Timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;

				Dust d = Dust.NewDustDirect(player.Center + MainAxisDirection * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (Timer > 44 * timeMul)
			{
				hasHit = 0;
				NextAttackType();
			}
		}

		if (CurrantAttackType == 1)
		{
			if (Timer < 9 * timeMul)// 前摇
			{
				UseTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				MainAxisDirection += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
			if (Timer == (int)(10 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 9 * timeMul && Timer < 42 * timeMul)
			{
				IsAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.17f / timeMul;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(110, Projectile.rotation, 0, 0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((Timer - 9 * timeMul) / (30f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;

				Dust d = Dust.NewDustDirect(player.Center + MainAxisDirection * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (Timer > 55 * timeMul)
			{
				hasHit = 0;
				NextAttackType();
			}
		}

		if (CurrantAttackType == 2)
		{
			if (Timer < 10 * timeMul)// 前摇
			{
				UseTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 1.6f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				MainAxisDirection += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
			if (Timer == (int)(10 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 9 * timeMul && Timer < 25 * timeMul)
			{
				IsAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.45f / timeMul;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(110, Projectile.rotation, -0.2f, 0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((Timer - 9 * timeMul) / (10f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;
				for (int i = 0; i < 3; i++)
				{
					Dust d = Dust.NewDustDirect(player.Center + MainAxisDirection * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
					d.scale = Main.rand.NextFloat(2f, 4.5f);
				}
			}
			if (Timer > 30 * timeMul)
			{
				hasHit = 0;
				NextAttackType();
			}
		}

		if (CurrantAttackType == 3)
		{
			if (Timer < 2 * timeMul)// 前摇
			{
				Projectile.ai[0] = 0;
				Projectile.ai[1] = player.direction;
				Style3StartPoint = player.Center;
				UseTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				MainAxisDirection += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
				if (Timer == 1)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<ComingGhost_Shimmer>(), 0, Projectile.knockBack, player.whoAmI, player.direction);
				}
			}
			if (Timer == (int)(10 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 2 * timeMul && Timer < 25 * timeMul)
			{
				LockPlayerDir(player);
				IsAttacking = true;
				player.immuneAlpha = 255;
				Projectile.rotation += Projectile.spriteDirection * 0.4f / timeMul;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(110, Projectile.rotation, -1.25f, -0.1f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((Timer - 16 * timeMul) / (28f * timeMul) * MathHelper.Pi) * 0.6f * Projectile.ai[1];
				player.legRotation = -player.fullRotation;
				float duration = (Timer - 2 * timeMul) / (float)(23 * timeMul) * 100f;
				if (duration > 10 * Projectile.ai[0])
				{
					Projectile.ai[0]++;
					if (!Collision.SolidCollision(player.position + new Vector2(35 * Projectile.ai[1], 0), player.width, player.height - 20))
					{
						player.position.X += 35 * Projectile.ai[1];
					}

					Vector2 v0 = new Vector2(0, 14 * Main.rand.NextFloat(0.65f, 1.8f)).RotatedByRandom(MathHelper.TwoPi);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Style3StartPoint + new Vector2((60 + duration * 2.5f) * Projectile.ai[1], 0) - v0 * 8, v0, ModContent.ProjectileType<ComingGhost_Slash>(), Projectile.damage, Projectile.knockBack);
				}
			}
			if (Timer > 44 * timeMul)
			{
				hasHit = 0;
				NextAttackType();
			}
			if (Timer == (int)(40 * timeMul))
			{
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<ComingGhost_Shimmer>(), 0, Projectile.knockBack, player.whoAmI, -player.direction);
			}
		}
	}

	public Vector2 Style3StartPoint = Vector2.zeroVector;

	public override void OnKill(int timeLeft)
	{
		Player player = Main.player[Projectile.owner];
		player.fullRotation = 0;
	}

	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		if (CurrantAttackType == 3)
		{
			return;
		}

		// drawScale = new Vector2(-0.6f, 1.14f);
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}

	public override void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(SlashTrail.ToList()); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (SlashTrail.Count != 0)
		{
			SmoothTrail.Add(SlashTrail.ToArray()[SlashTrail.Count - 1]);
		}

		int length = SmoothTrail.Count;
		if (length <= 3)
		{
			return;
		}

		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = TrailAlpha(factor);
			Color c0 = Color.White;
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.3f * Projectile.scale, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, c0, new Vector3(factor, 0, w)));
		}
		bars.Add(new Vertex2D(Projectile.Center + MainAxisDirection * 0.3f * Projectile.scale, Color.White, new Vector3(0, 1, 0f)));
		bars.Add(new Vertex2D(Projectile.Center + MainAxisDirection * Projectile.scale, Color.White, new Vector3(0, 0, 1)));
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		Effect MeleeTrail = Commons.ModAsset.MeleeTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		// Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes[ShaderTypeName].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}

	public override void DrawWarp(VFXBatch spriteBatch)
	{
	}
}