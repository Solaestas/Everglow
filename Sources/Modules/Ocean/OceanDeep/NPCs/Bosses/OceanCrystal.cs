using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs.OceanCrystal
{
    [AutoloadBossHead]
    public class OceanCrystal : ModNPC
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("湛海魔晶");
			Main.npcFrameCount[base.npc.type] = 3;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "湛海魔晶");
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
			base.npc.noGravity = true;
			base.npc.damage = 75;
			base.npc.width = 180;
			base.npc.height = 200;
			base.npc.defense = 50;
			base.npc.lifeMax = (Main.expertMode ? 170000 : 100000);
            if(MythWorld.Myth)
            {
                base.npc.lifeMax = 80000;
            }
            for (int i = 0; i < base.npc.buffImmune.Length; i++)
            {
                base.npc.buffImmune[i] = true;
            }
            base.npc.buffImmune[39] = false;
            base.npc.buffImmune[189] = false;
            base.npc.buffImmune[69] = false;
            base.npc.alpha = 0;
			base.npc.aiStyle = -1;
			this.aiType = -1;
            base.npc.knockBackResist = 0f;
            base.npc.boss = true;
            base.npc.noGravity = true;
            base.npc.noTileCollide = true;
			base.npc.value = (float)Item.buyPrice(0, 10, 0, 0);
			base.npc.DeathSound = SoundID.NPCDeath28;
            base.npc.scale =1;
            this.music = mod.GetSoundSlot((Terraria.ModLoader.SoundType)51, "Sounds/Music/onoken-Alexandrite魔晶");
        }
        public override void AI()
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            Player player = Main.player[base.npc.target];
            bool zoneUnderworldHeight = player.ZoneUnderworldHeight;
            base.npc.TargetClosest(true);
            Vector2 center = base.npc.Center;
            float num = player.Center.X - center.X;
            float num2 = player.Center.Y - center.Y;
            float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
            npc.localAI[0] += 1;
            if (npc.life > npc.lifeMax * 0.75)
            {
                flag1 = true;
            }
            if (npc.life <= npc.lifeMax * 0.75 && npc.life > npc.lifeMax * 0.5)
            {
                flag1 = false;
                flag2 = true;
            }
            if (npc.life <= npc.lifeMax * 0.5 && npc.life > npc.lifeMax * 0.35)
            {
                flag2 = false;
                flag3 = true;
            }
            if (npc.life <= npc.lifeMax * 0.35 && npc.life > npc.lifeMax * 0.1)
            {
                flag3 = false;
                flag4 = true;
            }
            if (npc.life <= npc.lifeMax * 0.1 && npc.life > 0)
            {
                flag4 = false;
                flag5 = true;
            }
            if (flag1)
            {
                if (npc.localAI[0] > 0 && npc.localAI[0] <= 120)
                {
                    npc.localAI[0] = 2400;
                    npc.velocity = (player.Center + new Vector2(0, -250) - base.npc.Center) * (15 / num3);
                    npc.rotation = npc.velocity.X * 0.05f;
                }
                if (npc.localAI[0] > 121 && npc.localAI[0] <= 720)
                {
                    npc.velocity = (player.Center + new Vector2((float)Math.Sin((npc.localAI[0] / 120f + 1) * Math.PI) * 150f, -250) - base.npc.Center) * (15 / num3);
                    npc.rotation = npc.velocity.X * 0.05f;

                    if (npc.localAI[0] % 60 == 0)
                    {
                        for (int zk = 0; zk < 15; zk++)
                        {
                            Dust.NewDust(new Vector2(base.npc.Center.X - 5, base.npc.Center.Y + 30), 10, 10, 147, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 1.2f);
                            Dust.NewDust(new Vector2(base.npc.Center.X - 5, base.npc.Center.Y + 30), 10, 10, 33, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 2f);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 v = new Vector2(0, Main.rand.Next(0, 2000) / 400f).RotatedByRandom(Math.PI * 2);
                            Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y + 35, v.X, v.Y, base.mod.ProjectileType("StarFish1"), 95, 0.2f, Main.myPlayer, 0f, 0f);
                            NPC.NewNPC((int)base.npc.Center.X - 10, (int)base.npc.Center.Y + 43, 67, 0, 0f, 0f, 0f, 0f, 255);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 v = new Vector2(0, Main.rand.Next(0, 2000) / 400f).RotatedByRandom(Math.PI * 2);
                            Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y + 35, v.X, v.Y, base.mod.ProjectileType("StarFish2"), 95, 0.2f, Main.myPlayer, 0f, 0f);
                            NPC.NewNPC((int)base.npc.Center.X - 10, (int)base.npc.Center.Y + 43, 67, 0, 0f, 0f, 0f, 0f, 255);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 v = new Vector2(0, Main.rand.Next(0, 2000) / 400f).RotatedByRandom(Math.PI * 2);
                            Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y + 35, v.X, v.Y, base.mod.ProjectileType("StarFish3"), 95, 0.2f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                if (npc.localAI[0] > 721 && npc.localAI[0] <= 1440)
                {
                    npc.velocity = (player.Center + new Vector2(0, -250) - base.npc.Center) * (15 / num3);
                    npc.rotation = npc.velocity.X * 0.05f;
                    if (npc.localAI[0] % 15 == 0)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(45, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.npc.Center.X + 45, base.npc.Center.Y + 45, v.X, v.Y, base.mod.ProjectileType("AzureRay"), 75, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (npc.Center + new Vector2(-25, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.npc.Center.X - 25, base.npc.Center.Y + 45, v2.X, v2.Y, base.mod.ProjectileType("AzureRay"), 75, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                    if (npc.localAI[0] % 30 == 0)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(30, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.npc.Center.X + 30, base.npc.Center.Y + 43, v.X, v.Y, base.mod.ProjectileType("AzureRay2"), 105, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (npc.Center + new Vector2(-10, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y + 43, v2.X, v2.Y, base.mod.ProjectileType("AzureRay2"), 105, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (npc.localAI[0] > 1441 && npc.localAI[0] <= 2520)
                {
                    npc.velocity = (player.Center + new Vector2(0, -250) - base.npc.Center) * (15 / num3);
                    npc.rotation = npc.velocity.X * 0.05f;
                    if (npc.localAI[0] % 150 == 0)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(45, 45))) * (1 / num3);
                        int num4 = Projectile.NewProjectile(base.npc.Center.X + 45, base.npc.Center.Y + 45, v.X, v.Y, base.mod.ProjectileType("Vortex"), 137, 0.2f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num4].scale = 0;
                    }
                }
                if (npc.localAI[0] <= 2520)
                {
                    a = 0;
                    npc.noTileCollide = true;
                }
                if (npc.localAI[0] > 2521 && npc.localAI[0] <= 3000)
                {
                    npc.rotation = 0;
                    npc.noTileCollide = false;
                    if (npc.collideY)
                    {
                        npc.collideY = false;
                        npc.localAI[0] = 3001;
                        npc.noTileCollide = true;
                        npc.velocity.Y = 0.5f;
                    }
                    else
                    {
                        npc.velocity.Y = 7;
                        npc.velocity.X *= 0.9f;
                    }
                }
                if (npc.localAI[0] > 3001 && npc.localAI[0] <= 3600)
                {
                    npc.velocity.X *= 0.5f;
                    if (npc.localAI[0] <= 3015)
                    {
                        npc.velocity.Y = 1f;
                    }
                    else
                    {
                        npc.velocity.Y = 0;
                    }
                    down = false;
                    npc.noTileCollide = true;
                    if (npc.localAI[0] % 45 == 0)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Vector2 v = (player.Center - (npc.Center + new Vector2(30, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                            Projectile.NewProjectile(base.npc.Center.X + 30, base.npc.Center.Y, v.X, v.Y, base.mod.ProjectileType("OceanCrystalSpice"), 90, 0.2f, Main.myPlayer, 0f, 0f);
                            Vector2 v2 = (player.Center - (npc.Center + new Vector2(-10, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                            Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y, v2.X, v2.Y, base.mod.ProjectileType("OceanCrystalSpice"), 90, 0.2f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                if (npc.localAI[0] > 3601 && npc.localAI[0] <= 4200)
                {
                    down = false;
                    npc.noTileCollide = true;
                    if (npc.localAI[0] % 15 == 0)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(45, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.npc.Center.X + 45, base.npc.Center.Y + 45, v.X, v.Y, base.mod.ProjectileType("AzureRay"), 75, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (npc.Center + new Vector2(-25, 45))) * (10 / num3);
                        Projectile.NewProjectile(base.npc.Center.X - 25, base.npc.Center.Y + 45, v2.X, v2.Y, base.mod.ProjectileType("AzureRay"), 75, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                    if (npc.localAI[0] % 30 == 0)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(30, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.npc.Center.X + 30, base.npc.Center.Y + 43, v.X, v.Y, base.mod.ProjectileType("AzureRay2"), 105, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (npc.Center + new Vector2(-10, 43))) * (13 / num3);
                        Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y + 43, v2.X, v2.Y, base.mod.ProjectileType("AzureRay2"), 105, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (npc.localAI[0] == 4201)
                {
                    down = true;
                }
                if (npc.localAI[0] > 4201)
                {
                    npc.localAI[0] = 120;
                }
            }
            if (flag2 || flag3 || flag4)
            {
                if(flag2s)
                {
                    npc.localAI[0] = 0;
                    NPC.NewNPC((int)base.npc.Center.X - 10, (int)base.npc.Center.Y + 43, base.mod.NPCType("OceanCrystalEye1"), 0, 0f, 0f, 0f, 0f, 255);
                    NPC.NewNPC((int)base.npc.Center.X + 30, (int)base.npc.Center.Y + 43, base.mod.NPCType("OceanCrystalEye1"), 0, 0f, 0f, 0f, 0f, 255);
                    NPC.NewNPC((int)base.npc.Center.X - 25, (int)base.npc.Center.Y + 45, base.mod.NPCType("OceanCrystalEye1"), 0, 0f, 0f, 0f, 0f, 255);
                    NPC.NewNPC((int)base.npc.Center.X + 45, (int)base.npc.Center.Y + 45, base.mod.NPCType("OceanCrystalEye1"), 0, 0f, 0f, 0f, 0f, 255);
                    float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                    Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块"), 1f);
                    Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块2"), 1f);
                    flag2s = false;
                    for (int k = 0; k < 10; k++)
                    {
                        Vector2 v = new Vector2(4000, Main.rand.Next(0, 6000) / 400f).RotatedByRandom(Math.PI * 2);
                        Vector2 v2 = new Vector2(1000, Main.rand.Next(0, 4000) / 400f).RotatedByRandom(Math.PI * 2);
                        Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y + 35, v.X, v.Y, base.mod.ProjectileType("海洋波纹2"), 111, 0.2f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y + 35, v2.X, v2.Y, base.mod.ProjectileType("AzureRay"), 75, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                npc.localAI[0] += 1;
                if(flag2a)
                {
                    down = true;
                    if (NPC.CountNPCS(base.mod.NPCType("OceanCrystalEye1")) < 1 && NPC.CountNPCS(base.mod.NPCType("OceanCrystalEye1")) < 1)
                    {
                        flag2a = false;
                        npc.dontTakeDamage = false;
                        npc.localAI[0] = 3109;
                    }
                    npc.dontTakeDamage = true;
                }
                else
                {
                    npc.dontTakeDamage = false;
                }
                if (npc.localAI[0] >= 0 && npc.localAI[0] < 1000)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 1f);
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 1.2f);
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 1.5f);
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 1.8f);
                    down = true;
                    base.npc.velocity *= 0.94f;
                    float num20 = 1.8f;
                    if (base.npc.velocity.Length() < num20 * 3f)
                    {
                        base.npc.TargetClosest(true);
                        float num21 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num21 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) * ((base.npc.velocity.Length() - 1.8f) / 3.6f) + (float)Math.Atan2(-num4, num5) * (1 - ((base.npc.velocity.Length() - 1.8f) / 3.6f)) + (float)Math.PI;
                    }
                    else
                    {
                        base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    }
                    int p = Dust.NewDust(base.npc.position, npc.width, npc.height, 33, 0, 0, 0, default(Color), 2f);
                    Main.dust[p].velocity.X = 0;
                    Main.dust[p].velocity.Y = 0;
                    Main.dust[p].noGravity = true;
                    if (base.npc.velocity.Length() < num20)
                    {
                        base.npc.TargetClosest(true);
                        float num21 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num21 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 4;
                        base.npc.velocity.Y = num5 * 4;
                        return;
                    }
                    flag2b = true;
                }
                if (npc.localAI[0] >= 1000 && npc.localAI[0] < 2200)
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
                    base.npc.TargetClosest(true);
                    base.npc.velocity += (player.Center - base.npc.Center) * (6 / ((player.Center - base.npc.Center).Length() + 1)) * 0.04f;
                    if(npc.velocity.Length() > n)
                    {
                        npc.velocity *= 0.98f;
                    }
                    if (npc.velocity.Length() < n - 1)
                    {
                        npc.velocity *= 1.02f;
                    }
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    Vector2 v = new Vector2(0, 5).RotatedByRandom(Math.PI * 2);
                    if (npc.localAI[0] < 2000)
                    {
                        int p = Dust.NewDust(base.npc.Center, 0, 0, 71, v.X, v.Y, 0, default(Color), 2f);
                    }
                    int numk = 0;
                    if (npc.localAI[0] <= 1199 && npc.localAI[0] % 200 == 50)
                    {
                        flag2b = false;
                        int num5 = (int)Player.FindClosest(base.npc.Center, 1, 1);
                        float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.npc.Center.X) * (Main.player[num5].Center.X - base.npc.Center.X) + (Main.player[num5].Center.Y - base.npc.Center.Y) * (Main.player[num5].Center.Y - base.npc.Center.Y));
                        Vector2 vector = (Main.player[num5].Center - base.npc.Center).RotatedBy(1.4) / num6;
                        numk = Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, vector.X, vector.Y, base.mod.ProjectileType("PoisonRayPurple"), 125, 0, Main.myPlayer, 0f, 0f);
                    }
                    if (!flag2b)
                    {
                        if (Main.projectile[numk].type == base.mod.ProjectileType("PoisonRayPurple"))
                        {
                            Main.projectile[numk].Center = base.npc.Center;
                        }
                        //if(Main.projectile[numk].type == base.mod.ProjectileType("PoisonRayPurple"))
                        //{
                           // Main.projectile[numk].timeLeft = 0;
                            //Main.projectile[numk].active = false;
                        //}
                    }
                }
                if (npc.localAI[0] >= 2200 && npc.localAI[0] < 3200)
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
                    base.npc.TargetClosest(true);
                    base.npc.velocity += (player.Center - base.npc.Center) * (6 / ((player.Center - base.npc.Center).Length() + 1)) * 0.04f;
                    if (npc.velocity.Length() > n)
                    {
                        npc.velocity *= 0.98f;
                    }
                    if (npc.velocity.Length() < n - 1)
                    {
                        npc.velocity *= 1.02f;
                    }
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    if (npc.localAI[0] % 180 == 50)
                    {
                        for(int i = 0; i < 45; i++)
                        {
                            int num5 = (int)Player.FindClosest(base.npc.Center, 1, 1);
                            float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.npc.Center.X) * (Main.player[num5].Center.X - base.npc.Center.X) + (Main.player[num5].Center.Y - base.npc.Center.Y) * (Main.player[num5].Center.Y - base.npc.Center.Y));
                            Vector2 vector = (Main.player[num5].Center - base.npc.Center).RotatedBy(Main.rand.Next(-200, 200) / 2000f) / num6 * Main.rand.Next(300,2000) / 200f;
                            Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, vector.X, vector.Y, base.mod.ProjectileType("PhantomFish"), 88, 0, Main.myPlayer, Main.rand.Next(-200,200) / 100f, 0f);
                        }
                    }
                }
                if (npc.localAI[0] >= 1000 && flag2a)
                {
                    npc.localAI[0] = 0;
                }
                if (npc.localAI[0] >= 3200)
                {
                    npc.localAI[0] = 0;
                }
            }
            if (flag5)
            {
                if(flag5s)
                {
                    float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                    Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                    Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                    Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                    Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                    flag5s = false;
                }
                flag2b = true;
                npc.localAI[0] += 1;
                down = true;
                base.npc.velocity *= 0.97f;
                float num20 = 1.8f;
                if (base.npc.velocity.Length() < num20 * 3f)
                {
                    base.npc.TargetClosest(true);
                    float num21 = 8f;
                    Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                    float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                    float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    num6 = num21 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) * ((base.npc.velocity.Length() - 1.8f) / 3.6f) + (float)Math.Atan2(-num4, num5) * (1 - ((base.npc.velocity.Length() - 1.8f) / 3.6f)) + (float)Math.PI;
                }
                else
                {
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                }
                int p = Dust.NewDust(base.npc.position, npc.width, npc.height, 33, 0, 0, 0, default(Color), 2f);
                Main.dust[p].velocity.X = 0;
                Main.dust[p].velocity.Y = 0;
                Main.dust[p].noGravity = true;
                if (base.npc.velocity.Length() < num20)
                {
                    base.npc.TargetClosest(true);
                    float num21 = 8f;
                    Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                    float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                    float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    num6 = num21 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    base.npc.velocity.X = num4 * 4;
                    base.npc.velocity.Y = num5 * 4;
                    return;
                }
                flag2b = true;
                if (npc.localAI[0] % 500 == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        Vector2 vector = new Vector2(Main.rand.Next(1370, 1550) / 20f, 0).RotatedByRandom(Math.PI * 2);
                        int numl = Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, vector.X, vector.Y, 405, 88, 0, Main.myPlayer,0, 0f);
                        Main.projectile[numl].scale = Main.rand.Next(95,110) / 100f;
                        Main.projectile[numl].friendly = false;
                        Main.projectile[numl].hostile = true;
                        Main.projectile[numl].timeLeft = 120;
                    }
                }
                if (npc.localAI[0] % 700 == 250)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        int num5 = (int)Player.FindClosest(base.npc.Center, 1, 1);
                        float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.npc.Center.X) * (Main.player[num5].Center.X - base.npc.Center.X) + (Main.player[num5].Center.Y - base.npc.Center.Y) * (Main.player[num5].Center.Y - base.npc.Center.Y));
                        Vector2 vector = (Main.player[num5].Center - base.npc.Center).RotatedBy(Main.rand.Next(-200, 200) / 2000f) / num6 * Main.rand.Next(300, 2000) / 200f;
                        Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, vector.X, vector.Y, base.mod.ProjectileType("PhantomFish"), 88, 0, Main.myPlayer, Main.rand.Next(-200, 200) / 100f, 0f);
                    }
                }
                if (npc.localAI[0] % 500 == 250)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        int num5 = (int)Player.FindClosest(base.npc.Center, 1, 1);
                        float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.npc.Center.X) * (Main.player[num5].Center.X - base.npc.Center.X) + (Main.player[num5].Center.Y - base.npc.Center.Y) * (Main.player[num5].Center.Y - base.npc.Center.Y));
                        Vector2 vector = (Main.player[num5].Center - base.npc.Center).RotatedBy(Main.rand.Next(-200, 200) / 1500f) / num6 * Main.rand.Next(300, 2000) / 400f;
                        Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, vector.X, vector.Y, base.mod.ProjectileType("空间粒子流"), 45, 0, Main.myPlayer, 0, 0f);
                    }
                }
                if (npc.localAI[0] % 167 == 0)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(30, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                        Projectile.NewProjectile(base.npc.Center.X + 30, base.npc.Center.Y, v.X, v.Y, base.mod.ProjectileType("OceanCrystalSpice"), 111, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (npc.Center + new Vector2(-10, 0))).RotatedByRandom(Math.PI * 0.5f) * (17 / num3);
                        Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y, v2.X, v2.Y, base.mod.ProjectileType("OceanCrystalSpice"), 111, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (npc.localAI[0] % 333 == 0)
                {
                    Vector2 v = (player.Center - (npc.Center + new Vector2(45, 45))) * (1 / num3);
                    int num4 = Projectile.NewProjectile(base.npc.Center.X + 45, base.npc.Center.Y + 45, v.X, v.Y, base.mod.ProjectileType("Vortex"), 137, 0.2f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num4].scale = 0;
                }
                if (npc.localAI[0] % 1429 == 0)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        Vector2 v = (player.Center - (npc.Center + new Vector2(30, 0))).RotatedByRandom(Math.PI * 0.5f) * (Main.rand.Next(0, 250) / 12f / num3);
                        Projectile.NewProjectile(base.npc.Center.X + 30, base.npc.Center.Y, v.X, v.Y, base.mod.ProjectileType("AttractLightBall"), 82, 0.2f, Main.myPlayer, 0f, 0f);
                        Vector2 v2 = (player.Center - (npc.Center + new Vector2(-10, 0))).RotatedByRandom(Math.PI * 0.5f) * (Main.rand.Next(0,250) / 12f / num3);
                        Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y, v2.X, v2.Y, base.mod.ProjectileType("LeaveLightBall"), 82, 0.2f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
        }
		public override void FindFrame(int frameHeight)
		{
            frameHeight = 280;
            base.npc.frameCounter += 0.1f;
			int num = (int)base.npc.frameCounter;
            if(down)
            {
                base.npc.frame.Y = (num % 3) * frameHeight;
            }
            else
            {
                if(npc.localAI[0] > 3000 && npc.localAI[0] <= 3020)
                {
                    base.npc.frame.Y = 0 * frameHeight;
                }
                if (npc.localAI[0] > 3020)
                {
                    base.npc.frame.Y = 1 * frameHeight;
                }
            }
        }
        public override bool CheckActive()
		{
			return this.canDespawn;
		}
      
		public override void HitEffect(int hitDirection, double damage)
		{
            Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 27, 1f, 0f);
            for (int i = 0; i < 15; i++)
			{
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, mod.DustType("Crystal2"), (float)hitDirection, -1f, 0, default(Color), 1f);
			}
            MythWorld.downedHYFY = true;
			if (base.npc.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
                Vector2 v2 = new Vector2(Main.rand.Next(0,15) / 3f,0).RotatedByRandom(Math.PI * 2) + npc.velocity;
                Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y, v2.X, v2.Y, base.mod.ProjectileType("OceanSealBroken3"), (int)(base.npc.damage * 1.2f), 0.2f, Main.myPlayer, 0f, 0f);
                Vector2 v3 = new Vector2(Main.rand.Next(0, 15) / 3f, 0).RotatedByRandom(Math.PI * 2) + npc.velocity;
                Projectile.NewProjectile(base.npc.Center.X - 10, base.npc.Center.Y, v3.X, v3.Y, base.mod.ProjectileType("OceanSealBroken4"), (int)(base.npc.damage * 1.2f), 0.2f, Main.myPlayer, 0f, 0f);
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                for (int i = 0; i < 120; i++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, mod.DustType("Crystal2"), (float)hitDirection, -1f, 0, default(Color), 2f);
                }
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石1"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石4"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石1"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石4"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块5"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块5"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块5"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块6"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块6"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块6"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块7"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块8"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                Gore.NewGore(base.npc.Center, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋封印碎块9"), 1f);
                MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
                mplayer.movieTime = 120;
            }
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Mod mod = ModLoader.GetMod("MythMod");
            Texture2D texture = Main.npcTexture[base.npc.type];
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
            //MythMod.DrawTexture(spriteBatch, texture, 0, base.npc, new Color?(drawColor), false);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), npc.frame, npc.GetAlpha(drawColor), npc.rotation, new Vector2((float)texture.Width / 2f, 140), npc.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (flag1)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (base.npc.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 value = new Vector2(base.npc.Center.X, base.npc.Center.Y);
                Vector2 vector = new Vector2((float)(Main.npcTexture[base.npc.type].Width / 2), (float)(Main.npcTexture[base.npc.type].Height / Main.npcFrameCount[base.npc.type] / 2));
                Vector2 vector2 = value - Main.screenPosition;
                vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/OceanCrystal/海洋封印Glow").Width, (float)(base.mod.GetTexture("NPCs/OceanCrystal/海洋封印Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
                vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
                Color color = Utils.MultiplyRGBA(new Color(297 - base.npc.alpha, 297 - base.npc.alpha, 297 - base.npc.alpha, 0), Color.Blue);
                Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/OceanCrystal/海洋封印Glow"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
            }
            //Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/OceanCrystal/海洋封印眼睛"), vector2, new Rectangle(0, 0, 10, 10), new Color(100, 100, 100, 0), base.npc.rotation, vector, 1f, effects, 0f);
        }
        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("OceanCrystalTreasureBag"), 1, false, 0, false, false);
                return;
            }
            else
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("Aquamarine"), Main.rand.Next(Main.rand.Next(14, 75), 80), false, 0, false, false);
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("MysteriesPearl"), Main.rand.Next(Main.rand.Next(10,20), 40), false, 0, false, false);
                int type = 0;
                switch (Main.rand.Next(1, 6))
                {
                    case 1:
                        type = base.mod.ItemType("OceanBubble");
                        break;
                    case 2:
                        type = base.mod.ItemType("OceanEverStone");
                        break;
                    case 3:
                        type = base.mod.ItemType("OceanCurrentRay");
                        break;
                    case 4:
                        type = base.mod.ItemType("OceanCrystalClub");
                        break;
                    case 5:
                        type = base.mod.ItemType("OceanCrystalShield");
                        break;
                }
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, type, Main.rand.Next(Main.rand.Next(14, 75), 80), false, 0, false, false);
            }
        }
	}
}
