namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MeteorClub_smash : ClubProj_Smash_metal
{
	public override string TrailColorTex() => "Everglow/" + ModAsset.Melee_MeteorClub_glow_Path;
	public override string Texture => "Everglow/" + ModAsset.Melee_MeteorClub_Path;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		int type = DustID.Flare;
		for (float d = 0.1f; d < Omega; d += 0.04f)
		{
			var D = Dust.NewDustDirect(target.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
			D.noGravity = true;
			D.velocity = new Vector2(0, Main.rand.NextFloat(Omega * 25f)).RotatedByRandom(6.283);
		}
		target.AddBuff(BuffID.OnFire, 300);
	}
	public override void DrawTrail2(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs2.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs2.Count != 0)
			SmoothTrail.Add(trailVecs2.ToArray()[trailVecs2.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			Color c0 = new Color(1f * factor, 1f * factor * factor, 0.5f * factor * factor * factor, 0);
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, c0, new Vector3(0, 1, 0f)));
			bars.Add(new Vertex2D(trail[i] - Main.screenPosition, c0, new Vector3(1, 0, 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.Melee_MeteorClub_glow.Value;
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);


		base.DrawTrail2(color);
	}
	public override void AI()
	{
		base.AI();
		Player player = Main.player[Projectile.owner];
		if (trailVecs2.Count > 0)
		{
			int type = DustID.Torch;
			for (float x = 0; x < Omega + 0.2 + player.velocity.Length() / 40f; x += 0.05f)
			{
				Vector2 pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] + trailVecs2.ToArray()[trailVecs2.Count - 1]) / 2f;
				float factor = Main.rand.NextFloat(0, 1f);
				if (trailVecs2.Count > 1)
				{
					pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] * factor + trailVecs2.ToArray()[trailVecs2.Count - 2] * (1 - factor));
				}
				pos = (pos - Projectile.Center) * 0.9f + Projectile.Center - player.velocity * factor;
				var d0 = Dust.NewDustDirect(pos - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, Main.rand.NextFloat(0.9f, 1.2f));
				d0.noGravity = true;
				d0.velocity = (trailVecs2.ToArray()[trailVecs2.Count - 1] - Projectile.Center).RotatedBy(MathHelper.PiOver2) / 150f;
			}

			for (float x = 0; x < Omega + 0.2 + player.velocity.Length() / 40f; x += 0.15f)
			{
				Vector2 pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] + trailVecs2.ToArray()[trailVecs2.Count - 1]) / 2f;
				float factor = Main.rand.NextFloat(0, 1f);
				if (trailVecs2.Count > 1)
				{
					pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] * factor + trailVecs2.ToArray()[trailVecs2.Count - 2] * (1 - factor));
				}
				pos = (pos - Projectile.Center) * 0.24f + Projectile.Center - player.velocity * factor;
				var d0 = Dust.NewDustDirect(pos - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, Main.rand.NextFloat(0.9f, 1.2f));
				d0.noGravity = true;
				d0.velocity = (trailVecs2.ToArray()[trailVecs2.Count - 1] - Projectile.Center).RotatedBy(MathHelper.PiOver2) / 150f;
			}
		}
	}
}
