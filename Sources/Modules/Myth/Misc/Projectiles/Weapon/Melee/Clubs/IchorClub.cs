using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Templates.Weapons.Clubs;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Misc.Projectiles.Accessory;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class IchorClub : ClubProj
{
	private float vfxTimer = 0;

	public override void SetDef()
	{
		Beta = 0.005f;
		MaxOmega = 0.45f;
		vfxTimer = 0;
		WarpValue = 0.3f;
	}

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

		if (Omega > 0.1f)
		{
			for (float d = 0.1f; d < Omega; d += 0.1f)
			{
			}
			if (flyClubCooling > 0)
			{
				flyClubCooling--;
			}

			if (flyClubCooling <= 0 && Omega > 0.2f)
			{
				flyClubCooling = 44;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IchorClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner);
			}
		}
		vfxTimer += Omega * 4;
		if (vfxTimer >= 1)
		{
			GenerateVFX((int)(vfxTimer * 3));
			vfxTimer = 0;
		}
	}

	private int flyClubCooling = 0;

	public override void PostDraw(Color lightColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (Projectile.spriteDirection == 1)
		{
			effects = SpriteEffects.FlipHorizontally;
		}

		Texture2D texture = ModAsset.IchorClub_glow.Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0f);
		for (int i = 0; i < 5; i++)
		{
			float fade = Omega * 2f + 0.2f;
			fade *= (5 - i) / 5f;
			var color2 = new Color(fade, fade, fade, 0);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation - i * 0.75f * Omega, texture.Size() / 2f, Projectile.scale, effects, 0f);
		}
	}

	public override void PostPreDraw()
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList()); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
		{
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
		}

		int length = SmoothTrail.Count;
		if (length <= 3)
		{
			return;
		}

		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		float fade = Omega * 2f + 0.2f;
		var color2 = new Color(fade, Math.Min(fade * 0.4f, 0.6f), 0, 0);

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
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		var lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
		lightColor.W = 0.7f * Omega;

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public void GenerateVFX(int Frequency)
	{
		Player player = Main.player[Projectile.owner];
		float mulVelocity = Main.rand.NextFloat(0.75f, 1.5f);


        for (int g = 0; g < Frequency * 2; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(MathHelper.TwoPi);
			float mulScale = Main.rand.NextFloat(1f, 6f);
			Vector2 startPos = new Vector2(Main.rand.NextFloat(0.7f, 1f) * HitLength * 1.9f, 0).RotatedBy(Projectile.rotation - Omega * Main.rand.NextFloat(1f) + MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Main.rand.NextBool(2))
			{
				startPos *= -1;
			}
			var blood = new IchorDrop
			{
				velocity = afterVelocity * mulVelocity / mulScale + startPos.RotatedBy(MathHelper.PiOver2) * Omega * 0.4f * HitLength / 32f + player.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + startPos,
				maxTime = Main.rand.Next(6, 12),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(3f)).RotatedByRandom(MathHelper.TwoPi);
			Vector2 startPos = new Vector2(MathF.Sqrt(Main.rand.NextFloat(0f, 1f)) * HitLength * 1.9f, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Main.rand.NextBool(2))
			{
				startPos *= -1;
			}
			var blood = new IchorSplash
			{
				velocity = afterVelocity * mulVelocity + startPos.RotatedBy(MathHelper.PiOver2) * Omega * 0.4f * HitLength / 32f + player.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + startPos,
				maxTime = Main.rand.Next(6, 12),
				scale = Main.rand.NextFloat(12f, 24f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.4f), 0 },
			};
			Ins.VFXManager.Add(blood);
		}
	}
}