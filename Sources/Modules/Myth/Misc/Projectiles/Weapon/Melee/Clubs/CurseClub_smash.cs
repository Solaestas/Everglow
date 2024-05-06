using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Misc.VFXs;
using SteelSeries.GameSense;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CurseClub_smash : ClubProj_Smash
{
	public override string Texture => "Everglow/" + ModAsset.Melee_CurseClub_Path;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.CursedInferno, (int)(818 * Omega));
	}
	public override void AI()
	{
		base.AI();
		Player player = Main.player[Projectile.owner];
		for (float x = 0; x < Omega + 0.2 + player.velocity.Length() / 100f; x += 0.12f)
		{
			Vector2 pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] + trailVecs2.ToArray()[trailVecs2.Count - 1]) / 2f;
			float factor = Main.rand.NextFloat(0, 1f);
			if (trailVecs2.Count > 1)
			{
				pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] * factor + trailVecs2.ToArray()[trailVecs2.Count - 2] * (1 - factor));
			}
			pos = (pos - Projectile.Center) * 0.9f + Projectile.Center - player.velocity * factor;
			Vector2 vel = Vector2.zeroVector;
			if (trailVecs2.Count > 1)
			{
				vel = trailVecs2.ToArray()[trailVecs2.Count - 1] - trailVecs2.ToArray()[trailVecs2.Count - 2];
			}
			if (trailVecs2.Count > 2)
			{
				vel = (trailVecs2.ToArray()[trailVecs2.Count - 1] - trailVecs2.ToArray()[trailVecs2.Count - 2]) * factor + (trailVecs2.ToArray()[trailVecs2.Count - 2] - trailVecs2.ToArray()[trailVecs2.Count - 3]) * (1 - factor);
			}
			vel += player.velocity;
			vel *= Main.rand.NextFloat(0.1f, 0.3f);
			float rot = 0;
			if (trailVecs2.Count > 1)
			{
				rot = (trailVecs2.ToArray()[trailVecs2.Count - 1] - Projectile.Center).ToRotation() - (trailVecs2.ToArray()[trailVecs2.Count - 2] - Projectile.Center).ToRotation();
			}
			var fire = new CursedFlameDust
			{
				velocity = vel,
				Active = true,
				Visible = true,
				position = pos,
				maxTime = Main.rand.Next(36, 75),

				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), rot * 0.1f, Main.rand.NextFloat(3.6f, 30f) }
			};
			Ins.VFXManager.Add(fire);
			for (int g = 0; g < 1; g++)
			{
				var spark = new CurseFlameSparkDust
				{
					velocity = vel.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)),
					Active = true,
					Visible = true,
					position = pos,
					maxTime = Main.rand.Next(36, 75),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), rot * 0.1f }
				};
				Ins.VFXManager.Add(spark);
			}	
		}
	}
	public override void DrawTrail2(Color color)
	{
		base.DrawTrail2(color);

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
			Color c0 = new Color(1f * factor * factor, 1f * factor, 1f * factor * factor, 0);
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, c0, new Vector3(0, 1, 0f)));
			bars.Add(new Vertex2D(trail[i] - Main.screenPosition, c0, new Vector3(1, 0, 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.Melee_CurseClub_glow.Value;
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
}