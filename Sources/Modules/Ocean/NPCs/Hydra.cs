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
    public class Hydra : ModNPC
	{
		// Token: 0x06001475 RID: 5237 RVA: 0x000082F6 File Offset: 0x000064F6
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("babyjellyfish");
			Main.npcFrameCount[base.npc.type] = 4;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "水螅");
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000B399C File Offset: 0x000B1B9C
		public override void SetDefaults()
		{
			base.npc.noGravity = true;
			base.npc.damage = 38;
			base.npc.width = 26;
			base.npc.height = 26;
			base.npc.defense = 5;
			base.npc.lifeMax = 650;
			base.npc.alpha = 190;
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
				if (Main.player[base.npc.target].wet && !Main.player[base.npc.target].dead)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
			}
		}
        public override bool PreAI()
        {
            Player player = Main.player[base.npc.target];
			NPC npc = Main.npc[NPC.FindFirstNPC(base.mod.NPCType("StarJellyfish"))];
			double num22 = (double)base.npc.ai[1];
			double num23 = num22 * 0.017453292519943295;
			double num24 = 150.0;
			base.npc.position.X = npc.Center.X - (float)((int)(Math.Cos(num23) * num24)) - (float)(base.npc.width / 2);
			base.npc.position.Y = npc.Center.Y - (float)((int)(Math.Sin(num23) * num24)) - (float)(base.npc.height / 2);
			base.npc.ai[1] += 2f;
			return false;
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
            player.AddBuff(base.mod.BuffType("ExPoi"), 5, true);
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
        // Token: 0x02000413 RID: 1043
        public override void NPCLoot()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("VoidBubble"), 1, false, 0, false, false);
            }
        }
	}
}
