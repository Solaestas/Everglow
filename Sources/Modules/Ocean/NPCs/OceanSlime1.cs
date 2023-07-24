using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
	public class OceanSlime1 : ModNPC
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蓝史莱姆前置");
			Main.npcFrameCount[base.npc.type] = 4;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝史莱姆");
		}
		public override void SetDefaults()
		{
			base.npc.aiStyle = 14;
			base.npc.damage = 132;
			base.npc.width = 40;
			base.npc.height = 30;
			base.npc.defense = 65;
			base.npc.lifeMax = 1300;
			base.npc.knockBackResist = 0f;
			this.animationType = 121;
			base.npc.alpha = 40;
			base.npc.lavaImmune = false;
			base.npc.noGravity = false;
			base.npc.noTileCollide = false;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			base.npc.buffImmune[24] = true;
		}
		public override void AI()
		{
			float num = 1.0025f;
			NPC npc = base.npc;
			if(Math.Abs(npc.velocity.X) <= 2f)
			{
				npc.velocity.X = npc.velocity.X * num;
			}
			NPC npc2 = base.npc;
			if(Math.Abs(npc.velocity.Y) <= 1f)
			{
				npc2.velocity.Y = npc2.velocity.Y * num;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            Player player = Main.player[Main.myPlayer];
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (mplayer.ZoneOcean && !player.wet)
            {
                return 0f;
            }
            else
            {
                return 0f;
            }
        }
		public override bool PreNPCLoot()
		{
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 1 && base.npc.life <= 0)
			{
				Vector2 vector = base.npc.Center + new Vector2(0f, (float)base.npc.height / 2f);
				NPC.NewNPC((int)vector.X, (int)vector.Y, base.mod.NPCType("OceanSlime"), 0, 0f, 0f, 0f, 0f, 255);
			}
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
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode)
			{
				player.AddBuff(94, 60, true);
			}
		}
	}
}
