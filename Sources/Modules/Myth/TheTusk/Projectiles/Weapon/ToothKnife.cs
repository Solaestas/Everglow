using Everglow.Myth.Misc.Dusts;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles.Weapon;

class ToothKnife : MeleeProj, IWarpProjectile
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
		maxAttackType = 5;
		trailLength = 20;
		shadertype = "Trail";
		AutoEnd = false;
		selfWarp = true;
	}
	private int HasHit = 0;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		HasHit++;
		if (HasHit > 2)
			return;
		Player player = Main.player[Projectile.owner];
		Vector2 v = new Vector2(0, 6).RotatedByRandom(Math.PI * 2) * 5f;
		Projectile.NewProjectile(null, target.Center - v * 3, v, ModContent.ProjectileType<TuskSlash>(), Projectile.damage / 3, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(-0.05f, 0.05f));
	}
	public override string TrailShapeTex()
	{
		return Commons.ModAsset.Melee_Mod;
	}
	public override string TrailColorTex()
	{
		return "Everglow/Myth/TheTusk/Projectiles/Weapon/TuskKnife_meleeColor";
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
		HasHit = 0;
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		base.AI();
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		useTrail = true;
		float timeMul = 1 / player.meleeSpeed;
		if (attackType == 0)
		{
			if (timer < 14 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(82, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(14 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 14 * timeMul && timer < 35 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, -1.2f, -0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;

				//Dust d = Dust.NewDustDirect(player.Center + mainVec * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				//d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (timer > 44 * timeMul)
			{
				HasHit = 0;
				NextAttackType();
			}
		}
		if (attackType == 1)
		{
			if (timer < 4 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(82, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(4 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 4 * timeMul && timer < 25 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, -1.0f, 0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.2f * player.direction;
				player.legRotation = -player.fullRotation;

				//Dust d = Dust.NewDustDirect(player.Center + mainVec * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				//d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (timer > 34 * timeMul)
			{
				HasHit = 0;
				NextAttackType();
			}
		}

		if (attackType == 2)
		{
			if (timer < 4 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(82, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(4 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 4 * timeMul && timer < 25 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.32f / timeMul;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, -1.1f, -0.6f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = -MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;

				//Dust d = Dust.NewDustDirect(player.Center + mainVec * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				//d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (timer > 34 * timeMul)
			{
				HasHit = 0;
				NextAttackType();
			}
		}

		if (attackType == 3)
		{
			if (timer < 4 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(82, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(4 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 4 * timeMul && timer < 25 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, -1.0f, 0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.3f * player.direction;
				player.legRotation = -player.fullRotation;

				//Dust d = Dust.NewDustDirect(player.Center + mainVec * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				//d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (timer > 34 * timeMul)
			{
				HasHit = 0;
				NextAttackType();
			}
		}
		if (attackType == 4)
		{
			if (timer < 4 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(82, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(4 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 4 * timeMul && timer < 25 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.32f / timeMul;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, -0.6f, -0.6f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = -MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;

				//Dust d = Dust.NewDustDirect(player.Center + mainVec * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				//d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (timer > 34 * timeMul)
			{
				HasHit = 0;
				NextAttackType();
			}
		}
		if (attackType == 5)
		{
			if (timer < 14 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 2.4f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(82, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();

				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = -MathF.Sin((timer * timeMul) / (28f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;

				if (timer == (int)(1 * timeMul))
				{
					for (int i = 0; i < 5; i++)
					{
						if(player.gravDir == 1)
						{
							Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), player.Top - new Vector2(0, 30 + i * 20), new Vector2(player.direction, 1) / 10000f, ModContent.ProjectileType<TuskPin>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
							p0.timeLeft = 80 + i * 5 + Main.rand.Next(5);
						}
						else
						{
							Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), player.Bottom + new Vector2(0, 30 + i * 20), new Vector2(player.direction, -1) / 10000f, ModContent.ProjectileType<TuskPin>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
							p0.timeLeft = 80 + i * 5 + Main.rand.Next(5);
						}
					}
				}
			}
			
			if (timer == (int)(4 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 14 * timeMul && timer < 35 * timeMul)
			{
				isAttacking = true;
				float coefficientTimer = (timer * timeMul - 14 * timeMul) / (28f * timeMul);
				Projectile.rotation += Projectile.spriteDirection * 1.3f / MathF.Pow(timeMul, 5.5f) * coefficientTimer * coefficientTimer;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, 0.1f, 0.3f * Projectile.spriteDirection), 0.9f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = -MathF.Sin((timer - 14 * timeMul) / (18f * timeMul) * MathHelper.Pi) * 0.7f * player.direction;
				player.legRotation = -player.fullRotation;

				//Dust d = Dust.NewDustDirect(player.Center + mainVec * MathF.Sqrt(Main.rand.NextFloat(1f)), 0, 0, ModContent.DustType<Crow>());
				//d.scale = Main.rand.NextFloat(2f, 4.5f);
			}
			if (timer > 54 * timeMul)
			{
				HasHit = 0;
				NextAttackType();
			}
		}
	}
	public override void OnKill(int timeLeft)
	{
		Player player = Main.player[Projectile.owner];
		player.fullRotation = 0;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		if (attackType == 3)
		{
			return;
		}
		//drawScale = new Vector2(-0.6f, 1.14f);
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}
	public override void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = TrailAlpha(factor);
			Point point = (Projectile.Center + trail[i] * Projectile.scale).ToPoint();
			Color c0 = Lighting.GetColor(point.X / 16, point.Y / 16);
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.1f * Projectile.scale, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, c0, new Vector3(factor, 0, w)));
		}
		Point point1 = (Projectile.Center + mainVec * Projectile.scale).ToPoint();
		Color c1 = Lighting.GetColor(point1.X / 16, point1.Y / 16);
		bars.Add(new Vertex2D(Projectile.Center + mainVec * 0.1f * Projectile.scale, c1, new Vector3(0, 1, 0f)));
		bars.Add(new Vertex2D(Projectile.Center + mainVec * Projectile.scale, c1, new Vector3(0, 0, 1)));
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		Effect MeleeTrail = Commons.ModAsset.MeleeTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.Wave_slash.Value;
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes["ArcBladeAffectByEnvironmentLight"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void DrawWarp(VFXBatch spriteBatch)
	{
		base.DrawWarp(spriteBatch);
	}
}
