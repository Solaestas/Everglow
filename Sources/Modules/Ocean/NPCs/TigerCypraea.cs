using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
    public class TigerCypraea : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("Shell");
			Main.npcFrameCount[base.NPC.type] = 6;
			// base.DisplayName.AddTranslation(GameCulture.Chinese, "黑星宝螺");
		}
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 67;
			base.NPC.damage = 0;
			base.NPC.width = 50;
			base.NPC.height = 24;
			base.NPC.defense = 0;
			base.NPC.lifeMax = 10;
			base.NPC.knockBackResist = 0f;
			this.AnimationType = 81;
			base.NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.NPC.color = new Color(0, 0, 0, 0);
			base.NPC.alpha = 0;
			base.NPC.lavaImmune = false;
			base.NPC.noGravity = false;
			base.NPC.noTileCollide = false;
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
            {
                return 0.0002f;
            }
            return 0f;
        }
        public override void OnKill()
		{
			Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.Shell3>(), 1, false, 0, false, false);
		}
	}
}
