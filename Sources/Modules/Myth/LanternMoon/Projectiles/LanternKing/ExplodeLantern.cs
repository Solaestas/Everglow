using Everglow.Myth.Common;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing.VFXs;
using Terraria;
using Terraria.Audio;
namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class ExplodeLantern : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 3600;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;

	}
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}
	internal bool Appear = false;
	internal int timeToAppear = 120;
	public override void AI()
	{
		if (!Appear)
			timeToAppear -= 1;
		else
		{
			Projectile.velocity *= 1.14f;
			if (Projectile.ai[0] < 1)
				Projectile.ai[0] += 0.05f;
			else
			{
				Projectile.ai[0] = 1f;
			}
			float Sc = Projectile.velocity.Length() / 10f;
			if (Sc > 1)
			{
				Sc = 1;
				Projectile.hostile = true;
			}
			if (Projectile.velocity.Length() > 150)
				Projectile.Kill();
			for (int g = 0; g < Projectile.velocity.Length() / 22f; g++)
			{
				int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) - Projectile.velocity / Projectile.velocity.Length() * g * 4, 0, 0, ModContent.DustType<Dusts.Flame>(), 0, 0, 0, default, 4f * Sc);
				Main.dust[r].noGravity = true;
				if (Main.rand.Next(100) > 60)
				{
					int r0 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 36f)).RotatedByRandom(MathHelper.TwoPi) - Projectile.velocity / Projectile.velocity.Length() * g * 4, 0, 0, ModContent.DustType<Dusts.Flame3>(), 0, 0, 0, default, 7f * Sc);
					Main.dust[r0].noGravity = true;
				}
			}
		}
		if (timeToAppear <= 0)
		{
			timeToAppear = 0;
			Appear = true;
			Projectile.tileCollide = true;
		}
		Projectile.ai[1] += 0.01f;
		if (Projectile.ai[1] % 0.15 == 0)
		{
			if (Projectile.frame < 2)
				Projectile.frame++;
			else
			{
				Projectile.frame = 0;
			}
		}
		if (Main.rand.NextBool(4))
			GenerateVFX(1);
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (Appear)
			Projectile.Kill();
		return false;
	}
	public void GenerateVFXExpolode(int Frequency, float mulVelocity = 1f)
	{
		for (int g = 0; g < Frequency * 3; g++)
		{
			var cf = new FlameDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(4.65f, 5.5f)).RotatedByRandom(6.283) * mulVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-56f, 56f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(16, 36),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.18f, 0.18f), Main.rand.NextFloat(8f, 12f) }
			};
			Ins.VFXManager.Add(cf);
		}
		for (int g = 0; g < Frequency; g++)
		{
			var cf = new FlameDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(6.65f, 10.5f)).RotatedByRandom(6.283) * mulVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(12, 30),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(22f, 32f) }
			};
			Ins.VFXManager.Add(cf);
		}
	}
	public void GenerateVFX(int Frequency)
	{
		float mulVelocity = 1f;
		for (int g = 0; g < Frequency; g++)
		{
			var cf = new FlameDust
			{
				velocity = Projectile.velocity * Main.rand.NextFloat(0.65f, 2.5f) * mulVelocity + Projectile.velocity.SafeNormalize(new Vector2(0, -1)),
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * 1,
				maxTime = Main.rand.Next(27, 72),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(3.6f, 10f) * mulVelocity }
			};
			Ins.VFXManager.Add(cf);
		}
	}
	public override void OnKill(int timeLeft)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 33).RotatedByRandom(6.283);

		GenerateVFXExpolode(24, 2.2f);

		for (int d = 0; d < 70; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Torch, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(3.65f, 7.5f)).RotatedByRandom(6.283);
		}

		Vector2 GorePos;
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra0 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore3").Type, 1f);
		Main.gore[gra0].timeLeft = Main.rand.Next(300, 600);
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra1 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore4").Type, 1f);
		Main.gore[gra1].timeLeft = Main.rand.Next(300, 600);
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra2 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore5").Type, 1f);
		Main.gore[gra2].timeLeft = Main.rand.Next(300, 600);
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra3 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore6").Type, 1f);
		Main.gore[gra3].timeLeft = Main.rand.Next(300, 600);

		int HitType = ModContent.ProjectileType<StrikePlayer>();
		foreach (Player player in Main.player)
		{
			if (player != null)
			{
				float Dis = (player.Center - Projectile.Center).Length();
				if (Dis < 125)
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.Zero, HitType, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolumeScale(0.8f), Projectile.Center);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		var texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
		int frameHeight = texture2D.Height;

		if (Appear)
		{
			float ColorValue = 1f * Projectile.ai[0] * (float)(Math.Sin(Projectile.ai[1]) + 2) / 3f;
			var colorT = new Color(ColorValue, ColorValue, ColorValue, 0.5f * ColorValue);
			Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, frameHeight)), colorT, Projectile.rotation, new Vector2(texture2D.Width / 2f, frameHeight / 2f), Projectile.scale, SpriteEffects.None, 1f);
			Main.spriteBatch.Draw(MythContent.QuickTexture("LanternMoon/Projectiles/LanternKing/LanternFire"), Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 30 * Projectile.frame, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
		}

		for (int j = 0; j < 6; j++)
		{
			Vector2 v = new Vector2(0, 30).RotatedBy(j / 3d * Math.PI + Main.timeForVisualEffects / 40d);
			v.Y *= 0.2f;
			float S = 1f / v.Length() + 0.2f;
			if (Projectile.velocity.Length() > 0.01f)
				S /= Projectile.velocity.Length() * 100f;
			Main.spriteBatch.Draw(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightEffect"), Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + v, new Rectangle?(new Rectangle(0, 0, 256, 256)), new Color(S, 0, 0, 0), 0, new Vector2(128, 128f), 0.7f * (S + 0.2f), SpriteEffects.None, 1f);
		}
		return false;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		Projectile.Kill();
	}
	public void DrawWarp(VFXBatch sb)
	{
		Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightEffect");
		sb.BindTexture(t);
		for (int j = 0; j < 6; j++)
		{
			Vector2 v = new Vector2(0, 30).RotatedBy(j / 3d * Math.PI + Main.timeForVisualEffects / 40d);
			v.Y *= 0.2f;

			sb.Draw(Projectile.Center - Main.screenPosition + v, Main.hslToRgb((float)(Main.timeForVisualEffects * 0.1f + j) % 1, 0.8f, 1f, 0));
		}
	}
	public void DrawTrail()
	{
		float DrawC = Projectile.ai[0] * Projectile.ai[0];

		var bars = new List<Vertex2D>();
		int TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				if (i == 1)
					return;
				break;
			}

			TrueL = i;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			float width = 6;
			if (Projectile.timeLeft <= 30)
				width *= Projectile.timeLeft / 30f;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.Zero);

			var factor = i / (float)TrueL;
			var color = Color.Lerp(new Color(DrawC, DrawC, DrawC, 0), new Color(0, 0, 0, 0), factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

		}

		if (bars.Count > 2)
		{
			Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}
}