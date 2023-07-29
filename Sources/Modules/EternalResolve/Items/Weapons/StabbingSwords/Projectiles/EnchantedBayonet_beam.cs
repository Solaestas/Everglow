using Everglow.Commons.Vertex;
using Everglow.EternalResolve.VFXs;
using Terraria.Audio;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class EnchantedBayonet_beam : ModProjectile
	{
		public override string Texture => "Everglow/Commons/Weapons/StabbingSwords/StabbingProjectile";
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 3600;
		}
		public override void AI()
		{
			base.AI();
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 50;
			Vector2 start = Projectile.Center;
			Vector2 end = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 90f;
			float time = (float)(Main.time * 0.03);
			List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 255, 0),new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 255, 0),new Vector3(1 + time, 1, 0)),
				new Vertex2D(end + normalized, Color.Transparent,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized, Color.Transparent,new Vector3(0f + time, 1, 1))
			};

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			Effect effect = ModAsset.BeamFlow.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.CurrentTechnique.Passes[0].Apply();
			Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return base.OnTileCollide(oldVelocity);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
		}
		public override void Kill(int timeLeft)
		{
			SoundStyle ss = SoundID.NPCHit4;
			SoundEngine.PlaySound(ss.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), Projectile.Center);
			for (int g = 0; g < 20; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
				var spark = new Spark_EnchantedStabDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center,
					maxTime = Main.rand.Next(1, 25),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(0.1f, 17.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) }
				};
				Ins.VFXManager.Add(spark);
			}
		}
	}
}