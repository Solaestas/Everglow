using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.Weapons;
using Everglow.SpellAndSkull.Dusts;
using Terraria.Audio;

namespace Everglow.SpellAndSkull.Projectiles.WaterBolt;

public class NewWaterBolt : TrailingProjectile
{
	public override void SetDefaults()
	{
		base.SetDefaults();
	}
	public override void SetDef()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 1000;
		Projectile.alpha = 0;
		Projectile.penetrate = 1;
		Projectile.scale = 1f;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;

		TrailColor = new Color(0, 0.4f, 1, 0f);
		TrailWidth = 30f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_5.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_5_black.Value;
	}
	public override void AI()
	{
		Projectile.velocity *= 0.9993f;
		Projectile.velocity.Y += 0.02f;

		if (Main.rand.NextBool(4))
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(-15f, -7.5f)).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
			float mulScale = Main.rand.NextFloat(2f, 8f);
			var blood = new WaterBoltDrop
			{
				velocity = afterVelocity / mulScale,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(32, 64),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) }
			};
			Ins.VFXManager.Add(blood);
		}

		base.AI();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrailDark();
		DrawTrail();
		if (TimeTokill <= 0)
		{
			DrawSelf();
			var bars = new List<Vertex2D>();
			for (int i = 0; i < 4; ++i)
			{
				float factor = i / 3f;
				float timeValue = -(float)Main.time * 0.06f;

				Vector2 normalizedVel = Vector2.Normalize(Projectile.velocity);
				Vector2 drawPos = Projectile.Center + normalizedVel * 20f * (i - 2.4f);
				Color drawC = new Color(0f, 0.5f, 1f, 0);
				bars.Add(new Vertex2D(drawPos + normalizedVel.RotatedBy(MathHelper.PiOver2) * 40, drawC * (i / 3f), new Vector3(timeValue, 1, 1 - factor)));
				bars.Add(new Vertex2D(drawPos - normalizedVel.RotatedBy(MathHelper.PiOver2) * 40, drawC * (i / 3f), new Vector3(timeValue, 0, 1 - factor)));
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Effect effect = TrailShader;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.CurrentTechnique.Passes[0].Apply();
			Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
			Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			if (bars.Count > 3)
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		return false;
	}
	public override void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

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
			float width = TrailWidthFunction(factor);
			float timeValue = -(float)Main.time * 0.06f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor((drawPos / 16f).ToPoint());
				drawC.R = (byte)(lightC.R * drawC.R / 255f);
				drawC.G = (byte)(lightC.G * drawC.G / 255f);
				drawC.B = (byte)(lightC.B * drawC.B / 255f);
			}
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void DrawTrailDark()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = SmoothTrail.Count - 1; i > 0; --i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = -(float)Main.time * 0.06f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = Color.White;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTextureBlack;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		float width = TrailWidth;
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			if (SmoothTrail[i] == Vector2.Zero)
				break;
			var normalDir = SmoothTrail[i - 1] - SmoothTrail[i];
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float widthZ = TrailWidthFunction(factor);
			var c0 = new Color(1f - (normalDir.X + 5f) / 10f, 1f - (normalDir.Y + 5f) / 10f, 0.1f, 1);


			float x0 = factor * 1.3f + (float)(Main.time * 0.03f);
			Vector2 drawPos = SmoothTrail[i] - Main.screenPosition + halfSize;

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * width, c0, new Vector3(x0, 1, widthZ)));
			bars.Add(new Vertex2D(drawPos, c0, new Vector3(x0, 0.5f, widthZ)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * width, c0, new Vector3(x0, 1, widthZ)));
			bars2.Add(new Vertex2D(drawPos, c0, new Vector3(x0, 0.5f, widthZ)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * width, c0, new Vector3(x0, 1, widthZ)));
			bars3.Add(new Vertex2D(drawPos, c0, new Vector3(x0, 0.5f, widthZ)));
		}
		Texture2D warpTex = Commons.ModAsset.Trail_1.Value;
		if (bars.Count > 3)
			spriteBatch.Draw(warpTex, bars, PrimitiveType.TriangleStrip);
		if (bars2.Count > 3)
			spriteBatch.Draw(warpTex, bars2, PrimitiveType.TriangleStrip);
		if (bars3.Count > 3)
			spriteBatch.Draw(warpTex, bars3, PrimitiveType.TriangleStrip);
	}
	public override void KillMainStructure()
	{
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RipplingWave>(), 0, 0, Projectile.owner, 10f, 3f);
		switch (Main.rand.Next(2))
		{
			case 0:
				SoundEngine.PlaySound(new SoundStyle("Everglow/SpellAndSkull/Sounds/WaterBolt1"), Projectile.Center);
				break;

			case 1:
				SoundEngine.PlaySound(new SoundStyle("Everglow/SpellAndSkull/Sounds/WaterBolt2"), Projectile.Center);
				break;
		}

		for (int j = 0; j < 10; j++)
		{
			Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale;
			int dust0 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<ShatterDrop_1>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * Projectile.scale * 0.4f * 5);
			Main.dust[dust0].noGravity = true;
		}
		for (int j = 0; j < 20; j++)
		{
			Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale;
			int dust1 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<ShatterDrop_0>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(3.7f, 5.1f) * 5);
			Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / 5);
			Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
		}
		foreach (NPC target in Main.npc)
		{
			float Dis = (target.Center - Projectile.Center).Length();

			if (Dis < 250)
			{
				if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
				{
					NPC.HitModifiers npcHitM = new NPC.HitModifiers();
					NPC.HitInfo hit = npcHitM.ToHitInfo(Projectile.damage / (Dis + 35f) * 35f, Main.rand.NextFloat(100f) < Main.player[Projectile.owner].GetTotalCritChance(Projectile.DamageType), 0.2f);
					target.StrikeNPC(hit, true, true);
					NetMessage.SendStrikeNPC(target, hit);
				}
			}
		}
		base.KillMainStructure();
	}
}