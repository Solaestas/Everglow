using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	// Token: 0x02000421 RID: 1057
    public class FlagFish : ModNPC
	{
		// Token: 0x0600147D RID: 5245 RVA: 0x0000832F File Offset: 0x0000652F
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("Sailfish");
			Main.npcFrameCount[base.NPC.type] = 4;
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "旗鱼");
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x000B4364 File Offset: 0x000B2564
		public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 88;
			base.NPC.width = 189;
			base.NPC.height = 70;
			base.NPC.defense = 25;
			base.NPC.lifeMax = 2300;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
			for (int i = 0; i < base.NPC.buffImmune.Length; i++)
			{
				base.NPC.buffImmune[i] = true;
			}
			base.NPC.value = (float)Item.buyPrice(0, 1, 6, 0);
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath40;
			base.NPC.knockBackResist = 0.8f;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x000B4440 File Offset: 0x000B2640
		public override void AI()
		{
			base.NPC.spriteDirection = ((base.NPC.direction > 0) ? 1 : -1);
			base.NPC.noGravity = true;
			base.NPC.chaseable = this.hasBeenHit;
			if (base.NPC.direction == 0)
			{
				base.NPC.TargetClosest(true);
			}
			if (base.NPC.justHit)
			{
				this.hasBeenHit = true;
			}
			if (base.NPC.wet)
			{
				bool flag = this.hasBeenHit;
				base.NPC.TargetClosest(false);
				if (Main.player[base.NPC.target].wet && !Main.player[base.NPC.target].dead && (Main.player[base.NPC.target].Center - base.NPC.Center).Length() < 200f && NPC.life < NPC.lifeMax)
				{
					flag = true;
				}
				if (Main.player[base.NPC.target].dead && flag)
				{
					flag = false;
				}
				if (!flag)
				{
					if (base.NPC.collideX)
					{
						base.NPC.velocity.X = base.NPC.velocity.X * -7f;
						base.NPC.direction *= -1;
						base.NPC.netUpdate = true;
					}
					if (base.NPC.collideY)
					{
						base.NPC.netUpdate = true;
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
				}
				if (flag)
				{
					base.NPC.TargetClosest(true);
					base.NPC.velocity.X = base.NPC.velocity.X + (float)base.NPC.direction * 2.5f;
					base.NPC.velocity.Y = base.NPC.velocity.Y + (float)base.NPC.directionY * 1.5f;
					if (base.NPC.velocity.X > 25f)
					{
						base.NPC.velocity.X = 25f;
					}
					if (base.NPC.velocity.X < -25f)
					{
						base.NPC.velocity.X = -25f;
					}
					if (base.NPC.velocity.Y > 10f)
					{
						base.NPC.velocity.Y = 10f;
					}
					if (base.NPC.velocity.Y < -10f)
					{
						base.NPC.velocity.Y = -10f;
					}
				}
				else
				{
					base.NPC.velocity.X = base.NPC.velocity.X + (float)base.NPC.direction * 0.1f;
					if (base.NPC.velocity.X < -2.5f || base.NPC.velocity.X > 2.5f)
					{
						base.NPC.velocity.X = base.NPC.velocity.X * 0.95f;
					}
					if (base.NPC.ai[0] == -1f)
					{
						base.NPC.velocity.Y = base.NPC.velocity.Y - 0.01f;
						if ((double)base.NPC.velocity.Y < -0.3)
						{
							base.NPC.ai[0] = 1f;
						}
					}
					else
					{
						base.NPC.velocity.Y = base.NPC.velocity.Y + 0.01f;
						if ((double)base.NPC.velocity.Y > 0.3)
						{
							base.NPC.ai[0] = -1f;
						}
					}
				}
				int num = (int)(base.NPC.position.X + (float)(base.NPC.width / 2)) / 16;
				int num2 = (int)(base.NPC.position.Y + (float)(base.NPC.height / 2)) / 16;
				if (Main.tile[num, num2 - 1] == null)
				{
					Main.tile[num, num2 - 1] = new Tile();
				}
				if (Main.tile[num, num2 + 1] == null)
				{
					Main.tile[num, num2 + 1] = new Tile();
				}
				if (Main.tile[num, num2 + 2] == null)
				{
					Main.tile[num, num2 + 2] = new Tile();
				}
				if (Main.tile[num, num2 - 1].LiquidAmount > 128)
				{
					if (Main.tile[num, num2 + 1].HasTile)
					{
						base.NPC.ai[0] = -1f;
					}
					else if (Main.tile[num, num2 + 2].HasTile)
					{
						base.NPC.ai[0] = -1f;
					}
				}
				if ((double)base.NPC.velocity.Y > 0.4 || (double)base.NPC.velocity.Y < -0.4)
				{
					base.NPC.velocity.Y = base.NPC.velocity.Y * 0.95f;
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

		// Token: 0x06001480 RID: 5248 RVA: 0x000B4BEC File Offset: 0x000B2DEC
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.minion)
			{
				return new bool?(this.hasBeenHit);
			}
			return null;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
            {
                return 0.01f;
            }
            else
            {
                return 0f;
            }
        }
        // Token: 0x06001481 RID: 5249 RVA: 0x000B4C18 File Offset: 0x000B2E18
        public override void FindFrame(int frameHeight)
		{
			if (!base.NPC.wet)
			{
				base.NPC.frameCounter = 0.0;
				return;
			}
			base.NPC.frameCounter += (double)(this.hasBeenHit ? 0.15f : 0.075f);
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
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
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 115, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 25; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 115, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
                for (int j = 0; j < 25; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 117, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/旗鱼碎块1"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/旗鱼碎块2"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/旗鱼碎块3"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/旗鱼碎块4"), 1f);
			}
		}
        public override void OnKill()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.空灵泡>(), 1, false, 0, false, false);
            }
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.利刃鳞>(), 1, false, 0, false, false);
            }
        }
        public bool hasBeenHit;
	}
}
