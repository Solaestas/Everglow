using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class SquamousRockSpike : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 40;
		Projectile.height = 40;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 6;
		Projectile.timeLeft = 3600;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
	}
	internal int Target = -1;
	internal int TimeTokill = -1;
	public override void OnSpawn(IEntitySource source)
	{
		Target = (int)(Projectile.ai[0]);
	}
	public override void AI()
	{
		if(Target == -1)
		{
			Projectile.Kill();
			return;
		}
		Player player = Main.player[Target];
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		if (TimeTokill <= 15 && TimeTokill > 0)
			Projectile.velocity = Projectile.oldVelocity;
		TimeTokill--;
		if (TimeTokill < 0)
		{
			if(Projectile.timeLeft >= 3520)
			{
				Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
				Projectile.velocity = Projectile.velocity * 0.3f + Utils.SafeNormalize((player.Center - Projectile.Center),Vector2.zeroVector) * 0.4f;
			}
			else
			{
				if (Projectile.timeLeft < 3500)
				{
					if (Projectile.velocity.Length() < 10f)
					{
						Projectile.velocity *= 10f / Projectile.velocity.Length();
					}
				}
			}
		}
		else
		{
			if (TimeTokill < 10)
			{
				Projectile.damage = 0;
				Projectile.friendly = false;
			}
			Projectile.velocity *= 0f;
		}
	}
	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(40f, 55f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		AmmoHit();
		return false;
	}
	public void AmmoHit()
	{
		TimeTokill = 240;
		Projectile.velocity = Projectile.oldVelocity;
		for (int x = 0; x < 16; x++)
		{
			Dust.NewDust(Projectile.Center - Projectile.velocity * 2 - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<SquamousShellStone>(), 0f, 0f, 0, default, 0.7f);
		}
		GenerateSmog(8);
		Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<SquamousRockExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 10);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		if (TimeTokill > 0)
			return false;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect dissolve = Commons.ModAsset.DissolveWithLight.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		float dissolveDuration = (3600 - Projectile.timeLeft) / 80f * 1.2f - 0.2f;
		if (Projectile.timeLeft < 3520)
		{
			dissolveDuration = 1;
		}
		dissolve.Parameters["uTransform"].SetValue(model * projection);
		dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_cell.Value);
		dissolve.Parameters["duration"].SetValue(dissolveDuration);
		dissolve.Parameters["uLightColor"].SetValue(lightColor.ToVector4());
		dissolve.Parameters["uDissolveColor"].SetValue(new Vector4(0.2f, 0.6f, 0.7f, 1f));
		dissolve.Parameters["uNoiseSize"].SetValue(2f);
		dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(Projectile.ai[1], Projectile.ai[2]));
		dissolve.CurrentTechnique.Passes[0].Apply();

		var TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Main.spriteBatch.Draw(TexMain, Projectile.Center, null, lightColor, Projectile.rotation, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		DrawTrail_dark(lightColor);
		DrawTrail(lightColor);
	}
	public void DrawTrail(Color light)
	{
		float dissolveDuration = (3600 - Projectile.timeLeft) / 80f - 0.7f;
		if (Projectile.timeLeft < 3450)
		{
			dissolveDuration = 1;
		}
		float drawC = 0.4f * dissolveDuration;

		var bars = new List<Vertex2D>();
		int trueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				if (i == 1)
					return;
				break;
			}

			trueL = i;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			float width = 27;
			if (Projectile.timeLeft <= 30)
				width *= Projectile.timeLeft / 30f;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.Zero);

			var factor = i / (float)trueL;
			var color = Color.Lerp(new Color(drawC * light.R / 255f * 0.3f, drawC * light.G / 255f * 0.2f, drawC * light.B / 255f * 0.1f, 0), new Color(0, 0, 0, 0), factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(Projectile.width / 2f) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(Projectile.width / 2f) - Main.screenPosition, color, new Vector3(1, 1, 0)));

		}
		if (bars.Count > 2)
		{
			Texture2D t = Commons.ModAsset.Trail_4.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}
	public void DrawTrail_dark(Color light)
	{
		float dissolveDuration = (3600 - Projectile.timeLeft) / 80f - 0.7f;
		if (Projectile.timeLeft < 3450)
		{
			dissolveDuration = 1;
		}
		float drawC = 0.4f * dissolveDuration;
		var bars = new List<Vertex2D>();
		int trueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				if (i == 1)
					return;
				break;
			}
			trueL = i;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			float width = 27;
			if (Projectile.timeLeft <= 30)
				width *= Projectile.timeLeft / 30f;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.Zero);

			var factor = i / (float)trueL;
			var color = Color.Lerp(new Color(drawC * light.R / 255f, drawC * light.G / 255f, drawC * light.B / 255f, drawC), new Color(0, 0, 0, 0), factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(Projectile.width / 2f) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(Projectile.width / 2f) - Main.screenPosition, color, new Vector3(1, 1, 0)));
		}
		if (bars.Count > 2)
		{
			Texture2D t = Commons.ModAsset.Trail_4_black.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}
}