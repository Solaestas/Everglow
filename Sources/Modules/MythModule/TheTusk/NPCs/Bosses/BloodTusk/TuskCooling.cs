using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk
{
    public class TuskCooling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override void SetDefaults()
        {
            NPC.behindTiles = true;
            NPC.damage = 0;
            NPC.width = 30;
            NPC.height = 30;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.aiStyle = -1;
            NPC.alpha = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Musics/TuskBiome");
            }
        }
        public override void AI()
        {
            if (NPC.CountNPCS(NPC.type) > 1)
            {
                NPC.active = false;
            }
            NPC.localAI[0] += 1;
            if (NPC.localAI[0] > 900)
            {
                NPC.active = false;
            }
        }
    }
}
