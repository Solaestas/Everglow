using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	public class OceanSlime1 : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("海蓝史莱姆前置");
			Main.npcFrameCount[base.NPC.type] = 4;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝史莱姆");
		}
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 14;
			base.NPC.damage = 132;
			base.NPC.width = 40;
			base.NPC.height = 30;
			base.NPC.defense = 65;
			base.NPC.lifeMax = 1300;
			base.NPC.knockBackResist = 0f;
			this.AnimationType = 121;
			base.NPC.alpha = 40;
			base.NPC.lavaImmune = false;
			base.NPC.noGravity = false;
			base.NPC.noTileCollide = false;
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
			base.NPC.buffImmune[24] = true;
		}
		public override void AI()
		{
			float num = 1.0025f;
			NPC npc = base.NPC;
			if(Math.Abs(npc.velocity.X) <= 2f)
			{
				npc.velocity.X = npc.velocity.X * num;
			}
			NPC npc2 = base.NPC;
			if(Math.Abs(npc.velocity.Y) <= 1f)
			{
				npc2.velocity.Y = npc2.velocity.Y * num;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            Player player = Main.player[Main.myPlayer];
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            if (mplayer.ZoneOcean && !player.wet)
            {
                return 0f;
            }
            else
            {
                return 0f;
            }
        }
		public override bool PreKill()
		{
			return false;
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != 1 && base.NPC.life <= 0)
			{
				Vector2 vector = base.NPC.Center + new Vector2(0f, (float)base.NPC.height / 2f);
				NPC.NewNPC((int)vector.X, (int)vector.Y, base.Mod.Find<ModNPC>("OceanSlime").Type, 0, 0f, 0f, 0f, 0f, 255);
			}
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
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.expertMode)
			{
				player.AddBuff(94, 60, true);
			}
		}
	}
}
