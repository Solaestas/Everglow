using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Everglow.Yggdrasil.CorruptWormHive.Buffs;
using Everglow.Yggdrasil.Common;
using Everglow.Commons.VFX;
using Everglow.Commons.Utilities;
using Everglow.Commons.MEAC;
using Everglow.Commons.Vertex;
using Everglow.Yggdrasil.CorruptWormHive.VFXs;

namespace Everglow.Yggdrasil.CorruptWormHive.Projectiles;

public class TrueDeathSickle : MeleeProj, IOcclusionProjectile, IWarpProjectile, IBloomProjectile
{
	public override void SetDef()
	{
		maxAttackType = 2;
		trailLength = 200;
		longHandle = true;
		shadertype = "Trail";
		AutoEnd = false;
		CanLongLeftClick = false;
		CanIgnoreTile = true;
		selfWarp = true;
	}
	public override string TrailShapeTex()
	{
		return "Everglow/Yggdrasil/CorruptWormHive/Projectiles/FlameLine";
	}
	public override string TrailColorTex()
	{
		return "Everglow/Yggdrasil/CorruptWormHive/Projectiles/DeathSickle_Color";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.3f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.AlphaBlend;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
	{
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		//DrawEffect(Ins.Batch);
		Ins.Batch.End();
		Player player = Main.player[Projectile.owner];
		Texture2D tex = ModAsset.TrueDeathSickle_Handle.Value;
		if (attackType == 1 && timer > 18)
			tex = ModAsset.TrueDeathSickle_Handle_Filp.Value;
		if (attackType == 2)
		{
			tex = ModAsset.Projectiles_TrueDeathSickle.Value;
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + (float)(Math.PI / 4d) * player.direction - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.22f, player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			return;
		}
		float texWidth = 208;
		float texHeight = 188;
		float Size = 0.864f;
		double baseRotation = 0.7854;

		float exScale = 2f;
		var origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

		Vector2 Zoom = new Vector2(exScale * drawScaleFactor * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

		double ProjRotation = MainVec_WithoutGravDir.ToRotation() + Math.PI / 4;

		float QuarterSqrtTwo = 0.35355f;

		Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
		Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
		Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

		Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * Size;
		Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * Size;

		Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
		ITexNormal.X /= tex.Width;
		ITexNormal.Y /= tex.Height;
		Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
		JTexNormal.X /= tex.Width;
		JTexNormal.Y /= tex.Height;

		Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = Vector2.Normalize(INormal) * origin.Y - Vector2.Normalize(JNormal) * origin.X;
		Vector2 TopRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) + Vector2.Normalize(INormal) * origin.Y;
		Vector2 BottomLeft = -Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y) - Vector2.Normalize(JNormal) * origin.X;
		Vector2 BottomRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) - Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y);


		Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
		Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
		Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
		Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

		if (Player.direction * Player.gravDir == -1)
		{
			sourceTopLeft = sourceBottomLeft;
			sourceTopRight = sourceBottomRight;
			sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
			sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
		}

		var vertex2Ds = new List<Vertex2D>
		{
			new Vertex2D(drawCenter2 + TopLeft, lightColor, new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
			new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
			new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

			new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
			new Vertex2D(drawCenter + BottomRight, lightColor, new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
			new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
		};


		Main.graphics.GraphicsDevice.Textures[0] = tex;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
	}
	private Vector2 SelfRotated(ref Vector2 vector2, float angle)
	{
		return vector2.RotatedBy(angle);
	}
	public void DrawOcclusion(VFXBatch spriteBatch)
	{
		var lightColor = new Color(0, 0, 0, 255);
		Player player = Main.player[Projectile.owner];
		Texture2D tex = ModAsset.TrueDeathSickle_Shade_Handle.Value;
		if (attackType == 1)
			tex = ModAsset.TrueDeathSickle_Shade_Handle_Flip.Value;
		if (attackType == 2)
		{
			tex = ModAsset.TrueDeathSickle_Shade.Value;
			Vector2 drawCenter3 = Projectile.Center - Main.screenPosition;
			var origin2 = new Vector2(tex.Width, tex.Height);
			if (player.direction == -1)
				origin2 = new Vector2(0, tex.Height);
			var sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			float rotation2 = Projectile.rotation + (float)(Math.PI / 4d) * player.direction - 0.25f + MathF.PI * 0.5f;
			Vector2 TopLeft2 = sourceRectangle.TopLeft() - origin2;
			Vector2 TopRight2 = sourceRectangle.TopRight() - origin2;
			Vector2 BottomLeft2 = sourceRectangle.BottomLeft() - origin2;
			Vector2 BottomRight2 = sourceRectangle.BottomRight() - origin2;
			TopLeft2 = TopLeft2.RotatedBy(rotation2);
			TopRight2 = TopRight2.RotatedBy(rotation2);
			BottomLeft2 = BottomLeft2.RotatedBy(rotation2);
			BottomRight2 = BottomRight2.RotatedBy(rotation2);

			float scale2 = 1.22f;

			var TL = new Vector3(1, 0, 0);
			var TR = new Vector3(1, 1, 0);
			var BL = new Vector3(0, 0, 0);
			var BR = new Vector3(0, 1, 0);
			if (player.direction == -1)
			{
				(TL, TR) = (TR, TL);
				(BL, BR) = (BR, BL);
			}
			var vertex2Ds2 = new List<Vertex2D>
			{
				new Vertex2D(drawCenter3 + TopLeft2 * scale2, lightColor, TL),
				new Vertex2D(drawCenter3 + TopRight2 * scale2, lightColor, TR),

				new Vertex2D(drawCenter3 + BottomLeft2 * scale2, lightColor, BL),
				new Vertex2D(drawCenter3 + BottomRight2 * scale2, lightColor, BR)
			};
			if (vertex2Ds2.Count == 4)
				spriteBatch.Draw(tex, vertex2Ds2, PrimitiveType.TriangleStrip);
			return;
		}
		float texWidth = 208;
		float texHeight = 188;
		float Size = 0.864f;
		double baseRotation = 0.7854;

		float exScale = 2f;
		var origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

		Vector2 Zoom = new Vector2(exScale * drawScaleFactor * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

		double ProjRotation = MainVec_WithoutGravDir.ToRotation() + Math.PI / 4;

		float QuarterSqrtTwo = 0.35355f;

		Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
		Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
		Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

		Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * Size;
		Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * Size;

		Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
		ITexNormal.X /= tex.Width;
		ITexNormal.Y /= tex.Height;
		Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
		JTexNormal.X /= tex.Width;
		JTexNormal.Y /= tex.Height;

		Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = Vector2.Normalize(INormal) * origin.Y - Vector2.Normalize(JNormal) * origin.X;
		Vector2 TopRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) + Vector2.Normalize(INormal) * origin.Y;
		Vector2 BottomLeft = -Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y) - Vector2.Normalize(JNormal) * origin.X;
		Vector2 BottomRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) - Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y);


		Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
		Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
		Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
		Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

		if (Player.direction * Player.gravDir == -1)
		{
			sourceTopLeft = sourceBottomLeft;
			sourceTopRight = sourceBottomRight;
			sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
			sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
		}
		var vertex2Ds = new List<Vertex2D>
		{
				new Vertex2D(drawCenter2 + TopLeft, lightColor, new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
				new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

				new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
				new Vertex2D(drawCenter + BottomRight, lightColor, new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0))
		};

		if (vertex2Ds.Count == 4)
			spriteBatch.Draw(tex, vertex2Ds, PrimitiveType.TriangleStrip);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		Player player = Main.player[Projectile.owner];


		ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
		float ShakeStrength = 3f;
		Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 24f * ShakeStrength, 100)).RotatedByRandom(6.283);
		modifiers.Knockback *= 5f;
		int HitType = ModContent.ProjectileType<TrueDeathSickleHit>();
		if (attackType == 2)
		{
			modifiers.FinalDamage *= 2.3f;
			modifiers.Knockback *= 2.3f;
			GenerateVFXFromTarget(target, 36, 2.6f);
			if (player.ownedProjectileCounts[HitType] < 5)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.One, HitType, 0, 0, Projectile.owner, 30, Projectile.rotation);
		}
		else
		{
			GenerateVFXFromTarget(target, 18, 2f);
			if (player.ownedProjectileCounts[HitType] < 5)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.One, HitType, 0, 0, Projectile.owner, 15, Projectile.rotation);
		}
	}
	public override void Attack()
	{
		useTrail = true;
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		if (attackType == 0)
		{

			if (timer < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 + Player.direction * -1.6f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(250, targetRot, 1.2f), 0.15f);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 20)
			{
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleeSwing"));

			}

			if (timer == 50)
			{
				ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
				Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
			}

			if (timer > 20 && timer < 52)
			{
				GenerateVFX(4);
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
				isAttacking = true;
				Projectile.rotation += player.direction * 0.1f * timer / 20f;
				mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);

				float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (timer >= 52)
				Projectile.extraUpdates = 16;
			if (timer > 320)
			{
				Projectile.extraUpdates = 2;
				NextAttackType();
			}
		}

		if (attackType == 1)
		{
			if (timer < 50)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 + Player.direction * 1.2f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.09f);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 50)
			{
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleeSwing"));
			}

			if (timer == 70)
			{
				ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
				Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
			}


			if (timer > 50 && timer < 75)
			{
				GenerateVFX(4);
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
				isAttacking = true;
				Projectile.rotation -= player.direction * 0.24f;
				mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);

				float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (timer >= 75)
				Projectile.extraUpdates = 16;
			if (timer > 400)
			{
				Projectile.extraUpdates = 2;
				NextAttackType();
			}
		}
		if (attackType == 2)
		{
			trailLength = 400;
			longHandle = false;
			drawScaleFactor = 1.6f;
			if (timer < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = 0;
				if (player.direction == 1)
					targetRot = -MathHelper.PiOver4 * 3;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(240, targetRot, 0.3f, 0.3f), 0.15f);
				mainVec += Projectile.DirectionFrom(Player.Center) * -3;
				Projectile.rotation = mainVec.ToRotation();
				disFromPlayer = -10;
			}
			if (timer == 30)
			{
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleePowerSwing"));
			}
			if (timer == 50)
			{
				ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
				Gsplayer.FlyCamPosition = new Vector2(0, 60).RotatedByRandom(6.283);
			}
			if (timer > 20 && timer < 55)
			{
				GenerateVFX(timer / 5);
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
				isAttacking = true;
				Projectile.rotation += player.direction * 0.162f * (float)(1 - Math.Cos((timer - 20) / 17.5d * Math.PI));
				mainVec = Vector2Elipse(240, Projectile.rotation, 0.3f, 0.3f, 10000);

				float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (timer >= 55)
				Projectile.extraUpdates = 6;
			if (timer > 380)
			{
				trailLength = 200;
				longHandle = true;
				drawScaleFactor = 1f;
				NextAttackType();
			}
		}
	}
	public void GenerateVFX(int Frequency)
	{
		Player player = Main.player[Projectile.owner];
		float dir = player.direction * ((attackType + 1) % 2 - 0.5f) * 2;
		float mulVelocity = 1f;
		mulVelocity *= Frequency / 5f;
		if (attackType == 2)
			mulVelocity = 2.4f;
		for (int g = 0; g < Frequency; g++)
		{
			var df = new DevilFlameDust
			{
				velocity = Vector2.Normalize(mainVec).RotatedBy(1.57 * dir) * 250f / mainVec.Length() * Main.rand.NextFloat(0.65f, 8.6f) * mulVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + mainVec * Main.rand.NextFloat(0.85f, 1.3f) + new Vector2(Main.rand.NextFloat(0.05f, 36f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(9, 24),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(0.01f, 0.1f) * dir, Main.rand.NextFloat(8f, 32f) }
			};
			Ins.VFXManager.Add(df);
		}
	}
	public void GenerateVFXFromTarget(NPC target, int Frequency, float mulVelocity = 1f)
	{
		for (int g = 0; g < Frequency; g++)
		{
			var df = new DevilFlameDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(4.5f, 9f)).RotatedByRandom(6.283) * mulVelocity,
				Active = true,
				Visible = true,
				position = target.Center,
				maxTime = Main.rand.Next(12, 30),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(8f, 32f) }
			};
			Ins.VFXManager.Add(df);
		}
	}
	public void DrawBloom()
	{
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		Effect MeleeTrail = ModAsset.FlameTrail.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		MeleeTrail.Parameters["uTime"].SetValue(timer2 * 0.007f);
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();
		DrawEffect(Ins.Batch);
		Ins.Batch.End();


		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		Effect effect = ModAsset.Null.Value;
		model = Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		DrawOcclusion(Ins.Batch);
		Ins.Batch.End();
	}
	public void DrawWarp(VFXBatch spriteBatch)
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
		float w = 1f;
		if (timer > 200)
			w = Math.Max(0, (380 - timer) / 180f);
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);

			float d = trail[i].ToRotation() + 3.14f + 1.57f;
			if (d > 6.28f)
				d -= 6.28f;
			float dir = d / MathHelper.TwoPi;

			float dir1 = dir;
			if (i > 0)
			{
				float d1 = trail[i - 1].ToRotation() + 3.14f + 1.57f;
				if (d1 > 6.28f)
					d1 -= 6.28f;
				dir1 = d1 / MathHelper.TwoPi;
			}

			if (dir - dir1 > 0.5)
			{
				var midValue = (1 - dir) / (1 - dir + dir1);
				var midPoint = midValue * trail[i] + (1 - midValue) * trail[i - 1];
				var oldFactor = (i - 1) / (length - 1f);
				var midFactor = midValue * factor + (1 - midValue) * oldFactor;
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
			}
			if (dir1 - dir > 0.5)
			{
				var midValue = (1 - dir1) / (1 - dir1 + dir);
				var midPoint = midValue * trail[i - 1] + (1 - midValue) * trail[i];
				var oldFactor = (i - 1) / (length - 1f);
				var midFactor = midValue * oldFactor + (1 - midValue) * factor;
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
			}
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, w, 0, 1), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, w, 0, 1), new Vector3(factor, 0, w)));
		}

		spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/MEAC/Images/Warp").Value, bars, PrimitiveType.TriangleStrip);

		DrawOcclusion(spriteBatch);
		return;
	}
	public override void DrawTrail(Color color)
	{

	}
	internal int timer2 = 0;
	public void DrawEffect(VFXBatch spriteBatch)
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
			if (attackType == 2)
				w *= 1.2f;
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.05f * Projectile.scale, Color.White, new Vector3(factor * 1, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale * 1.08f, Color.White, new Vector3(factor * 2, 0, w)));
		}
		spriteBatch.Draw(ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, bars, PrimitiveType.TriangleStrip);
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
		target.AddBuff(ModContent.BuffType<DeathFlame>(), 1800);
	}
}

