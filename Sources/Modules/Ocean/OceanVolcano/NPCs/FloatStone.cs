using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
	public class FloatStone : ModNPC
	{
		private int num1 = 0;
		private int num2;
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("火山浮石");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "火山浮石");
		}
		public override void SetDefaults()
		{
			base.npc.damage = 90;
			base.npc.width = 50;
			base.npc.height = 50;
			base.npc.defense = 65;
			base.npc.lifeMax = 4000;
			base.npc.knockBackResist = 0.4f;
			base.npc.alpha = 0;
			base.npc.lavaImmune = true;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
            base.npc.aiStyle = -1;
        }
		public override void AI()
		{
			num1 += 1;
            Player player = Main.player[Main.myPlayer];
            Vector2 playerposition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
			float num4 = player.position.X - vector.X;
            float num5 = player.position.Y - vector.Y;
            float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
            if(npc.ai[3] <= 0)
            {
                npc.velocity += new Vector2(num4, num5) / num6 * 0.1f;
                if (npc.velocity.Length() < 3)
                {
                    npc.velocity *= 1.05f;
                }
                if (npc.velocity.Length() > 5)
                {
                    npc.velocity *= 0.95f;
                }
            }
            else
            {
                npc.ai[3] -= 1;
                npc.velocity += new Vector2(npc.ai[1], npc.ai[2]);
            }
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneVolcano)
			{
				return 0.6f;
			}
            else
            {
                return 0f;
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/火山浮石发光部分").Width, (float)(base.mod.GetTexture("NPCs/火山浮石发光部分").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.npc.alpha, 97 - base.npc.alpha, 97 - base.npc.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/火山浮石发光部分"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
		public override void HitEffect(int hitDirection, double damage)
        {
			Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 10, 1f, 0f);
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            for (int j = 0; j < 3; j++)
            {
                Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.npc.life <= 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
				for (int j = 0; j < 25; j++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
                float scaleFactor = (float)(Main.rand.Next(-8, 8) / 100f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块1"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块2"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块3"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块4"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                int num3 = 0;
            }
        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("FlameEdge"), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("LavaStone"), Main.rand.Next(1, 4), false, 0, false, false);
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		}
	}
}
