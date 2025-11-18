using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Commons.Vertex;
using Terraria.Audio;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class VertebralSpur_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			StabColor = new Color(105, 92, 76);
			StabShade = 1f;
			StabDistance = 0.70f;
			StabEffectWidth = 0.4f;
		}

		public override void DrawEffect(Color lightColor)
		{
			Vector2 normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 50 * ToKill / 120f * StabEffectWidth;
			Vector2 start = StartCenter;
			Vector2 end = Projectile.Center + Projectile.velocity * 100 * StabDistance;
			if (EndPos != Vector2.Zero)
			{
				end = EndPos;
			}
			float value = (Projectile.timeLeft + ToKill) / 135f;
			Vector2 middle = Vector2.Lerp(end, start, MathF.Sqrt(value) * 0.5f);
			float time = (float)(Main.time * 0.03);
			float dark = MathF.Sin(value * MathF.PI) * 4;
			List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized, new Color(120, 120, 120, 120) * 0.9f * StabShade, new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized, new Color(120, 120, 120, 120) * 0.9f * StabShade, new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized, Color.White * 0.8f * dark * StabShade, new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized, Color.White * 0.8f * dark * StabShade, new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized, Color.White * 0.9f * dark * StabShade, new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized, Color.White * 0.9f * dark * StabShade, new Vector3(0f + time, 1, 1)),
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = ModAsset.VertebralSpurEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(0.5f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_black.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
			Color alphaColor = StabColor;
			alphaColor.A = 0;
			alphaColor.R = (byte)(alphaColor.R * lightColor.R / 255f);
			alphaColor.G = (byte)(alphaColor.G * lightColor.G / 255f);
			alphaColor.B = (byte)(alphaColor.B * lightColor.B / 255f);

			normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 72 * ToKill / 120f * StabEffectWidth;
			bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized, new Color(0, 0, 0, 0), new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized, new Color(0, 0, 0, 0), new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized, alphaColor, new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized, alphaColor, new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized, alphaColor * 1.2f, new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized, alphaColor * 1.2f, new Vector3(0f + time, 1, 1)),
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = ModAsset.VertebralSpurEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(0.5f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_6.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}

		public override void AI()
		{
			base.AI();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Player player = Main.player[Projectile.owner];
			float distanceValue = (player.Center - target.Center).Length();
			modifiers.FinalDamage += Math.Max(0, 180 - distanceValue) / 180f;
		}

		public override void HitTile()
		{
			SoundEngine.PlaySound(SoundID.Dig.WithPitchOffset(Main.rand.NextFloat(0.6f, 1f)), Projectile.Center);
		}
	}
}