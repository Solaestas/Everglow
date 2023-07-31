using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs.OceanCrystal
{
    [AutoloadBossHead]
    public class OceanCrystal : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("湛海魔晶");
			Main.npcFrameCount[base.NPC.type] = 3;
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "湛海魔晶");
		}
        private int a = 0;
        private bool down = true;
        private bool initialization = true;
        private bool flag2s = true;
        private bool flag2a = true;
        private bool flag1 = false;
        private bool flag2 = false;
        private bool flag3 = false;
        private bool flag4 = false;
        private bool flag5 = false;
        private bool flag5s = true;
        private bool flag2b = true;
        private bool canDespawn;
        public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 75;
			base.NPC.width = 180;
			base.NPC.height = 200;
			base.NPC.defense = 50;
			base.NPC.lifeMax = (Main.expertMode ? 170000 : 100000);
            if(MythWorld.Myth)
            {
                base.NPC.lifeMax = 80000;
            }
            for (int i = 0; i < base.NPC.buffImmune.Length; i++)
            {
                base.NPC.buffImmune[i] = true;
            }
            base.NPC.buffImmune[39] = false;
            base.NPC.buffImmune[189] = false;
            base.NPC.buffImmune[69] = false;
            base.NPC.alpha = 0;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
            base.NPC.knockBackResist = 0f;
            base.NPC.boss = true;
            base.NPC.noGravity = true;
            base.NPC.noTileCollide = true;
			base.NPC.value = (float)Item.buyPrice(0, 10, 0, 0);
			base.NPC.DeathSound = SoundID.NPCDeath28;
            base.NPC.scale =1;
            this.Music = Mod.GetSoundSlot((Terraria.ModLoader.SoundType)51, "Sounds/Music/onoken-Alexandrite魔晶");
        }
        public override void AI()
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            Player player = Main.player[base.NPC.target];
            bool zoneUnderworldHeight = player.ZoneUnderworldHeight;
            base.NPC.TargetClosest(true);
            Vector2 center = base.NPC.Center;
            float num = player.Center.X - center.X;
            float num2 = player.Center.Y - center.Y;
            float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
            NPC.localAI[0] += 1;
            if (NPC.life > NPC.lifeMax * 0.75)
            {
                flag1 = true;
            }
            if (NPC.life <= NPC.lifeMax * 0.75 && NPC.life > NPC.lifeMax * 0.5)
            {
                flag1 = false;
                flag2 = true;
            }
            if (NPC.life <= NPC.lifeMax * 0.5 && NPC.life > NPC.lifeMax * 0.35)
            {
                flag2 = false;
                flag3 = true;
            }
            if (NPC.life <= NPC.lifeMax * 0.35 && NPC.life > NPC.lifeMax * 0.1)
            {
                flag3 = false;
                flag4 = true;
            }
            if (NPC.life <= NPC.lifeMax * 0.1 && NPC.life > 0)
            {
                flag4 = false;
                flag5 = true;
            }
            if (flag1)
            {
                if (NPC.localAI[0] > 0 && NPC.localAI[0] <= 120)
                {
                    NPC.localAI[0] = 2400;
                    NPC.velocity = (player.Center + new Vector2(0, -250) - base.NPC.Center) * (15 / num3);
                    NPC.rotation = NPC.velocity.X * 0.05f;
                }
                if (NPC.localAI[0] > 121 && NPC.localAI[0] <= 720)
                {
                    NPC.velocity = (player.Center + new Vector2((float)Math.Sin((NPC.localAI[0] / 120f + 1) * Math.PI) * 150f, -250) - base.NPC.Center) * (15 / num3);
                    NPC.rotation = NPC.velocity.X * 0.05f;

                    if (NPC.localAI[0] % 60 == 0)
                    {
                        for (int zk = 0; zk < 15; zk++)
                        {
                            Dust.NewDust(new Vector2(base.NPC.Center.X - 5, base.NPC.Center.Y + 30), 10, 10, 147, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 1.2f);
                            Dust.NewDust(new Vector2(base.NPC.Center.X - 5, base.NPC.Center.Y + 30), 10, 10, 33, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 2f);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 v = new Vector2(0, Main.rand.Next(0, 2000) / 400f).RotatedByRandom(Math.PI * 2);
                            Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y + 35, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.StarFish1>(), 95, 0.2f, Main.myPlayer, 0f, 0f);
                            NPC.NewNPC((int)base.NPC.Center.X - 10, (int)base.NPC.Center.Y + 43, 67, 0, 0f, 0f, 0f, 0f, 255);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 v = new Vector2(0, Main.rand.Next(0, 2000) / 400f).RotatedByRandom(Math.PI * 2);
                            Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y + 35, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.StarFish2>(), 95, 0.2f, Main.myPlayer, 0f, 0f);
                            NPC.NewNPC((int)base.NPC.Center.X - 10, (int)base.NPC.Center.Y + 43, 67, 0, 0f, 0f, 0f, 0f, 255);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 v = new Vector2(0, Main.rand.Next(0, 2000) / 400f).RotatedByRandom(Math.PI * 2);
                            Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y + 35, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.StarFish3>(), 95, 0.2f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                if (NPC.localAI[0] > 721 && NPC.localAI[0] <= 1440)
                {
                    NPC.velocity = (player.Center + new Vector2(0, -250) - base.NPC.Center) * (15 / num3);
                    NPC.rotation = NPC.velocity.X * 0.05f;
                    if (NPC.localAI[0] % 15 == 0)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(45, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X + 45, base.NPC.Center.Y + 45, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay>(), 75, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-25, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X - 25, base.NPC.Center.Y + 45, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay>(), 75, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                    if (NPC.localAI[0] % 30 == 0)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(30, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X + 30, base.NPC.Center.Y + 43, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay2>(), 105, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-10, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y + 43, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay2>(), 105, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (NPC.localAI[0] > 1441 && NPC.localAI[0] <= 2520)
                {
                    NPC.velocity = (player.Center + new Vector2(0, -250) - base.NPC.Center) * (15 / num3);
                    NPC.rotation = NPC.velocity.X * 0.05f;
                    if (NPC.localAI[0] % 150 == 0)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(45, 45))) * (1 / num3);
                        int num4 = Projectile.NewProjectile(base.NPC.Center.X + 45, base.NPC.Center.Y + 45, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.Vortex>(), 137, 0.2f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num4].scale = 0;
                    }
                }
                if (NPC.localAI[0] <= 2520)
                {
                    a = 0;
                    NPC.noTileCollide = true;
                }
                if (NPC.localAI[0] > 2521 && NPC.localAI[0] <= 3000)
                {
                    NPC.rotation = 0;
                    NPC.noTileCollide = false;
                    if (NPC.collideY)
                    {
                        NPC.collideY = false;
                        NPC.localAI[0] = 3001;
                        NPC.noTileCollide = true;
                        NPC.velocity.Y = 0.5f;
                    }
                    else
                    {
                        NPC.velocity.Y = 7;
                        NPC.velocity.X *= 0.9f;
                    }
                }
                if (NPC.localAI[0] > 3001 && NPC.localAI[0] <= 3600)
                {
                    NPC.velocity.X *= 0.5f;
                    if (NPC.localAI[0] <= 3015)
                    {
                        NPC.velocity.Y = 1f;
                    }
                    else
                    {
                        NPC.velocity.Y = 0;
                    }
                    down = false;
                    NPC.noTileCollide = true;
                    if (NPC.localAI[0] % 45 == 0)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Vector2 v = (player.Center - (NPC.Center + new Vector2(30, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                            Projectile.NewProjectile(base.NPC.Center.X + 30, base.NPC.Center.Y, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanCrystalSpice>(), 90, 0.2f, Main.myPlayer, 0f, 0f);
                            Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-10, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                            Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanCrystalSpice>(), 90, 0.2f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                if (NPC.localAI[0] > 3601 && NPC.localAI[0] <= 4200)
                {
                    down = false;
                    NPC.noTileCollide = true;
                    if (NPC.localAI[0] % 15 == 0)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(45, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X + 45, base.NPC.Center.Y + 45, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay>(), 75, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-25, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X - 25, base.NPC.Center.Y + 45, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay>(), 75, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                    if (NPC.localAI[0] % 30 == 0)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(30, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X + 30, base.NPC.Center.Y + 43, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay2>(), 105, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-10, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y + 43, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay2>(), 105, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (NPC.localAI[0] == 4201)
                {
                    down = true;
                }
                if (NPC.localAI[0] > 4201)
                {
                    NPC.localAI[0] = 120;
                }
            }
            if (flag2 || flag3 || flag4)
            {
                if(flag2s)
                {
                    NPC.localAI[0] = 0;
                    NPC.NewNPC((int)base.NPC.Center.X - 10, (int)base.NPC.Center.Y + 43, ModContent.NPCType<Everglow.Ocean.NPCs.OceanCrystalEye1>(), 0, 0f, 0f, 0f, 0f, 255);
                    NPC.NewNPC((int)base.NPC.Center.X + 30, (int)base.NPC.Center.Y + 43, ModContent.NPCType<Everglow.Ocean.NPCs.OceanCrystalEye1>(), 0, 0f, 0f, 0f, 0f, 255);
                    NPC.NewNPC((int)base.NPC.Center.X - 25, (int)base.NPC.Center.Y + 45, ModContent.NPCType<Everglow.Ocean.NPCs.OceanCrystalEye1>(), 0, 0f, 0f, 0f, 0f, 255);
                    NPC.NewNPC((int)base.NPC.Center.X + 45, (int)base.NPC.Center.Y + 45, ModContent.NPCType<Everglow.Ocean.NPCs.OceanCrystalEye1>(), 0, 0f, 0f, 0f, 0f, 255);
                    float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                    Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块"), 1f);
                    Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块2"), 1f);
                    flag2s = false;
                    for (int k = 0; k < 10; k++)
                    {
                        Vector2 v = new Vector2(4000, Main.rand.Next(0, 6000) / 400f).RotatedByRandom(Math.PI * 2);
                        Vector2 v2 = new Vector2(1000, Main.rand.Next(0, 4000) / 400f).RotatedByRandom(Math.PI * 2);
                        Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y + 35, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海洋波纹2>(), 111, 0.2f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y + 35, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureRay>(), 75, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                NPC.localAI[0] += 1;
                if(flag2a)
                {
                    down = true;
                    if (NPC.CountNPCS(ModContent.NPCType<Everglow.Ocean.NPCs.OceanCrystalEye1>()) < 1 && NPC.CountNPCS(ModContent.NPCType<Everglow.Ocean.NPCs.OceanCrystalEye1>()) < 1)
                    {
                        flag2a = false;
                        NPC.dontTakeDamage = false;
                        NPC.localAI[0] = 3109;
                    }
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    NPC.dontTakeDamage = false;
                }
                if (NPC.localAI[0] >= 0 && NPC.localAI[0] < 1000)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 1f);
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 1.2f);
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 1.5f);
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 1.8f);
                    down = true;
                    base.NPC.velocity *= 0.94f;
                    float num20 = 1.8f;
                    if (base.NPC.velocity.Length() < num20 * 3f)
                    {
                        base.NPC.TargetClosest(true);
                        float num21 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num21 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) * ((base.NPC.velocity.Length() - 1.8f) / 3.6f) + (float)Math.Atan2(-num4, num5) * (1 - ((base.NPC.velocity.Length() - 1.8f) / 3.6f)) + (float)Math.PI;
                    }
                    else
                    {
                        base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    }
                    int p = Dust.NewDust(base.NPC.position, NPC.width, NPC.height, 33, 0, 0, 0, default(Color), 2f);
                    Main.dust[p].velocity.X = 0;
                    Main.dust[p].velocity.Y = 0;
                    Main.dust[p].noGravity = true;
                    if (base.NPC.velocity.Length() < num20)
                    {
                        base.NPC.TargetClosest(true);
                        float num21 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num21 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 4;
                        base.NPC.velocity.Y = num5 * 4;
                        return;
                    }
                    flag2b = true;
                }
                if (NPC.localAI[0] >= 1000 && NPC.localAI[0] < 2200)
                {
                    float n = 2;
                    if (Main.expertMode)
                    {
                        n = 2.5f;
                        if (MythWorld.Myth)
                        {
                            n = 3f;
                        }
                    }
                    down = true;
                    base.NPC.TargetClosest(true);
                    base.NPC.velocity += (player.Center - base.NPC.Center) * (6 / ((player.Center - base.NPC.Center).Length() + 1)) * 0.04f;
                    if(NPC.velocity.Length() > n)
                    {
                        NPC.velocity *= 0.98f;
                    }
                    if (NPC.velocity.Length() < n - 1)
                    {
                        NPC.velocity *= 1.02f;
                    }
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    Vector2 v = new Vector2(0, 5).RotatedByRandom(Math.PI * 2);
                    if (NPC.localAI[0] < 2000)
                    {
                        int p = Dust.NewDust(base.NPC.Center, 0, 0, 71, v.X, v.Y, 0, default(Color), 2f);
                    }
                    int numk = 0;
                    if (NPC.localAI[0] <= 1199 && NPC.localAI[0] % 200 == 50)
                    {
                        flag2b = false;
                        int num5 = (int)Player.FindClosest(base.NPC.Center, 1, 1);
                        float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.NPC.Center.X) * (Main.player[num5].Center.X - base.NPC.Center.X) + (Main.player[num5].Center.Y - base.NPC.Center.Y) * (Main.player[num5].Center.Y - base.NPC.Center.Y));
                        Vector2 vector = (Main.player[num5].Center - base.NPC.Center).RotatedBy(1.4) / num6;
                        numk = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector.X, vector.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.PoisonRayPurple>(), 125, 0, Main.myPlayer, 0f, 0f);
                    }
                    if (!flag2b)
                    {
                        if (Main.projectile[numk].type ==ModContent.ProjectileType<Everglow.Ocean.Projectiles.PoisonRayPurple>())
                        {
                            Main.projectile[numk].Center = base.NPC.Center;
                        }
                        //if(Main.projectile[numk].type == base.mod.ProjectileType("PoisonRayPurple"))
                        //{
                           // Main.projectile[numk].timeLeft = 0;
                            //Main.projectile[numk].active = false;
                        //}
                    }
                }
                if (NPC.localAI[0] >= 2200 && NPC.localAI[0] < 3200)
                {
                    float n = 2;
                    if(Main.expertMode)
                    {
                        n = 2.5f;
                        if (MythWorld.Myth)
                        {
                            n = 3f;
                        }
                    }
                    flag2b = true;
                    down = true;
                    base.NPC.TargetClosest(true);
                    base.NPC.velocity += (player.Center - base.NPC.Center) * (6 / ((player.Center - base.NPC.Center).Length() + 1)) * 0.04f;
                    if (NPC.velocity.Length() > n)
                    {
                        NPC.velocity *= 0.98f;
                    }
                    if (NPC.velocity.Length() < n - 1)
                    {
                        NPC.velocity *= 1.02f;
                    }
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    if (NPC.localAI[0] % 180 == 50)
                    {
                        for(int i = 0; i < 45; i++)
                        {
                            int num5 = (int)Player.FindClosest(base.NPC.Center, 1, 1);
                            float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.NPC.Center.X) * (Main.player[num5].Center.X - base.NPC.Center.X) + (Main.player[num5].Center.Y - base.NPC.Center.Y) * (Main.player[num5].Center.Y - base.NPC.Center.Y));
                            Vector2 vector = (Main.player[num5].Center - base.NPC.Center).RotatedBy(Main.rand.Next(-200, 200) / 2000f) / num6 * Main.rand.Next(300,2000) / 200f;
                            Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector.X, vector.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.PhantomFish>(), 88, 0, Main.myPlayer, Main.rand.Next(-200,200) / 100f, 0f);
                        }
                    }
                }
                if (NPC.localAI[0] >= 1000 && flag2a)
                {
                    NPC.localAI[0] = 0;
                }
                if (NPC.localAI[0] >= 3200)
                {
                    NPC.localAI[0] = 0;
                }
            }
            if (flag5)
            {
                if(flag5s)
                {
                    float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                    Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                    Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                    Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                    Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                    flag5s = false;
                }
                flag2b = true;
                NPC.localAI[0] += 1;
                down = true;
                base.NPC.velocity *= 0.97f;
                float num20 = 1.8f;
                if (base.NPC.velocity.Length() < num20 * 3f)
                {
                    base.NPC.TargetClosest(true);
                    float num21 = 8f;
                    Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                    float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                    float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    num6 = num21 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) * ((base.NPC.velocity.Length() - 1.8f) / 3.6f) + (float)Math.Atan2(-num4, num5) * (1 - ((base.NPC.velocity.Length() - 1.8f) / 3.6f)) + (float)Math.PI;
                }
                else
                {
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                }
                int p = Dust.NewDust(base.NPC.position, NPC.width, NPC.height, 33, 0, 0, 0, default(Color), 2f);
                Main.dust[p].velocity.X = 0;
                Main.dust[p].velocity.Y = 0;
                Main.dust[p].noGravity = true;
                if (base.NPC.velocity.Length() < num20)
                {
                    base.NPC.TargetClosest(true);
                    float num21 = 8f;
                    Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                    float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                    float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    num6 = num21 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    base.NPC.velocity.X = num4 * 4;
                    base.NPC.velocity.Y = num5 * 4;
                    return;
                }
                flag2b = true;
                if (NPC.localAI[0] % 500 == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        Vector2 vector = new Vector2(Main.rand.Next(1370, 1550) / 20f, 0).RotatedByRandom(Math.PI * 2);
                        int numl = Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector.X, vector.Y, 405, 88, 0, Main.myPlayer,0, 0f);
                        Main.projectile[numl].scale = Main.rand.Next(95,110) / 100f;
                        Main.projectile[numl].friendly = false;
                        Main.projectile[numl].hostile = true;
                        Main.projectile[numl].timeLeft = 120;
                    }
                }
                if (NPC.localAI[0] % 700 == 250)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        int num5 = (int)Player.FindClosest(base.NPC.Center, 1, 1);
                        float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.NPC.Center.X) * (Main.player[num5].Center.X - base.NPC.Center.X) + (Main.player[num5].Center.Y - base.NPC.Center.Y) * (Main.player[num5].Center.Y - base.NPC.Center.Y));
                        Vector2 vector = (Main.player[num5].Center - base.NPC.Center).RotatedBy(Main.rand.Next(-200, 200) / 2000f) / num6 * Main.rand.Next(300, 2000) / 200f;
                        Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector.X, vector.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.PhantomFish>(), 88, 0, Main.myPlayer, Main.rand.Next(-200, 200) / 100f, 0f);
                    }
                }
                if (NPC.localAI[0] % 500 == 250)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        int num5 = (int)Player.FindClosest(base.NPC.Center, 1, 1);
                        float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.NPC.Center.X) * (Main.player[num5].Center.X - base.NPC.Center.X) + (Main.player[num5].Center.Y - base.NPC.Center.Y) * (Main.player[num5].Center.Y - base.NPC.Center.Y));
                        Vector2 vector = (Main.player[num5].Center - base.NPC.Center).RotatedBy(Main.rand.Next(-200, 200) / 1500f) / num6 * Main.rand.Next(300, 2000) / 400f;
                        Projectile.NewProjectile(base.NPC.Center.X, base.NPC.Center.Y, vector.X, vector.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.空间粒子流>(), 45, 0, Main.myPlayer, 0, 0f);
                    }
                }
                if (NPC.localAI[0] % 167 == 0)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(30, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X + 30, base.NPC.Center.Y, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanCrystalSpice>(), 111, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-10, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                        Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanCrystalSpice>(), 111, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (NPC.localAI[0] % 333 == 0)
                {
                    Vector2 v = (player.Center - (NPC.Center + new Vector2(45, 45))) * (1 / num3);
                    int num4 = Projectile.NewProjectile(base.NPC.Center.X + 45, base.NPC.Center.Y + 45, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.Vortex>(), 137, 0.2f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num4].scale = 0;
                }
                if (NPC.localAI[0] % 1429 == 0)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        Vector2 v = (player.Center - (NPC.Center + new Vector2(30, 0))).RotatedByRandom(Math.PI * 0.5f) * (Main.rand.Next(0, 250) / 12f / num3);
                        Projectile.NewProjectile(base.NPC.Center.X + 30, base.NPC.Center.Y, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.AttractLightBall>(), 82, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (NPC.Center + new Vector2(-10, 0))).RotatedByRandom(Math.PI * 0.5f) * (Main.rand.Next(0,250) / 12f / num3);
                        Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.LeaveLightBall>(), 82, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
        }
		public override void FindFrame(int frameHeight)
		{
            frameHeight = 280;
            base.NPC.frameCounter += 0.1f;
			int num = (int)base.NPC.frameCounter;
            if(down)
            {
                base.NPC.frame.Y = (num % 3) * frameHeight;
            }
            else
            {
                if(NPC.localAI[0] > 3000 && NPC.localAI[0] <= 3020)
                {
                    base.NPC.frame.Y = 0 * frameHeight;
                }
                if (NPC.localAI[0] > 3020)
                {
                    base.NPC.frame.Y = 1 * frameHeight;
                }
            }
        }
        public override bool CheckActive()
		{
			return this.canDespawn;
		}
      
		public override void HitEffect(NPC.HitInfo hit)
		{
            SoundEngine.PlaySound(SoundID.Item27, new Vector2(base.NPC.position.X, base.NPC.position.Y));
            for (int i = 0; i < 15; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, ModContent.DustType<Everglow.Ocean.Dusts.Crystal2>(), (float)hitDirection, -1f, 0, default(Color), 1f);
			}
            MythWorld.downedHYFY = true;
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
                Vector2 v2 = new Vector2(Main.rand.Next(0,15) / 3f,0).RotatedByRandom(Math.PI * 2) + NPC.velocity;
                Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y, v2.X, v2.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanSealBroken3>(), (int)(base.NPC.damage * 1.2f), 0.2f, Main.myPlayer, 0f, 0f);
                Vector2 v3 = new Vector2(Main.rand.Next(0, 15) / 3f, 0).RotatedByRandom(Math.PI * 2) + NPC.velocity;
                Projectile.NewProjectile(base.NPC.Center.X - 10, base.NPC.Center.Y, v3.X, v3.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanSealBroken4>(), (int)(base.NPC.damage * 1.2f), 0.2f, Main.myPlayer, 0f, 0f);
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                for (int i = 0; i < 120; i++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, ModContent.DustType<Everglow.Ocean.Dusts.Crystal2>(), (float)hitDirection, -1f, 0, default(Color), 2f);
                }
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石1"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石4"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石1"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石4"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块5"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块5"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块5"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块6"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块6"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块6"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块7"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                Gore.NewGore(base.NPC.Center, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
                mplayer.movieTime = 120;
            }
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Mod mod = ModLoader.GetMod("Everglow.Ocean.");
            Texture2D texture = TextureAssets.Npc[base.NPC.type].Value;
            if(!down)
            {
                texture = mod.GetTexture("NPCs/OceanCrystal/海洋封印II");
            }
            if (flag2)
            {
                texture = mod.GetTexture("NPCs/OceanCrystal/海洋封印IV");
            }
            if (flag3)
            {
                texture = mod.GetTexture("NPCs/OceanCrystal/海洋封印V");
            }
            if (flag4)
            {
                texture = mod.GetTexture("NPCs/OceanCrystal/海洋封印VI");
            }
            if (flag5)
            {
                texture = mod.GetTexture("NPCs/OceanCrystal/海洋封印VII");
            }
            //Everglow.Ocean.DrawTexture(spriteBatch, texture, 0, base.npc, new Color?(drawColor), false);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2((float)texture.Width / 2f, 140), NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (flag1)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (base.NPC.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
                Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
                Vector2 vector2 = value - Main.screenPosition;
                vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印Glow").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印Glow").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
                vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
                Color color = Utils.MultiplyRGBA(new Color(297 - base.NPC.alpha, 297 - base.NPC.alpha, 297 - base.NPC.alpha, 0), Color.Blue);
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印Glow"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
            }
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印眼睛"), vector2, new Rectangle(0, 0, 10, 10), new Color(100, 100, 100, 0), base.npc.rotation, vector, 1f, effects, 0f);
        }
        public override void OnKill()
        {
            if (Main.expertMode)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.OceanCrystalTreasureBag>(), 1, false, 0, false, false);
                return;
            }
            else
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.Aquamarine>(), Main.rand.Next(Main.rand.Next(14, 75), 80), false, 0, false, false);
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.MysteriesPearl>(), Main.rand.Next(Main.rand.Next(10,20), 40), false, 0, false, false);
                int type = 0;
                switch (Main.rand.Next(1, 6))
                {
                    case 1:
                        type = ModContent.ItemType<Everglow.Ocean.Items.OceanBubble>();
                        break;
                    case 2:
                        type = ModContent.ItemType<Everglow.Ocean.Items.OceanEverStone>();
                        break;
                    case 3:
                        type = ModContent.ItemType<Everglow.Ocean.Items.OceanCurrentRay>();
                        break;
                    case 4:
                        type = ModContent.ItemType<Everglow.Ocean.Items.OceanCrystalClub>();
                        break;
                    case 5:
                        type = ModContent.ItemType<Everglow.Ocean.Items.OceanCrystalShield>();
                        break;
                }
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, type, Main.rand.Next(Main.rand.Next(14, 75), 80), false, 0, false, false);
            }
        }
	}
}
