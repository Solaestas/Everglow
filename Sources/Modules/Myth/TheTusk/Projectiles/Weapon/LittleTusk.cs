﻿using Everglow.Myth.Common;
using Everglow.Myth.TheTusk;
using Terraria;

namespace Everglow.Myth.TheTusk.Projectiles.Weapon;

class LittleTusk : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 14;
		Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 30;
		//Projectile.extraUpdates = 10;
		Projectile.tileCollide = true;
		Projectile.DamageType = DamageClass.Melee;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int y = 0; y < 12; y++)
		{
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.VampireHeal, 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.2f));
			Main.dust[num90].noGravity = true;
			Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
		}
		for (int y = 0; y < 16; y++)
		{
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.4f));
			Main.dust[num90].noGravity = false;
			Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.4f, 2.2f), 0).RotatedByRandom(Math.PI * 2d);
		}
	}

	public override void AI()
	{
		Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
		if (Projectile.oldPosition != Vector2.Zero && Projectile.timeLeft < 28)
		{
			for (int g = 0; g < Projectile.velocity.Length() * 2.5f; g++)
			{
				Vector2 a0 = new Vector2(Projectile.width, Projectile.height) / 2f;
				Vector2 v3 = Projectile.oldPosition + a0;
				Vector2 v4 = Vector2.Normalize(Projectile.velocity) * 0.6f;
				int num92 = Dust.NewDust(v3 + v4 * g - new Vector2(4, 4), 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 2f) * 0.4f);
				Main.dust[num92].noGravity = true;
				Main.dust[num92].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * 0.5f;
			}
		}
	}
	private Effect ef;
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		var bars = new List<Vertex2D>();
		ef = MythContent.QuickEffect("Effects/Trail");
		// 把所有的点都生成出来，按照顺序
		int width = 30;
		if (Projectile.timeLeft < 30)
			width = Projectile.timeLeft;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			//spriteBatch.Draw(Main.magicPixel, Projectile.oldPos[i] - Main.screenPosition,
			//    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);


			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)Projectile.oldPos.Length;
			var color = Color.Lerp(Color.White, Color.Red, factor);
			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(18, 18), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(18, 18), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
		}

		var triangleList = new List<Vertex2D>();

		if (bars.Count > 2)
		{

			// 按照顺序连接三角形
			triangleList.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
			triangleList.Add(bars[1]);
			triangleList.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				triangleList.Add(bars[i]);
				triangleList.Add(bars[i + 2]);
				triangleList.Add(bars[i + 1]);

				triangleList.Add(bars[i + 1]);
				triangleList.Add(bars[i + 2]);
				triangleList.Add(bars[i + 3]);
			}
			RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
			// 干掉注释掉就可以只显示三角形栅格
			//RasterizerState rasterizerState = new RasterizerState();
			//rasterizerState.CullMode = CullMode.None;
			//rasterizerState.FillMode = FillMode.WireFrame;
			//Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

			// 把变换和所需信息丢给shader
			ef.Parameters["uTransform"].SetValue(model * projection);
			ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
			Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapRedBeta").Value;
			Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/Lightline").Value;
			Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/FogTrace").Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
			//Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
			//Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
			//Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

			ef.CurrentTechnique.Passes[0].Apply();


			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

			Main.graphics.GraphicsDevice.RasterizerState = originalState;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		for (int y = 0; y < 12; y++)
		{
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.VampireHeal, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
			Main.dust[num90].noGravity = true;
			Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
		}
		for (int y = 0; y < 16; y++)
		{
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(0.6f, 1.4f));
			Main.dust[num90].noGravity = false;
			Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.4f, 2.2f), 0).RotatedByRandom(Math.PI * 2d);
		}
		Projectile.Kill();
		return true;
	}
}
