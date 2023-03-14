using Everglow.Myth;
using Everglow.Myth.Common;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.Bosses.Acytaea.Projectiles
{
	public class BloodBlade2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("BloodBlade");
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1080;
			Projectile.penetrate = 1;
			Projectile.scale = 1;
			Projectile.alpha = 255;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color((255 - Projectile.alpha) / 255f, (255 - Projectile.alpha) / 255f, (255 - Projectile.alpha) / 255f, 0));
		}
		private Vector2 v0 = new Vector2(0);
		public override void OnSpawn(IEntitySource source)
		{
			v0 = Main.player[Projectile.owner].Center;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.12f / 15f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0f / 255f);
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[0] == 0)
			{
				Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
				Projectile.velocity *= 1 + 0.5f / Projectile.velocity.Length();

				if (Projectile.penetrate <= 0)
					Projectile.Kill();

				int index = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1f);
				Main.dust[index].noGravity = false;
				Main.dust[index].velocity *= 0;
				index = Dust.NewDust(Projectile.Center - new Vector2(4, 4) - new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1f);
				Main.dust[index].noGravity = false;
				Main.dust[index].velocity *= 0;
				index = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1.5f);
				Main.dust[index].velocity *= 0;
				index = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1.5f);
				Main.dust[index].velocity *= 0.2f;

				if (Collision.CanHit(Projectile.Center - Vector2.Normalize(Projectile.velocity) * 84, 1, 1, Main.player[Player.FindClosest(Projectile.Center, 60, 60)].Center, 1, 1))
					Projectile.tileCollide = true;
				if ((Projectile.Center - v0).Length() < 20)
					Projectile.Kill();
			}
			if (Projectile.ai[0] == 1)
			{
				Vector2 v0 = player.Center - Projectile.Center;
				Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI / 4d) * 0.1f + Projectile.rotation * 0.9f;
				if (Projectile.alpha > 0)
					Projectile.alpha -= 5;
				else
				{
					Projectile.alpha = 0;
				}
				int num = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1f);
				Main.dust[num].noGravity = false;
				Main.dust[num].velocity *= 0;
				int num20 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) - new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1f);
				Main.dust[num20].noGravity = false;
				Main.dust[num20].velocity *= 0;
				int num21 = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1.5f);
				Main.dust[num21].velocity *= 0;
				int num22 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1.5f);
				Main.dust[num22].velocity *= 0.2f;
			}
		}
		public override void Kill(int timeLeft)
		{
			ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
			mplayer.FlyCamPosition = new Vector2(0, 12).RotatedByRandom(6.283);
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
			for (int j = 0; j < 6; j++)
			{
				Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
				Projectile.NewProjectile(null, Projectile.Center + v * 2f, v, ModContent.ProjectileType<BrokenAcytaea2>(), 0, 1, Main.myPlayer);
			}
			for (int j = 0; j < 30; j++)
			{
				Vector2 v0 = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(Math.PI * 2);
				Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), v0.X, v0.Y, 0, default, 1.5f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			var texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(61, 61), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			var bars = new List<Vertex2D>();
			Effect ef = MythContent.QuickEffect("Effects/Trail");
			int width = 40;
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;

				var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

				var factor = i / (float)Projectile.oldPos.Length;
				var color = Color.Lerp(Color.White, new Color(255 - Projectile.alpha, 0, 0), factor);
				var w = MathHelper.Lerp(1f, 0.05f, factor);

				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(30, 30), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(30, 30), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
			}
			var triangleList = new List<Vertex2D>();
			if (bars.Count > 2)
			{
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
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
				ef.Parameters["uTransform"].SetValue(model * projection);
				ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
				Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapRed").Value;
				Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
				Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTrace").Value;
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
				Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
				Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
				ef.CurrentTechnique.Passes[0].Apply();
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
				Main.graphics.GraphicsDevice.RasterizerState = originalState;
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
}