using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Ocean.NPCs.VolCano
{
    public class VolcanoSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // // base.DisplayName.SetDefault("Abyss slime");
            Main.npcFrameCount[base.NPC.type] = 2;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "火山史莱姆");
        }
        public override void SetDefaults()
        {
            base.NPC.aiStyle = 1;
            base.NPC.damage = 150;
            base.NPC.width = 40;
            base.NPC.height = 30;
            base.NPC.defense = 70;
            base.NPC.lifeMax = 2870;
            base.NPC.knockBackResist = 0f;
            this.AnimationType = 81;
            base.NPC.value = (float)Item.buyPrice(0, 0, 5, 0);
            base.NPC.color = new Color(0, 0, 0, 0);
            base.NPC.alpha = 50;
            base.NPC.lavaImmune = false;
            base.NPC.noGravity = true;
            base.NPC.noTileCollide = false;
            base.NPC.HitSound = SoundID.NPCHit1;
            base.NPC.DeathSound = SoundID.NPCDeath1;
            this.Banner = base.NPC.type;
            this.BannerItem = base.Mod.Find<ModItem>("AbyssSlimeBanner").Type;
        }
        public override void AI()
        {
            if (base.NPC.velocity.Y >= 5f)
            {
                base.NPC.velocity.Y = 5f;
            }
            else
            {
                base.NPC.velocity.Y += 0.15f;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneVolcano)
            {
                return 5f;
            }
            return 0f;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
            Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)base.Mod.GetTexture("NPCs/火山史莱姆Glow").Width, (float)(base.Mod.GetTexture("NPCs/火山史莱姆Glow").Height / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = new Color(0.49f, 0.49f, 0.49f, 0);
            Main.spriteBatch.Draw(base.Mod.GetTexture("NPCs/火山史莱姆Glow"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.NPC.life <= 0)
            {
                for (int j = 0; j < 20; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
            }
        }
        public override void OnKill()
        {
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, Mod.Find<ModItem>("MeteorFlame").Type, 1, false, 0, false, false);
            }
            Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, Mod.Find<ModItem>("LavaStone").Type, Main.rand.Next(1, 4), false, 0, false, false);
        }
    }
}
