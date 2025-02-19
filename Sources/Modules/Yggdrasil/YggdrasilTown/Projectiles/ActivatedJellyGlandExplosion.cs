using Everglow.Commons.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ActivatedJellyGlandExplosion : ModProjectile
{
	private const int Duration = 30;

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.magic = true;
		Projectile.timeLeft = Duration;
		Projectile.penetrate = -1;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Vector2 drawCenter = Projectile.Center - Main.screenPosition;
		var bars = new List<Vertex2D>();
		var progress = (Duration - Projectile.timeLeft) / (float)Duration;

		float outerScale;
		const float expandDuration = 0.3f;
		if (progress < expandDuration)
		{
			float x = progress * (1 / expandDuration);
			outerScale = x * x * x * (x * (x * 6 - 15) + 10);
		}
		else if (progress >= expandDuration && progress <= (1 - expandDuration))
		{
			outerScale = 1f;
		}
		else
		{
			float x = (1 - progress) * (1 / expandDuration);
			outerScale = x * x * x * (x * (x * 6 - 15) + 10);
		}
		float innerScale;
		innerScale = MathF.Sin(progress * MathHelper.Pi);

		for (int i = 0; i <= 180; i++)
		{
			Color drawColor = new Color(0.254f, 0.41f, 0.88f, 0.3f);
			float rotation = i / 180f * MathHelper.TwoPi + (float)Main.timeForVisualEffects * 0.003f;
			bars.Add(drawCenter + new Vector2(0, 20 * outerScale).RotatedBy(rotation), drawColor * 0.2f, new Vector3(i / 90f, 1, 0));
			bars.Add(drawCenter + new Vector2(0, 0).RotatedBy(rotation), drawColor, new Vector3(i / 90f, 0, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_burn.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		bars = [];
		for (int i = 0; i <= 180; i++)
		{
			Color drawColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			float rotation = i / 180f * MathHelper.TwoPi + (float)Main.timeForVisualEffects * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 0).RotatedBy(rotation), drawColor, new Vector3(i / 90f, 0, 0));
			bars.Add(drawCenter + new Vector2(0, 8 * innerScale).RotatedBy(rotation), drawColor, new Vector3(i / 90f, 1, 0));
		}

		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_4.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		return false;
	}
}