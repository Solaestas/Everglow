using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Everglow.Ocean.NPCs
{
    public class WaterBomb : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("WaterBoom");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "水雷");
			Main.npcFrameCount[base.NPC.type] = 3;
		}
		public override void SetDefaults()
		{
			base.NPC.damage = 120;
			base.NPC.width = 54;
			base.NPC.height = 54;
			base.NPC.defense = 0;
            base.NPC.lifeMax = 1;
			base.NPC.knockBackResist = 0f;
			base.NPC.alpha = 0;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = true;
		}
        private int u = 0;

        public override void AI()
        {
            base.NPC.spriteDirection = -1;
            u += 1;
            base.NPC.velocity.Y = (float)Math.Sin((float)u / 105f * Math.PI);
        }
        public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += 0;
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (projectile.penetrate == -1)
			{
				damage = (int)((double)damage * 2);
				return;
			}
			if (projectile.penetrate > 1)
			{
				damage = (int)((double)damage * 4);
				return;
			}
			projectile.penetrate = 1;
		}
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            player.velocity = (NPC.velocity - player.velocity) / (NPC.velocity - player.velocity).Length() * 5;
            Projectile.NewProjectile(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f, 0f, 0f, 164, 10, 4f, Main.myPlayer, 0f, 0f);
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块1"), 1f);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块2"), 1f);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块3"), 1f);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块4"), 1f);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            Projectile.NewProjectile(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f, 0f, 0f, 164, 150, 4f, Main.myPlayer, 0f, 0f);
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块1"), 1f);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块2"), 1f);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块3"), 1f);
            Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/水雷碎块4"), 1f);
        }
		public override bool CheckActive()
		{
			return false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
			{
				return 0.7f;
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
	}
}
