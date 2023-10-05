using Terraria.Audio;
namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class DragonScaleHammerProj : MeleeProj
{
	public override void SetDef()
	{
		Projectile.height = 20;
		Projectile.width = 20;
		maxAttackType = 4;
		trailLength = 20;
		shadertype = "Trail";
		longHandle = true;
		Projectile.scale *= 1.1f;
	}
	public override string TrailShapeTex()
	{
		return "Everglow/Yggdrasil/YggdrasilTown/Projectiles/DragonScaleHammerProj_Melee";
	}
	public override string TrailColorTex()
	{
		return "Everglow/Yggdrasil/YggdrasilTown/Projectiles/DragonScaleHammerProj_heatMap";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.15f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}
	public override void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
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
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.6f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.99f * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

		Effect MeleeTrail = ModContent.Request<Effect>("Everglow/MEAC/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Melee.Value;
		//Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		drawScale = new Vector2(-0.1f, 1.4f);
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 50, 50)).RotatedByRandom(6.283);
		base.ModifyHitNPC(target, ref modifiers);
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
	public override void Attack()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		useTrail = true;
		float timeMul = 1f - GetMeleeSpeed(player) / 100f;

		if (attackType == 0)
		{
			float timeValue = timer - 30;
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				Vector2 elipse = Vector2Elipse(110, targetRot, 2 * player.direction, 0);
				mainVec = Vector2.Lerp(mainVec, elipse, 0.1f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 24)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing"));
			if (timer > 30 && timer < 70)
			{
				isAttacking = true;

				timeValue = timeValue / 20f;
				Projectile.rotation += Projectile.spriteDirection * 0.085f * timeValue * timeValue;
				mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f, 0f);
			}

			if (timer > 70 + 20 * timeMul)
			{
				player.fullRotation = 0;
				player.legRotation = 0;
				NextAttackType();
			}
			else
			{
				float BodyRotation = (float)Math.Sin((timeValue - 10) / 70d * Math.PI) * 0.4f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
		}
		if (attackType == 1)
		{
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 + player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(120, targetRot, 1.3f), 0.1f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 20)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing").WithPitchOffset(-0.5f));
			if (timer > 30 && timer < 80)
			{
				isAttacking = true;
				float timeValue = timer - 30;
				timeValue = timeValue / 30f;
				Projectile.rotation += Projectile.spriteDirection * 0.095f * timeValue * timeValue;
				mainVec = Vector2Elipse(140 + timeValue * 0.7f, Projectile.rotation, 1.3f,- 0.4f * player.direction);
			}
			if (timer > 80 + 20 * timeMul)
				NextAttackType();
			float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
			player.fullRotation = BodyRotation;
			player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
			player.legRotation = -BodyRotation;
			player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
			Tplayer.HeadRotation = -BodyRotation;
		}
		if (attackType == 2)
		{
			float timeValue = timer - 30;
			float BodyRotation = 0;
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.7f;
				mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 100, 0.15f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
				BodyRotation = (float)timeValue * 0.012f * player.direction * player.gravDir;
			}
			if (timer > 30 && timer < 50)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.14f;
				mainVec = Projectile.rotation.ToRotationVector2() * 130;

				BodyRotation = (float)timeValue * 0.024f * player.direction * player.gravDir;

			}
			if (timer < 50)
			{
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (timer == 20)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing").WithPitchOffset(-0.2f));
			if (timer > 50 + 25 * timeMul)
				NextAttackType();
		}
		if (attackType == 3)
		{
			float timeValue = timer - 70;
			float BodyRotation = 0;
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = MathHelper.PiOver2 - player.direction * 0.57f;
				mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 100, 0.15f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
				BodyRotation = (float)timeValue * 0.012f * player.direction * player.gravDir;
			}
			if (timer > 30 && timer < 70)
			{
				isAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.14f * timeValue * timeValue / 800f;
				mainVec = Projectile.rotation.ToRotationVector2() * 130;

				BodyRotation = -(float)timeValue * 0.024f * player.direction * player.gravDir;

			}
			if (timer < 70)
			{
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (timer == 20)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing").WithPitchOffset(-0.2f));
			if (timer > 70 + 25 * timeMul)
				NextAttackType();
		}
		if (attackType == 4)
		{
			float timeValue = (timer - 30) / 40f;
			float BodyRotation = 0;
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = - MathHelper.PiOver2 - player.direction * 1.6f;
				mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 120, 0.15f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer > 30 && timer < 80)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.17f * timeValue * timeValue * timeValue;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(130, Projectile.rotation, 0f), 0.4f);

			}
			if (timer < 80)
			{
				BodyRotation = (float)Math.Sin((timer - 10) / 50d * Math.PI) * -0.7f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (timer == 32)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing").WithPitchOffset(-0.6f));
			if (timer >80 + 25 * timeMul)
				NextAttackType();
		}
	}
}