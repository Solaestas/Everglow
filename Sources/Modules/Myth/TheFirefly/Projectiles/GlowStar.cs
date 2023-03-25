using Everglow.Myth.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class GlowStar : ModProjectile
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
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 1000;
		Projectile.alpha = 0;
		Projectile.penetrate = 1;
		Projectile.scale = 1f;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}
	public int targetWhoAmI = -1;
	public override void AI()
	{
		Projectile.velocity *= 0.9993f;
		float minDistance = 450 - Projectile.ai[0] * 25;

		if (targetWhoAmI == -1)
		{
			int tryRandomTarget = Main.rand.Next(Main.npc.Length);
			NPC target = Main.npc[tryRandomTarget];
			Vector2 toTarget = target.Center - Projectile.Center - Projectile.velocity;
			float Dis = toTarget.Length();

			if (Dis < minDistance)
			{
				if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
				{
					targetWhoAmI = target.whoAmI;
				}
			}
		}
		else
		{ 
			if(!Main.npc[targetWhoAmI].active)
			{
				targetWhoAmI = -1;
				return;
			}
			Vector2 toTarget = Main.npc[targetWhoAmI].Center - Projectile.Center - Projectile.velocity;
			Projectile.velocity = Projectile.velocity * 0.96f + Vector2.Normalize(toTarget) * (3f) * 0.04f;
			if (Projectile.velocity.Length() > 15f)
			{
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 15f;
			}
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D Light = ModAsset.GlowStar.Value;
		if((int)(Projectile.ai[0] * 25 + Main.timeForVisualEffects) % 8 >= 5 && Projectile.timeLeft >= 1000)
		{
			Light = ModAsset.BlueFlameDark.Value;
		}
		float k1 = (100f + Projectile.ai[0] * 25) * 0.3f;
		float k0 = (1000 - Projectile.timeLeft) / k1;
		float k2 = 1f;
		if (Projectile.timeLeft <= 1000 - k1)
			k0 = 1;
		if (Projectile.timeLeft < 200)
			k2 = Projectile.timeLeft / 200f;
		var c0 = new Color(k0 * k0 * 0.3f, k0 * k0 * 0.8f, k0 * 0.8f + 0.2f, 1 - k0);
		if ((int)(Projectile.ai[0] * 25 + Main.timeForVisualEffects) % 8 >= 5 && Projectile.timeLeft >= 1000)
		{
			c0 = Color.White;
		}
		else if ((int)(Projectile.ai[0] * 25 + Main.timeForVisualEffects) % 8 >= 3 && Projectile.timeLeft >= 1000)
		{
			c0 = new Color(0, 0, 1, 0);
		}
		var bars = new List<Vertex2D>();
		float width = 12;
		float k3 = Projectile.ai[1] / 60f;
		if (Projectile.ai[1] > 0)
			width *= k3;
		width *= (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2;
		int TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			TrueL++;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(factor, 0, w)));
		}
		Texture2D t = Common.MythContent.QuickTexture("TheFirefly/Projectiles/MothGreyLine");
		Main.graphics.GraphicsDevice.Textures[0] = t;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2, SpriteEffects.None, 0);
		return false;
	}
	public override bool PreKill(int timeLeft)
	{
		return timeLeft < 995;
	}
	public override void Kill(int timeLeft)
	{
		float value = Math.Min(Projectile.damage / 30f, 1f);
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GlowStarExplosion>(), 0, 0, Projectile.owner, 2.2f / (Projectile.ai[0] + 2) * 0.6f * value, 0.3f);
		SoundEngine.PlaySound(SoundID.Item38, Projectile.Center);
		float k1 = Math.Clamp(Projectile.velocity.Length(), 1, 3);
		float k2 = Math.Clamp(Projectile.velocity.Length(), 6, 10);
		float k0 = 1f / (Projectile.ai[0] + 2) * 2 * k2;
		for (int j = 0; j < 30; j++)
		{
			Vector2 v0 = new Vector2(Main.rand.NextFloat(1, 3f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
			int dust0 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueGlowAppear>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(0.2f, 0.6f) * Projectile.scale);
			Main.dust[dust0].noGravity = true;
		}
		for (int j = 0; j < 30; j++)
		{
			Vector2 v0 = new Vector2(Main.rand.NextFloat(1, 3f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
			int dust1 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueParticleDark2>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(0.7f, 2.5f));
			Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50);
			Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
		}
		foreach (NPC target in Main.npc)
		{
			float Dis = (target.Center - Projectile.Center).Length();

			if (Dis < k0 * 50)
			{
				if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
				{
					NPC.HitModifiers npcHitM = new NPC.HitModifiers();
					NPC.HitInfo hit = npcHitM.ToHitInfo(Projectile.damage / (Dis + 35f) * 35f, Main.rand.NextFloat(100f) < Main.player[Projectile.owner].GetTotalCritChance(Projectile.DamageType), 0.2f);
					target.StrikeNPC(hit, true, true);
					NetMessage.SendStrikeNPC(target, hit);
				}
			}
		}
	}
}