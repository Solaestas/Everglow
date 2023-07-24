using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythMod.NPCs
{
    public class WaterBomb : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("WaterBoom");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "水雷");
			Main.npcFrameCount[base.npc.type] = 3;
		}
		public override void SetDefaults()
		{
			base.npc.damage = 120;
			base.npc.width = 54;
			base.npc.height = 54;
			base.npc.defense = 0;
            base.npc.lifeMax = 1;
			base.npc.knockBackResist = 0f;
			base.npc.alpha = 0;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
		}
        private int u = 0;

        public override void AI()
        {
            base.npc.spriteDirection = -1;
            u += 1;
            base.npc.velocity.Y = (float)Math.Sin((float)u / 105f * Math.PI);
        }
        public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 0;
			base.npc.frameCounter %= (double)Main.npcFrameCount[base.npc.type];
			int num = (int)base.npc.frameCounter;
			base.npc.frame.Y = num * frameHeight;
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
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
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.velocity = (npc.velocity - player.velocity) / (npc.velocity - player.velocity).Length() * 5;
            Projectile.NewProjectile(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f, 0f, 0f, 164, 10, 4f, Main.myPlayer, 0f, 0f);
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块1"), 1f);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块2"), 1f);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块3"), 1f);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块4"), 1f);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            Projectile.NewProjectile(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f, 0f, 0f, 164, 150, 4f, Main.myPlayer, 0f, 0f);
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块1"), 1f);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块2"), 1f);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块3"), 1f);
            Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/水雷碎块4"), 1f);
        }
		public override bool CheckActive()
		{
			return false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneOcean && spawnInfo.water)
			{
				return 0.7f;
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
	}
}
