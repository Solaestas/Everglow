using Everglow.Commons.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Melee.EvilHalbertBarnacle;

public class EvilHalbertBarnacle_proj_Thrust : ModProjectile
{
	public override string Texture => ModAsset.EvilHalbertBarnacle_proj_Mod;

	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeProjectiles;

	public float HoldoutRangeMin => -24f;

	public float HoldoutRangeMax => 162f;

	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
	}

	public override bool PreAI()
	{
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 * 3;
		if (Projectile.velocity.X > 0)
		{
			Projectile.spriteDirection = 1;
		}
		else
		{
			Projectile.spriteDirection = -1;
			Projectile.rotation -= MathHelper.PiOver2;
		}
		Player player = Main.player[Projectile.owner];
		int duration = (int)(14 / player.meleeSpeed);

		player.heldProj = Projectile.whoAmI;

		if (Projectile.timeLeft > duration)
		{
			Projectile.timeLeft = duration;
		}

		Projectile.velocity = Vector2.Normalize(Projectile.velocity);

		float halfDuration = duration * 0.5f;
		float progress;

		if (Projectile.timeLeft < halfDuration)
		{
			progress = Projectile.timeLeft / halfDuration;
		}
		else
		{
			progress = (duration - Projectile.timeLeft) / halfDuration;
		}

		Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
		return false;
	}

	public Vector2 oldPos;

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Vector2 velNor2 = Projectile.velocity.NormalizeSafe() * tex.Width / MathF.Sqrt(2f);
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition - velNor2, null, lightColor, Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		Vector2 drawCenter = Projectile.Center;
		float timeValue = 1f;
		if (Projectile.timeLeft < 4)
		{
			timeValue = Projectile.timeLeft / 4f;
		}
		Vector2 vel = Vector2.Normalize(Projectile.velocity);
		Vector2 width = vel.RotatedBy(MathHelper.PiOver2) * 60 * timeValue;
		float timeEffectValue = (float)(Main.time * 0.06f) + Projectile.whoAmI * 0.27f;

		Player player = Main.player[Projectile.owner];
		int duration = (int)(14 / player.meleeSpeed);
		float progress2;
		float halfDuration = duration * 0.5f;
		if (Projectile.timeLeft < halfDuration)
		{
			progress2 = 2 - Projectile.timeLeft / halfDuration;
			drawCenter = player.MountedCenter + Vector2.Lerp(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress2 * 0.5f + 1.5f * 0.5f);
		}
		else
		{
			progress2 = (duration - Projectile.timeLeft) / halfDuration;
		}
		float unitDistance = (HoldoutRangeMax - HoldoutRangeMin) / 19f * progress2 * 1.4f;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int x = 0; x < 20; x++)
		{
			Vector2 drawPos = drawCenter - vel * 7 * x;
			Color drawColor = Lighting.GetColor(drawPos.ToTileCoordinates());
			drawColor.A = 0;
			float mulWidth = MathF.Sin(x / 23f * MathHelper.PiOver2);
			float value = 19 - x;
			if (value < 5)
			{
				value *= 0.2f;
			}
			int fadeDis = 5;
			if(x > Projectile.timeLeft * 2 - fadeDis)
			{
				value *= Math.Max(0, (Projectile.timeLeft * 2 - x) / (float)fadeDis);
			}
			drawColor *= value;
			bars.Add(drawCenter - vel * unitDistance * x + width, drawColor, new Vector3(-x / 20f - timeEffectValue, 1, mulWidth));
			bars.Add(drawCenter - vel * unitDistance * x - width, drawColor, new Vector3(-x / 20f - timeEffectValue, 0, mulWidth));
		}

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
		Effect effect = Commons.ModAsset.StabSwordEffect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uProcession"].SetValue(0.5f);
		effect.CurrentTechnique.Passes[0].Apply();

		Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_8.Value;
		Main.graphics.graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}