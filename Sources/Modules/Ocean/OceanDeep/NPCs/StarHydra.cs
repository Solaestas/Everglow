using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
	// Token: 0x02000420 RID: 1056
    public class StarHydra : ModNPC
	{
		private int num1 = 0;
		private bool flag1 = true;
		// Token: 0x06001475 RID: 5237 RVA: 0x000082F6 File Offset: 0x000064F6
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("babyjellyfish");
			Main.npcFrameCount[base.npc.type] = 4;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "星渊水螅");
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000B399C File Offset: 0x000B1B9C
		public override void SetDefaults()
		{
			base.npc.noGravity = true;
			base.npc.damage = 30;
			base.npc.width = 26;
			base.npc.height = 26;
			base.npc.defense = 5;
			base.npc.lifeMax = 650;
			base.npc.alpha = 90;
			base.npc.aiStyle = -1;
			this.aiType = -1;
			base.npc.buffImmune[70] = true;
			base.npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.npc.HitSound = SoundID.NPCHit25;
			base.npc.DeathSound = SoundID.NPCDeath28;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000B3A6C File Offset: 0x000B1C6C
		public override void AI()
		{
			base.npc.TargetClosest(true);
			num1 += 1;
			base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
            base.npc.velocity *= 0.94f;
            float num2 = 0.5f;
            if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
            {
                float num3 = 8f;
				float num7 = Main.rand.Next(1000, 5000) / 1000f;
                Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                num6 = num3 / num6;
                num4 *= num6;
                num5 *= num6;
                base.npc.velocity.X = num4 * num7;
                base.npc.velocity.Y = num5 * num7;
                return;
            }
		}
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 0.7f;
			return null;
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
            player.AddBuff(base.mod.BuffType("极剧毒"), 5, true);
		}
        // Token: 0x02000413 RID: 1043

		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.npc.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/星渊水螅光辉").Width, (float)(base.mod.GetTexture("NPCs/星渊水螅光辉").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.npc.alpha, 297 - base.npc.alpha, 297 - base.npc.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/星渊水螅光辉"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
        // Token: 0x02000413 RID: 1043
        public override void NPCLoot()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("空灵泡"), 1, false, 0, false, false);
            }
        }
	}
}
