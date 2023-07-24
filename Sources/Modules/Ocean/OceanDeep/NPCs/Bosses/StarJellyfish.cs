using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
    [AutoloadBossHead]
    public class StarJellyfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("星渊水母");
			Main.npcFrameCount[base.npc.type] = 5;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "星渊水母");
		}
        private bool initialization = true;
        private bool canDespawn;
        private bool num14 = true;
        private bool num18 = true;
        private bool num20 = true;
        private bool flag7 = false;
        private float num11 = 0;
        private float b;
        private float num9;
        private float maxX;
        private float num10 = -1f;
        private float num13 = 0;
        private float num19 = 0;
        private int num15 = 0;
        private int npclocalai100 = 0;
        private Vector2 vector3;
		public override void SetDefaults()
		{
			base.npc.noGravity = true;
			base.npc.damage = 20;
			base.npc.width = 100;
			base.npc.height = 100;
			base.npc.defense = 50;
			base.npc.lifeMax = (Main.expertMode ? 36000 : 30000);
            if(MythWorld.Myth)
            {
                base.npc.lifeMax = 20650;
            }
			base.npc.alpha = 50;
			base.npc.aiStyle = -1;
			this.aiType = -1;
			base.npc.buffImmune[70] = true;
            base.npc.knockBackResist = 0f;
            base.npc.boss = true;
            base.npc.noGravity = true;
            base.npc.noTileCollide = false;
			base.npc.value = (float)Item.buyPrice(0, 7, 0, 0);
			base.npc.HitSound = SoundID.NPCHit25;
			base.npc.DeathSound = SoundID.NPCDeath28;
            NPCID.Sets.TrailCacheLength[base.npc.type] = 60;
            NPCID.Sets.TrailingMode[base.npc.type] = 0;
            this.music = mod.GetSoundSlot((Terraria.ModLoader.SoundType)51, "Sounds/Music/Glow");
        }
        public override void AI()
        {
            base.npc.localAI[0] += 2;
            Player player = Main.player[base.npc.target];
            bool flag3 = (double)base.npc.life <= (double)base.npc.lifeMax * 1 && (double)base.npc.life > (double)base.npc.lifeMax * 0.8 && !flag7;
            bool flag4 = (double)base.npc.life <= (double)base.npc.lifeMax * 0.8 && (double)base.npc.life > (double)base.npc.lifeMax * 0.7 && !flag7;
            bool flag5 = (double)base.npc.life <= (double)base.npc.lifeMax * 0.7 && (double)base.npc.life > (double)base.npc.lifeMax * 0.6 && !flag7;
            bool flag6 = (double)base.npc.life <= (double)base.npc.lifeMax * 0.6 && !flag7;
            bool flag = false;
            if((double)base.npc.life <= (double)base.npc.lifeMax * 0.5)
            {
                flag7 = true;
            }
            if (base.npc.ai[1] == 1f)
            {
                flag = true;
            }
            else
            {
                base.npc.dontTakeDamage = false;
            }
            float num = 1f;
            if (flag)
            {
                num += 0.5f;
            }
            if (base.npc.direction == 0)
            {
                base.npc.TargetClosest(true);
            }
            if (flag)
            {
                return;
            }
            if (!base.npc.wet && flag5 == false && flag6 == false && flag7 == false)
            {
                base.npc.dontTakeDamage = true;
                base.npc.rotation += base.npc.velocity.X * 0.1f;
                base.npc.velocity.Y = base.npc.velocity.Y + 0.2f;
                if (base.npc.velocity.Y > 7f)
                {
                    base.npc.velocity.Y = 7f;
                }
                if (base.npc.velocity.X > 4f)
                {
                    base.npc.velocity.X = 4f;
                }
                if (base.npc.velocity.X < -4f)
                {
                    base.npc.velocity.X = -4f;
                }
                if (base.npc.velocity.Y < -7f)
                {
                    base.npc.velocity.Y = -7f;
                }
                base.npc.velocity *= 0.99f;
                base.npc.ai[0] = 1f;
                if(Math.Abs(npc.velocity.Y) <= 0.25f)
                {
                    num9 += 1;
                }
                if(num9 == 120)
                {
                    vector3 = npc.position;
                    maxX = npc.position.X;
                }
                if(num9 > 120f && Math.Abs(npc.velocity.Y) <= 0.25f)
                {
                    num10 += 1;
                    if(npc.position.X > (float)(Main.maxTilesX * 4) && num10 % 80 == 0)
                    {
                        if(npc.position != vector3)
                        {
                            npc.velocity.X = 7f;
                            npc.velocity.Y = -7f;
                            if(npc.position.X > maxX)
                            {
                                maxX = npc.position.X;
                            }
                            else
                            {
                                num13 += 1;
                            }
                        }
                        else
                        {
                            npc.velocity.X = -2f;
                            npc.velocity.Y = -5f;
                        }
                        if(num13 > 5)
                        {
                            npc.noTileCollide = true;
                            npc.velocity.X = 70f;
                            npc.velocity.Y = -140f;
                            num13 = 0;
                            npc.noTileCollide = false;
                        }
                        vector3 = npc.position;
                    }
                    if(npc.position.X < (float)(Main.maxTilesX * 4) && num10 % 80 == 0)
                    {
                        if(npc.position != vector3)
                        {
                            npc.velocity.X = -7f;
                            npc.velocity.Y = -7f;
                            if(npc.position.X < maxX)
                            {
                                maxX = npc.position.X;
                            }
                            else
                            {
                                num13 += 1;
                            }
                        }
                        else
                        {
                            npc.velocity.X = 2f;
                            npc.velocity.Y = -5f;
                        }
                        if(num13 > 5)
                        {
                            npc.noTileCollide = true;
                            npc.velocity.X = -70f;
                            npc.velocity.Y = -140f;
                            num13 = 0;
                            npc.noTileCollide = false;
                        }
                        vector3 = npc.position;
                    }
                }
                return;
            }
            else
            {
                num9 = 0;
                if (flag5 == false && flag6 == false)
                {
                    if(flag7 == false)
                    {
                    base.npc.localAI[2] = 1f;
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.96f;
                    float num2 = 0.8f;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 7;
                        base.npc.velocity.Y = num5 * 7;
                        return;
                    }
                    }
                }
                else
                {
                }
            }

            if (base.npc.collideX && !flag7)
            {
                base.npc.velocity.X = base.npc.velocity.X * -1f;
                base.npc.direction *= -1;
            }
            if (base.npc.collideY && !flag7)
            {
                if (base.npc.velocity.Y > 0f)
                {
                    base.npc.velocity.Y = Math.Abs(base.npc.velocity.Y) * -1f;
                    base.npc.directionY = -1;
                    base.npc.ai[0] = -1f;
                }
                else if (base.npc.velocity.Y < 0f)
                {
                    base.npc.velocity.Y = Math.Abs(base.npc.velocity.Y);
                    base.npc.directionY = 1;
                    base.npc.ai[0] = 1f;
                }
            }
            bool flag2 = false;
            if (!base.npc.friendly)
            {
                base.npc.TargetClosest(false);
                if (!Main.player[base.npc.target].dead)
                {
                    flag2 = true;
                }
            }
            if (flag2)
            {
                if(!flag7)
                {
                base.npc.localAI[2] = 1f;
                base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                base.npc.velocity *= 0.96f;
                float num2 = 0.8f;
                if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                {
                    base.npc.TargetClosest(true);
                    float num3 = 8f;
                    Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                    float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                    float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    base.npc.velocity.X = num4 * 4;
                    base.npc.velocity.Y = num5 * 4;
                    return;
                }
                }
            }
            else
            {
                if(!flag7)
                {
                base.npc.localAI[2] = 0f;
                base.npc.velocity.X = base.npc.velocity.X + (float)base.npc.direction * 0.02f;
                base.npc.rotation = base.npc.velocity.X * 0.4f;
                if (base.npc.velocity.X < -1f || base.npc.velocity.X > 1f)
                {
                    base.npc.velocity.X = base.npc.velocity.X * 0.95f;
                }
                if (base.npc.ai[0] == -1f)
                {
                    base.npc.velocity.Y = base.npc.velocity.Y - 0.01f;
                    if (base.npc.velocity.Y < -1f)
                    {
                        base.npc.ai[0] = 1f;
                    }
                }
                else
                {
                    base.npc.velocity.Y = base.npc.velocity.Y + 0.01f;
                    if (base.npc.velocity.Y > 1f)
                    {
                        base.npc.ai[0] = -1f;
                    }
                }
                int num7 = (int)(base.npc.position.X + (float)(base.npc.width / 2)) / 16;
                int num8 = (int)(base.npc.position.Y + (float)(base.npc.height / 2)) / 16;
                if (Main.tile[num7, num8 - 1] == null)
                {
                    Main.tile[num7, num8 - 1] = new Tile();
                }
                if (Main.tile[num7, num8 + 1] == null)
                {
                    Main.tile[num7, num8 + 1] = new Tile();
                }
                if (Main.tile[num7, num8 + 2] == null)
                {
                    Main.tile[num7, num8 + 2] = new Tile();
                }
                if (Main.tile[num7, num8 - 1].liquid > 128)
                {
                    if (Main.tile[num7, num8 + 1].active())
                    {
                        base.npc.ai[0] = -1f;
                    }
                    else if (Main.tile[num7, num8 + 2].active())
                    {
                        base.npc.ai[0] = -1f;
                    }
                }
                else
                {
                    base.npc.ai[0] = 1f;
                }
                if ((double)base.npc.velocity.Y > 1.2 || (double)base.npc.velocity.Y < -1.2)
                {
                    base.npc.velocity.Y = base.npc.velocity.Y * 0.99f;
                }
                }
            }

            if (flag3 && !flag7)
            {
                if (npc.localAI[0] % 300 == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float num100 = -1000;
                        for (int j = 0; j < 8; j++)
                        {
                            Projectile.NewProjectile(Main.player[base.npc.target].position.X + 1000f, Main.player[base.npc.target].position.Y + num100, -4f, 0f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Main.player[base.npc.target].position.X - 1000f, Main.player[base.npc.target].position.Y + num100 - 125f, 4f, 0f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                            num100 += 250;
                        }
                    }
                }
            }
            if (flag4 && !flag7)
            {
                if (npc.localAI[0] % 450 == 0)
                {
                    float num100 = -1000;
                    float num102 = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        Projectile.NewProjectile(Main.player[base.npc.target].position.X + 1000f, Main.player[base.npc.target].position.Y + num100, -5f, 0f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(Main.player[base.npc.target].position.X - 1000f, Main.player[base.npc.target].position.Y + num100, 5f, 0f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(Main.player[base.npc.target].position.X + 500f + num102, Main.player[base.npc.target].position.Y + num100 * 0.7660254038f, -2.5f, 6.92820323f * 0.625f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(Main.player[base.npc.target].position.X - 500f - num102, Main.player[base.npc.target].position.Y + num100 * 0.7660254038f, 2.5f, 6.92820323f * 0.625f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(Main.player[base.npc.target].position.X + 500f + num102, Main.player[base.npc.target].position.Y - num100 * 0.7660254038f, -2.5f, -6.92820323f * 0.625f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(Main.player[base.npc.target].position.X - 500f - num102, Main.player[base.npc.target].position.Y - num100 * 0.7660254038f, 2.5f, -6.92820323f * 0.625f, base.mod.ProjectileType("RougeRay"), 40, 0f, Main.myPlayer, 0f, 0f);
                        num100 += 250;
                        num102 += 125;
                    }
                }
            }
            if (flag5 && !flag7)
            {
                for (int i = 0; i < 3; i++)
                {
                    base.npc.localAI[2] = 1f;
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.96f;
                    float num2 = 0.8f;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 9;
                        base.npc.velocity.Y = num5 * 9;
                        Vector2 vector2 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)(base.npc.height / 2));
                        float num111 = Main.player[base.npc.target].position.X + (float)Main.player[base.npc.target].width * 0.5f - vector2.X + (float)Main.rand.Next(-20, 21);
                        float num112 = Main.player[base.npc.target].position.Y + (float)Main.player[base.npc.target].height * 0.5f - vector2.Y + (float)Main.rand.Next(-20, 21);
                        float num113 = (float)Math.Sqrt((double)(num111 * num111 + num112 * num112));
                        int num114 = base.mod.ProjectileType("灿金射线");
                        float num116 = 6f;
                        num113 = num116 / num113;
                        num111 *= num113;
                        num112 *= num113;
                        num111 += (float)Main.rand.Next(-5, 6) * 0.05f;
                        num112 += (float)Main.rand.Next(-5, 6) * 0.05f;
                        vector2.X += num111 * 5f;
                        vector2.Y += num112 * 5f;
                        Projectile.NewProjectile(vector2.X, vector2.Y, num111, num112, num114, 40, 0f, Main.myPlayer, 0f, 0f);
                        base.npc.netUpdate = true;
                        return;
                    }
                }
                if (npc.localAI[0] % 550 == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float num100 = -1000;
                        for (int j = 0; j < 8; j++)
                        {
                            Projectile.NewProjectile(Main.player[base.npc.target].position.X + 1000f, Main.player[base.npc.target].position.Y + num100, -4f, 0f, base.mod.ProjectileType("RougeRay"), 200, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Main.player[base.npc.target].position.X - 1000f, Main.player[base.npc.target].position.Y + num100 - 125f, 4f, 0f, base.mod.ProjectileType("RougeRay"), 200, 0f, Main.myPlayer, 0f, 0f);
                            num100 += 250;
                        }
                    }
                }
            }
            if (flag6 && !flag7)
            {
                for (int i = 0; i < 3; i++)
                {
                    base.npc.localAI[2] = 1f;
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.96f;
                    float num2 = 0.8f;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 9;
                        base.npc.velocity.Y = num5 * 9;
                        for (int l = 0; l < 3; l++)
                        {
                            Vector2 vector2 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)(base.npc.height / 2));
                            float num111 = Main.player[base.npc.target].position.X + (float)Main.player[base.npc.target].width * 0.5f - vector2.X + (float)Main.rand.Next(-60, 61);
                            float num112 = Main.player[base.npc.target].position.Y + (float)Main.player[base.npc.target].height * 0.5f - vector2.Y + (float)Main.rand.Next(-60, 61);
                            float num113 = (float)Math.Sqrt((double)(num111 * num111 + num112 * num112));
                            int num114 = base.mod.ProjectileType("灿金射线");
                            float num116 = 6f;
                            num113 = num116 / num113;
                            num111 *= num113;
                            num112 *= num113;
                            num111 += (float)Main.rand.Next(-5, 6) * 0.05f;
                            num112 += (float)Main.rand.Next(-5, 6) * 0.05f;
                            vector2.X += num111 * 5f;
                            vector2.Y += num112 * 5f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num111, num112, num114, 40, 0f, Main.myPlayer, 0f, 0f);
                            base.npc.netUpdate = true;
                        }
                        return;
                    }
                }
            }
            if (flag7)
            {
                flag3 = false;
                flag4 = false;
                flag5 = false;
                flag6 = false;
                npclocalai100 += 1;
                base.npc.noTileCollide = true;
                if(num14)
                {
                    Color messageColor = Color.Red;
                    Main.NewText(Language.GetTextValue("开始吧!"), messageColor);
                    Main.NewText(Language.GetTextValue("海水已经被StarPoi污染，你的身体一旦触碰到阳光，毒素就会开始溶解你的皮肤"), messageColor);
                    num15 = npc.lifeMax;
                    num14 = false;
                }
                if(npclocalai100 <= num15 / 240f + 2)
                {
                    npc.lifeMax += 180;
                    if(npc.life <= npc.lifeMax - 420)
                    {
                        npc.life += 420;
                    }
                    else
                    {
                        npc.life = npc.lifeMax;
                    }
                    int type;
                    for (int j = 0; j < 8; j++)
                    {
                        Vector2 vector4 = new Vector2(0,Main.rand.Next(0,200)).RotatedBy(Math.PI * Main.rand.Next(0,2000) / 1000f);
                        if(Main.rand.Next(0,100) > 94)
                        {
                            type = mod.DustType("GoldGlitter");
                        }
                        else
                        {
                            type = 183;
                        }
                        int num13 = Dust.NewDust(base.npc.Center, 0, 0, 183, (float)Math.Sin(j / 4f * Math.PI) * 20f, (float)Math.Cos(j / 4f * Math.PI) * 20f, 200, Color.White, 2.4f);
                        Main.dust[num13].noGravity = true;
                        Main.dust[num13].velocity = new Vector2((float)Math.Sin(j / 4f * Math.PI) * 20f, (float)Math.Cos(j / 4f * Math.PI) * 20f).RotatedBy((float)Math.PI * Main.rand.Next(0,400) / 200f) * Main.rand.Next(0,400) / 400f;
                    }
                    npc.velocity *= 0.95f;
                    if(npclocalai100 <= num15 / 480f + 2)
                    {
                        num19 += 0.01f;
                        npc.rotation += num19;
                    }
                    if(npclocalai100 > num15 / 480f + 2 && npclocalai100 < num15 / 240f + 2)
                    {
                        num19 -= 0.01f;
                        npc.rotation += num19;
                    }
                }
                if(npclocalai100 % 15 == 0)
                {
                    if(npc.life <= npc.lifeMax - 7)
                    {
                        npc.life += 7;
                    }
                    else
                    {
                        npc.life = npc.lifeMax;
                    }
                }
                if(npclocalai100 > num15 / 240f + 2 && npclocalai100 < num15 / 240f + 3)
                {
                    int type;
                    for (int k = 0; k < 80; k++)
                    {
                        if(Main.rand.Next(0,100) > 94)
                        {
                            type = mod.DustType("GoldGlitter");
                        }
                        else
                        {
                            type = 183;
                        }
                        int num13 = Dust.NewDust(base.npc.Center, 0, 0, 183, (float)Math.Sin(k / 40f * Math.PI) * 20f, (float)Math.Cos(k / 40f * Math.PI) * 20f, 200, Color.White, 2.4f);
                        Main.dust[num13].noGravity = true;
                        Main.dust[num13].velocity = new Vector2((float)Math.Sin(k / 40f * Math.PI) * 20f, (float)Math.Cos(k / 40f * Math.PI) * 20f);
                    }
                }
                if(npclocalai100 > num15 / 240f + 2 && npclocalai100 < num15 / 240f + 880)
                {
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.98f;
                    float num2 = 2f;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 7;
                        base.npc.velocity.Y = num5 * 7;
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 980 && npclocalai100 % 50 == 0)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 40);
                    int num16 = Projectile.NewProjectile(vector5.X, vector5.Y, 0, 0, base.mod.ProjectileType("星渊水母幻影"), 48, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num16].scale = Main.rand.Next(70, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2480 && (npclocalai100 - num15 / 240f) % 180 < 27)
                {
                    base.npc.velocity *= 0.95f;
                    npc.alpha += 10;
                    if(npc.alpha >= 255)
                    {
                        base.npc.rotation = 0;
                        npc.position = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 10);
                    }
                    Lighting.AddLight(base.npc.Center, 0.2f * (255 - base.npc.alpha) / 255, 0, 0f);
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2480 && (npclocalai100 - num15 / 240f) % 180 >= 27)
                {
                    npc.alpha -= 10;
                    if(npc.alpha <= 50)
                    {
                        npc.alpha = 50;
                    }
                    if(base.npc.velocity.Y > -0.8f)
		        	{
		        		base.npc.velocity = new Vector2(0, -8 * base.npc.scale);
	        		}
	        		base.npc.velocity *= 0.96f;
                    Lighting.AddLight(base.npc.Center, 0.2f * (255 - base.npc.alpha) / 255, 0, 0f);
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2280 && npclocalai100 % 80 == 0 && !MythWorld.Myth)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(vector5.X, vector5.Y, 0, 0, base.mod.ProjectileType("星渊水母幻影"), 48, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(70, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2280 && npclocalai100 % 60 == 0 && MythWorld.Myth)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(vector5.X, vector5.Y, 0, 0, base.mod.ProjectileType("星渊水母幻影"), 48, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(70, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 2480 && npclocalai100 < num15 / 240f + 3500)
                {
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.97f;
                    float num2 = 1.8f;
                    int p = Dust.NewDust(base.npc.Center, 0, 0, 183, 0, 0, 0, default(Color), 2f);
	        	    Main.dust[p].velocity.X = 0;
	        	    Main.dust[p].velocity.Y = 0;
	        	    Main.dust[p].noGravity = true;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 4;
                        base.npc.velocity.Y = num5 * 4;
                        Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 14, 1f, 0f);
                        for (int k = 0; k < 20; k++)
                        {
                            float i = k + 0.5f;
                            if((int)k % 2 == 1)
                            {
                                Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, (float)Math.Cos(((float)i / 10f) * Math.PI) * 7 / 3 * 1.44f, (float)(0 - (float)Math.Sin(((float)i / 10f) * Math.PI) * 7) / 3 * 1.44f, base.mod.ProjectileType("水母磷光1"), base.npc.damage / 2, 0.2f, Main.myPlayer, 0f, 0f);
                            }
                            else
                            {
                                Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, (float)Math.Cos(((float)i / 10f) * Math.PI) * 7 / 3 * 1.44f, (float)(0 - (float)Math.Sin(((float)i / 10f) * Math.PI) * 7) / 3 * 1.44f, base.mod.ProjectileType("水母磷光2"), base.npc.damage / 2, 0.2f, Main.myPlayer, 0f, 0f);
                            }
                        }
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 3500 && npclocalai100 < num15 / 240f + 4500 && npclocalai100 % 18 == 0)
                {
                    Vector2 vector8 = new Vector2(0, 1000).RotatedBy(npclocalai100 / 40f);
                    Vector2 vector6 = Main.player[base.npc.target].position + vector8;
                    Vector2 vector7 = Main.player[base.npc.target].position - vector8;
                    Projectile.NewProjectile(vector6.X, vector6.Y, -vector8.X / 180f, -vector8.Y / 180f, base.mod.ProjectileType("RougeRay"), 44, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(vector7.X, vector7.Y, vector8.X / 180f, vector8.Y / 180f, base.mod.ProjectileType("RougeRay"), 44, 0f, Main.myPlayer, 0f, 0f);
                }
                if(npclocalai100 > num15 / 240f + 3500 && npclocalai100 < num15 / 240f + 4500)
                {
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.97f;
                    float num2 = 1.8f;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 4;
                        base.npc.velocity.Y = num5 * 4;
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 4520 && npc.life >= npc.lifeMax * 0.45f)
                {
                    npclocalai100 = (int)(num15 / 240f + 20);
                }
                if(npc.life <= npc.lifeMax * 0.65f && num18 && npclocalai100 > num15 / 240f + 100)
                {
			    	Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, 2.5f, 2.5f, base.mod.ProjectileType("渊海磷光"), base.npc.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, -2.5f, 2.5f, base.mod.ProjectileType("渊海磷光"), base.npc.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, 2.5f, -2.5f, base.mod.ProjectileType("渊海磷光"), base.npc.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, -2.5f, -2.5f, base.mod.ProjectileType("渊海磷光"), base.npc.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    num18 = false;
                }
                if(npclocalai100 > num15 / 240f + 4550 && npclocalai100 < num15 / 240f + 4650 && npclocalai100 % 100 == 0)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(vector5.X, vector5.Y, 0, 0, base.mod.ProjectileType("水母电击"), 200, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(90, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 4650 && npclocalai100 < num15 / 240f + 5250 && npclocalai100 % 600 == 0)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(vector5.X, vector5.Y, 0, 0, base.mod.ProjectileType("水母电击"), 200, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(90, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 4550 && npclocalai100 < num15 / 240f + 5255)
                {
                    base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
                    base.npc.velocity *= 0.97f;
                    float num2 = 1.8f;
                    if (base.npc.velocity.X > -num2 && base.npc.velocity.X < num2 && base.npc.velocity.Y > -num2 && base.npc.velocity.Y < num2)
                    {
                        base.npc.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                        float num4 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2) - vector.X;
                        float num5 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.npc.velocity.X = num4 * 4;
                        base.npc.velocity.Y = num5 * 4;
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 5255)
                {
                    npclocalai100 = (int)(num15 / 240f + 20);
                }
                if(npc.life <= npc.lifeMax * 0.35f && MythWorld.Myth && npclocalai100 % 480 == 0)
                {
                    Vector2 vector = npc.Center + new Vector2(0f, (float)npc.height / 2f);
                    NPC.NewNPC((int)vector.X, (int)vector.Y, mod.NPCType("StarHydra"), 0, 0f, 0f, 0f, 0f, 255);
                }
                if(npc.life <= npc.lifeMax * 0.05f && MythWorld.Myth && num20)
                {
                    for (int j = 0; j < 18; j++)
                    {
                        Vector2 vector = npc.Center + new Vector2(0f, (float)npc.height / 2f);
                        NPC.NewNPC((int)vector.X, (int)vector.Y, mod.NPCType("StarHydra"), 0, 0f, 0f, 0f, 0f, 255);
                    }
                    num20 = false;
                }
                else
		    	{
			     	this.canDespawn = false;
		    	}
                if(NPC.CountNPCS(mod.NPCType("StarHydra")) > 7 || !player.wet)      
                {
                    npc.dontTakeDamage = true;
                }
                else
                {
                    npc.dontTakeDamage = false;
                    flag7 = true;
                }
                MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
                if(mplayer.ZoneDeepocean)
                {
                    npc.defense = 0;
                    base.npc.damage = 40;
                    player.AddBuff(base.mod.BuffType("StarPoi"), 1, true);
                }
                else
                {
                    npc.defense = 160;
                    base.npc.damage = 200;
                    if(!player.wet)
                    {
                        player.AddBuff(base.mod.BuffType("StarPoi3"), 1, true);
                    }
                    else
                    {
                        player.AddBuff(base.mod.BuffType("StarPoi2"), 1, true);
                    }
                }
            }
            if (!player.active || player.dead)
		    	{
			     	player = Main.player[base.npc.target];
			     	if (!player.active || player.dead)
			    	{
			    		base.npc.velocity += new Vector2(0f, 2.5f);
                        flag7 = false;
                        flag6 = false;
                        flag5 = false;
                        flag4 = false;
                        flag3 = false;
                        flag2 = false;
                        flag = false;
			    		this.canDespawn = true;
			    		if (base.npc.timeLeft > 150)
			    		{
				    		base.npc.timeLeft = 150;
				     	}
				    	return;
			    	}
		    	}
        }
		// Token: 0x06001478 RID: 5240 RVA: 0x000A9970 File Offset: 0x000A7B70
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 0.15000000596046448;
			base.npc.frameCounter %= (double)Main.npcFrameCount[base.npc.type];
			int num = (int)base.npc.frameCounter;
			base.npc.frame.Y = num * frameHeight;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000B4268 File Offset: 0x000B2468

		// Token: 0x0600147A RID: 5242 RVA: 0x0000801E File Offset: 0x0000621E
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(70, 240, true);
            player.AddBuff(base.mod.BuffType("ExPoi"), 60, true);
		}
        // Token: 0x02000413 RID: 1043
        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
			//SpriteEffects effects = SpriteEffects.None;
            //if (base.npc.spriteDirection == 1)
            //{
                //effects = SpriteEffects.FlipHorizontally;
            //}
			//int frameHeight = 100;
            //Vector2 value = new Vector2(base.npc.Center.X, base.npc.Center.Y);
            //Vector2 vector = new Vector2((float)(Main.npcTexture[base.npc.type].Width / 2) + 2, (float)(Main.npcTexture[base.npc.type].Height / Main.npcFrameCount[base.npc.type] / 2) + 4);
            //Vector2 vector2 = value - Main.screenPosition;
            //for (int k = 0; k < npc.oldPos.Length; k++)
            //{
				//Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + vector + new Vector2(0f, npc.gfxOffY);
                //Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
                //spriteBatch.Draw(base.mod.GetTexture("NPCs/星渊水母触手"), drawPos, new Rectangle(0 ,frameHeight * (2 - (((int)npc.frameCounter + (int)(k / 1.58333333f)) % 3)), (int)(base.mod.GetTexture("NPCs/星渊水母触手").Width) ,frameHeight), color, (float)npc.oldRot[k], vector, npc.scale, effects, 0f);
            //}
            //return true;
        //}
        public override bool CheckActive()
		{
			return this.canDespawn;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.npc.Center.X, base.npc.Center.Y);
            Vector2 vector = new Vector2((float)(Main.npcTexture[base.npc.type].Width / 2), (float)(Main.npcTexture[base.npc.type].Height / Main.npcFrameCount[base.npc.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/星渊水母光辉").Width, (float)(base.mod.GetTexture("NPCs/星渊水母光辉").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.npc.alpha, 297 - base.npc.alpha, 297 - base.npc.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/星渊水母光辉"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.npc.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
        // Token: 0x02000413 RID: 1043
        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("StarJellyFishTreasureBag"), 1, false, 0, false, false);
                return;
            }
            else
            {
                if (Main.rand.Next(9) == 0)
                {
                    Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("StarJellyFishPlatf"), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("TentacleBow"), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("GlowingJellyStaff"), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("CarmineBlade"), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("RedGlassSpear"), 1, false, 0, false, false);
                }
            }
            if (!MythWorld.downedXYSM)
            {
                MythWorld.downedXYSM = true;
            }
        }
	}
}
