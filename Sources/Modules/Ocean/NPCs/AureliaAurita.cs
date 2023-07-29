using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
    public class AureliaAurita : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("jellyfish");
			Main.npcFrameCount[base.NPC.type] = 5;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海月水母");
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            Player player = Main.player[Main.myPlayer];
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            if (mplayer.ZoneOcean && spawnInfo.Water)
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
			base.NPC.noGravity = true;
			base.NPC.damage = 60;
			base.NPC.width = 30;
			base.NPC.height = 30;
			base.NPC.defense = 5;
			base.NPC.lifeMax = 650;
			base.NPC.alpha = 180;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
			base.NPC.buffImmune[70] = true;
			base.NPC.value = (float)Item.buyPrice(0, 0, 42, 0);
			base.NPC.HitSound = SoundID.NPCHit25;
			base.NPC.DeathSound = SoundID.NPCDeath28;
			this.Banner = base.NPC.type;
			this.BannerItem = base.Mod.Find<ModItem>("MoonJellyfishBanner").Type;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000B3A6C File Offset: 0x000B1C6C
		public override void AI()
		{
			bool flag = false;
			if (base.NPC.wet && base.NPC.ai[1] == 1f)
			{
				flag = true;
			}
			else
			{
				base.NPC.dontTakeDamage = false;
			}
			float num = 1f;
			if (flag)
			{
				num += 0.5f;
			}
			if (base.NPC.direction == 0)
			{
				base.NPC.TargetClosest(true);
			}
			if (flag)
			{
				return;
			}
			if (!base.NPC.wet)
			{
				base.NPC.rotation += base.NPC.velocity.X * 0.1f;
				if (base.NPC.velocity.Y == 0f)
				{
					base.NPC.velocity.X = base.NPC.velocity.X * 0.98f;
					if ((double)base.NPC.velocity.X > -0.01 && (double)base.NPC.velocity.X < 0.01)
					{
						base.NPC.velocity.X = 0f;
					}
				}
				base.NPC.velocity.Y = base.NPC.velocity.Y + 0.2f;
				if (base.NPC.velocity.Y > 10f)
				{
					base.NPC.velocity.Y = 10f;
				}
				base.NPC.ai[0] = 1f;
				return;
			}
			if (base.NPC.collideX)
			{
				base.NPC.velocity.X = base.NPC.velocity.X * -1f;
				base.NPC.direction *= -1;
			}
			if (base.NPC.collideY)
			{
				if (base.NPC.velocity.Y > 0f)
				{
					base.NPC.velocity.Y = Math.Abs(base.NPC.velocity.Y) * -1f;
					base.NPC.directionY = -1;
					base.NPC.ai[0] = -1f;
				}
				else if (base.NPC.velocity.Y < 0f)
				{
					base.NPC.velocity.Y = Math.Abs(base.NPC.velocity.Y);
					base.NPC.directionY = 1;
					base.NPC.ai[0] = 1f;
				}
			}
			bool flag2 = false;
			if (!base.NPC.friendly)
			{
				base.NPC.TargetClosest(false);
				if (Main.player[base.NPC.target].wet && !Main.player[base.NPC.target].dead && NPC.life < NPC.lifeMax)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				base.NPC.localAI[2] = 1f;
				base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
				base.NPC.velocity *= 0.96f;
				float num2 = 0.8f;
				if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
				{
					base.NPC.TargetClosest(true);
					float num3 = 8f;
					Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
					float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
					float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
					float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
					num6 = num3 / num6;
					num4 *= num6;
					num5 *= num6;
					base.NPC.velocity.X = num4 / 4;
					base.NPC.velocity.Y = num5 / 4;
					return;
				}
			}
			else
			{
				base.NPC.localAI[2] = 0f;
				base.NPC.velocity.X = base.NPC.velocity.X + (float)base.NPC.direction * 0.02f;
				base.NPC.rotation = base.NPC.velocity.X * 0.4f;
				if (base.NPC.velocity.X < -1f || base.NPC.velocity.X > 1f)
				{
					base.NPC.velocity.X = base.NPC.velocity.X * 0.95f;
				}
				if (base.NPC.ai[0] == -1f)
				{
					base.NPC.velocity.Y = base.NPC.velocity.Y - 0.01f;
					if (base.NPC.velocity.Y < -1f)
					{
						base.NPC.ai[0] = 1f;
					}
				}
				else
				{
					base.NPC.velocity.Y = base.NPC.velocity.Y + 0.01f;
					if (base.NPC.velocity.Y > 1f)
					{
						base.NPC.ai[0] = -1f;
					}
				}
				int num7 = (int)(base.NPC.position.X + (float)(base.NPC.width / 2)) / 16;
				int num8 = (int)(base.NPC.position.Y + (float)(base.NPC.height / 2)) / 16;
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
				if (Main.tile[num7, num8 - 1].LiquidAmount > 128)
				{
					if (Main.tile[num7, num8 + 1].HasTile)
					{
						base.NPC.ai[0] = -1f;
					}
					else if (Main.tile[num7, num8 + 2].HasTile)
					{
						base.NPC.ai[0] = -1f;
					}
				}
				else
				{
					base.NPC.ai[0] = 1f;
				}
				if ((double)base.NPC.velocity.Y > 1.2 || (double)base.NPC.velocity.Y < -1.2)
				{
					base.NPC.velocity.Y = base.NPC.velocity.Y * 0.99f;
				}
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000A9970 File Offset: 0x000A7B70
		public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += 0.15000000596046448;
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000B4268 File Offset: 0x000B2468

		// Token: 0x0600147A RID: 5242 RVA: 0x0000801E File Offset: 0x0000621E
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			player.AddBuff(70, 240, true);
            player.AddBuff(base.Mod.Find<ModBuff>("极剧毒").Type, 20, true);
		}
        // Token: 0x02000413 RID: 1043
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
            Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)base.Mod.GetTexture("NPCs/海月水母光辉").Width, (float)(base.Mod.GetTexture("NPCs/海月水母光辉").Height / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.NPC.alpha, 297 - base.NPC.alpha, 297 - base.NPC.alpha, 0), Color.Blue);
            Main.spriteBatch.Draw(base.Mod.GetTexture("NPCs/海月水母光辉"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }

		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 33, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 33, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
        // Token: 0x02000413 RID: 1043
        public override void OnKill()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, base.Mod.Find<ModItem>("VoidBubble").Type, 1, false, 0, false, false);
            }
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, base.Mod.Find<ModItem>("OceanDustCore").Type, 1, false, 0, false, false);
            }
            if (Main.rand.Next(150) == 0 && Main.hardMode)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, base.Mod.Find<ModItem>("FluoresceinYoyo").Type, 1, false, 0, false, false);
            }
        }
	}
}
