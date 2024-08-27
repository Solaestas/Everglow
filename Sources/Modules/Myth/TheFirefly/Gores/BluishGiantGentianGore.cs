using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly.Gores;

[Pipeline(typeof(BluishGiantGentianGorePipeline))]
public abstract class BluishGiantGentianGore : DissolveGore
{
	public override void Update()
	{
		LightValue = 0.4f;
		base.Update();
		if(timer < 10)
		{
			foreach(Projectile projectile in Main.projectile)
			{
				if(projectile.active)
				{
					if((projectile.Center - position).Length() < 350)
					{
						if(projectile.velocity.Length() > velocity.Length() * 0.2f)
						{
							velocity += projectile.velocity * 0.1f;
							if(Main.rand.NextBool(6))
							{
								velocity += new Vector2(Main.rand.NextFloat(4f, 15f), 0).RotatedByRandom(MathHelper.TwoPi);
							}
						}
					}
				}
			}
		}
		float alpha2 = timer / (float)maxTime * 0.2f;
		alpha2 = Math.Clamp(alpha2, 0.0f, 1.0f);
		alpha2 = MathF.Sin(alpha2 * MathHelper.Pi);
		if(Main.rand.NextFloat(5, 100) < velocity.Length())
		{
			Dust dust = Dust.NewDustDirect(position, width, height, ModContent.DustType<BluishGiantGentian_dust_wither>());
			dust.velocity = velocity * Main.rand.NextFloat(0.2f, 0.8f);
			dust.noGravity = true;
			dust.alpha = 50;
		}
		Lighting.AddLight(position, new Vector3(0f, 0.5f, 1.8f) * alpha2 * width / 60f);
	}

	public override void DrawDissolvePart()
	{
		Vector2 v0 = position + new Vector2(-width, -height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v1 = position + new Vector2(width, -height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v2 = position + new Vector2(-width, height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v3 = position + new Vector2(width, height).RotatedBy(rotation) * 0.5f * scale;

		alpha = 1;

		Color c0 = Lighting.GetColor((v0 / 16f).ToPoint()) * alpha;
		Color c1 = Lighting.GetColor((v1 / 16f).ToPoint()) * alpha;
		Color c2 = Lighting.GetColor((v2 / 16f).ToPoint()) * alpha;
		Color c3 = Lighting.GetColor((v3 / 16f).ToPoint()) * alpha;

		float alpha2 = timer / (float)maxTime * 0.2f;
		alpha2 = Math.Clamp(alpha2, -0.4f, 1.4f);
		alpha2 = MathF.Sin(alpha2 * MathHelper.Pi);

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(v0, c0, new Vector3(0, 0, alpha2)),
			new Vertex2D(v1, c1, new Vector3(1, 0, alpha2)),

			new Vertex2D(v2, c2, new Vector3(0, 1, alpha2)),
			new Vertex2D(v3, c3, new Vector3(1, 1, alpha2)),
		};
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.Textures[1] = DissolveAnimationTexture;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
}