using Everglow.Commons.DataStructures;
using Everglow.Commons.Mechanics.ElementalDebuff;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Weapons;

public class DevilHeartGyroscope_Proj_Hit : ModProjectile
{
	public override string Texture => Commons.ModAsset.Empty_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 80;
		Projectile.height = 80;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = 60;
		Projectile.extraUpdates = 3;
		base.SetDefaults();
	}

	public override void AI() => base.AI();

	public override bool PreDraw(ref Color lightColor)
	{
		float timeValue = Projectile.timeLeft / 60f;
		var drawColor = new Color(220, 20, 239, 0);
		float range = (1 - timeValue) * 150;
		var drawPos = Projectile.Center - Main.screenPosition;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			var ringColor = drawColor;
			bars.Add(drawPos + new Vector2(range + timeValue * 20f, 0).RotatedBy(i / 100f * MathHelper.TwoPi), ringColor * 0, new Vector3(i / 100f * 4f, timeValue, 0));
			bars.Add(drawPos + new Vector2(range, 0).RotatedBy(i / 100f * MathHelper.TwoPi), ringColor, new Vector3(i / 100f * 4f, 0.05f + timeValue, 0));
		}
		if (bars.Count > 0)
		{
			SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_16.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);
		}
		return false;
	}
}