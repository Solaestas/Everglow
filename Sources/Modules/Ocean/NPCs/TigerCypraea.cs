using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
    public class TigerCypraea : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Shell");
			Main.npcFrameCount[base.npc.type] = 6;
			base.DisplayName.AddTranslation(GameCulture.Chinese, "黑星宝螺");
		}
		public override void SetDefaults()
		{
			base.npc.aiStyle = 67;
			base.npc.damage = 0;
			base.npc.width = 50;
			base.npc.height = 24;
			base.npc.defense = 0;
			base.npc.lifeMax = 10;
			base.npc.knockBackResist = 0f;
			this.animationType = 81;
			base.npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.npc.color = new Color(0, 0, 0, 0);
			base.npc.alpha = 0;
			base.npc.lavaImmune = false;
			base.npc.noGravity = false;
			base.npc.noTileCollide = false;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe)
            {
                return 0f;
            }
            if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneOcean && spawnInfo.water)
            {
                return 0.0002f;
            }
            return 0f;
        }
        public override void NPCLoot()
		{
			Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("Shell3"), 1, false, 0, false, false);
		}
	}
}
