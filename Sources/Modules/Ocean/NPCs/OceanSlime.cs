using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
    public class OceanSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // // base.DisplayName.SetDefault("Ocean Slime");
            Main.npcFrameCount[base.NPC.type] = 2;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝史莱姆");
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = Main.player[Main.myPlayer];
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
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
            base.NPC.aiStyle = 1;
            base.NPC.damage = 182;
            base.NPC.width = 40;
            base.NPC.height = 30;
            base.NPC.defense = 90;
            base.NPC.lifeMax = 1950;
            base.NPC.knockBackResist = 0f;
            this.AnimationType = 81;
            base.NPC.value = (float)Item.buyPrice(0, 0, 3, 0);
            base.NPC.color = new Color(0, 0, 0, 0);
            base.NPC.alpha = 50;
            base.NPC.lavaImmune = false;
            base.NPC.noGravity = false;
            base.NPC.noTileCollide = false;
            base.NPC.HitSound = SoundID.NPCHit1;
            base.NPC.DeathSound = SoundID.NPCDeath1;
            this.Banner = base.NPC.type;
            this.BannerItem = base.Mod.Find<ModItem>("OceanSlimeBanner").Type;
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
        public override void AI()
        {
            NPC.ai[0] += 0.05f;
        }
        public override void OnKill()
        {
            Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, base.Mod.Find<ModItem>("OceanBlueOre").Type, Main.rand.Next(7, 13), false, 0, false, false);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
           /*spriteBatch.End();
           spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
           Everglow.Ocean.npcEffect2.CurrentTechnique.Passes["Test"].Apply();*/
           return true;
        }
    }
}
