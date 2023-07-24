 using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
    public class VolcanoSpicySlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Lava Slime");
			Main.npcFrameCount[base.npc.type] = 2;
			base.DisplayName.AddTranslation(GameCulture.Chinese, "尖刺火山史莱姆");
		}
		public override void SetDefaults()
		{
			base.npc.aiStyle = 1;
			base.npc.damage = 168;
			base.npc.width = 40;
			base.npc.height = 30;
			base.npc.defense = 10;
			base.npc.lifeMax = 3500;
			base.npc.knockBackResist = 0f;
			this.animationType = 81;
			base.npc.value = (float)Item.buyPrice(0, 1, 0, 0);
			base.npc.alpha = 25;
			base.npc.lavaImmune = false;
			base.npc.noGravity = false;
			base.npc.noTileCollide = false;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			base.npc.buffImmune[24] = true;
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/尖刺史莱姆1Glow").Width, (float)(base.mod.GetTexture("NPCs/尖刺史莱姆1Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = new Color(0.49f, 0.49f, 0.49f, 0);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/尖刺史莱姆1Glow"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
        public override void AI()
		{
			if (this.spikeTimer > 0f)
			{
				this.spikeTimer -= 1f;
			}
			if (!base.npc.wet)
			{
				Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
				float num = Main.player[base.npc.target].position.X + (float)Main.player[base.npc.target].width * 0.5f - vector.X;
				float num2 = Main.player[base.npc.target].position.Y - vector.Y;
				float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				if (Main.expertMode && num3 < 120f && Collision.CanHit(base.npc.position, base.npc.width, base.npc.height, Main.player[base.npc.target].position, Main.player[base.npc.target].width, Main.player[base.npc.target].height) && base.npc.velocity.Y == 0f)
				{
					base.npc.ai[0] = -40f;
					if (base.npc.velocity.Y == 0f)
					{
						base.npc.velocity.X = base.npc.velocity.X * 0.9f;
					}
					if (Main.netMode != 1 && this.spikeTimer == 0f)
					{
						for (int i = 0; i < 5; i++)
						{
							Vector2 value = new Vector2((float)(i - 2), -4f);
							value.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							value.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							value.Normalize();
							value *= 4f + (float)Main.rand.Next(-50, 51) * 0.01f;
							Projectile.NewProjectile(vector.X, vector.Y, value.X, value.Y, base.mod.ProjectileType("烈火球"), 192, 0f, Main.myPlayer, 0f, 0f);
							this.spikeTimer = 30f;
						}
						return;
					}
				}
				else if (num3 < 360f && Collision.CanHit(base.npc.position, base.npc.width, base.npc.height, Main.player[base.npc.target].position, Main.player[base.npc.target].width, Main.player[base.npc.target].height) && base.npc.velocity.Y == 0f)
				{
					base.npc.ai[0] = -40f;
					if (base.npc.velocity.Y == 0f)
					{
						base.npc.velocity.X = base.npc.velocity.X * 0.9f;
					}
					if (Main.netMode != 1 && this.spikeTimer == 0f)
					{
						num2 = Main.player[base.npc.target].position.Y - vector.Y - (float)Main.rand.Next(0, 200);
						num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
						num3 = 6.5f / num3;
						num *= num3;
						num2 *= num3;
						this.spikeTimer = 50f;
                        Projectile.NewProjectile(vector.X, vector.Y, num, num2, base.mod.ProjectileType("烈火球"), 155, 0f, Main.myPlayer, 0f, 0f);
					}
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 4, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.npc.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 4, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneVolcano)
			{
				return 5f;
			}
			return 0f;
		}
		public override void NPCLoot()
		{
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("LavaCupStaff"), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("LavaStone"), Main.rand.Next(1,4), false, 0, false, false);
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode)
			{
				player.AddBuff(24, 540, true);
			}
		}
		public float spikeTimer = 60f;
	}
}
