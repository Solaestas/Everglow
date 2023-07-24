using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace MythMod.NPCs
{
	// Token: 0x02000421 RID: 1057
    public class Clownfish : ModNPC
	{
		// Token: 0x0600147D RID: 5245 RVA: 0x0000832F File Offset: 0x0000652F
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sailfish");
			Main.npcFrameCount[base.npc.type] = 4;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "小丑鱼");
		}
        private bool flag = true;
        private int num = 0;
        private Vector2 v = new Vector2(0, 0);
        // Token: 0x0600147E RID: 5246 RVA: 0x000B4364 File Offset: 0x000B2564
        public override void SetDefaults()
		{
			base.npc.noGravity = true;
			base.npc.damage = 0;
			base.npc.width = 44;
			base.npc.height = 28;
			base.npc.defense = 0;
			base.npc.lifeMax = 10;
			base.npc.aiStyle = -1;
			this.aiType = -1;
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
			base.npc.value = (float)Item.buyPrice(0, 1, 6, 0);
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath40;
			base.npc.knockBackResist = 0.2f;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x000B4440 File Offset: 0x000B2640
		public override void AI()
		{
            num += 1;
            base.npc.direction = (int)(npc.velocity.X / Math.Abs(npc.velocity.X));
            base.npc.spriteDirection = ((base.npc.direction > 0) ? 1 : -1);
            base.npc.noGravity = true;
            if (base.npc.direction == 0)
			{
				base.npc.TargetClosest(false);
			}
            if(flag)
            {
                npc.scale = Main.rand.Next(800, 1200) / 1000f;
                Vector2 v = npc.Center;
                npc.velocity.X = (1 - (Main.rand.Next(0, 1) * 2)) * 0.8f;
                flag = false;
            }
			if (base.npc.wet)
			{
                npc.velocity.Y = (float)Math.Sin(num / 50f) * 0.15f;
                if(num % 1200 == 1)
                {
                    npc.velocity *= -1;
                }
                if (base.npc.collideX)
                {
                    if (npc.velocity.X < 0)
                    {
                        npc.position.X += 5f;
                        base.npc.velocity.X = 0.8f;
                    }
                    else
                    {
                        npc.position.X -= 5f;
                        base.npc.velocity.X = -0.8f;
                    }
                    base.npc.direction *= -1;
                }
            }
			else
			{
				if (base.npc.velocity.Y == 0f)
				{
					base.npc.velocity.X = base.npc.velocity.X * 0.94f;
					if ((double)base.npc.velocity.X > -0.2 && (double)base.npc.velocity.X < 0.2)
					{
						base.npc.velocity.X = 0f;
					}
				}
				base.npc.velocity.Y = base.npc.velocity.Y + 0.4f;
				if (base.npc.velocity.Y > 12f)
				{
					base.npc.velocity.Y = 12f;
				}
				base.npc.ai[0] = 1f;
			}
			base.npc.rotation = base.npc.velocity.Y * (float)base.npc.direction * 0.1f;
			if ((double)base.npc.rotation < -0.2)
			{
				base.npc.rotation = -0.2f;
			}
			if ((double)base.npc.rotation > 0.2)
			{
				base.npc.rotation = 0.2f;
			}
		}


		// Token: 0x06001481 RID: 5249 RVA: 0x000B4C18 File Offset: 0x000B2E18
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += (double)(0.15f);
			base.npc.frameCounter %= (double)Main.npcFrameCount[base.npc.type];
			int num = (int)base.npc.frameCounter;
			base.npc.frame.Y = num * frameHeight;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneOcean && spawnInfo.water)
			{
				return 0.2f;
			}
			return 0f;
		}
		// Token: 0x06001482 RID: 5250 RVA: 0x00008065 File Offset: 0x00006265
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(30, 420, true);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00007FE0 File Offset: 0x000061E0

		// Token: 0x06001484 RID: 5252 RVA: 0x000B4CB4 File Offset: 0x000B2EB4
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.npc.life <= 0)
			{
				for (int j = 0; j < 25; j++)
				{
					Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/小丑鱼碎块1"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/小丑鱼碎块2"), 1f);
			}
			base.npc.spriteDirection = ((base.npc.direction > 0) ? 1 : -1);
		}
	}
}
