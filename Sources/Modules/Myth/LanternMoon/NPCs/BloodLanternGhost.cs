using Everglow.Commons.DataStructures;
using Everglow.Myth.LanternMoon.Gores;
using Everglow.Myth.LanternMoon.LanternCommon;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Myth.LanternMoon.NPCs;

public class BloodLanternGhost : ModNPC
{
	public override string Texture => "Everglow/Myth/LanternMoon/NPCs/BloodLanternGhost_drained";
	public LanternMoonProgress LanternMoonProgress = ModContent.GetInstance<LanternMoonProgress>();
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 8;
	}
	public override void SetDefaults()
	{
		NPC.damage = 75;
		NPC.lifeMax = 906;
		NPC.npcSlots = 0.5f;
		NPC.width = 60;
		NPC.height = 60;
		NPC.defense = 24;
		NPC.value = 200;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.2f;
		NPC.dontTakeDamage = false;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit3;
	}
	public override void OnSpawn(IEntitySource source)
	{
		NPC.frame.Y = 0;
		NPC.ai[0] = 0;
		NPC.ai[1] = 0;
		NPC.ai[2] = 0;
	}
	public override void FindFrame(int frameHeight)
	{
		if (DizzyTime <= 0)
		{
			NPC.frameCounter++;
		}
		else
		{
			NPC.frame.Y = 432;
		}
		if (NPC.frameCounter > 4)
		{
			NPC.frameCounter = 0;
			NPC.frame.Y += 144;
			if (NPC.frame.Y >= 576)
			{
				NPC.frame.Y = 0;
			}
		}
	}
	public override void AI()
	{
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		float timeValue = (float)(Main.time * 0.12f + NPC.whoAmI * 0.428571f);
		if (DizzyTime < 0)
		{
			Lighting.AddLight(NPC.Center, new Vector3(1f, 0.3f, 0.3f) * (MathF.Sin(timeValue) * 0.4f + 0.6f) * 0.1f);
		}
		if (ExplosionTimer > 0)
		{
			Lighting.AddLight(NPC.Center, new Vector3(1f, 0.3f, 0.3f) * (MathF.Sin(timeValue) * 0.4f + 0.6f) * 0.1f);
		}
		if (ExplosionTimer > 0)
		{
			UpdateExplosion();
			return;
		}

		Vector2 toPlayer = player.Center + new Vector2(0, -100).RotatedBy(MathF.Sin(NPC.whoAmI)) - NPC.Center - NPC.velocity;
		if (NPC.ai[1] == 0)//巡游
		{
			if (toPlayer.Length() > 1000)
			{
				toPlayer = Vector2.Normalize(toPlayer) * 1000f;
			}

			if (toPlayer.Length() < 300)
			{
				NPC.ai[1] = 1;
				NPC.ai[0] = 0;
			}
			else
			{
				NPC.velocity += toPlayer * 0.0007f;
				NPC.velocity *= 0.95f;
				NPC.rotation = NPC.velocity.X * 0.05f;
				NPC.direction = -1;
				if (NPC.velocity.X < 0)
				{
					NPC.direction = 1;
				}
			}
		}
		else//冲撞
		{

			NPC.ai[0]++;
			if (NPC.ai[0] < 30)
			{
				NPC.velocity *= 0.8f;
				NPC.rotation *= 0.8f;
			}
			if (NPC.ai[0] == 30)
			{
				NPC.velocity += Vector2.Normalize(toPlayer) * 20f;
				NPC.rotation = NPC.velocity.X * 0.02f;
				NPC.direction = -1;
				if (NPC.velocity.X < 0)
				{
					NPC.direction = 1;
				}
			}
			if (NPC.ai[0] > 50)
			{
				NPC.velocity *= 0.8f;
			}
			if (NPC.ai[0] > 90)
			{
				NPC.velocity.Y -= 2.5f;
			}
			if ((toPlayer.Y > 200 && NPC.ai[0] >= 50) || NPC.ai[0] > 200)//结束冲撞
			{
				NPC.ai[1] = 0;
			}
			if (NPC.ai[0] > 30)
			{
				if (Collision.SolidCollision(NPC.position + new Vector2(NPC.velocity.X, 0), NPC.width, NPC.height))
				{
					NPC.velocity.X *= -0.7f;
					NPC.ai[2] = Main.rand.NextFloat(-1.4f, 1.4f);
					DizzyTime = 240;
				}
				if (Collision.SolidCollision(NPC.position + new Vector2(0, NPC.velocity.Y), NPC.width, NPC.height))
				{
					NPC.velocity.Y *= -0.7f;
					NPC.ai[2] = Main.rand.NextFloat(-1.4f, 1.4f);
					DizzyTime = 240;
				}
			}
		}
	}
	public void UpdateExplosion()
	{
		Player player = Main.player[NPC.target];
		ExplosionTimer--;
		NPC.scale += 0.01f;
		NPC.velocity *= 0.92f;
		if (ExplosionTimer > 20 && NPC.Center.Y > player.Center.Y)
		{
			NPC.velocity.Y -= 2.7f;
		}

		if (ExplosionTimer <= 0)
		{
			KillMe();
		}
	}
	public int DizzyTime = -1;
	public int ExplosionTimer = -1;
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}
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
		var p = Projectile.NewProjectileDirect(NPC.GetSource_Death(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<DarkLanternBombExplosion>(), 55, 0.6f, NPC.target, 5);

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
		NPC.active = false;
		LanternMoonProgress.AddPoint(45);
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		float timeValue = (float)(Main.time * 0.12f + NPC.whoAmI * 0.428571f);

		SpriteEffects effects = SpriteEffects.None;
		NPC.spriteDirection = NPC.direction;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, new Vector2(72), NPC.scale, effects, 0);
		Texture2D glow0 = ModAsset.BombLantern_glow.Value;

		spriteBatch.Draw(glow0, NPC.Center - Main.screenPosition, NPC.frame, new Color(1f, 0.6f, 0.2f, 0), NPC.rotation, new Vector2(72), NPC.scale, effects, 0);
		Texture2D glow1 = ModAsset.BombLantern_glow2.Value;
		if (ExplosionTimer > 0)
		{
			float deathValue = 1 - ExplosionTimer / 30f;
			Color glow1C = new Color(deathValue, deathValue * deathValue, deathValue * deathValue * deathValue, 0);
			for (int t = 0; t < deathValue * 6; t++)
			{
				spriteBatch.Draw(glow1, NPC.Center - Main.screenPosition, NPC.frame, glow1C, NPC.rotation, new Vector2(72), NPC.scale, effects, 0);
			}
		}



		if (DizzyTime > 0)
		{
			float mulSize = 1f;
			if (DizzyTime < 15f)
			{
				mulSize = DizzyTime / 15f;
			}
			SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Color c0 = drawColor;
			c0.B = 0;
			c0.A = 0;

			for (int j = 0; j < 2; j++)
			{
				var bars = new List<Vertex2D>();
				for (int i = 0; i <= 20; i++)
				{
					Vector2 v0 = new Vector2(0, 50 - j * 17).RotatedBy(i / 20f * MathHelper.TwoPi) * mulSize;
					v0.Y *= 0.3f;
					v0 = v0.RotatedBy(MathF.Sin(timeValue + j * 1.8f) * 0.5f * (1 - j * 0.4f));
					Vector2 drawCenter = NPC.Center - Main.screenPosition + new Vector2(0, -50);
					bars.Add(drawCenter + v0, c0, new Vector3(i / 20f + timeValue, 0.2f, 0));
					bars.Add(drawCenter + v0 * 0.7f, c0, new Vector3(i / 20f + timeValue, 0.8f, 0));
					if (i % 4 == (int)(timeValue * 3) % 4)
					{
						spriteBatch.Draw(TextureAssets.Star[0].Value, drawCenter + v0, null, drawColor, 0, TextureAssets.Star[0].Value.Size() * 0.5f, NPC.scale, effects, 0);
					}
				}
				if (bars.Count > 2)
				{
					Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_0.Value;
					Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);
		}
		else
		{
			spriteBatch.Draw(glow1, NPC.Center - Main.screenPosition, NPC.frame, new Color(1f, 0.6f, 0.2f, 0) * (MathF.Sin(timeValue) * 0.4f + 0.6f), NPC.rotation, new Vector2(72), NPC.scale, effects, 0);
		}
	}
}
