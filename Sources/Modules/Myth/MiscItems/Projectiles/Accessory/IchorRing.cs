using Everglow.Commons.Utilities;
using Everglow.Myth.Common;
using Everglow.Myth.MagicWeaponsReplace.Projectiles.GoldenShower;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.MiscItems.Projectiles.Accessory;

public class IchorRing : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.timeLeft = 780;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
	}
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, ModContent.ProjectileType<GoldenShowerBomb>(), 0, 0, Projectile.owner, 30f, Main.rand.NextFloat(6.283f));
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = player.Center;
		if (Projectile.ai[0] <= 180)
			Projectile.ai[0] = 180 * 0.06f + Projectile.ai[0] * 0.94f;
		for (int x = 0; x < 5; x++)
		{
			GenerateDust();
		}
		if (Projectile.timeLeft < 60)
			Projectile.friendly = false;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		if (Projectile.timeLeft > 690)
			damage *= 5;
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
		target.AddBuff(BuffID.Ichor, 600);
	}
	private bool insertWithRing(Vector2 point1, Vector2 point2, float radious, float toleranceWidth)
	{
		return Math.Abs((point1 - point2).Length() - radious) < toleranceWidth;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		if (insertWithRing(targetHitbox.BottomLeft(), Projectile.Center, Projectile.ai[0], 20))
			return true;
		if (insertWithRing(targetHitbox.BottomRight(), Projectile.Center, Projectile.ai[0], 20))
			return true;
		if (insertWithRing(targetHitbox.TopLeft(), Projectile.Center, Projectile.ai[0], 20))
			return true;
		if (insertWithRing(targetHitbox.TopRight(), Projectile.Center, Projectile.ai[0], 20))
			return true;
		return false;
	}
	private void GenerateDust()
	{
		float rotation = Main.rand.NextFloat(MathF.PI * 2);
		Vector2 v0 = new Vector2(0, Projectile.ai[0] * Main.rand.NextFloat(0.9f, 1f)).RotatedBy(rotation);
		//Vector2 v1 = new Vector2(0, 1).RotatedBy(rotation);
		float Speed = 0.08f;
		var D = Dust.NewDustDirect(Projectile.Center + v0 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.Ichor, -v0.Y * Speed, v0.X * Speed, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
		D.noGravity = true;
		D.velocity = new Vector2(-v0.Y * Speed, v0.X * Speed);
	}
	private void DrawTexLiquidCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, int textureDrawTimes = 1, double addRot = 0)
	{
		var circle = new List<Vertex2D>();
		for (int h = 0; h < radious / 2; h += 2)
		{

			float coordX = (h * 2 / radious * textureDrawTimes + (float)Main.timeForVisualEffects * 0.02f) % 1f;
			float nextCoordX = ((h + 2) * 2 / radious * textureDrawTimes + (float)Main.timeForVisualEffects * 0.02f) % 1f;
			float OutWave = width * (1 + MathF.Sin(coordX * MathF.PI * 2) * 0.03f);
			float InWave = width * (1 + MathF.Sin(-coordX * MathF.PI * 4) * 0.23f);
			InWave = Math.Min(InWave, radious);
			circle.Add(new Vertex2D(center + new Vector2(0, radious - InWave).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(coordX, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious + OutWave).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(coordX, 0, 0)));
			if (nextCoordX < coordX)
			{
				float midValue = (1 - coordX) / (nextCoordX + 1 - coordX);
				OutWave = width;
				InWave = width;
				InWave = Math.Min(InWave, radious);
				circle.Add(new Vertex2D(center + new Vector2(0, radious - InWave).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3(1, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious + OutWave).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3(1, 0, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious - InWave).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3(0, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious + OutWave).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3(0, 0, 0)));
			}
		}
		float coordx = (float)Main.timeForVisualEffects * 0.02f % 1f;
		float outWave = width * (1 + MathF.Sin(coordx * MathF.PI * 2) * 0.23f);
		float inWave = width * (1 + MathF.Sin(-coordx * MathF.PI * 4) * 0.13f);
		inWave = Math.Min(inWave, radious);

		circle.Add(new Vertex2D(center + new Vector2(0, radious - inWave).RotatedBy(addRot), color, new Vector3(coordx, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious + outWave).RotatedBy(addRot), color, new Vector3(coordx, 0, 0)));

		if (circle.Count > 0)
		{
			Main.graphics.GraphicsDevice.Textures[0] = tex;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceShade5xDark");
		float width = 40f;
		if (Projectile.timeLeft < 120f)
			width = Projectile.timeLeft / 3f;
		DrawTexLiquidCircle(Projectile.ai[0] * 0.9f, width, new Color(255, 100, 100, 100), Projectile.Center - Main.screenPosition, tex, 5, Main.time * 0.05);
		tex = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceLight");
		DrawTexLiquidCircle(Projectile.ai[0] * 0.9f, width, new Color(255, 190, 0, 0), Projectile.Center - Main.screenPosition, tex, 5, Main.time * 0.03);
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
			if (Main.rand.NextBool(randvalue))
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

			if (Main.rand.NextBool(randvalue))
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

			if (Main.rand.NextBool(randvalue))
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
			if (Main.rand.NextBool(randvalue))
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
		return false;
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
			float delta = 1 - timeLeft / 64f * 0.45f;
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
			listVec.Add(v1.RotatedBy(0.3f * value * 1.23f) * (v0.Length() * 0.96f + Projectile.ai[0] * 1.03f * 0.04f));
			for (int f = 0; f < 2; f++)
			{
				Vector2 v2 = listVec[Main.rand.Next(listVec.Count - 1)].SafeNormalize(Vector2.Zero) * 1.05f;
				v2 *= Projectile.ai[0] * Main.rand.NextFloat(0.9f, 1.0f);
				float Speed = Math.Min(0.3f * value, 0.221f);
				var D = Dust.NewDustDirect(Projectile.Center + v2 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.Ichor, -v2.Y * Speed, v2.X * Speed, 150, default, Main.rand.NextFloat(0.5f, 1.2f));
				v2 *= Main.rand.NextFloat(0.6f, 1f);
				D.noGravity = true;
				D.velocity = new Vector2(-v2.Y * Speed, v2.X * Speed);
			}
		}
	}
	private void ActivateMoon(ref List<Vector2> listVec)
	{
		if (Projectile.timeLeft < 100)
			return;
		if (listVec.Count > 0)
			return;
		listVec = new List<Vector2>
		{
			new Vector2(0, Projectile.ai[0]).RotatedByRandom(6.283) * Main.rand.NextFloat(0.95f, 1.45f)
		};
		SoundEngine.PlaySound(SoundID.Splash.WithPitchOffset(0.5f), Projectile.Center);
	}
	private void DeactivateMoon(ref List<Vector2> listVec)
	{
		listVec.Clear();
	}
}