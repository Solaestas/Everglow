using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.GoldenShower;
public class GoldenShowerII : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 2;
		Projectile.timeLeft = 240;
		Projectile.alpha = 0;
		Projectile.penetrate = 1;
		Projectile.scale = 1f;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
	}
	public void GenerateVFX(int Frequency)
	{
		for (int g = 0; g < Frequency; g++)
		{
			var blood = new IchorDrop
			{
				velocity = Projectile.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(32, 64),
				scale = Main.rand.NextFloat(6f, 14f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) }
			};
			Ins.VFXManager.Add(blood);
		}
	}
	public void GenerateVFXII(int Frequency)
	{
		for (int g = 0; g < Frequency; g++)
		{
			var blood = new IchorSplash
			{
				velocity = Projectile.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(12, 36),
				scale = Main.rand.NextFloat(1f, 5f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 0.0f) }
			};
			Ins.VFXManager.Add(blood);
		}
	}
	public override void AI()
	{
		int vfxFrequency = 200;
		if(Ins.VisualQuality.High)
		{
			vfxFrequency = 3;
		}
		if(VFXManager.InScreen(Projectile.position, 160))
		{
			if (Main.rand.NextBool(vfxFrequency))
			{
				GenerateVFX(1);
				if (Ins.VisualQuality.High && Main.rand.NextBool(vfxFrequency))
				{
					GenerateVFXII(1);
				}
			}
		}
		
		float kTime = 1f;
		if (Projectile.timeLeft < 90f)
			kTime = Projectile.timeLeft / 90f;
		Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0.32f * kTime, 0.23f * kTime, 0);
		for(int x = 0; x < 8;x++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) + Projectile.velocity * Main.rand.NextFloat(1f);
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Ichor, 0, 0, 0, default, 0.6f / (Projectile.ai[0] + 1));
			d0.noGravity = true;
			d0.velocity *= 0;
		}
		Projectile.velocity.Y += 0.15f;
		
		if (Projectile.timeLeft == 210)
			Projectile.friendly = true;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
		for (int x = 0; x < 15; x++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Ichor, 0, 0, 0, default, 0.6f);
			d0.noGravity = true;
		}
		if (Projectile.ai[0] != 3)
		{
			for (int x = 0; x < 3; x++)
			{
				Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
				var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
				p.friendly = false;
				p.CritChance = Projectile.CritChance;
			}
		}
		target.AddBuff(BuffID.Ichor, 600);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float width = 16;

		int trueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			trueL++;
		}
		var bars = new List<Vertex2D>();
		for (int i = 1; i < trueL; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			float k0 = (float)Math.Atan2(normalDir.Y, normalDir.X);
			k0 += 3.14f + 1.57f;
			if (k0 > 6.28f)
				k0 -= 6.28f;
			var c0 = new Color(k0 / 6.28f, 0.4f, 0, 0);

			var factor = i / (float)trueL;
			float x0 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x0 %= 1f;
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(x0, 1, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(x0, 0, 0)));
		}
		if (bars.Count > 2)
		{
			spriteBatch.Draw(bars, PrimitiveType.TriangleStrip);
		}
	}
	//public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;
	//public override void Draw()
	//{
	//	float k1 = 60f;
	//	float k0 = (240 - Projectile.timeLeft) / k1;

	//	if (Projectile.timeLeft <= 240 - k1)
	//		k0 = 1;

	//	var c0 = new Color(k0 * 0.8f + 0.2f, k0 * k0 * 0.4f + 0.2f, 0f, 0);
	//	int trueL = 0;
	//	for (int i = 1; i < Projectile.oldPos.Length; ++i)
	//	{
	//		if (Projectile.oldPos[i] == Vector2.Zero)
	//			break;

	//		trueL++;
	//	}

	//	var bars = new List<Vertex2D>();
	//	float width = 36;
	//	if (Projectile.timeLeft <= 40)
	//		width = Projectile.timeLeft * 0.9f;
	//	if (Projectile.ai[0] == 3)
	//		width *= 0.5f;
	//	for (int i = 1; i < trueL; ++i)
	//	{

	//		if (Projectile.oldPos[i] == Vector2.Zero)
	//			break;

	//		var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
	//		normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
	//		var factor = i / (float)trueL;
	//		float x0 = factor * 0.6f + (float)(Main.time * 0.03) + Projectile.whoAmI * 0.03f;
	//		bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(5f), c0, new Vector3(x0, 1, factor * 0.7f + 0.3f)));
	//		bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(5f), c0, new Vector3(x0, 0, factor * 0.7f + 0.3f)));
	//	}
	//	if (bars.Count > 2)
	//	{
	//		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	//	}
	//}
}