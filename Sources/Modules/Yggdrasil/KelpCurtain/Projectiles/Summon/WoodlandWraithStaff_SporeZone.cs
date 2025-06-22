using Everglow.Commons.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaff_SporeZone : ModProjectile
{
	public float Range = 0;

	public override void SetDefaults()
	{
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.friendly = false;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.timeLeft = 900;
	}

	public override void AI()
	{
		foreach(var proj in Main.projectile)
		{
			if(proj != null && proj.active && proj.owner == Projectile.owner && proj.type == Type && proj != Projectile)
			{
				if(proj.timeLeft < Projectile.timeLeft && proj.timeLeft > 60)
				{
					proj.timeLeft = 60;
				}
			}
		}
		if (Range < 300)
		{
			Range = Range * 0.9f + 305f * 0.1f;
		}
		else
		{
			Range = 300;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		List<Vertex2D> bars = new List<Vertex2D>();
		var drawColor = new Color(220, 220, 239, 0);
		if(Projectile.timeLeft < 60)
		{
			drawColor = drawColor * (Projectile.timeLeft / 60f);
		}
		for (int i = 0; i <= 100; i++)
		{
			bars.Add(drawPos + new Vector2(Range + 20, 0).RotatedBy(i / 100f * MathHelper.TwoPi), drawColor, Vector3.zero);
			bars.Add(drawPos + new Vector2(Range, 0).RotatedBy(i / 100f * MathHelper.TwoPi), drawColor, Vector3.zero);
		}
		if(bars.Count > 0)
		{
			SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.White.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count -2);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);
		}
		return false;
	}
}