using Everglow.Commons.DataStructures;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class LanternFlowLine : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
		Projectile.penetrate = -1;
		Projectile.scale = 0;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
		ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 14400;
	}
	public override void AI()
	{
		if(Projectile.timeLeft > 500)
		{
			Projectile.scale += 0.01f;
		}
		if (Projectile.timeLeft < 50)
		{
			Projectile.scale -= 0.02f;
		}
		Vector2 addPos = new Vector2(Main.rand.NextFloat(0f, 90f), 0).RotateRandom(6.283);
		if(Projectile.timeLeft > 120 && Projectile.timeLeft < 600)
		{
			float mulScale = Main.rand.NextFloat(0.5f, 1.2f);
			var gore2 = new LanternFlow_lantern2
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(-1.2f, 1.2f), -5 * mulScale),
				scale = mulScale,
				position = Projectile.Center + new Vector2(0, Main.rand.NextFloat(-1400f, -600f)),
			};
			Ins.VFXManager.Add(gore2);
		}
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return targetHitbox.Left < projHitbox.Right && targetHitbox.Right > projHitbox.Left && Math.Abs(targetHitbox.Center.Y - projHitbox.Center.Y) < 2000 && Projectile.timeLeft < 500 && Projectile.timeLeft > 50;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		float timeValue = (float)(-Main.time * 0.02f) + Projectile.whoAmI / 7f;
		var bars = new List<Vertex2D>();
		Vector2 drawPoint = new Vector2(0, -2000) + Projectile.Center;
		float mulColor = 1;
		if(Projectile.timeLeft > 500)
		{
			mulColor = (600 - Projectile.timeLeft) / 100f;
		}
		for(int y = 0;y < 600;y++)
		{
			bars.Add(drawPoint - Main.screenPosition + new Vector2(150 * Projectile.scale, 0), new Color(1f, 0.2f, 0, 0) * mulColor, new Vector3(y / 40f + timeValue, 0, 0));
			bars.Add(drawPoint - Main.screenPosition + new Vector2(-150 * Projectile.scale, 0), new Color(1f, 0.2f, 0, 0) * mulColor, new Vector3(y / 40f + timeValue, 1, 0));
			drawPoint += new Vector2(0, 30);
			if(y > 20)
			{
				drawPoint += new Vector2(0, MathF.Min(30 + (y - 20), 150));
			}
			if(!VFXManager.InScreen(drawPoint, 500) && VFXManager.InScreen(drawPoint + new Vector2(0, -100), 500))
			{
				break;
			}
		}
		if(bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_2_thick.Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
		return false;
	}
	public override void OnKill(int timeLeft)
	{
		base.OnKill(timeLeft);
	}
}