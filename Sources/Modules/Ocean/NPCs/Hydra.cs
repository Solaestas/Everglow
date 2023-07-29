using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	// Token: 0x02000420 RID: 1056
    public class Hydra : ModNPC
	{
		// Token: 0x06001475 RID: 5237 RVA: 0x000082F6 File Offset: 0x000064F6
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("babyjellyfish");
			Main.npcFrameCount[base.NPC.type] = 4;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "水螅");
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000B399C File Offset: 0x000B1B9C
		public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 38;
			base.NPC.width = 26;
			base.NPC.height = 26;
			base.NPC.defense = 5;
			base.NPC.lifeMax = 650;
			base.NPC.alpha = 190;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
			base.NPC.buffImmune[70] = true;
			base.NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.NPC.HitSound = SoundID.NPCHit25;
			base.NPC.DeathSound = SoundID.NPCDeath28;
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
				if (Main.player[base.NPC.target].wet && !Main.player[base.NPC.target].dead)
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
            Player player = Main.player[base.NPC.target];
			NPC npc = Main.npc[NPC.FindFirstNPC(base.Mod.Find<ModNPC>("StarJellyfish").Type)];
			double num22 = (double)base.NPC.ai[1];
			double num23 = num22 * 0.017453292519943295;
			double num24 = 150.0;
			base.NPC.position.X = npc.Center.X - (float)((int)(Math.Cos(num23) * num24)) - (float)(base.NPC.width / 2);
			base.NPC.position.Y = npc.Center.Y - (float)((int)(Math.Sin(num23) * num24)) - (float)(base.NPC.height / 2);
			base.NPC.ai[1] += 2f;
			return false;
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
            player.AddBuff(base.Mod.Find<ModBuff>("ExPoi").Type, 5, true);
		}
        // Token: 0x02000413 RID: 1043

		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
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
        }
	}
}
