using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace MythMod.NPCs.VolCano
{
    public class VolcanoSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Abyss slime");
            Main.npcFrameCount[base.npc.type] = 2;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "火山史莱姆");
        }
        public override void SetDefaults()
        {
            base.npc.aiStyle = 1;
            base.npc.damage = 150;
            base.npc.width = 40;
            base.npc.height = 30;
            base.npc.defense = 70;
            base.npc.lifeMax = 2870;
            base.npc.knockBackResist = 0f;
            this.animationType = 81;
            base.npc.value = (float)Item.buyPrice(0, 0, 5, 0);
            base.npc.color = new Color(0, 0, 0, 0);
            base.npc.alpha = 50;
            base.npc.lavaImmune = false;
            base.npc.noGravity = true;
            base.npc.noTileCollide = false;
            base.npc.HitSound = SoundID.NPCHit1;
            base.npc.DeathSound = SoundID.NPCDeath1;
            this.banner = base.npc.type;
            this.bannerItem = base.mod.ItemType("AbyssSlimeBanner");
        }
        public override void AI()
        {
            if (base.npc.velocity.Y >= 5f)
            {
                base.npc.velocity.Y = 5f;
            }
            else
            {
                base.npc.velocity.Y += 0.15f;
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/火山史莱姆Glow").Width, (float)(base.mod.GetTexture("NPCs/火山史莱姆Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = new Color(0.49f, 0.49f, 0.49f, 0);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/火山史莱姆Glow"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
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
        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("MeteorFlame"), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("LavaStone"), Main.rand.Next(1, 4), false, 0, false, false);
        }
    }
}
