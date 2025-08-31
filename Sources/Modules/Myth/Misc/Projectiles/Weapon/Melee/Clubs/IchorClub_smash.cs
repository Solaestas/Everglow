using Everglow.Commons.Graphics;
using Everglow.Commons.Templates.Weapons.Clubs;
using Everglow.Commons.VFX.CommonVFX_Rework;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Misc.Projectiles.Accessory;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class IchorClub_smash : ClubProj_Smash
{
	public override string Texture => "Everglow/" + ModAsset.IchorClub_Path;

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int x = 0; x < 2; x++)
		{
			Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
			var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center + velocity * -2, velocity, ModContent.ProjectileType<IchorCurrent>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
			p.friendly = false;
			p.CritChance = Projectile.CritChance;
		}
		target.AddBuff(BuffID.Ichor, (int)(818 * Omega));
	}

	public override void AI()
	{
		base.AI();
		Player player = Main.player[Projectile.owner];
		for (float x = 0; x < Omega + 0.6 + player.velocity.Length() / 180f; x += 0.05f)
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

			if (Main.rand.NextBool(30))
			{
				var ichor = new IchorSplash
				{
					velocity = vel,
					Active = true,
					Visible = true,
					position = pos,
					maxTime = Main.rand.Next(6, 32),
					scale = Main.rand.NextFloat(6f, 12f),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), rot * 0.01f, Main.rand.NextFloat(3.6f, 30f) },
				};

				Ins.VFXManager.Add(ichor);
			}
			if (Main.rand.NextBool(12))
			{
				int time = Main.rand.Next(10, 30);
				GradientColor color = new GradientColor();
				color.colorList.Add((Color.White, 0f));
				color.colorList.Add((Color.Yellow, 0.2f));
				color.colorList.Add((new Color(1f, 0.3f, 0f), 1f));

				var splash = new Splash
				{
					position = Vector2.Lerp(pos, Projectile.Center, Main.rand.NextFloat(-0.2f, 0.5f)),

					color = color,
					gravity = 0.2f,
					velocity = vel * 0.2f - new Vector2(0, 1f),
					scale = Main.rand.NextFloat(0.1f, 0.4f),
					maxTimeleft = time,
					timeleft = time,
				};
				Ins.VFXManager.Add(splash);
			}
			if (Main.rand.NextBool(3))
			{
				for (int g = 0; g < 1; g++)
				{
					var spark = new IchorDrop
					{
						velocity = vel.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * 0.5f,
						Active = true,
						Visible = true,
						position = pos,
						maxTime = Main.rand.Next(36, 75),
						scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(1f, 8.0f)),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.1f, 1f), rot * 0.1f },
					};
					Ins.VFXManager.Add(spark);
				}
			}
		}
	}
	public override void Smash(int level)
	{
		for (int i = 0; i < 10 + level * 5; i++)
		{
			var ichor = new IchorSplash
			{
				velocity = new Vector2(Main.rand.NextFloat(-15, 15), Main.rand.NextFloat(-10, -5)) * 1.5f * (1 + level * 0.3f),
				Active = true,
				Visible = true,
				position = Player.Bottom+ new Vector2(0, 20),
				maxTime = Main.rand.Next(15, 40),
				scale = Main.rand.NextFloat(6f, 12f),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), 0.01f, Main.rand.NextFloat(3.6f, 30f) },
			};

			Ins.VFXManager.Add(ichor);
		}

            for (int g = 0; g < 20; g++)
            {
                var spark = new IchorDrop
                {
                    velocity = new Vector2(Main.rand.NextFloat(-15, 15), Main.rand.NextFloat(-10, -5)),
                    Active = true,
                    Visible = true,
                    position = Player.Center ,
                    maxTime = Main.rand.Next(36, 75),
                    scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(1f, 8.0f)),
                    rotation = Main.rand.NextFloat(6.283f),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), 0.2f },
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
			Color c0 = new Color(1f, 1.2f * factor, 0.0f, 0) * factor ;
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, c0, new Vector3(factor, 1.2f, 0f)));
			bars.Add(new Vertex2D(trail[i] - Main.screenPosition, c0, new Vector3(factor, 0f, 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.IchorClub_Trail2.Value;
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

	}
}