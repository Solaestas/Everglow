using Terraria.Audio;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Melee;

public class AcroporaSpear_proj : MeleeProj
{
	public override void SetDef()
	{
		maxAttackType = 3;
		trailLength = 20;
		longHandle = true;
		shadertype = "Trail";
		AutoEnd = false;
		CanLongLeftClick = true;
	}
	public override string TrailShapeTex()
	{
		return Commons.ModAsset.Melee_Mod;
	}
	public override string TrailColorTex()
	{
		return ModAsset.Acropora_Color_Mod;
	}
	public override float TrailAlpha(float factor)
	{
		if (attackType == 3)
			return base.TrailAlpha(factor) * 1.5f;
		return base.TrailAlpha(factor) * 1.3f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		//伤害倍率
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		float ShakeStrength = 0.2f;
		if (attackType == 0)
		{
			modifiers.FinalDamage *= 1.4f;
			ShakeStrength = 1f;
		}

		Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12f * ShakeStrength, 100)).RotatedByRandom(6.283);
	}
	public override void Attack()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		if (Main.myPlayer == Projectile.owner && Main.mouseRight && Main.mouseRightRelease)
		{

		}

		useTrail = true;

		Vector2 vToMouse = Main.MouseWorld - player.Top;
		float AddHeadRotation = (float)Math.Atan2(vToMouse.Y, vToMouse.X) + (1 - player.direction) * 1.57f;
		if (player.gravDir == -1)
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation >= 0.57f && AddHeadRotation < 2)
					AddHeadRotation = 0.57f;
			}
			else
			{
				if (AddHeadRotation <= -0.57f)
					AddHeadRotation = -0.57f;
			}
		}
		else
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation >= 2 && AddHeadRotation < 5.71f)
					AddHeadRotation = 5.71f;
			}
			else
			{
				if (AddHeadRotation >= 0.57f)
					AddHeadRotation = 0.57f;
			}
		}

		if (attackType == 0)
		{
			int t = timer;
			if (t < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 - Player.direction * 0.8f;
				mainVec = Vector2.Lerp(mainVec, Player.DirectionTo(MouseWorld_WithoutGravDir) * 150, 0.2f);
				disFromPlayer = MathHelper.Lerp(disFromPlayer, -30, 0.2f);
				Projectile.rotation = mainVec.ToRotation();
			}
			if (t >= 20 && t < 40)
			{
				if (t == 20)
				{
					SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					if (Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(Player.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 20, ModContent.ProjectileType<AcroporaThumpEff>(), Projectile.damage, 0, Projectile.owner);
				}
				if (t == 26)
				{
					SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					if (Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(Player.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec).RotatedBy(0.3) * 20, ModContent.ProjectileType<AcroporaThumpEff>(), Projectile.damage, 0, Projectile.owner);
				}
				if (t == 32)
				{
					SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					if (Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(Player.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec).RotatedBy(-0.3) * 20, ModContent.ProjectileType<AcroporaThumpEff>(), Projectile.damage, 0, Projectile.owner);
				}
				if (t < 24)
				{
					disFromPlayer += 60;
					isAttacking = true;
				}
				else if (t < 26)
				{
					disFromPlayer -= 90;
					isAttacking = false;
				}
				else if (t < 30)
				{
					disFromPlayer += 60;
					isAttacking = true;
				}
				else if (t < 32)
				{
					disFromPlayer -= 90;
					isAttacking = false;
				}
				else if (t < 34)
				{
					disFromPlayer += 60;
					isAttacking = true;
				}
				else if (t < 36)
				{
					disFromPlayer -= 90;
					isAttacking = false;
				}
				else if (t < 40)
				{
					disFromPlayer += 60;
					isAttacking = true;
				}
				else
				{
					disFromPlayer -= 36;
					isAttacking = false;
				}
			}
			if (t > 40)
			{
				disFromPlayer = 6;
				NextAttackType();
			}
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.3f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation + AddHeadRotation;
			}
		}
		if (attackType == 1)
		{
			float rot = 0.45f * player.direction;
			if (timer < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 - Player.direction * 1.2f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f, rot), 0.15f);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 8)
			{
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleeSwing"));
			}

			if (timer > 20 && timer < 35)
			{
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.36f, 0f);
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.4f;
				mainVec = Vector2Elipse(220, Projectile.rotation, -1.2f, rot);
			}
			if (timer > 40)
				NextAttackType();
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
		}
		if (attackType == 2)
		{
			if (timer < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 + Player.direction * 1.2f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 8)
			{
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleeSwing"));
			}
			if (timer % 10 == 8 && timer > 30)
				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			if (timer > 20 && timer < 75)
			{
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.36f, 0f);
				isAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.4f;
				mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);
			}
			if (timer > 80)
				NextAttackType();
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
		}
		if (attackType == 3)
		{
			if (timer < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 + Player.direction * 1.2f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 8)
			{
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleeSwing"));
			}
			if (timer % 10 == 8 && timer > 30)
				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			if (timer > 20 && timer < 75)
			{
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.06f, 0f);
				isAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.4f;
				mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);
			}
			if (timer > 70)
				NextAttackType();
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
		}
	}
	public override void DrawTrail(Color color)
	{
		base.DrawTrail(color);
		List<Vector2> smoothTrail_current = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < smoothTrail_current.Count - 1; x++)
		{
			SmoothTrail.Add(smoothTrail_current[x]);
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
			if (!longHandle)
			{
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.15f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
			}
			else
			{
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.2f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
			}
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;

		Effect MeleeTrail = ModContent.Request<Effect>("Everglow/MEAC/Effects/MeleeTrailFade", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value; // MeleeTrailFade should be moved to Everglow.Function -> MEAC -> Effects, instead of Modules -> MEAC -> Effects, or find another way. ~Setnour6
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.Acropora_RedColor.Value;
		float k0 = timer / 80f + 0.3f;
		MeleeTrail.Parameters["FadeValue"].SetValue(MathF.Sqrt(k0 * 1.2f));
		MeleeTrail.Parameters["tex1"].SetValue(ModAsset.Acropora_TexBlood.Value);
		MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

	}
	public override void End()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
		player.fullRotation = 0;
		player.legRotation = 0;
		Tplayer.HeadRotation = 0;
		Tplayer.HideLeg = false;
		player.legPosition = Vector2.Zero;
		Projectile.Kill();
		player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.Poisoned, 600);
	}
}

