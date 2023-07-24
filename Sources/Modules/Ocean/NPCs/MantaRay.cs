using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace MythMod.NPCs
{
    public class MantaRay : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sailfish");
			Main.npcFrameCount[base.npc.type] = 7;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "斑点魟");
		}
		public override void SetDefaults()
		{
			base.npc.noGravity = true;
			base.npc.damage = 140;
			base.npc.width = 100;
			base.npc.height = 68;
			base.npc.defense = 25;
			base.npc.lifeMax = 1400;
			base.npc.aiStyle = 16;
			this.aiType = -1;
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
			base.npc.value = (float)Item.buyPrice(0, 1, 6, 0);
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath40;
			base.npc.knockBackResist = 0.2f;
            //this.banner = base.npc.type;
            //this.bannerItem = base.mod.ItemType("斑点魟Banner");
        }
		public override void AI()
		{
			base.npc.spriteDirection = ((base.npc.direction > 0) ? 1 : -1);
			base.npc.noGravity = true;
			base.npc.chaseable = this.hasBeenHit;
			if (base.npc.direction == 0)
			{
				base.npc.TargetClosest(true);
			}
			if (base.npc.justHit)
			{
				this.hasBeenHit = true;
			}
			if (base.npc.wet)
			{
				bool flag = this.hasBeenHit;
				base.npc.TargetClosest(false);
				if (Main.player[base.npc.target].wet && !Main.player[base.npc.target].dead && (Main.player[base.npc.target].Center - base.npc.Center).Length() < 200f)
				{
					flag = true;
				}
				if (Main.player[base.npc.target].dead && flag)
				{
					flag = false;
				}
				if (!flag)
				{
					if (base.npc.collideX)
					{
						base.npc.velocity.X = base.npc.velocity.X * -10f;
						base.npc.direction *= -1;
						base.npc.netUpdate = true;
					}
					if (base.npc.collideY)
					{
						base.npc.netUpdate = true;
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
				}
				if (flag)
				{
					base.npc.TargetClosest(true);
					base.npc.velocity.X = base.npc.velocity.X + (float)base.npc.direction * 0.5f;
					base.npc.velocity.Y = base.npc.velocity.Y + (float)base.npc.directionY * 0.1f;
					if (base.npc.velocity.X > 11f)
					{
						base.npc.velocity.X = 11f;
					}
					if (base.npc.velocity.X < -11f)
					{
						base.npc.velocity.X = -11f;
					}
					if (base.npc.velocity.Y > 8f)
					{
						base.npc.velocity.Y = 8f;
					}
					if (base.npc.velocity.Y < -8f)
					{
						base.npc.velocity.Y = -8f;
					}
				}
				else
				{
					base.npc.velocity.X = base.npc.velocity.X + (float)base.npc.direction * 0.1f;
					if (base.npc.velocity.X < -6.5f || base.npc.velocity.X > 6.5f)
					{
						base.npc.velocity.X = base.npc.velocity.X * 0.95f;
					}
					if (base.npc.ai[0] == -1f)
					{
						base.npc.velocity.Y = base.npc.velocity.Y - 0.01f;
						if ((double)base.npc.velocity.Y < -0.3)
						{
							base.npc.ai[0] = 1f;
						}
					}
					else
					{
						base.npc.velocity.Y = base.npc.velocity.Y + 0.01f;
						if ((double)base.npc.velocity.Y > 1.5f)
						{
							base.npc.ai[0] = -1f;
						}
					}
				}
				int num = (int)(base.npc.position.X + (float)(base.npc.width / 2)) / 16;
				int num2 = (int)(base.npc.position.Y + (float)(base.npc.height / 2)) / 16;
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
				if (Main.tile[num, num2 - 1].liquid > 128)
				{
					if (Main.tile[num, num2 + 1].active())
					{
						base.npc.ai[0] = -1f;
					}
					else if (Main.tile[num, num2 + 2].active())
					{
						base.npc.ai[0] = -1f;
					}
				}
				if ((double)base.npc.velocity.Y > 0.4 || (double)base.npc.velocity.Y < -0.4)
				{
					base.npc.velocity.Y = base.npc.velocity.Y * 0.95f;
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
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.minion)
			{
				return new bool?(this.hasBeenHit);
			}
			return null;
		}
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += (double)(this.hasBeenHit ? 0.15f : 0.075f);
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
				return 0.1f;
			}
			return 0f;
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(30, 420, true);
		}
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
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/斑点魟碎块"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/斑点魟碎块2"), 1f);
			}
			base.npc.spriteDirection = ((base.npc.direction > 0) ? 1 : -1);
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/斑点魟发光部分").Width, (float)(base.mod.GetTexture("NPCs/斑点魟发光部分").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.npc.alpha, 97 - base.npc.alpha, 97 - base.npc.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/斑点魟发光部分"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
		public bool hasBeenHit;
		public override void NPCLoot()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("OceanDustCore"), 1, false, 0, false, false);
            }
        }
	}
}
