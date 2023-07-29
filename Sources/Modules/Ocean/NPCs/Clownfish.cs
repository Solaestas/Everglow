using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Everglow.Ocean.NPCs
{
	// Token: 0x02000421 RID: 1057
    public class Clownfish : ModNPC
	{
		// Token: 0x0600147D RID: 5245 RVA: 0x0000832F File Offset: 0x0000652F
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("Sailfish");
			Main.npcFrameCount[base.NPC.type] = 4;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "小丑鱼");
		}
        private bool flag = true;
        private int num = 0;
        private Vector2 v = new Vector2(0, 0);
        // Token: 0x0600147E RID: 5246 RVA: 0x000B4364 File Offset: 0x000B2564
        public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 0;
			base.NPC.width = 44;
			base.NPC.height = 28;
			base.NPC.defense = 0;
			base.NPC.lifeMax = 10;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
			for (int i = 0; i < base.NPC.buffImmune.Length; i++)
			{
				base.NPC.buffImmune[i] = true;
			}
			base.NPC.value = (float)Item.buyPrice(0, 1, 6, 0);
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath40;
			base.NPC.knockBackResist = 0.2f;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x000B4440 File Offset: 0x000B2640
		public override void AI()
		{
            num += 1;
            base.NPC.direction = (int)(NPC.velocity.X / Math.Abs(NPC.velocity.X));
            base.NPC.spriteDirection = ((base.NPC.direction > 0) ? 1 : -1);
            base.NPC.noGravity = true;
            if (base.NPC.direction == 0)
			{
				base.NPC.TargetClosest(false);
			}
            if(flag)
            {
                NPC.scale = Main.rand.Next(800, 1200) / 1000f;
                Vector2 v = NPC.Center;
                NPC.velocity.X = (1 - (Main.rand.Next(0, 1) * 2)) * 0.8f;
                flag = false;
            }
			if (base.NPC.wet)
			{
                NPC.velocity.Y = (float)Math.Sin(num / 50f) * 0.15f;
                if(num % 1200 == 1)
                {
                    NPC.velocity *= -1;
                }
                if (base.NPC.collideX)
                {
                    if (NPC.velocity.X < 0)
                    {
                        NPC.position.X += 5f;
                        base.NPC.velocity.X = 0.8f;
                    }
                    else
                    {
                        NPC.position.X -= 5f;
                        base.NPC.velocity.X = -0.8f;
                    }
                    base.NPC.direction *= -1;
                }
            }
			else
			{
				if (base.NPC.velocity.Y == 0f)
				{
					base.NPC.velocity.X = base.NPC.velocity.X * 0.94f;
					if ((double)base.NPC.velocity.X > -0.2 && (double)base.NPC.velocity.X < 0.2)
					{
						base.NPC.velocity.X = 0f;
					}
				}
				base.NPC.velocity.Y = base.NPC.velocity.Y + 0.4f;
				if (base.NPC.velocity.Y > 12f)
				{
					base.NPC.velocity.Y = 12f;
				}
				base.NPC.ai[0] = 1f;
			}
			base.NPC.rotation = base.NPC.velocity.Y * (float)base.NPC.direction * 0.1f;
			if ((double)base.NPC.rotation < -0.2)
			{
				base.NPC.rotation = -0.2f;
			}
			if ((double)base.NPC.rotation > 0.2)
			{
				base.NPC.rotation = 0.2f;
			}
		}


		// Token: 0x06001481 RID: 5249 RVA: 0x000B4C18 File Offset: 0x000B2E18
		public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += (double)(0.15f);
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
			{
				return 0.2f;
			}
			return 0f;
		}
		// Token: 0x06001482 RID: 5250 RVA: 0x00008065 File Offset: 0x00006265
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			player.AddBuff(30, 420, true);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00007FE0 File Offset: 0x000061E0

		// Token: 0x06001484 RID: 5252 RVA: 0x000B4CB4 File Offset: 0x000B2EB4
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 25; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/小丑鱼碎块1"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/小丑鱼碎块2"), 1f);
			}
			base.NPC.spriteDirection = ((base.NPC.direction > 0) ? 1 : -1);
		}
	}
}
