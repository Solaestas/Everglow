using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythMod.NPCs
{
	// Token: 0x020004FC RID: 1276
    public class BigWaterBomb : ModNPC
	{
		// Token: 0x06001BA4 RID: 7076 RVA: 0x0000B6E0 File Offset: 0x000098E0
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("WaterBoom");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "大型水雷");
			Main.npcFrameCount[base.npc.type] = 1;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x001539E4 File Offset: 0x00151BE4
		public override void SetDefaults()
		{
			base.npc.damage = 350;
			base.npc.width = 108;
			base.npc.height = 108;
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
        // Token: 0x06001BA7 RID: 7079 RVA: 0x0000B73F File Offset: 0x0000993F
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
        // Token: 0x02000413 RID: 1043
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸"), (int)npc.Center.X, (int)npc.Center.Y);
            player.velocity = (npc.velocity - player.velocity) / (npc.velocity - player.velocity).Length() * 13;
            Projectile.NewProjectile(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f, 0f, 0f, 164, 10, 4f, Main.myPlayer, 0f, 0f);
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4,40)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块1"), 2f);
            }
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 400)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块2"), 2f);
            }
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 40)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块3"), 2f);
            }
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 400)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块4"), 2f);
            }
            for (int k = 0; k <= 30; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(0, 60)).RotatedByRandom(Math.PI * 2);
                int num4 = Projectile.NewProjectile(npc.Center.X + v.X, npc.Center.Y + v.Y, 0, 0, base.mod.ProjectileType("MeltingpotBlaze"), 200, 0, Main.myPlayer, Main.rand.Next(2500, 3200) / 4000f, 0f);
                Main.projectile[num4].hostile = true;
            }
        }
        // Token: 0x02000413 RID: 1043
        public override void HitEffect(int hitDirection, double damage)
        {
            Player player = Main.player[Main.myPlayer];
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸"), (int)npc.Center.X, (int)npc.Center.Y);
            Projectile.NewProjectile(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f, 0f, 0f, 164, 10, 4f, Main.myPlayer, 0f, 0f);
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 40)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块1"), 2f);
            }
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 400)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块2"), 2f);
            }
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 40)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块3"), 2f);
            }
            for (int k = 0; k <= 2; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(4, 400)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.npc.Center, v, base.mod.GetGoreSlot("Gores/水雷碎块4"), 2f);
            }
            for (int k = 0; k <= 30; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(0, 60)).RotatedByRandom(Math.PI * 2);
                int num4 = Projectile.NewProjectile(npc.Center.X + v.X, npc.Center.Y + v.Y, 0, 0, base.mod.ProjectileType("MeltingpotBlaze"), 200, 0, Main.myPlayer, Main.rand.Next(2500, 3200) / 4000f, 0f);
                Main.projectile[num4].hostile = true;
            }
        }
		// Token: 0x06001BA8 RID: 7080 RVA: 0x000037AF File Offset: 0x000019AF
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
				return 0.1f;
			}
			else
            {
                return 0f;
            }
		}
		// Token: 0x06001BA9 RID: 7081 RVA: 0x000037AF File Offset: 0x000019AF
		public override bool PreNPCLoot()
		{
			return false;
		}
	}
}
