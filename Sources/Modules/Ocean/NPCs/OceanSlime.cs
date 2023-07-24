using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
    public class OceanSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Ocean Slime");
            Main.npcFrameCount[base.npc.type] = 2;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝史莱姆");
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = Main.player[Main.myPlayer];
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (mplayer.ZoneOcean && !player.wet)
            {
                return 0.02f;
            }
            else
            {
                return 0f;
            }
        }
        public override void SetDefaults()
        {
            base.npc.aiStyle = 1;
            base.npc.damage = 182;
            base.npc.width = 40;
            base.npc.height = 30;
            base.npc.defense = 90;
            base.npc.lifeMax = 1950;
            base.npc.knockBackResist = 0f;
            this.animationType = 81;
            base.npc.value = (float)Item.buyPrice(0, 0, 3, 0);
            base.npc.color = new Color(0, 0, 0, 0);
            base.npc.alpha = 50;
            base.npc.lavaImmune = false;
            base.npc.noGravity = false;
            base.npc.noTileCollide = false;
            base.npc.HitSound = SoundID.NPCHit1;
            base.npc.DeathSound = SoundID.NPCDeath1;
            this.banner = base.npc.type;
            this.bannerItem = base.mod.ItemType("OceanSlimeBanner");
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
        public override void AI()
        {
            npc.ai[0] += 0.05f;
        }
        public override void NPCLoot()
        {
            Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("OceanBlueOre"), Main.rand.Next(7, 13), false, 0, false, false);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
           /*spriteBatch.End();
           spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
           MythMod.npcEffect2.CurrentTechnique.Passes["Test"].Apply();*/
           return true;
        }
    }
}
