using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
    public class AureliaAurita : ModNPC
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("jellyfish");
			Main.npcFrameCount[base.npc.type] = 5;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海月水母");
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            Player player = Main.player[Main.myPlayer];
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (mplayer.ZoneOcean && spawnInfo.water)
            {
                return 0.2f;
            }
            else
            {
                return 0f;
            }
        }

		// Token: 0x06001476 RID: 5238 RVA: 0x000B399C File Offset: 0x000B1B9C
		public override void SetDefaults()
		{
			base.npc.noGravity = true;
			base.npc.damage = 60;
			base.npc.width = 30;
			base.npc.height = 30;
			base.npc.defense = 5;
			base.npc.lifeMax = 650;
			base.npc.alpha = 180;
			base.npc.aiStyle = -1;
			this.aiType = -1;
			base.npc.buffImmune[70] = true;
			base.npc.value = (float)Item.buyPrice(0, 0, 42, 0);
			base.npc.HitSound = SoundID.NPCHit25;
			base.npc.DeathSound = SoundID.NPCDeath28;
			this.banner = base.npc.type;
			this.bannerItem = base.mod.ItemType("MoonJellyfishBanner");
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000B3A6C File Offset: 0x000B1C6C
		public override void AI()
		{
			bool flag = false;
			if (base.npc.wet && base.npc.ai[1] == 1f)
			{
				flag = true;
			}
			else
			{
				base.npc.dontTakeDamage = false;
			}
			float num = 1f;
			if (flag)
			{
				num += 0.5f;
			}
			if (base.npc.direction == 0)
			{
				base.npc.TargetClosest(true);
			}
			if (flag)
			{
				return;
			}
			if (!base.npc.wet)
			{
				base.npc.rotation += base.npc.velocity.X * 0.1f;
				if (base.npc.velocity.Y == 0f)
				{
					base.npc.velocity.X = base.npc.velocity.X * 0.98f;
					if ((double)base.npc.velocity.X > -0.01 && (double)base.npc.velocity.X < 0.01)
					{
						base.npc.velocity.X = 0f;
					}
				}
				base.npc.velocity.Y = base.npc.velocity.Y + 0.2f;
				if (base.npc.velocity.Y > 10f)
				{
					base.npc.velocity.Y = 10f;
				}
				base.npc.ai[0] = 1f;
				return;
			}
			if (base.npc.collideX)
			{
				base.npc.velocity.X = base.npc.velocity.X * -1f;
				base.npc.direction *= -1;
			}
			if (base.npc.collideY)
			{
				if (base.npc.velocity.Y > 0f)
				{
					base.npc.velocity.Y = Math.Abs(base.npc.velocity.Y) * -1f;
					base.npc.directionY = -1;
					base.npc.ai[0] = -1f;
				}
				else if (base.npc.velocity.Y < 0f)
				{
					base.npc.velocity.Y = Math.Abs(base.npc.velocity.Y);
					base.npc.directionY = 1;
					base.npc.ai[0] = 1f;
				}
			}
			bool flag2 = false;
			if (!base.npc.friendly)
			{
				base.npc.TargetClosest(false);
				if (Main.player[base.npc.target].wet && !Main.player[base.npc.target].dead && npc.life < npc.lifeMax)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				base.npc.localAI[2] = 1f;
				base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
				base.npc.velocity *= 0.96f;
				float num2 = 0.8f;
				if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
				{
					base.npc.TargetClosest(true);
					float num3 = 8f;
					Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
					float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
					float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
					float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
					num6 = num3 / num6;
					num4 *= num6;
					num5 *= num6;
					base.npc.velocity.X = num4 / 4;
					base.npc.velocity.Y = num5 / 4;
					return;
				}
			}
			else
			{
				base.npc.localAI[2] = 0f;
				base.npc.velocity.X = base.npc.velocity.X + (float)base.npc.direction * 0.02f;
				base.npc.rotation = base.npc.velocity.X * 0.4f;
				if (base.npc.velocity.X < -1f || base.npc.velocity.X > 1f)
				{
					base.npc.velocity.X = base.npc.velocity.X * 0.95f;
				}
				if (base.npc.ai[0] == -1f)
				{
					base.npc.velocity.Y = base.npc.velocity.Y - 0.01f;
					if (base.npc.velocity.Y < -1f)
					{
						base.npc.ai[0] = 1f;
					}
				}
				else
				{
					base.npc.velocity.Y = base.npc.velocity.Y + 0.01f;
					if (base.npc.velocity.Y > 1f)
					{
						base.npc.ai[0] = -1f;
					}
				}
				int num7 = (int)(base.npc.position.X + (float)(base.npc.width / 2)) / 16;
				int num8 = (int)(base.npc.position.Y + (float)(base.npc.height / 2)) / 16;
				if (Main.tile[num7, num8 - 1] == null)
				{
					Main.tile[num7, num8 - 1] = new Tile();
				}
				if (Main.tile[num7, num8 + 1] == null)
				{
					Main.tile[num7, num8 + 1] = new Tile();
				}
				if (Main.tile[num7, num8 + 2] == null)
				{
					Main.tile[num7, num8 + 2] = new Tile();
				}
				if (Main.tile[num7, num8 - 1].liquid > 128)
				{
					if (Main.tile[num7, num8 + 1].active())
					{
						base.npc.ai[0] = -1f;
					}
					else if (Main.tile[num7, num8 + 2].active())
					{
						base.npc.ai[0] = -1f;
					}
				}
				else
				{
					base.npc.ai[0] = 1f;
				}
				if ((double)base.npc.velocity.Y > 1.2 || (double)base.npc.velocity.Y < -1.2)
				{
					base.npc.velocity.Y = base.npc.velocity.Y * 0.99f;
				}
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000A9970 File Offset: 0x000A7B70
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 0.15000000596046448;
			base.npc.frameCounter %= (double)Main.npcFrameCount[base.npc.type];
			int num = (int)base.npc.frameCounter;
			base.npc.frame.Y = num * frameHeight;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000B4268 File Offset: 0x000B2468

		// Token: 0x0600147A RID: 5242 RVA: 0x0000801E File Offset: 0x0000621E
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(70, 240, true);
            player.AddBuff(base.mod.BuffType("极剧毒"), 20, true);
		}
        // Token: 0x02000413 RID: 1043
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.npc.Center.X, base.npc.Center.Y);
            Vector2 vector = new Vector2((float)(Main.npcTexture[base.npc.type].Width / 2), (float)(Main.npcTexture[base.npc.type].Height / Main.npcFrameCount[base.npc.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/海月水母光辉").Width, (float)(base.mod.GetTexture("NPCs/海月水母光辉").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.npc.alpha, 297 - base.npc.alpha, 297 - base.npc.alpha, 0), Color.Blue);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/海月水母光辉"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }

		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 33, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.npc.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 33, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
        // Token: 0x02000413 RID: 1043
        public override void NPCLoot()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("VoidBubble"), 1, false, 0, false, false);
            }
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("OceanDustCore"), 1, false, 0, false, false);
            }
            if (Main.rand.Next(150) == 0 && Main.hardMode)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("FluoresceinYoyo"), 1, false, 0, false, false);
            }
        }
	}
}
