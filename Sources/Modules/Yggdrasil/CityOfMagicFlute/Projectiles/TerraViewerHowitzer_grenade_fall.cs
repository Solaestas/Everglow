using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.CityOfMagicFlute.Projectiles;

public class TerraViewerHowitzer_grenade_fall : TrailingProjectile
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 2400;
		Projectile.alpha = 0;
		Projectile.penetrate = 30;
		Projectile.scale = 1f;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = 10;
		Projectile.extraUpdates = 2;
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void AI()
	{
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (timeTokill >= 0 && timeTokill <= 2)
		{
			Projectile.Kill();
		}

		if (timeTokill <= 15 && timeTokill > 0)
		{
			Projectile.velocity = Projectile.oldVelocity;
		}

		timeTokill--;
		if (timeTokill < 0)
		{
			if (Projectile.timeLeft == 2310)
			{
				Projectile.friendly = true;
			}
		}
		else
		{
			if (timeTokill < 10)
			{
				Projectile.damage = 0;
				Projectile.friendly = false;
			}
			Projectile.velocity *= 0f;
		}
		if (Projectile.timeLeft < 2380)
		{
			Projectile.tileCollide = true;
		}
		Projectile.hide = true;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
	}

	public int timeTokill = -1;

	public override void KillMainStructure()
	{
		Projectile.velocity = Projectile.oldVelocity;
		Projectile.friendly = false;
		if (timeTokill < 0)
		{
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<TerraViewerHowitzer_grenade_fall_explosion>(), Projectile.damage / 3, Projectile.knockBack * 0.4f, Projectile.owner, MathF.Sqrt(Projectile.ai[0]) * 3);
		}
		timeTokill = 90;
	}

	public override void DrawTrail()
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		float k1 = 20f;
		float colorValue0 = (2400 - Projectile.timeLeft) / k1;

		if (Projectile.timeLeft <= 2400 - k1)
		{
			colorValue0 = 1;
		}

		int trueLength = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			trueLength++;
		}

		float velocityValue = 1f;
		colorValue0 *= velocityValue;
		var c0 = new Color(colorValue0, colorValue0 * colorValue0 * 0.6f, colorValue0 * colorValue0 * 0.1f, 0);
		var drawPos = Projectile.Hitbox.Size() * 0.5f - Main.screenPosition;
		Texture2D t = Commons.ModAsset.Trail_2_black_thick.Value;

		var barsDark = new List<Vertex2D>();
		for (int i = 1; i < trueLength; ++i)
		{
			float width = 20;
			if (Projectile.timeLeft <= 40)
			{
				width = Projectile.timeLeft * 0.9f;
			}

			if (i < 4)
			{
				width *= i / 4f;
			}

			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			var factor = i / (float)trueLength;
			var c1 = new Color(255, 255, 255, 255) * (1 - factor) * velocityValue;
			float x0 = factor * 1.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
			barsDark.Add(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + drawPos, c1, new Vector3(x0, 1, 0));
			barsDark.Add(Projectile.oldPos[i] + normalDir * width * (1 - factor) + drawPos, c1, new Vector3(x0, 0, 0));
		}

		t = Commons.ModAsset.Trail_2_black_thick.Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;
		if (barsDark.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsDark.ToArray(), 0, barsDark.Count - 2);
		}

		var bars = new List<Vertex2D>();
		for (int i = 1; i < trueLength; ++i)
		{
			float width = 24;
			if (Projectile.timeLeft <= 40)
			{
				width = Projectile.timeLeft * 0.9f;
			}

			if (i < 10)
			{
				width *= i / 10f;
			}

			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			var factor = i / (float)trueLength;
			var c1 = c0 * (1 - factor);
			float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
			bars.Add(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + drawPos, c1, new Vector3(x0, 1, 0));
			bars.Add(Projectile.oldPos[i] + normalDir * width * (1 - factor) + drawPos, c1, new Vector3(x0, 0, 0));
		}
		t = Commons.ModAsset.Trail_4.Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void DrawTrailDark()
	{
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		Texture2D star = ModAsset.TerraViewerHowitzer_grenade_fall.Value;
		if (timeTokill < 0)
		{
			float light = Projectile.ai[0] * Projectile.ai[0] / 30000f;
			Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition - Projectile.velocity, null, lightColor, Projectile.rotation, star.Size() / 2f, 1f, SpriteEffects.None, 0);
		}
		return false;
	}

	public new void DrawWarp(VFXBatch spriteBatch)
	{
		float width = 16;
		float velocityValue = Projectile.velocity.Length() / 30f;
		float colorValueG = velocityValue;
		int trueLength = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			trueLength++;
		}
		var bars = new List<Vertex2D>();
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			float MulColor = 1f;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			if (i == 1)
			{
				MulColor = 0f;
			}
			if (i >= 2)
			{
				var normalDirII = Projectile.oldPos[i - 2] - Projectile.oldPos[i - 1];
				normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
				if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
				{
					MulColor = 0f;
				}
			}
			if (i < Projectile.oldPos.Length - 1)
			{
				var normalDirII = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
				normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
				if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
				{
					MulColor = 0f;
				}
			}

			float colorValue0 = (float)Math.Atan2(normalDir.Y, normalDir.X);
			colorValue0 += 3.14f + 1.57f;
			if (colorValue0 > 6.28f)
			{
				colorValue0 -= 6.28f;
			}

			var c0 = new Color(colorValue0, 0.4f * colorValueG * MulColor, 0, 0);

			var factor = i / (float)trueLength;
			float x0 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x0 %= 1f;
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(x0, 1, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(x0, 0, 0)));
			var factorII = factor;
			factorII = (i + 1) / (float)trueLength;
			var x1 = factorII * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x1 %= 1f;
			if (x0 > x1)
			{
				float DeltaValue = 1 - x0;
				var factorIII = factorII * x0 + factor * DeltaValue;
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(1, 1, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(1, 0, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(0, 1, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(0, 0, 0)));
			}
		}
		Texture2D t = Commons.ModAsset.Trail_4.Value;

		if (bars.Count > 3)
		{
			spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
		}
	}
}