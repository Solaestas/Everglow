using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs.Ocean
{
	// Token: 0x020004EB RID: 1259
    public class DeepOceanCrab : ModNPC
	{
		// Token: 0x06001B17 RID: 6935 RVA: 0x0000B428 File Offset: 0x00009628
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("深海蟹");
			Main.npcFrameCount[base.NPC.type] = 8;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "深海蟹");
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}
		// Token: 0x06001B18 RID: 6936 RVA: 0x0014B828 File Offset: 0x00149A28
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 3;
			base.NPC.damage = 20;
			base.NPC.width = 44;
			base.NPC.height = 34;
			base.NPC.defense = 5;
			base.NPC.lifeMax = 23;
			base.NPC.knockBackResist = 0.8f;
			this.AnimationType = 67;
			base.NPC.lavaImmune = false;
			base.NPC.noGravity = false;
			base.NPC.noTileCollide = false;
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
			base.NPC.buffImmune[24] = true;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0014B900 File Offset: 0x00149B00
		public override void AI()
		{
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000037AF File Offset: 0x000019AF
		public override bool PreKill()
		{
			return false;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0014B944 File Offset: 0x00149B44
		public override void HitEffect(NPC.HitInfo hit)
		{

		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0000B461 File Offset: 0x00009661
	}
}
