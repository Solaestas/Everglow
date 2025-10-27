using Everglow.Commons.Graphics;
using Everglow.Commons.Templates.Weapons.Clubs;
using Everglow.Commons.VFX.CommonVFXDusts;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CurseClub_smash : ClubProj_Smash
{
	public override string Texture => "Everglow/" + ModAsset.CurseClub_Path;

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.CursedInferno, (int)(818 * Omega));
	}

	public bool smashed = false;

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
				pos = trailVecs2.ToArray()[trailVecs2.Count - 1] * factor + trailVecs2.ToArray()[trailVecs2.Count - 2] * (1 - factor);
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

			if (Main.rand.NextBool(5))
			{
				GradientColor color = new GradientColor();
				color.colorList.Add((new Color(1f, 1f, 0.1f), 0f));
				color.colorList.Add((new Color(0.1f, 0.6f, 0.1f), 0.3f));
				int time = Main.rand.Next(15, 35);
				var fire = new Flare()
				{
					position = Vector2.Lerp(player.Center, pos, Main.rand.NextFloat(0.4f, 1.25f)),
					velocity = vel * 0.2f,
					color = color,
					timeleft = time,
					maxTimeleft = time,
					scale = Main.rand.NextFloat(0.3f, 0.6f) * (smashed ? 1 : 0.5f),
				};
				Ins.VFXManager.Add(fire);
			}
			/*
			for (int g = 0; g < 3; g++)
			{
				var fire = new CurseFlame_HighQualityDust
				{
					velocity = vel * 0.3f,
					Active = true,
					Visible = true,
					position = pos + vel * g,
					maxTime = Main.rand.Next(16, 35),

					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), rot * 0.1f, Main.rand.NextFloat(3.6f, 30f) },
				};
				Ins.VFXManager.Add(fire);
			}*/

			for (int g = 0; g < 2; g++)
			{
				var spark = new CurseFlameSparkDust
				{
					velocity = vel.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)),
					Active = true,
					Visible = true,
					position = pos,
					maxTime = Main.rand.Next(6, Main.rand.Next(6, 75)),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), rot * 0.1f },
				};
				Ins.VFXManager.Add(spark);
			}
		}
	}

	public override void Smash(int level)
	{
		Player player = Main.player[Projectile.owner];
		smashed = true;
		for (int t = 0; t < 5; t++)
		{
			Vector2 vel = new Vector2(Main.rand.NextFloat(-15, 15), Main.rand.NextFloat(-10, -5)) * 1.5f * (1 + level * 0.3f);
			var fire = new CurseFlame_HighQualityDust
			{
				velocity = vel,
				Active = true,
				Visible = true,
				position = player.Bottom - vel * 3 + Main.rand.NextVector2Circular(30, 30),
				maxTime = Main.rand.Next(10, 25),

				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(3.6f, 30f) },
			};
			Ins.VFXManager.Add(fire);
		}
		for (int g = 0; g < 20; g++)
		{
			Vector2 vel = new Vector2(0, -20).RotatedBy(Main.rand.NextFloatDirection());
			var spark = new CurseFlameSparkDust
			{
				velocity = vel.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)),
				Active = true,
				Visible = true,
				position = player.Bottom - vel * 3,
				maxTime = Main.rand.Next(6, Main.rand.Next(6, 405)),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.1f, 0.1f) },
			};
			Ins.VFXManager.Add(spark);
		}

		base.Smash(level);
	}

	public override void DrawTrail2(Color color)
	{
		base.DrawTrail2(color);

		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs2.ToList()); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs2.Count != 0)
		{
			SmoothTrail.Add(trailVecs2.ToArray()[trailVecs2.Count - 1]);
		}

		int length = SmoothTrail.Count;
		if (length <= 3)
		{
			return;
		}

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

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.CurseClub_glow.Value;
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
}