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

namespace Everglow.Ocean.NPCs
{
    [AutoloadBossHead]
    public class StarJellyfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("星渊水母");
			Main.npcFrameCount[base.NPC.type] = 5;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "星渊水母");
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
			base.NPC.noGravity = true;
			base.NPC.damage = 20;
			base.NPC.width = 100;
			base.NPC.height = 100;
			base.NPC.defense = 50;
			base.NPC.lifeMax = (Main.expertMode ? 36000 : 30000);
            if(Main.masterMode)
            {
                base.NPC.lifeMax = 20650;
            }
			base.NPC.alpha = 50;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
			base.NPC.buffImmune[70] = true;
            base.NPC.knockBackResist = 0f;
            base.NPC.boss = true;
            base.NPC.noGravity = true;
            base.NPC.noTileCollide = false;
			base.NPC.value = (float)Item.buyPrice(0, 7, 0, 0);
			base.NPC.HitSound = SoundID.NPCHit25;
			base.NPC.DeathSound = SoundID.NPCDeath28;
            NPCID.Sets.TrailCacheLength[base.NPC.type] = 60;
            NPCID.Sets.TrailingMode[base.NPC.type] = 0;
            //this.Music = Mod.GetSoundSlot((Terraria.ModLoader.SoundType)51, "Sounds/Music/Glow");
        }
        public override void AI()
        {
            base.NPC.localAI[0] += 2;
            Player player = Main.player[base.NPC.target];
            bool flag3 = (double)base.NPC.life <= (double)base.NPC.lifeMax * 1 && (double)base.NPC.life > (double)base.NPC.lifeMax * 0.8 && !flag7;
            bool flag4 = (double)base.NPC.life <= (double)base.NPC.lifeMax * 0.8 && (double)base.NPC.life > (double)base.NPC.lifeMax * 0.7 && !flag7;
            bool flag5 = (double)base.NPC.life <= (double)base.NPC.lifeMax * 0.7 && (double)base.NPC.life > (double)base.NPC.lifeMax * 0.6 && !flag7;
            bool flag6 = (double)base.NPC.life <= (double)base.NPC.lifeMax * 0.6 && !flag7;
            bool flag = false;
            if((double)base.NPC.life <= (double)base.NPC.lifeMax * 0.5)
            {
                flag7 = true;
            }
            if (base.NPC.ai[1] == 1f)
            {
                flag = true;
            }
            else
            {
                base.NPC.dontTakeDamage = false;
            }
            float num = 1f;
            if (flag)
            {
                num += 0.5f;
            }
            if (base.NPC.direction == 0)
            {
                base.NPC.TargetClosest(true);
            }
            if (flag)
            {
                return;
            }
            if (!base.NPC.wet && flag5 == false && flag6 == false && flag7 == false)
            {
                base.NPC.dontTakeDamage = true;
                base.NPC.rotation += base.NPC.velocity.X * 0.1f;
                base.NPC.velocity.Y = base.NPC.velocity.Y + 0.2f;
                if (base.NPC.velocity.Y > 7f)
                {
                    base.NPC.velocity.Y = 7f;
                }
                if (base.NPC.velocity.X > 4f)
                {
                    base.NPC.velocity.X = 4f;
                }
                if (base.NPC.velocity.X < -4f)
                {
                    base.NPC.velocity.X = -4f;
                }
                if (base.NPC.velocity.Y < -7f)
                {
                    base.NPC.velocity.Y = -7f;
                }
                base.NPC.velocity *= 0.99f;
                base.NPC.ai[0] = 1f;
                if(Math.Abs(NPC.velocity.Y) <= 0.25f)
                {
                    num9 += 1;
                }
                if(num9 == 120)
                {
                    vector3 = NPC.position;
                    maxX = NPC.position.X;
                }
                if(num9 > 120f && Math.Abs(NPC.velocity.Y) <= 0.25f)
                {
                    num10 += 1;
                    if(NPC.position.X > (float)(Main.maxTilesX * 4) && num10 % 80 == 0)
                    {
                        if(NPC.position != vector3)
                        {
                            NPC.velocity.X = 7f;
                            NPC.velocity.Y = -7f;
                            if(NPC.position.X > maxX)
                            {
                                maxX = NPC.position.X;
                            }
                            else
                            {
                                num13 += 1;
                            }
                        }
                        else
                        {
                            NPC.velocity.X = -2f;
                            NPC.velocity.Y = -5f;
                        }
                        if(num13 > 5)
                        {
                            NPC.noTileCollide = true;
                            NPC.velocity.X = 70f;
                            NPC.velocity.Y = -140f;
                            num13 = 0;
                            NPC.noTileCollide = false;
                        }
                        vector3 = NPC.position;
                    }
                    if(NPC.position.X < (float)(Main.maxTilesX * 4) && num10 % 80 == 0)
                    {
                        if(NPC.position != vector3)
                        {
                            NPC.velocity.X = -7f;
                            NPC.velocity.Y = -7f;
                            if(NPC.position.X < maxX)
                            {
                                maxX = NPC.position.X;
                            }
                            else
                            {
                                num13 += 1;
                            }
                        }
                        else
                        {
                            NPC.velocity.X = 2f;
                            NPC.velocity.Y = -5f;
                        }
                        if(num13 > 5)
                        {
                            NPC.noTileCollide = true;
                            NPC.velocity.X = -70f;
                            NPC.velocity.Y = -140f;
                            num13 = 0;
                            NPC.noTileCollide = false;
                        }
                        vector3 = NPC.position;
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
                    base.NPC.localAI[2] = 1f;
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.96f;
                    float num2 = 0.8f;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 7;
                        base.NPC.velocity.Y = num5 * 7;
                        return;
                    }
                    }
                }
                else
                {
                }
            }

            if (base.NPC.collideX && !flag7)
            {
                base.NPC.velocity.X = base.NPC.velocity.X * -1f;
                base.NPC.direction *= -1;
            }
            if (base.NPC.collideY && !flag7)
            {
                if (base.NPC.velocity.Y > 0f)
                {
                    base.NPC.velocity.Y = Math.Abs(base.NPC.velocity.Y) * -1f;
                    base.NPC.directionY = -1;
                    base.NPC.ai[0] = -1f;
                }
                else if (base.NPC.velocity.Y < 0f)
                {
                    base.NPC.velocity.Y = Math.Abs(base.NPC.velocity.Y);
                    base.NPC.directionY = 1;
                    base.NPC.ai[0] = 1f;
                }
            }
            bool flag2 = false;
            if (!base.NPC.friendly)
            {
                base.NPC.TargetClosest(false);
                if (!Main.player[base.NPC.target].dead)
                {
                    flag2 = true;
                }
            }
            if (flag2)
            {
                if(!flag7)
                {
                base.NPC.localAI[2] = 1f;
                base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                base.NPC.velocity *= 0.96f;
                float num2 = 0.8f;
                if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                {
                    base.NPC.TargetClosest(true);
                    float num3 = 8f;
                    Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                    float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                    float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    base.NPC.velocity.X = num4 * 4;
                    base.NPC.velocity.Y = num5 * 4;
                    return;
                }
                }
            }
            else
            {
                if(!flag7)
                {
                base.NPC.localAI[2] = 0f;
                base.NPC.velocity.X = base.NPC.velocity.X + (float)base.NPC.direction * 0.02f;
                base.NPC.rotation = base.NPC.velocity.X * 0.4f;
                if (base.NPC.velocity.X < -1f || base.NPC.velocity.X > 1f)
                {
                    base.NPC.velocity.X = base.NPC.velocity.X * 0.95f;
                }
                if (base.NPC.ai[0] == -1f)
                {
                    base.NPC.velocity.Y = base.NPC.velocity.Y - 0.01f;
                    if (base.NPC.velocity.Y < -1f)
                    {
                        base.NPC.ai[0] = 1f;
                    }
                }
                else
                {
                    base.NPC.velocity.Y = base.NPC.velocity.Y + 0.01f;
                    if (base.NPC.velocity.Y > 1f)
                    {
                        base.NPC.ai[0] = -1f;
                    }
                }
                int num7 = (int)(base.NPC.position.X + (float)(base.NPC.width / 2)) / 16;
                int num8 = (int)(base.NPC.position.Y + (float)(base.NPC.height / 2)) / 16;
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
                if (Main.tile[num7, num8 - 1].LiquidAmount > 128)
                {
                    if (Main.tile[num7, num8 + 1].HasTile)
                    {
                        base.NPC.ai[0] = -1f;
                    }
                    else if (Main.tile[num7, num8 + 2].HasTile)
                    {
                        base.NPC.ai[0] = -1f;
                    }
                }
                else
                {
                    base.NPC.ai[0] = 1f;
                }
                if ((double)base.NPC.velocity.Y > 1.2 || (double)base.NPC.velocity.Y < -1.2)
                {
                    base.NPC.velocity.Y = base.NPC.velocity.Y * 0.99f;
                }
                }
            }

            if (flag3 && !flag7)
            {
                if (NPC.localAI[0] % 300 == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float num100 = -1000;
                        for (int j = 0; j < 8; j++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X + 1000f, Main.player[base.NPC.target].position.Y + num100, -4f, 0f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X - 1000f, Main.player[base.NPC.target].position.Y + num100 - 125f, 4f, 0f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                            num100 += 250;
                        }
                    }
                }
            }
            if (flag4 && !flag7)
            {
                if (NPC.localAI[0] % 450 == 0)
                {
                    float num100 = -1000;
                    float num102 = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X + 1000f, Main.player[base.NPC.target].position.Y + num100, -5f, 0f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X - 1000f, Main.player[base.NPC.target].position.Y + num100, 5f, 0f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X + 500f + num102, Main.player[base.NPC.target].position.Y + num100 * 0.7660254038f, -2.5f, 6.92820323f * 0.625f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X - 500f - num102, Main.player[base.NPC.target].position.Y + num100 * 0.7660254038f, 2.5f, 6.92820323f * 0.625f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X + 500f + num102, Main.player[base.NPC.target].position.Y - num100 * 0.7660254038f, -2.5f, -6.92820323f * 0.625f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X - 500f - num102, Main.player[base.NPC.target].position.Y - num100 * 0.7660254038f, 2.5f, -6.92820323f * 0.625f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        num100 += 250;
                        num102 += 125;
                    }
                }
            }
            if (flag5 && !flag7)
            {
                for (int i = 0; i < 3; i++)
                {
                    base.NPC.localAI[2] = 1f;
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.96f;
                    float num2 = 0.8f;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 9;
                        base.NPC.velocity.Y = num5 * 9;
                        Vector2 vector2 = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)(base.NPC.height / 2));
                        float num111 = Main.player[base.NPC.target].position.X + (float)Main.player[base.NPC.target].width * 0.5f - vector2.X + (float)Main.rand.Next(-20, 21);
                        float num112 = Main.player[base.NPC.target].position.Y + (float)Main.player[base.NPC.target].height * 0.5f - vector2.Y + (float)Main.rand.Next(-20, 21);
                        float num113 = (float)Math.Sqrt((double)(num111 * num111 + num112 * num112));
                        int num114 =ModContent.ProjectileType<Everglow.Ocean.Projectiles.灿金射线>();
                        float num116 = 6f;
                        num113 = num116 / num113;
                        num111 *= num113;
                        num112 *= num113;
                        num111 += (float)Main.rand.Next(-5, 6) * 0.05f;
                        num112 += (float)Main.rand.Next(-5, 6) * 0.05f;
                        vector2.X += num111 * 5f;
                        vector2.Y += num112 * 5f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), vector2.X, vector2.Y, num111, num112, num114, 40, 0f, Main.myPlayer, 0f, 0f);
                        base.NPC.netUpdate = true;
                        return;
                    }
                }
                if (NPC.localAI[0] % 550 == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float num100 = -1000;
                        for (int j = 0; j < 8; j++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X + 1000f, Main.player[base.NPC.target].position.Y + num100, -4f, 0f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 200, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[base.NPC.target].position.X - 1000f, Main.player[base.NPC.target].position.Y + num100 - 125f, 4f, 0f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 200, 0f, Main.myPlayer, 0f, 0f);
                            num100 += 250;
                        }
                    }
                }
            }
            if (flag6 && !flag7)
            {
                for (int i = 0; i < 3; i++)
                {
                    base.NPC.localAI[2] = 1f;
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.96f;
                    float num2 = 0.8f;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 9;
                        base.NPC.velocity.Y = num5 * 9;
                        for (int l = 0; l < 3; l++)
                        {
                            Vector2 vector2 = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)(base.NPC.height / 2));
                            float num111 = Main.player[base.NPC.target].position.X + (float)Main.player[base.NPC.target].width * 0.5f - vector2.X + (float)Main.rand.Next(-60, 61);
                            float num112 = Main.player[base.NPC.target].position.Y + (float)Main.player[base.NPC.target].height * 0.5f - vector2.Y + (float)Main.rand.Next(-60, 61);
                            float num113 = (float)Math.Sqrt((double)(num111 * num111 + num112 * num112));
                            int num114 =ModContent.ProjectileType<Everglow.Ocean.Projectiles.灿金射线>();
                            float num116 = 6f;
                            num113 = num116 / num113;
                            num111 *= num113;
                            num112 *= num113;
                            num111 += (float)Main.rand.Next(-5, 6) * 0.05f;
                            num112 += (float)Main.rand.Next(-5, 6) * 0.05f;
                            vector2.X += num111 * 5f;
                            vector2.Y += num112 * 5f;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), vector2.X, vector2.Y, num111, num112, num114, 40, 0f, Main.myPlayer, 0f, 0f);
                            base.NPC.netUpdate = true;
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
                base.NPC.noTileCollide = true;
                if(num14)
                {
                    Color messageColor = Color.Red;
                    Main.NewText(Language.GetTextValue("开始吧!"), messageColor);
                    Main.NewText(Language.GetTextValue("海水已经被StarPoi污染，你的身体一旦触碰到阳光，毒素就会开始溶解你的皮肤"), messageColor);
                    num15 = NPC.lifeMax;
                    num14 = false;
                }
                if(npclocalai100 <= num15 / 240f + 2)
                {
                    NPC.lifeMax += 180;
                    if(NPC.life <= NPC.lifeMax - 420)
                    {
                        NPC.life += 420;
                    }
                    else
                    {
                        NPC.life = NPC.lifeMax;
                    }
                    int type;
                    for (int j = 0; j < 8; j++)
                    {
                        Vector2 vector4 = new Vector2(0,Main.rand.Next(0,200)).RotatedBy(Math.PI * Main.rand.Next(0,2000) / 1000f);
                        if(Main.rand.Next(0,100) > 94)
                        {
                            type = ModContent.DustType<Everglow.Ocean.Dusts.GoldGlitter>();
                        }
                        else
                        {
                            type = 183;
                        }
                        int num13 = Dust.NewDust(base.NPC.Center, 0, 0, 183, (float)Math.Sin(j / 4f * Math.PI) * 20f, (float)Math.Cos(j / 4f * Math.PI) * 20f, 200, Color.White, 2.4f);
                        Main.dust[num13].noGravity = true;
                        Main.dust[num13].velocity = new Vector2((float)Math.Sin(j / 4f * Math.PI) * 20f, (float)Math.Cos(j / 4f * Math.PI) * 20f).RotatedBy((float)Math.PI * Main.rand.Next(0,400) / 200f) * Main.rand.Next(0,400) / 400f;
                    }
                    NPC.velocity *= 0.95f;
                    if(npclocalai100 <= num15 / 480f + 2)
                    {
                        num19 += 0.01f;
                        NPC.rotation += num19;
                    }
                    if(npclocalai100 > num15 / 480f + 2 && npclocalai100 < num15 / 240f + 2)
                    {
                        num19 -= 0.01f;
                        NPC.rotation += num19;
                    }
                }
                if(npclocalai100 % 15 == 0)
                {
                    if(NPC.life <= NPC.lifeMax - 7)
                    {
                        NPC.life += 7;
                    }
                    else
                    {
                        NPC.life = NPC.lifeMax;
                    }
                }
                if(npclocalai100 > num15 / 240f + 2 && npclocalai100 < num15 / 240f + 3)
                {
                    int type;
                    for (int k = 0; k < 80; k++)
                    {
                        if(Main.rand.Next(0,100) > 94)
                        {
                            type = ModContent.DustType<Everglow.Ocean.Dusts.GoldGlitter>();
                        }
                        else
                        {
                            type = 183;
                        }
                        int num13 = Dust.NewDust(base.NPC.Center, 0, 0, 183, (float)Math.Sin(k / 40f * Math.PI) * 20f, (float)Math.Cos(k / 40f * Math.PI) * 20f, 200, Color.White, 2.4f);
                        Main.dust[num13].noGravity = true;
                        Main.dust[num13].velocity = new Vector2((float)Math.Sin(k / 40f * Math.PI) * 20f, (float)Math.Cos(k / 40f * Math.PI) * 20f);
                    }
                }
                if(npclocalai100 > num15 / 240f + 2 && npclocalai100 < num15 / 240f + 880)
                {
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.98f;
                    float num2 = 2f;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 7;
                        base.NPC.velocity.Y = num5 * 7;
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 980 && npclocalai100 % 50 == 0)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 40);
                    int num16 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector5.X, vector5.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.星渊水母幻影>(), 48, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num16].scale = Main.rand.Next(70, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2480 && (npclocalai100 - num15 / 240f) % 180 < 27)
                {
                    base.NPC.velocity *= 0.95f;
                    NPC.alpha += 10;
                    if(NPC.alpha >= 255)
                    {
                        base.NPC.rotation = 0;
                        NPC.position = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 10);
                    }
                    Lighting.AddLight(base.NPC.Center, 0.2f * (255 - base.NPC.alpha) / 255, 0, 0f);
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2480 && (npclocalai100 - num15 / 240f) % 180 >= 27)
                {
                    NPC.alpha -= 10;
                    if(NPC.alpha <= 50)
                    {
                        NPC.alpha = 50;
                    }
                    if(base.NPC.velocity.Y > -0.8f)
		        	{
		        		base.NPC.velocity = new Vector2(0, -8 * base.NPC.scale);
	        		}
	        		base.NPC.velocity *= 0.96f;
                    Lighting.AddLight(base.NPC.Center, 0.2f * (255 - base.NPC.alpha) / 255, 0, 0f);
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2280 && npclocalai100 % 80 == 0 && !Main.masterMode)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector5.X, vector5.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.星渊水母幻影>(), 48, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(70, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 880 && npclocalai100 < num15 / 240f + 2280 && npclocalai100 % 60 == 0 && Main.masterMode)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector5.X, vector5.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.星渊水母幻影>(), 48, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(70, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 2480 && npclocalai100 < num15 / 240f + 3500)
                {
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.97f;
                    float num2 = 1.8f;
                    int p = Dust.NewDust(base.NPC.Center, 0, 0, 183, 0, 0, 0, default(Color), 2f);
	        	    Main.dust[p].velocity.X = 0;
	        	    Main.dust[p].velocity.Y = 0;
	        	    Main.dust[p].noGravity = true;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 4;
                        base.NPC.velocity.Y = num5 * 4;
                        SoundEngine.PlaySound(SoundID.Item14, new Vector2(base.NPC.position.X, base.NPC.position.Y));
                        for (int k = 0; k < 20; k++)
                        {
                            float i = k + 0.5f;
                            if((int)k % 2 == 1)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, (float)Math.Cos(((float)i / 10f) * Math.PI) * 7 / 3 * 1.44f, (float)(0 - (float)Math.Sin(((float)i / 10f) * Math.PI) * 7) / 3 * 1.44f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.水母磷光1>(), base.NPC.damage / 2, 0.2f, Main.myPlayer, 0f, 0f);
                            }
                            else
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, (float)Math.Cos(((float)i / 10f) * Math.PI) * 7 / 3 * 1.44f, (float)(0 - (float)Math.Sin(((float)i / 10f) * Math.PI) * 7) / 3 * 1.44f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.水母磷光2>(), base.NPC.damage / 2, 0.2f, Main.myPlayer, 0f, 0f);
                            }
                        }
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 3500 && npclocalai100 < num15 / 240f + 4500 && npclocalai100 % 18 == 0)
                {
                    Vector2 vector8 = new Vector2(0, 1000).RotatedBy(npclocalai100 / 40f);
                    Vector2 vector6 = Main.player[base.NPC.target].position + vector8;
                    Vector2 vector7 = Main.player[base.NPC.target].position - vector8;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), vector6.X, vector6.Y, -vector8.X / 180f, -vector8.Y / 180f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 44, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), vector7.X, vector7.Y, vector8.X / 180f, vector8.Y / 180f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.RougeRay>(), 44, 0f, Main.myPlayer, 0f, 0f);
                }
                if(npclocalai100 > num15 / 240f + 3500 && npclocalai100 < num15 / 240f + 4500)
                {
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.97f;
                    float num2 = 1.8f;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 4;
                        base.NPC.velocity.Y = num5 * 4;
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 4520 && NPC.life >= NPC.lifeMax * 0.45f)
                {
                    npclocalai100 = (int)(num15 / 240f + 20);
                }
                if(NPC.life <= NPC.lifeMax * 0.65f && num18 && npclocalai100 > num15 / 240f + 100)
                {
			    	Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, 2.5f, 2.5f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.渊海磷光>(), base.NPC.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, -2.5f, 2.5f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.渊海磷光>(), base.NPC.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, 2.5f, -2.5f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.渊海磷光>(), base.NPC.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, -2.5f, -2.5f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.渊海磷光>(), base.NPC.damage, 0.2f, Main.myPlayer, 0f, 0f);
                    num18 = false;
                }
                if(npclocalai100 > num15 / 240f + 4550 && npclocalai100 < num15 / 240f + 4650 && npclocalai100 % 100 == 0)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector5.X, vector5.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.水母电击>(), 200, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(90, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 4650 && npclocalai100 < num15 / 240f + 5250 && npclocalai100 % 600 == 0)
                {
                    Vector2 vector5 = player.position + new Vector2(0, Main.rand.Next(150, 400)).RotatedBy(Math.PI * Main.rand.Next(0, 10000) / 5000f) + new Vector2(0, 140);
                    int num17 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector5.X, vector5.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.水母电击>(), 200, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num17].scale = Main.rand.Next(90, 140) / 100f;
                }
                if(npclocalai100 > num15 / 240f + 4550 && npclocalai100 < num15 / 240f + 5255)
                {
                    base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
                    base.NPC.velocity *= 0.97f;
                    float num2 = 1.8f;
                    if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
                    {
                        base.NPC.TargetClosest(true);
                        float num3 = 8f;
                        Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                        float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                        float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        base.NPC.velocity.X = num4 * 4;
                        base.NPC.velocity.Y = num5 * 4;
                        return;
                    }
                }
                if(npclocalai100 > num15 / 240f + 5255)
                {
                    npclocalai100 = (int)(num15 / 240f + 20);
                }
                if(NPC.life <= NPC.lifeMax * 0.35f && Main.masterMode && npclocalai100 % 480 == 0)
                {
                    Vector2 vector = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<Everglow.Ocean.NPCs.StarHydra>(), 0, 0f, 0f, 0f, 0f, 255);
                }
                if(NPC.life <= NPC.lifeMax * 0.05f && Main.masterMode && num20)
                {
                    for (int j = 0; j < 18; j++)
                    {
                        Vector2 vector = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<Everglow.Ocean.NPCs.StarHydra>(), 0, 0f, 0f, 0f, 0f, 255);
                    }
                    num20 = false;
                }
                else
		    	{
			     	this.canDespawn = false;
		    	}
                if(NPC.CountNPCS(ModContent.NPCType<Everglow.Ocean.NPCs.StarHydra>()) > 7 || !player.wet)      
                {
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    NPC.dontTakeDamage = false;
                    flag7 = true;
                }
                OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
                if(mplayer.ZoneDeepocean)
                {
                    NPC.defense = 0;
                    base.NPC.damage = 40;
                    player.AddBuff(base.Mod.Find<ModBuff>("StarPoi").Type, 1, true);
                }
                else
                {
                    NPC.defense = 160;
                    base.NPC.damage = 200;
                    if(!player.wet)
                    {
                        player.AddBuff(base.Mod.Find<ModBuff>("StarPoi3").Type, 1, true);
                    }
                    else
                    {
                        player.AddBuff(base.Mod.Find<ModBuff>("StarPoi2").Type, 1, true);
                    }
                }
            }
            if (!player.active || player.dead)
		    	{
			     	player = Main.player[base.NPC.target];
			     	if (!player.active || player.dead)
			    	{
			    		base.NPC.velocity += new Vector2(0f, 2.5f);
                        flag7 = false;
                        flag6 = false;
                        flag5 = false;
                        flag4 = false;
                        flag3 = false;
                        flag2 = false;
                        flag = false;
			    		this.canDespawn = true;
			    		if (base.NPC.timeLeft > 150)
			    		{
				    		base.NPC.timeLeft = 150;
				     	}
				    	return;
			    	}
		    	}
        }
		// Token: 0x06001478 RID: 5240 RVA: 0x000A9970 File Offset: 0x000A7B70
		public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += 0.15000000596046448;
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000B4268 File Offset: 0x000B2468

		// Token: 0x0600147A RID: 5242 RVA: 0x0000801E File Offset: 0x0000621E
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(70, 240, true);
			target.AddBuff(base.Mod.Find<ModBuff>("ExPoi").Type, 60, true);
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
                //spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水母触手"), drawPos, new Rectangle(0 ,frameHeight * (2 - (((int)npc.frameCounter + (int)(k / 1.58333333f)) % 3)), (int)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水母触手").Width()) ,frameHeight), color, (float)npc.oldRot[k], vector, npc.scale, effects, 0f);
            //}
            //return true;
        //}
        public override bool CheckActive()
		{
			return this.canDespawn;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
            Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水母光辉").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水母光辉").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.NPC.alpha, 297 - base.NPC.alpha, 297 - base.NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水母光辉"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
        // Token: 0x02000413 RID: 1043
        public override void OnKill()
        {
            if (Main.expertMode)
            {
                Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.StarJellyFishTreasureBag>(), 1, false, 0, false, false);
                return;
            }
            else
            {
                if (Main.rand.Next(9) == 0)
                {
                    Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.StarJellyFishPlatf>(), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.TentacleBow>(), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.GlowingJellyStaff>(), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.CarmineBlade>(), 1, false, 0, false, false);
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.RedGlassSpear>(), 1, false, 0, false, false);
                }
            }
            if (!MythWorld.downedXYSM)
            {
                MythWorld.downedXYSM = true;
            }
        }
	}
}
