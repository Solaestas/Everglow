using Everglow.Commons.DataStructures;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.LanternMoon.Gores;
using Everglow.Myth.LanternMoon.LanternCommon;
using Everglow.Myth.LanternMoon.Projectiles;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing;
using SteelSeries.GameSense;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Myth.LanternMoon.NPCs;

public class CylindricalLantern : ModNPC
{
	public LanternMoonProgress LanternMoonProgress = ModContent.GetInstance<LanternMoonProgress>();
	public override void SetDefaults()
	{
		NPC.damage = 72;
		NPC.lifeMax = 364;
		NPC.npcSlots = 0.5f;
		NPC.width = 40;
		NPC.height = 60;
		NPC.defense = 0;
		NPC.value = 0;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.2f;
		NPC.dontTakeDamage = false;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit3;
	}
	public override void OnSpawn(IEntitySource source)
	{
		NPC.ai[0] = 0;
		NPC.ai[1] = 0;
	}
	public override void AI()
	{
		NPC.ai[0]++;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		float timeValue = (float)(Main.time * 0.12f + NPC.whoAmI * 0.428571f);
		if (ExplosionTimer > 0)
		{
			UpdateExplosion();
			return;
		}
		NPC.velocity *= 0;
		if(NPC.ai[0] > 200)
		{
			NPC.ai[0] = 0;
			NPC.ai[1] = 60;
		}
		if(NPC.ai[1] > 0)
		{
			NPC.ai[1]--;
		}
		else
		{
			NPC.alpha = 0;
		}
		if(NPC.ai[1] > 30)
		{
			NPC.alpha += 10;
		}
		if (NPC.ai[1] == 30)
		{
			NPC.alpha = 255;
			NPC.Center = player.Center + new Vector2(0, -Main.rand.NextFloat(200, 500)).RotatedBy(Main.rand.NextFloat(-1.6f, 1.6f));
		}
		if (NPC.ai[1] < 30)
		{
			NPC.alpha -= 10;
		}
	}
	public void UpdateExplosion()
	{
		Player player = Main.player[NPC.target];
		ExplosionTimer--;
		NPC.scale += 0.01f;
		NPC.velocity *= 0.92f;
		if (ExplosionTimer <= 0)
		{
			KillMe();
		}
	}
	public int ExplosionTimer = -1;
	public override void HitEffect(NPC.HitInfo hit)
	{
		if (NPC.life <= 0)
		{
			ExplosionTimer = 30;
			NPC.life = 1;
			NPC.dontTakeDamage = true;
		}
	}
	public void KillMe()
	{
		ScreenShaker Gsplayer = Main.player[NPC.target].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 33).RotatedByRandom(6.283);
		var p1 = Projectile.NewProjectileDirect(NPC.GetSource_Death(), NPC.Center, new Vector2(0, -17), ModContent.ProjectileType<FlameCylinder>(), 85, 0.6f, NPC.target, 5);
		var p2 = Projectile.NewProjectileDirect(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 17), ModContent.ProjectileType<FlameCylinder>(), 85, 0.6f, NPC.target, 5);

		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolumeScale(0.4f), NPC.Center);
		for (int f = 0; f < 2; f++)
		{
			var gore2 = new FloatLanternGore3
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore2);
			var gore3 = new FloatLanternGore4
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore3);
			var gore4 = new FloatLanternGore5
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore4);
			var gore5 = new FloatLanternGore6
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore5);
		}
		LanternMoonProgress.AddPoint(15);

		for (int f = 0; f < 22; f++)
		{
			Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 12f)).RotatedByRandom(MathHelper.TwoPi);
			int r = Dust.NewDust(NPC.Center - new Vector2(4, 4) - new Vector2(4, 4), 8, 8, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default, Main.rand.NextFloat(0.6f, 1.8f));
			Main.dust[r].noGravity = true;
			Main.dust[r].velocity = v3;
		}

		for (int g = 0; g < 96; g++)
		{
			float scale = Main.rand.NextFloat(4f, 15f);
			float mulVelocity = Main.rand.NextFloat(40, 220);
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 10f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, -1f) * mulVelocity;
			var fire = new FireDust
			{
				velocity = newVelocity / scale * 4f,
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = mulVelocity - 30,
				scale = Main.rand.NextFloat(7f, 15f) * (mulVelocity - 30f) / 30f,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(fire);
		}
		for (int g = 0; g < 96; g++)
		{
			float scale = Main.rand.NextFloat(4f, 15f);
			float mulVelocity = Main.rand.NextFloat(40, 220);
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 10f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, 1f) * mulVelocity;
			var fire = new FireDust
			{
				velocity = newVelocity / scale * 4f,
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = mulVelocity - 30,
				scale = Main.rand.NextFloat(7f, 15f) * (mulVelocity - 30f) / 30f,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(fire);
		}
		for (int g = 0; g < 276; g++)
		{
			float mulVelocity = Main.rand.NextFloat(40, 220);
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 10f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, 1f) * mulVelocity;
			var spark = new FireSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = Main.rand.Next(17, 125),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(0.1f, 17.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.02f, 0.02f) }
			};
			Ins.VFXManager.Add(spark);
		}
		for (int g = 0; g < 276; g++)
		{
			float mulVelocity = Main.rand.NextFloat(40, 220);
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 10f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, -1f) * mulVelocity;
			var spark = new FireSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = Main.rand.Next(17, 125),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(0.1f, 17.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.02f, 0.02f) }
			};
			Ins.VFXManager.Add(spark);
		}
		NPC.active = false;
		LanternMoonProgress.AddPoint(45);
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
	public override void FindFrame(int frameHeight)
	{
		NPC.frameCounter++;
		if (NPC.frameCounter > 4)
		{
			NPC.frameCounter = 0;
			NPC.frame.Y += 28;
			if (NPC.frame.Y >= 84)
			{
				NPC.frame.Y = 0;
			}
		}
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		float timeValue = (float)(Main.time * 0.12f + NPC.whoAmI * 0.428571f);
		float mulColor = (255 - NPC.alpha) / 255f;

		SpriteEffects effects = SpriteEffects.None;
		NPC.spriteDirection = NPC.direction;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, drawColor * mulColor, NPC.rotation, texture.Size() * 0.5f, NPC.scale, effects, 0);

		Texture2D flame = ModAsset.CylindricalLantern_flame.Value;

		spriteBatch.Draw(flame, NPC.Center - Main.screenPosition, new Rectangle(0, NPC.frame.Y, 14, 28), new Color(0.7f, 0.6f, 0.6f, 0) * mulColor, NPC.rotation, new Vector2(7, 14), NPC.scale, effects, 0);

		Lighting.AddLight(NPC.Center, new Vector3(1f, 0.6f, 0.4f) * mulColor);

	}
}
