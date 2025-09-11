using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Templates.Weapons.Clubs;
using Everglow.Commons.VFX.CommonVFXDusts;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CurseClub : ClubProj
{
	public override void SetDef()
	{
		Beta = 0.005f;
		MaxOmega = 0.45f;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.CursedInferno, (int)(818 * Omega));
	}

	public override void AI()
	{
		base.AI();

		if (Omega > 0.1f)
		{
			for (float d = 0.1f; d < Omega; d += 0.1f)
			{
				GenerateDust();
			}
			if (flyClubCooling > 0)
			{
				flyClubCooling--;
			}

			if (flyClubCooling <= 0 && Omega > 0.2f)
			{
				flyClubCooling = 44;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CurseClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner);
			}
		}
		GenerateVFX();
	}

	private int flyClubCooling = 0;

	private void GenerateVFX()
	{
		Vector2 v2 = Main.player[Projectile.owner].velocity;
		float mulVelocity = 0.3f + Omega;
		var v0 = new Vector2(1, 1);
		v0 *= HitLength * Projectile.scale;
		v0.X *= Projectile.spriteDirection;
		if (Main.rand.NextBool(2))
		{
			v0 *= -1;
		}
		v0 = v0.RotatedBy(Projectile.rotation + Main.rand.NextFloat(Omega));
		float Speed = Math.Min(Omega * 0.15f, 0.061f) * Main.rand.NextFloat(1.1f, Main.rand.NextFloat(1.1f, 7f));
		var v1 = new Vector2(-v0.Y, v0.X) * Speed;
		/*
		var cf = new CurseFlame_HighQualityDust
		{
			velocity = v1 + v2 * 0.9f,
			Active = true,
			Visible = true,
			position = Projectile.Center + v0,
			maxTime = Main.rand.Next(17, 35),
			ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Omega * 0.5f, Main.rand.NextFloat(3.6f, 30f) * mulVelocity },
		};
		Ins.VFXManager.Add(cf);*/
		if (Main.rand.NextBool(4))
		{
			GradientColor color = new GradientColor();
			color.colorList.Add((new Color(1f, 1f, 0.1f), 0f));
			color.colorList.Add((new Color(0.1f, 0.6f, 0.1f), 0.3f));
			int time = Main.rand.Next(15, 35);
			var fire = new Flare()
			{
				position = Vector2.Lerp(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 30, Main.rand.NextFloat(0.4f, 1.25f)),
				velocity = Projectile.velocity * 0.5f,
				color = color,
				timeleft = time,
				maxTimeleft = time,
				scale = Main.rand.NextFloat(0.3f, 0.6f),
			};
			Ins.VFXManager.Add(fire);
		}
		for (int g = 0; g < 4; g++)
		{
			v0 = new Vector2(1, 1);
			v0 *= Main.rand.NextFloat(Main.rand.NextFloat(HitLength * 0.75f, HitLength), HitLength);
			v0.X *= Projectile.spriteDirection;
			if (Main.rand.NextBool(2))
			{
				v0 *= -1;
			}

			v0 = v0.RotatedBy(Projectile.rotation + Main.rand.NextFloat(Omega));
			Vector2 newVelocity = new Vector2(-v0.Y, v0.X) * Speed;
			float v0Length = v0.Length();
			var spark = new CurseFlameSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + v0,
				maxTime = Main.rand.Next(37, Main.rand.Next(37, 225)),
				scale = Main.rand.NextFloat(4f, 27.0f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Omega * 0.1f * v0Length / 14f, 15f },
			};
			Ins.VFXManager.Add(spark);
		}
	}

	private void GenerateDust()
	{
		var v0 = new Vector2(1, 1);
		v0 *= Main.rand.NextFloat(Main.rand.NextFloat(1, HitLength), HitLength);
		v0.X *= Projectile.spriteDirection;
		if (Main.rand.NextBool(2))
		{
			v0 *= -1;
		}

		v0 = v0.RotatedBy(Projectile.rotation);
		float Speed = Math.Min(Omega * 0.5f, 0.221f);
		var D = Dust.NewDustDirect(Projectile.Center + v0 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.CursedTorch, -v0.Y * Speed, v0.X * Speed, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
		D.noGravity = true;
		D.velocity = new Vector2(-v0.Y * Speed, v0.X * Speed);
	}

	public override void PostDraw(Color lightColor)
	{
		base.PostDraw(lightColor);
	}

	public override void PostPreDraw()
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(TrailVecs.ToList()); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (TrailVecs.Count != 0)
		{
			SmoothTrail.Add(TrailVecs.ToArray()[TrailVecs.Count - 1]);
		}

		int length = SmoothTrail.Count;
		if (length <= 3)
		{
			return;
		}

		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		float fade = Omega * 2f + 0.2f;
		var color2 = new Color(Math.Min(fade * 0.6f, 0.6f), fade, fade * 0.01f, fade);

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.5f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
		}
		bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
		bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.5f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center - trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.CurseClub_trail.Value;

		var lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
		lightColor.W = 0.7f * Omega;

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}