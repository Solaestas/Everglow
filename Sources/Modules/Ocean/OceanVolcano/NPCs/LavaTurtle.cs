using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
    public class LavaTurtle : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("lava tortoise");
			Main.npcFrameCount[base.npc.type] = 8;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "熔岩陆龟");
		}
		public override void SetDefaults()
		{
			base.npc.aiStyle = 39;
            base.npc.damage = 219;
			base.npc.width = 72;
			base.npc.height = 48;
			base.npc.defense = 140;
			base.npc.lifeMax = 2800;
			base.npc.knockBackResist = 0f;
			this.animationType = 39;
			base.npc.value = (float)Item.buyPrice(0, 0, 3, 0);
			base.npc.color = new Color(0, 0, 0, 0);
			base.npc.lavaImmune = true;
			base.npc.noGravity = true;
			base.npc.noTileCollide = false;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			//this.banner = base.npc.type;
			//this.bannerItem = base.mod.ItemType("熔岩陆龟Banner");
		}
        public override void AI()
        {
			if(base.npc.velocity.Y >= 5f)
			{
			    base.npc.velocity.Y = 5f;
			}
			else
			{
				base.npc.velocity.Y += 0.15f;
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/熔岩陆龟Glow").Width, (float)(base.mod.GetTexture("NPCs/熔岩陆龟Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = new Color(0.4f, 0.4f, 0.4f, 0);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/熔岩陆龟Glow"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneVolcano && !spawnInfo.water)
			{
				return 5f;
			}
			else
            {
                return 0f;
            }
		}
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.npc.life <= 0)
            {
                for (int j = 0; j < 20; j++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
            }
            if (base.npc.life <= 0)
            {
                float scaleFactor = (float)(Main.rand.Next(-8, 8) / 100f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/熔岩陆龟碎块"), 1f);
            }
        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("VolcanoBlade"), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("LavaStone"), Main.rand.Next(2, 5), false, 0, false, false);
        }
    }
}
