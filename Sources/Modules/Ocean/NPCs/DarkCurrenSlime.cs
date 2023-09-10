using System;
using Everglow.Ocean.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	// Token: 0x02000487 RID: 1159
    public class DarkCurrenSlime : ModNPC
	{
		// Token: 0x06001808 RID: 6152 RVA: 0x00009BEC File Offset: 0x00007DEC
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("Abyss slime");
			Main.npcFrameCount[base.NPC.type] = 2;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "深渊暗流史莱姆");
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0010AD00 File Offset: 0x00108F00
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 1;
            base.NPC.damage = 219;
			base.NPC.width = 40;
			base.NPC.height = 30;
			base.NPC.defense = 140;
			base.NPC.lifeMax = 2870;
			base.NPC.knockBackResist = 0f;
			this.AnimationType = 81;
			base.NPC.value = (float)Item.buyPrice(0, 0, 3, 0);
			base.NPC.color = new Color(0, 0, 0, 0);
			base.NPC.alpha = 50;
			base.NPC.lavaImmune = false;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = false;
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
			this.Banner = base.NPC.type;
			this.BannerItem = ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.AbyssSlimeBanner>();
		}
        public override void AI()
        {
			if(base.NPC.velocity.Y >= 5f)
			{
			    base.NPC.velocity.Y = 5f;
			}
			else
			{
				base.NPC.velocity.Y += 0.15f;
			}
		}
		// Token: 0x06001B17 RID: 6935 RVA: 0x0000B428 File Offset: 0x00009628
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
			{
				return 0.012f;
			}
			else
            {
                return 0f;
            }
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0010AE44 File Offset: 0x00109044
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0010AEF8 File Offset: 0x001090F8
		public override void OnKill()
		{
            Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.AbyssOre>(), Main.rand.Next(8, 15), false, 0, false, false);
		}
	}
}
