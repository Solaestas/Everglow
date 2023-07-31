using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs.VolCano
{
    public class DustDragon : ModNPC
	{
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("燃烬飞龙");
            Main.npcFrameCount[base.NPC.type] = 5;
        }
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 14;
			base.NPC.damage = 150;
			base.NPC.width = 40;
			base.NPC.height = 30;
			base.NPC.defense = 5;
			base.NPC.lifeMax = 2300;
			base.NPC.knockBackResist = 0.8f;
			this.AnimationType = 121;
			base.NPC.alpha = 75;
			base.NPC.lavaImmune = false;
			base.NPC.noGravity = false;
			base.NPC.noTileCollide = false;
			//base.npc.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 2000;
            base.NPC.buffImmune[24] = true;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneVolcano)
            {
                return 5f;
            }
            return 0f;
        }
        private int numk = 0;
		public override void AI()
		{
            NPC npc = base.NPC;
            float num = 1.0025f;
			NPC npc2 = base.NPC;
			if(Math.Abs(npc.velocity.X) <= 1.5f)
			{
				npc.velocity.X = npc.velocity.X * num;
			}
			else
			{
                npc.velocity.X = npc.velocity.X / num;
			}
			if(Math.Abs(npc2.velocity.Y) <= 1.5f)
			{
				npc2.velocity.Y = npc.velocity.Y * num;
			}
			else
			{
                npc2.velocity.Y = npc2.velocity.Y / num;
			}
            npc.localAI[0] += 1;
		}
		public override bool PreKill()
		{
            if(Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.MeltingStaff>(), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.LavaStone>(), Main.rand.Next(1, 4), false, 0, false, false);
            return true;
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 4, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
		}
	}
}
