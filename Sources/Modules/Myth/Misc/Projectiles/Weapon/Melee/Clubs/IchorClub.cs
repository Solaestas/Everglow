using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Common;
using Everglow.Myth.MagicWeaponsReplace.Projectiles.GoldenShower;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class IchorClub : ClubProj
{
	float vfxTimer = 0;
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
			var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
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
			if (FlyClubCooling > 0)
				FlyClubCooling--;
			if (FlyClubCooling <= 0 && Omega > 0.2f)
			{
				FlyClubCooling = 44;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IchorClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner);
			}
		}
		vfxTimer += (Omega * 4);
		if (vfxTimer >= 1)
		{
			GenerateVFX((int)(vfxTimer));
			vfxTimer = 0;
		}

	}
	private int FlyClubCooling = 0;
	public override void PostDraw(Color lightColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (Projectile.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		Texture2D texture = MythContent.QuickTexture("Misc/Projectiles/Weapon/Melee/Clubs/IchorClub_glow");
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
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
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
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		var lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
		lightColor.W = 0.7f * Omega;

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);



		if (!Main.gamePaused)
		{
			int randvalue = 10;
			if (MoonBladeI.Count > 1)
				randvalue *= 2;
			if (MoonBladeII.Count > 1)
				randvalue *= 2;
			if (MoonBladeIII.Count > 1)
				randvalue *= 2;
			if (MoonBladeIV.Count > 1)
				randvalue *= 2;
			if (Main.rand.NextBool(randvalue) && Omega > 0.1f)
				ActivateMoon(ref MoonBladeI);
			if (MoonBladeI.Count > 9 && timeI == 64)
			{
				if (Main.rand.Next(MoonBladeI.Count, 20) > 14)
					timeI--;
			}
			if (timeI < 64)
			{
				timeI--;
				if (timeI <= 0)
				{
					timeI = 64;
					DeactivateMoon(ref MoonBladeI);
				}
			}

			if (Main.rand.NextBool(randvalue) && Omega > 0.1f)
				ActivateMoon(ref MoonBladeII);
			if (MoonBladeII.Count > 9 && timeII == 64)
			{
				if (Main.rand.Next(MoonBladeII.Count, 20) > 14)
					timeII--;
			}
			if (timeII < 64)
			{
				timeII--;
				if (timeII <= 0)
				{
					timeII = 64;
					DeactivateMoon(ref MoonBladeII);
				}
			}

			if (Main.rand.NextBool(randvalue) && Omega > 0.1f)
				ActivateMoon(ref MoonBladeIII);
			if (MoonBladeIII.Count > 9 && timeIII == 64)
			{
				if (Main.rand.Next(MoonBladeIII.Count, 20) > 14)
					timeIII--;
			}
			if (timeIII < 64)
			{
				timeIII--;
				if (timeIII <= 0)
				{
					timeIII = 64;
					DeactivateMoon(ref MoonBladeIII);
				}
			}
			if (Main.rand.NextBool(randvalue) && Omega > 0.1f)
				ActivateMoon(ref MoonBladeIV);
			if (MoonBladeIV.Count > 9 && timeIV == 64)
			{
				if (Main.rand.Next(MoonBladeIV.Count, 20) > 14)
					timeIV--;
			}
			if (timeIV < 64)
			{
				timeIV--;
				if (timeIV <= 0)
				{
					timeIV = 64;
					DeactivateMoon(ref MoonBladeIV);
				}
			}

			UpdateMoon(ref MoonBladeI);
			UpdateMoon(ref MoonBladeIV);
			UpdateMoon(ref MoonBladeIII);
			UpdateMoon(ref MoonBladeIV);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		DrawMoon(MoonBladeI, timeI);
		DrawMoon(MoonBladeII, timeII);
		DrawMoon(MoonBladeIII, timeIII);
		DrawMoon(MoonBladeIV, timeIV);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	internal List<Vector2> MoonBladeI = new List<Vector2>();
	internal List<Vector2> MoonBladeII = new List<Vector2>();
	internal List<Vector2> MoonBladeIII = new List<Vector2>();
	internal List<Vector2> MoonBladeIV = new List<Vector2>();
	internal int timeI = 64;
	internal int timeII = 64;
	internal int timeIII = 64;
	internal int timeIV = 64;
	private void DrawMoon(List<Vector2> listVec, float timeLeft)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(listVec.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (listVec.Count != 0)
			SmoothTrail.Add(listVec.ToArray()[listVec.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;

		var bars = new List<Vertex2D>();
		var light = new Color(1f, 1f, 1f, 1f);
		light *= 2;
		light.A /= 4;
		light.G = (byte)(light.G * 0.8f);
		light.B = 0;
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float delta = (1 - timeLeft / 64f * 0.45f) * Omega / MaxOmega + 1 - Omega / MaxOmega;
			float maxValue = 20f;
			if (length < maxValue)
				delta = delta * length / maxValue + (maxValue - length) / maxValue;
			bars.Add(new Vertex2D(Projectile.Center + SmoothTrail[i] * delta * Projectile.scale - Main.screenPosition, light, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + SmoothTrail[i] * Projectile.scale - Main.screenPosition, light, new Vector3(factor, 0, 0f)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineShade");
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
	private void UpdateMoon(ref List<Vector2> listVec)
	{
		if (listVec.Count > 0)
		{
			float value = 1;
			if (listVec.Count > 6)
				value = 6 / (float)listVec.Count;
			Vector2 v0 = listVec[listVec.Count - 1];
			Vector2 v1 = v0.SafeNormalize(Vector2.Zero);
			listVec.Add(v1.RotatedBy(Omega * value * 1.23f) * (v0.Length() * 0.96f + HitLength * 1.73f * 0.04f));
			if (Main.rand.NextBool(2))
			{
				Vector2 v2 = listVec[Main.rand.Next(listVec.Count - 1)].SafeNormalize(Vector2.Zero) * 1.75f;
				if (Main.rand.NextBool(2))
					v2 *= -1;
				v2 *= HitLength * 0.90f;
				float Speed = Math.Min(Omega * value * 1.23f, 0.221f);
				var D = Dust.NewDustDirect(Projectile.Center + v2 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.Ichor, -v2.Y * Speed, v2.X * Speed, 150, default, Main.rand.NextFloat(0.1f, 1.1f) * Omega / 0.5f);
				v2 *= Main.rand.NextFloat(0.8f, 1.5f);
				D.noGravity = true;
				D.velocity = new Vector2(-v2.Y * Speed, v2.X * Speed);
			}
		}
	}
	private void ActivateMoon(ref List<Vector2> listVec)
	{
		if (listVec.Count > 0)
			return;
		listVec = new List<Vector2>();
		if (Main.rand.NextBool(2))
			listVec.Add(trailVecs.ToList()[1] * Main.rand.NextFloat(0.95f, Math.Min(1.75f, 1 + Omega * 1.5f)));
		else
		{
			listVec.Add(trailVecs.ToList()[1] * -Main.rand.NextFloat(0.95f, Math.Min(1.75f, 1 + Omega * 1.5f)));
		}

		//SoundEngine.PlaySound(SoundID.SplashWeak.WithPitchOffset(0.6f),Projectile.Center);
	}
	private void DeactivateMoon(ref List<Vector2> listVec)
	{
		listVec.Clear();
	}
	public void GenerateVFX(int Frequency)
	{
		Player player = Main.player[Projectile.owner];
		float mulVelocity = Main.rand.NextFloat(0.75f, 1.5f);
		for (int g = 0; g < Frequency * 2; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(MathHelper.TwoPi);
			float mulScale = Main.rand.NextFloat(6f, 14f);
			Vector2 startPos = new Vector2(MathF.Sqrt(Main.rand.NextFloat(0f, 1f)) * 56f, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Main.rand.NextBool(2))
			{
				startPos *= -1;
			}
			var blood = new IchorDrop
			{
				velocity = afterVelocity * mulVelocity / mulScale + startPos.RotatedBy(MathHelper.PiOver2) * Omega * 0.4f + player.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + startPos,
				maxTime = Main.rand.Next(6, 32),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) }
			};
			Ins.VFXManager.Add(blood);
		}
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(3f)).RotatedByRandom(MathHelper.TwoPi);
			Vector2 startPos = new Vector2(MathF.Sqrt(Main.rand.NextFloat(0f, 1f)) * 56f, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Main.rand.NextBool(2))
			{
				startPos *= -1;
			}
			var blood = new IchorSplash
			{
				velocity = afterVelocity * mulVelocity + startPos.RotatedBy(MathHelper.PiOver2) * Omega * 0.4f + player.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + startPos,
				maxTime = Main.rand.Next(6, 32),
				scale = Main.rand.NextFloat(6f, 12f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.4f), 0 }
			};
			Ins.VFXManager.Add(blood);
		}
	}
}