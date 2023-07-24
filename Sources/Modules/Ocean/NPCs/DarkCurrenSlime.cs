using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
	// Token: 0x02000487 RID: 1159
    public class DarkCurrenSlime : ModNPC
	{
		// Token: 0x06001808 RID: 6152 RVA: 0x00009BEC File Offset: 0x00007DEC
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Abyss slime");
			Main.npcFrameCount[base.npc.type] = 2;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "深渊暗流史莱姆");
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0010AD00 File Offset: 0x00108F00
		public override void SetDefaults()
		{
			base.npc.aiStyle = 1;
            base.npc.damage = 219;
			base.npc.width = 40;
			base.npc.height = 30;
			base.npc.defense = 140;
			base.npc.lifeMax = 2870;
			base.npc.knockBackResist = 0f;
			this.animationType = 81;
			base.npc.value = (float)Item.buyPrice(0, 0, 3, 0);
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
			if(base.npc.velocity.Y >= 5f)
			{
			    base.npc.velocity.Y = 5f;
			}
			else
			{
				base.npc.velocity.Y += 0.15f;
			}
		}
		// Token: 0x06001B17 RID: 6935 RVA: 0x0000B428 File Offset: 0x00009628
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneOcean && spawnInfo.water)
			{
				return 0.012f;
			}
			else
            {
                return 0f;
            }
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0010AE44 File Offset: 0x00109044
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

		// Token: 0x0600180C RID: 6156 RVA: 0x0010AEF8 File Offset: 0x001090F8
		public override void NPCLoot()
		{
            Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("AbyssOre"), Main.rand.Next(8, 15), false, 0, false, false);
		}
	}
}
