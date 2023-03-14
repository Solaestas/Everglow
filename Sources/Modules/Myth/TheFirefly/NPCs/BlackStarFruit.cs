using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs
{
	public class BlackStarFruit : ModNPC
	{
		public override void SetStaticDefaults()
		{
			//TODO: BlackStarFruit Localization File Text
			// DisplayName.SetDefault("Black Star Fruit");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "萤灯果");
		}
		public override void SetDefaults()
		{
			NPC.damage = 0;
			NPC.width = 40;
			NPC.height = 40;
			NPC.defense = 0;
			NPC.lifeMax = 1;
			NPC.knockBackResist = 0f;
			NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
			NPC.color = new Color(0, 0, 0, 0);
			NPC.alpha = 0;
			NPC.boss = false;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.aiStyle = -1;
			NPC.dontTakeDamage = true;
		}
		public override void OnSpawn(IEntitySource source)
		{
			NPC.position.Y -= 35f;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
			if (!fireflyBiome.IsBiomeActive(Main.LocalPlayer))
			{
				return 0f;
			}
			return 0.5f;
		}
		private float E = 0;
		public override void AI()
		{
			NPC.velocity = new Vector2(0, (float)(Math.Sin(Main.time / 50f + NPC.Center.X + NPC.whoAmI)) * 0.25f);
			if (E <= 0.2f)
			{
				for (int j = 0; j < 12; j++)
				{
					vl[j] = new Vector2(0, 24).RotatedByRandom(3.1415926535 * 2);
					kl[j] = Main.rand.NextFloat(0f, 1f);
				}
			}
			for (int j = 0; j < 12; j++)
			{
				kl[j] *= 0.98f;
				if (kl[j] < 0.07f)
				{
					kl[j] = 1;
					vl[j] = new Vector2(0, 20).RotatedByRandom(3.1415926535 * 2);
				}
			}
			if (E < 1)
			{
				E += 0.01f;
			}
			else
			{
				NPC.dontTakeDamage = false;
			}
			Lighting.AddLight((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16 - 1), 0, 0.1f, 0.8f);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);
			for (int h = 0; h < 60; h += 3)
			{
				Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d) + 2).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
				int r = Dust.NewDust(NPC.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.PureBlue>(), 0, 0, 0, default, 4f * Main.rand.NextFloat(0.7f, 2.9f));
				Main.dust[r].noGravity = true;
				Main.dust[r].velocity = v3 * 2;
			}
			for (int y = 0; y < 30; y += 3)
			{
				int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.5f, 2.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
			}
			for (int y = 0; y < 30; y += 3)
			{
				int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.5f, 2.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 7.5f)).RotatedByRandom(Math.PI * 2d);
			}
			for (int y = 0; y < 18; y++)
			{
				int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.4f, 2.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(1.8f, 2.5f)).RotatedByRandom(Math.PI * 2d);
			}
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.FruitBomb>(), 0, 0f, Main.myPlayer, 1);
			for (int j = 0; j < 200; j++)
			{
				if ((Main.npc[j].Center - NPC.Center).Length() < 250 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active && Main.npc[j].type != NPC.type)
				{
					int Dam = 20;
					if (Main.expertMode)
					{
						Dam = 24;
					}
					if (Main.masterMode)
					{
						Dam = 30;
					}
					Main.npc[j].StrikeNPC((int)(Dam * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(NPC.velocity.X), Main.rand.Next(100) < 10);
				}
			}
		}
		private Vector2[] vl = new Vector2[12];
		private float[] kl = new float[12];
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tx = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/Lightball").Value;
			Texture2D tg = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/Lightball").Value;
			Texture2D tb = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/NPCs/BlackStarFruitLight").Value;
			Vector2 vector = new Vector2(tx.Width / 2f, tx.Height / (float)Main.npcFrameCount[NPC.type] / 2f);
			Color color = new Color(10, 83, 110, 0);
			Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition + new Vector2(0, +19), null, color, NPC.rotation, vector, 0.025f * (float)(4 + Math.Sin(Main.time / 15d + NPC.position.X / 36d)) * E, effects, 0f);
			for (int j = 0; j < 12; j++)
			{
				Main.spriteBatch.Draw(tb, NPC.Center - Main.screenPosition + new Vector2(0, +19) + vl[j] * kl[j] * E, null, new Color(1 - kl[j], 1 - kl[j], 1 - kl[j], 0), NPC.rotation, new Vector2(5, 5), 1.2f * E * (1 - kl[j]), effects, 0f);
			}
		}
	}
}
