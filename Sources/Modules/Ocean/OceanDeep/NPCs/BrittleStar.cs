using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	// Token: 0x0200420 RID: 1056
    public class BrittleStar : ModNPC
	{
		// Token: 0x06001475 RID: 5237 RVA: 0x00082F6 File Offset: 0x00064F6
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("Stone");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蛇尾");
		}
		private bool canDespawn;
		private float M = 0.04f;
        private float num = 0;
        private float num2 = 0;
        private float num3 = 0;
        private float num4 = 0;
        private float num5 = 0;
        public override void SetDefaults()
		{
			base.NPC.knockBackResist = 0f;
			base.NPC.noGravity = true;
			base.NPC.damage = 60;
			base.NPC.width = 14;
			base.NPC.height = 14;
			base.NPC.defense = 5;
			base.NPC.lifeMax = 1040;
			base.NPC.HitSound = SoundID.NPCHit2;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneDeepocean && spawnInfo.Water)
            {
                if (NPC.CountNPCS(Mod.Find<ModNPC>("海蛇尾").Type) > 0)
                {
                    return 0f;
                }
                else
                {
                    return 0.1f;
                }
            }
            return 0f;
        }
        // Token: 0x06001477 RID: 5239 RVA: 0x00B3A6C File Offset: 0x00B1C6C
        public override void AI()
		{
            if (num > 300)
            {
                num -= Main.rand.Next(0, 6);
            }
            else if (num < -300)
            {
                num += Main.rand.Next(0, 6);
            }
            else
            {
                num += Main.rand.Next(-5, 6);
            }
            Vector2 vector = new Vector2(0, 2).RotatedBy((float)Math.PI * (0.4f + num / 2000f));
            int num35 = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector.X, vector.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海蛇尾触手>(), 20, 0f, Main.myPlayer, 0f, 0f);
            
            if (num2 > 300)
            {
                num2 -= Main.rand.Next(0, 6);
            }
            else if (num2 < -300)
            {
                num2 += Main.rand.Next(0, 6);
            }
            else
            {
                num2 += Main.rand.Next(-5, 6);
            }
            Vector2 vector2 = new Vector2(0, 2).RotatedBy((float)Math.PI * (0.8f + num2 / 2000f));
            int num36 = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector2.X, vector2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海蛇尾触手>(), 20, 0f, Main.myPlayer, 0f, 0f);
            
            if (num3 > 300)
            {
                num3 -= Main.rand.Next(0, 6);
            }
            else if (num3 < -300)
            {
                num3 += Main.rand.Next(0, 6);
            }
            else
            {
                num3 += Main.rand.Next(-5, 6);
            }
            Vector2 vector3 = new Vector2(0, 2).RotatedBy((float)Math.PI * (1.2f + num3 / 2000f));
            int num37 = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector3.X, vector3.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海蛇尾触手>(), 20, 0f, Main.myPlayer, 0f, 0f);
            
            if (num4 > 300)
            {
                num4 -= Main.rand.Next(0, 6);
            }
            else if (num4 < -300)
            {
                num4 += Main.rand.Next(0, 6);
            }
            else
            {
                num4 += Main.rand.Next(-5, 6);
            }
            Vector2 vector4 = new Vector2(0, 2).RotatedBy((float)Math.PI * (1.6f + num4 / 2000f));
            int num38 = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector4.X, vector4.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海蛇尾触手>(), 20, 0f, Main.myPlayer, 0f, 0f);
            
            if (num5 > 300)
            {
                num5 -= Main.rand.Next(0, 6);
            }
            else if (num5 < -300)
            {
                num5 += Main.rand.Next(0, 6);
            }
            else
            {
                num5 += Main.rand.Next(-5, 6);
            }
            Vector2 vector5 = new Vector2(0, 2).RotatedBy((float)Math.PI * (2f + num5 / 2000f));
            int num39 = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector5.X, vector5.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海蛇尾触手>(), 20, 0f, Main.myPlayer, 0f, 0f);
        }
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
		}
        // Token: 0x06001478 RID: 5240 RVA: 0x00A9970 File Offset: 0x00A7B70
        public override void FindFrame(int frameHeight)
		{
		}
        // Token: 0x0600147B RID: 5243 RVA: 0x00A99DC File Offset: 0x00A7BDC
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.NPC.life <= 0)
            {
                for (int j = 0; j < 25; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
                for (int k = 0; k < 5; k++)
                {
                    Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海蛇尾碎块"), 1f);
                }
            }
            base.NPC.spriteDirection = ((base.NPC.direction > 0) ? 1 : -1);
        }
        public override void OnKill()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, base.Mod.Find<ModItem>("BladeScale").Type, 1, false, 0, false, false);
            }
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, base.Mod.Find<ModItem>("OceanDustCore").Type, 1, false, 0, false, false);
            }
        }
    }
}
