using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Projectiles;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs
{
    [AutoloadBossHead]
    public class CorruptMoth : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Moth");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "腐檀巨蛾");
            Main.npcFrameCount[NPC.type] = 4;
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "Everglow/Sources/Modules/MythModule/Bosses/CorruptMoth/NPCs/CorruptMothBoss",
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
            NPCID.Sets.TrailCacheLength[NPC.type] = 4;
            //NPCID.Sets.TrailingMode[NPC.type] = 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            string tex = "Some worm may have cocoon breaking...";
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                tex = "有些时候,腐化蠕虫也能破茧...";
            }
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,

				// Sets the description of this NPC that is listed in the bestiary.
                
				new FlavorTextBestiaryInfoElement(tex)
            });
        }
        
        public override void OnKill()
        {
            //NPC.SetEventFlagCleared(ref DownedBossSystem.downedMoth, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void SetDefaults()
        {
            NPC.behindTiles = true;
            NPC.damage = 40;
            NPC.width = 80;
            NPC.height = 120;
            NPC.defense = 0;
            NPC.lifeMax = 7000;
            if (Main.expertMode)
            {
                NPC.lifeMax = 4000;
                NPC.damage = 30;
            }
            if (Main.masterMode)
            {
                NPC.lifeMax = 5000;
                NPC.damage = 24;
            }
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 2, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit23;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.dontTakeDamage = false;
            NPC.dontTakeDamageFromHostiles = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 4;
            //NPCID.Sets.TrailingMode[NPC.type] = 0;
            if (!Main.dedServ)
            {
                Music = Common.MythContent.QuickMusic("MothFighting");
            }
        }
        public override bool CheckActive()
        {
            return canDespawn;
        }
        private bool canDespawn;
        public static int secondStageHeadSlot = -1;
        public override void BossHeadSlot(ref int index)
        {

        }
        private bool Start = false;
        public static int StaTime = 0;
        private float phamtomDis//特效幻影的距离和透明度
        {
            get => npc.localAI[2];
            set => npc.localAI[2] = value;
        }
        float lightVisual = 0;
        private void Create4DCube()
        {
            int scale = 300;
            Vector3[] cubeVec = new Vector3[] {new(1,1,1),new(1,1,-1), new(1,-1,-1),new(1,-1,1),
                                           new(-1,1,1),new(-1,1,-1) ,new(-1,-1,-1),new(-1,-1,1)};
            int counts = 5;
            for (int w = -scale; w <= scale; w += scale * 2)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    for (int a = 0; a <= 4; a += 4)
                    {
                        Vector3 v1 = cubeVec[ii + a] * scale;
                        Vector3 v2 = cubeVec[(ii + 1) % 4 + a] * scale;
                        for (int i = 0; i < counts - 1; i++)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), npc.damage / 5, 0, Main.myPlayer, npc.whoAmI);
                            (proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(Vector3.Lerp(v1, v2, (float)i / (counts - 1)), w);
                        }
                    }
                    Vector3 v3 = cubeVec[ii] * scale;
                    Vector3 v4 = cubeVec[ii + 4] * scale;
                    for (int i = 1; i < counts - 1; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), npc.damage / 5, 0, Main.myPlayer, npc.whoAmI);
                        (proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(Vector3.Lerp(v3, v4, (float)i / (counts - 1)), w);
                    }
                }
            }

            for (int a = 0; a < cubeVec.Length; a++)
            {
                Vector3 v = cubeVec[a];
                for (int i = 1; i < counts - 1; i++)
                {
                    float c = (counts - 1) / 2;
                    Projectile proj = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), npc.damage / 5, 0, Main.myPlayer, npc.whoAmI);
                    (proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(v * scale, (float)(i - c) * scale / c);
                }
            }
        }
        public override void AI()
        {
            bool phase2 = npc.life < npc.lifeMax * 0.6f;
            Lighting.AddLight(npc.Center, 0f, 0f, 0.8f * (1 - npc.alpha / 255f));
            npc.friendly = npc.dontTakeDamage;
            if (lightVisual > 0)//光效
                lightVisual -= 0.04f;
            else
                lightVisual = 0;

            //贴图旋转
            if (npc.spriteDirection > 0)
                npc.rotation = npc.velocity.Y / 15;
            else
                npc.rotation = -npc.velocity.Y / 15;
            if (Math.Abs(npc.rotation) > 1.2f)
                npc.rotation = Math.Sign(npc.rotation) * 1.2f;
            #region #前言

            if (!Start)
            {
                npc.ai[0] = 0;
                //npc.ai[0] = 114514;
                //Create4DCube();
                npc.noTileCollide = true;
                Start = true;
                for (int h = 0; h < 15; h++)
                {
                    NPC.NewNPC(null, (int)NPC.Center.X + 25, (int)NPC.Center.Y + 150, ModContent.NPCType<MothSummonEffect>());
                }
                NPC.localAI[0] = 0;
                return;
            }
            StaTime++;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 150)
                    {
                        NPC.timeLeft = 150;
                    }
                    return;
                }
            }
            #endregion

            if (npc.ai[0] == 0)
            {
                npc.dontTakeDamage = true;
                npc.noTileCollide = false;
                npc.noGravity = false;
                phamtomDis = (150 - t) * 120f / 150;
                if (++t > 150)
                {
                    npc.dontTakeDamage = false;
                    npc.noTileCollide = true;
                    npc.noGravity = true;
                    npc.ai[0]++;
                    t = 0;
                }
            }//生成
            if (npc.ai[0] == 1)
            {
                if (++t < 200)
                {
                    MoveTo(player.Center, 5, 40);
                    GetDir_ByPlayer();
                }
                if (t == 200)
                {
                    npc.ai[2] = phase2 ? 1 : 0;
                }
                if (t > 200 && t < 650 && npc.ai[2] == 0)//冲刺
                {
                    int tt = (t - 200) % 150;
                    Vector2 getVec = new Vector2(npc.direction);
                    if (t > 500)
                        getVec = new Vector2(npc.direction, 0);
                    if (tt < 50)
                    {
                        GetDir_ByPlayer();
                        MoveTo(player.Center - getVec * 300, 15, 15);
                    }
                    if (tt > 50 && tt < 70)//前摇
                    {
                        npc.velocity = Vector2.Lerp(npc.velocity, -getVec * 10, 0.1f);
                    }
                    if (tt > 30 && tt < 70)
                        phamtomDis += (50 - tt) * 0.5f;
                    if (tt == 70)
                        lightVisual = 2;
                    if (tt > 70 && tt < 120)//冲刺
                    {
                        GreyVFx();
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                        if (t > 500 && t % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 6, 0f, Main.myPlayer);
                        if (t > 500)
                            npc.velocity = Vector2.Lerp(npc.velocity, getVec * 20, 0.15f);
                        else if (Vector2.Distance(npc.Center, player.Center) > 100)
                            npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * 20, 0.1f);
                    }
                    if (tt > 120)
                    {
                        if (Vector2.Distance(npc.Center, player.Center) > 600)
                            t++;
                        GetDir_ByPlayer();
                        MoveTo(player.Center, 6, 20);
                    }
                }
                if (t > 200 && t < 500 && npc.ai[2] == 1)//二阶段冲刺
                {
                    int tt = t % 100;
                    Vector2 getVec = new Vector2(npc.direction);
                    if (t > 400)
                        getVec = new Vector2(npc.direction, 0);
                    if (tt < 20)
                    {
                        GetDir_ByPlayer();
                        MoveTo(player.Center + player.velocity * 10 - getVec * 300, 15, 15);
                    }
                    if (tt > 20 && tt < 40)//前摇
                    {
                        npc.velocity = Vector2.Lerp(npc.velocity, -getVec * 10, 0.1f);
                    }
                    if (tt > 0 && tt < 40)
                        phamtomDis += (20 - tt) * 0.5f;
                    if (tt == 40)
                        lightVisual = 2;
                    if (tt > 40 && tt < 80)//冲刺
                    {
                        GreyVFx();
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                        if (t % 6 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 6, 0f, Main.myPlayer);
                        if (t > 500)
                            npc.velocity = Vector2.Lerp(npc.velocity, getVec * 20, 0.15f);
                        else if (Vector2.Distance(npc.Center, player.Center) > 100)
                            npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * 21, 0.1f);
                    }
                    if (tt > 80)
                    {
                        if (Vector2.Distance(npc.Center, player.Center) > 600)
                            t++;
                        GetDir_ByPlayer();
                        MoveTo(player.Center, 6, 20);
                    }
                }
                if (t > (npc.ai[2] == 1 ? 510 : 680))
                {
                    phamtomDis = 0;
                    npc.ai[2] = 0;
                    npc.ai[0]++;
                    t = 0;
                    if (phase2)
                        t += 80;
                }
            }//冲刺1
            if (npc.ai[0] == 2)
            {

                if (++t < 100)
                {
                    MoveTo(player.Center, 5, 40);
                    GetDir_ByPlayer();
                }
                if (t > 100 && t < 550)
                {
                    int tt = (t - 100) % 150;
                    if (tt < 20)
                        npc.velocity = Vector2.Lerp(npc.velocity, new Vector2(0, -8), 0.1f);
                    if (tt > 20 && tt < 120)
                    {
                        int Freq = 27;
                        if (Main.expertMode)
                            Freq = 20;
                        if (Main.masterMode)
                            Freq = 16;
                        GetDir_ByVel();
                        if (!phase2)
                            MoveTo(player.Center + new Vector2(0, -230), t > 400 ? 18 : 10, 30);
                        else
                            MoveTo(player.Center + new Vector2(0, -200), t > 400 ? 22 : 15, 30);

                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.MothBlue>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.MothBlue2>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));

                        if (t % Freq == 0 && Main.netMode != 1)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, 1), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                            if (Main.expertMode && !Main.masterMode)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                            }
                            if (Main.masterMode)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                            }
                        }
                    }
                    if (tt > 120 && tt < 150)
                        MoveTo(player.Center + npc.DirectionFrom(player.Center) * 150, 10, 20);
                    if (t == 220 && phase2&&Main.netMode!=1)
                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 v = new Vector2(0.1f + (i % 4) / 16f, 0).RotatedBy(i * MathHelper.TwoPi / 20);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<Projectiles.BlackCorruptRain3>(), NPC.damage / 5, 0f, Main.myPlayer, 0);
                        }
                }
                if (t > 550)
                {
                    t = 0;
                    if (npc.life < npc.lifeMax * 0.9f)
                        npc.ai[0]++;
                    else
                        npc.ai[0] = 1;
                }
            }//光球
            if (npc.ai[0] == 3)
            {
                int endTime = 220;
                int counts = 2;
                if (phase2)
                {
                    counts = 3;
                    endTime = 150;
                }
                if (++t < 30)
                {
                    npc.alpha += (int)(255 / 30f);
                    phamtomDis += 5;
                    MoveTo(player.Center, 5, 40);
                    npc.dontTakeDamage = true;
                }
                if (t == 40)
                {

                    npc.Center = player.Center + Main.rand.NextVector2Unit() * new Vector2(1.4f, 1f) * 300;
                    phamtomDis = 0;
                    npc.alpha = 255;
                    if (phase2)
                        t += 20;
                }
                if (t > 60 && t < 90)
                {
                    if (t == 70)
                        npc.dontTakeDamage = false;
                    npc.alpha -= (int)(255 / 30f);
                    phamtomDis += (75 - t);
                    npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionFrom(player.Center) * 10, 0.08f);
                }
                if (t == 90)
                    lightVisual = 2;
                if (t > 90 && t < 130)
                {
                    GreyVFx();
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                    if (t % 8 == 0 && npc.ai[2] == 2 && Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 6, 0f, Main.myPlayer);

                    if (Vector2.Distance(npc.Center, player.Center) > 100)
                        npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * 20, 0.1f);
                }
                if (t > 130 && t < endTime)
                {
                    MoveTo(player.Center, 5, 40);
                    GetDir_ByPlayer();
                }
                if (t > endTime)
                {
                    t = 0;
                    if (++npc.ai[2] >= counts)
                    {
                        npc.ai[0]++;
                        npc.ai[2] = 0;
                    }
                }
            }//瞬移冲刺
            if (npc.ai[0] == 4)
            {
                if (phamtomDis > 0)
                    phamtomDis -= 1;

                if (++t < 60)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 10, 20);
                    GetDir_ByPlayer();
                }
                else if (t == 60)
                {
                    npc.ai[2] = phase2 ? 1 : 0;
                }
                if (t >= 60)
                {   //90,240,390
                    //140,240,340
                    if ((t + 60) % (npc.ai[2] == 1 ? 100 : 150) == 0&&Main.netMode!=1)//发射弹幕
                    {
                        //npc.Center += npc.DirectionTo(player.Center)*Main.rand.Next(100,150);
                        lightVisual = 1.5f;
                        phamtomDis = 80;
                        for (int y = 0; y < 60; y++)
                        {
                            int num90 = Dust.NewDust(npc.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                            Main.dust[num90].noGravity = true;
                            Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
                        }
                        int style = Main.rand.Next(3);
                        float r = Main.rand.NextFloat() * 10;
                        if (style == 0)
                        {
                            int c = phase2 ? 6 : 5;
                            for (int i = 0; i < c; i++)
                            {
                                for (int j = -3; j <= 3; j++)
                                {
                                    Vector2 v = new Vector2(0.1f + j * 0.11f, 0).RotatedBy(j * 0.15f + i * MathHelper.TwoPi / c + r);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<Projectiles.BlackCorruptRain3>(), NPC.damage / 5, 0f, Main.myPlayer, 0);
                                }
                            }
                        }
                        if (style == 1)
                        {
                            for (int i = 0; i < 40; i++)
                            {
                                Vector2 v = new Vector2(0.1f + (i % 5) / 16f, 0).RotatedBy(i * MathHelper.TwoPi / 40 + r);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<Projectiles.BlackCorruptRain3>(), NPC.damage / 5, 0f, Main.myPlayer, 0);

                            }
                        }
                        if (style == 2)
                        {
                            int c = phase2 ? 60 : 50;
                            for (int i = 0; i < c; i++)
                            {
                                Vector2 v = new Vector2(0.18f + (float)Math.Sin(i * MathHelper.TwoPi / 10) * 0.17f, 0).RotatedBy(i * MathHelper.TwoPi / c + r);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<Projectiles.BlackCorruptRain3>(), NPC.damage / 5, 0f, Main.myPlayer, 0);

                            }
                        }
                    }
                    MoveTo(player.Center + new Vector2(0, -300), 8, 40);
                    GetDir_ByPlayer();

                }
                if (t >= 400)
                {
                    phamtomDis = 0;
                    npc.ai[2] = 0;
                    t = 0;
                    if (phase2)
                        npc.ai[0]++;
                    else
                        npc.ai[0] = 0;
                }

            }//弹幕
            if (npc.ai[0] == 5 || npc.ai[0] == 7)
            {
                if (++t < 50)
                {
                    MoveTo(player.Center, 8, 20);
                    GetDir_ByPlayer();
                    if (t > 20)
                        phamtomDis = MathHelper.Lerp(phamtomDis, 120, 0.1f);
                }

                if (t > 50 && t < 80)
                {
                    StraightMoveTo(player.Center + npc.DirectionFrom(player.Center) * 200, 0.1f);
                    GetDir_ByPlayer();
                }
                if (t > 70 && t < 90)
                    phamtomDis = MathHelper.Lerp(phamtomDis, 0, 0.1f);
                if (t > 80 && t < 90)
                {
                    npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center).RotatedBy(npc.spriteDirection * 1.57f) * 20f, 0.1f);
                }
                if (t > 90 && t < 130)
                {
                    lightVisual = 1;
                    SpinAI(npc, player.Center, npc.spriteDirection * MathHelper.TwoPi / 30, true);
                    int num90 = Dust.NewDust(npc.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                    Main.dust[num90].noGravity = true;
                    Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
                    if (t % 2 == 0 && Main.netMode != 1)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.05f + new Vector2(0, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                    }
                    //GetDir_ByVel();
                }
                if (t > 130)
                {
                    npc.velocity *= 0.5f;
                    npc.ai[0]++;

                    t = 0;
                }
            }//绕玩家转圈发弹幕
            if (npc.ai[0] == 6)
            {
                if (++t < 60)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 20, 20);
                    GetDir_ByPlayer();
                }
                if (t == 60)
                {
                    lightVisual += 1;
                    Create4DCube();
                }
                if (t >= 60 && t <= 210 && (t - 10) % 50 == 0)
                {
                    int counts = t / 10;
                    for (int y = 0; y < counts; y++)
                    {
                        int num90 = Dust.NewDust(npc.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
                    }
                }
                if (t == 60 && Main.netMode != 1)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 vel = (i * MathHelper.TwoPi / 30).ToRotationVector2();
                        Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, vel, ModContent.ProjectileType<ButterflyDream>(), 1, 0, Main.myPlayer, npc.whoAmI, 1).timeLeft = 800;
                    }
                }
                if (t > 60 && t < 260)
                {
                    phamtomDis = MathHelper.Lerp(phamtomDis, 120, 0.02f);
                    npc.velocity *= 0.95f;
                    GetDir_ByPlayer();
                }
                if (t > 260 && t < 960)
                {
                    phamtomDis = MathHelper.Lerp(phamtomDis, 0, 0.02f);
                    MoveTo(player.Center, 3, 20);
                    GetDir_ByPlayer();
                }
                if (t > 960)
                {
                    t = 0;
                    if (npc.life < npc.lifeMax * 0.4f)
                        npc.ai[0]++;
                    else
                    {
                        npc.ai[0] = 1;
                        t += 150;
                    }
                }
            }//超立方体
            if (npc.ai[0] == 8)
            {
                if (++t < 60)
                {
                    MoveTo(player.Center - new Vector2(0, 500), 10, 20);

                }
                if (t > 60 && t < 120)
                {
                    GetDir_ByPlayer();
                    MoveTo(player.Center - new Vector2(+player.velocity.X * 10, 500), 40, 20);
                }
                if (t == 120)
                {
                    npc.velocity = new Vector2(0, 25);
                    npc.ai[2] = 6;//ai2->发射数量
                }
                if (t >= 120 && t <= 160)
                {
                    if (t % 8 == 0 && Main.netMode != 1)
                    {
                        if (t < 140)
                            npc.ai[2] += 2;
                        else
                            npc.ai[2] -= 2;
                        for (int i = 0; i < npc.ai[2]; i++)
                        {
                            Vector2 vel = (i * MathHelper.TwoPi / npc.ai[2]).ToRotationVector2();
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel * npc.ai[2] / 2, ModContent.ProjectileType<ButterflyDream>(), 1, 0, Main.myPlayer, -vel.Y);
                        }
                    }
                }
                if (t > 160 && t < 240)
                {
                    GetDir_ByPlayer();
                    MoveTo(player.Center, 5, 20);
                }
                if (t >= 240)
                {
                    npc.ai[0]++;
                    t = 0;
                }
            }//下冲+蝶弹
            if (npc.ai[0] == 9)
            {
                if (++t < 60)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 10, 20);
                    GetDir_ByPlayer();
                }
                if (t == 60&&Main.netMode!=1)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MothBall>(), npc.damage / 6, 0, Main.myPlayer);
                }
                if (t > 60 && t < 700)
                {
                    MoveTo(player.Center, 6, 20);
                    GetDir_ByPlayer();
                }
                if (t > 700)
                {
                    npc.ai[0]++;
                    t = 0;
                }
            }//飞蛾球
            if (npc.ai[0] == 10)
            {
                if (t == 0)
                {
                    Stack<NPC> butterfies = new();
                    foreach (NPC npc in Main.npc)//记录所有蝴蝶
                        if (npc.active && npc.type == ModContent.NPCType<Butterfly>())
                            butterfies.Push(npc);

                    int scale = 15;
                    float rot = 3.14f;//把贴图旋转为向右边
                    
                    Texture2D tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BBow");
                    Color[] colors = new Color[tex.Width * tex.Height];
                    tex.GetData(colors);
                    for (int y = 0; y < tex.Height; y++)
                    {
                        for (int x = 0; x < tex.Width; x++)
                        {
                            int i = y * tex.Height + x;
                            if (colors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 1;//切换为弓AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = npc.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).TargetPos = new Vector2((x - tex.Width / 2f) * scale, (y - tex.Height / 2f) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BArrow");
                    colors = new Color[tex.Width * tex.Height];
                    tex.GetData(colors);
                    for (int y = 0; y < tex.Height; y++)
                    {
                        for (int x = 0; x < tex.Width; x++)
                        {
                            int i = y * tex.Height + x;
                            if (colors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 2;//切换为箭AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = npc.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).TargetPos = new Vector2((x - tex.Width / 2f) * scale, (y - tex.Height / 2f) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
                    {
                        butterfly.ai[0] = -1;//切换为游荡AI
                        butterfly.ai[1] = 0;//清空计时器
                        butterfly.ai[3] = npc.whoAmI;
                    }
                }
                t++;
                if (t < 120)
                {
                    MoveTo(player.Center + new Vector2(0, -300), 8, 20);
                    GetDir_ByPlayer();
                }
                if (t > 120)
                {
                    MoveTo(player.Center + new Vector2(0, -300), 5, 20);
                    GetDir_ByPlayer();
                }
                if (t > 120 && t < 180)
                {
                    if (t < 150)
                        phamtomDis += 4;
                    else
                        phamtomDis -= 4;
                }
                if (t > 340 && t < 400)
                {
                    if (t < 370)
                        phamtomDis += 4;
                    else
                        phamtomDis -= 4;
                }
                if (t == 400)//第二次射箭
                {

                }
                if (t > 480)
                {
                    npc.ai[0]++;
                    t = 0;
                }
            }//弓
            if (npc.ai[0] == 11)
            {
                if (t < 180)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 8, 20);
                    if (t > 140 && t < 160)
                        phamtomDis += 5;
                    else
                        phamtomDis -= 5;
                }
                if (t == 180)//开始挥剑
                {
                    lightVisual += 2;
                    npc.velocity = npc.DirectionTo(player.Center) * 20;
                }
                if (t > 220)
                {
                    MoveTo(player.Center, 5, 20);
                }
                GetDir_ByPlayer();
                if (t == 0)
                {
                    Stack<NPC> butterfies = new();
                    foreach (NPC npc in Main.npc)//记录所有蝴蝶
                        if (npc.active && npc.type == ModContent.NPCType<Butterfly>())
                            butterfies.Push(npc);
                    float rot = 0.785f;
                    int scale = 15;
                    Texture2D tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BSword");
                    Color[] colors = new Color[tex.Width * tex.Height];
                    tex.GetData(colors);
                    for (int y = 0; y < tex.Height; y++)
                    {
                        for (int x = 0; x < tex.Width; x++)
                        {
                            int i = y * tex.Height + x;
                            if (colors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 3;//切换为剑AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = npc.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).TargetPos = new Vector2(x * scale, (y - tex.Height) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
                    {
                        butterfly.ai[0] = -1;//切换为游荡AI
                        butterfly.ai[1] = 0;//清空计时器
                        butterfly.ai[3] = npc.whoAmI;
                    }
                }
                t++;
                if (t > 240)
                {
                    t = 0;
                    npc.ai[0]++;
                }
            }//剑
            if (npc.ai[0] == 12)
            {
                if (t > 120 && t < 160)
                {
                    MoveTo(player.Center + npc.DirectionFrom(player.Center) * 600, 8, 20);
                    if (t < 140)
                        phamtomDis += 5;
                    else
                        phamtomDis -= 5;
                }
                else if (t < 300)
                    MoveTo(player.Center + new Vector2(0, -200), 8, 20);

                GetDir_ByPlayer();
                if (t == 0)
                {
                    Stack<NPC> butterfies = new();
                    foreach (NPC npc in Main.npc)//记录所有蝴蝶
                        if (npc.active && npc.type == ModContent.NPCType<Butterfly>())
                            butterfies.Push(npc);
                    float rot = 3.14f;
                    int scale = 10;
                    Texture2D tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BFist");
                    Color[] colors = new Color[tex.Width * tex.Height];
                    tex.GetData(colors);
                    for (int y = 0; y < tex.Height; y++)
                    {
                        for (int x = 0; x < tex.Width; x++)
                        {
                            int i = y * tex.Height + x;
                            if (colors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 4;//切换为拳AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = npc.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).TargetPos = new Vector2((x - tex.Height / 2) * scale, (y - tex.Height / 2) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
                    {
                        butterfly.ai[0] = -1;//切换为游荡AI
                        butterfly.ai[1] = 0;//清空计时器
                        butterfly.ai[3] = npc.whoAmI;
                    }
                }
                t++;
                if (t > 300)
                {
                    npc.ai[0]++;
                    t = 0;
                }
            }//拳
            if (npc.ai[0] == 13)//
            {
                int counts = 0;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.type == ModContent.NPCType<Butterfly>() && npc.active)
                    {
                        counts++;
                    }
                }
                if (t == 0)
                {
                    npc.dontTakeDamage = true;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.type == ModContent.NPCType<Butterfly>() && npc.active && npc.ai[0] == -1)
                        {
                            npc.ai[0]--;
                            npc.ai[2] = Main.rand.NextFloat() * 6.28f;
                        }
                    }
                }
                if (t > 300 && t % 60 == 0)
                {
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, 1), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                        if (Main.expertMode && !Main.masterMode)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                        }
                        if (Main.masterMode)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                        }
                    }
                }
                t++;
                phamtomDis = MathHelper.Lerp(phamtomDis, MathHelper.Clamp(counts, 0, 40) * 3, 0.1f);
                if (counts > 0)
                {
                    MoveTo(player.Center, 5, 20);
                    GetDir_ByPlayer();
                }
                else
                {
                    npc.ai[0] = 1;
                    t = 150;
                    npc.dontTakeDamage = false;
                }
            }
            for (int i = npc.oldPos.Length - 1; i > 0; i--)
                npc.oldPos[i] = npc.oldPos[i - 1];
            npc.oldPos[0] = npc.Center;
        }
        private NPC npc => NPC;
        private int t
        {
            set => npc.ai[1] = value;
            get => (int)npc.ai[1];
        }
        private Player player => Main.player[npc.target];
        private void MoveTo(Vector2 targetPos, float Speed, float n)
        {
            Vector2 targetVec = Utils.SafeNormalize(targetPos - npc.Center, Vector2.Zero) * Speed;
            npc.velocity = (npc.velocity * n + targetVec) / (n + 1);
        }
        private void StraightMoveTo(Vector2 targetPos, float n)
        {
            npc.Center = Vector2.Lerp(npc.Center, targetPos, n);
        }
        private void SpinAI(Entity entity, Vector2 center, float v, bool changeVelocity = true)
        {
            Vector2 oldPos = entity.Center;
            entity.Center = center + (oldPos - center).RotatedBy(v);
            if (changeVelocity)
            {
                entity.velocity = entity.Center - oldPos;
                entity.position -= entity.velocity;
            }
        }
        private void GreyVFx()
        {
            if (NPC.velocity.Length() > 10&&Main.netMode!=1)
            {
                if (t % 12 == 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, Main.rand.NextFloat(0, 50f)).RotatedByRandom(3.14), NPC.velocity, ModContent.ProjectileType<Projectiles.MothGrey>(), 0, 0f, Main.myPlayer, 1);
                }
                if (t % 12 == 6)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, Main.rand.NextFloat(0, 50f)).RotatedByRandom(3.14), NPC.velocity, ModContent.ProjectileType<Projectiles.MothGrey>(), 0, 0f, Main.myPlayer, -1);
                }
            }
        }
        private void GetDir_ByPlayer()
        {
            npc.direction = npc.spriteDirection = (npc.Center.X - player.Center.X) > 0 ? -1 : 1;
        }
        private void GetDir_ByVel()
        {
            npc.direction = npc.spriteDirection = npc.velocity.X > 0 ? 1 : -1;
        }
        //int locktime = 0;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (lightVisual < 1)
                lightVisual += 0.3f;
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            

            //npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
            /*
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>(), 8, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomM(), ModContent.ItemType<Items.BossDrop.MothRelic>(), 1, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomM(), ModContent.ItemType<Items.Bosses.CorruptMothTreasureBag>(), 1, 1, 1, 1));


            //npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 125, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>(), 25, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Bosses.CorruptMothTreasureBag>(), 1, 1, 1, 1));

            //npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 500, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>(), 100, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Accessories.MothEye>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.ShadowWingBow>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.ScaleWingBlade>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.PhosphorescenceGun>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.EvilChrysalis>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.DustOfCorrupt>(), 6, 1, 1, 1));
            */
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.2f;
            int num = (int)NPC.frameCounter % Main.npcFrameCount[NPC.type];
            NPC.frame.Y = num * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                effects = SpriteEffects.FlipHorizontally;
            
            Texture2D tx = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(tx.Width, tx.Height / 4) / 2;
            Color origColor = NPC.GetAlpha(drawColor);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Color color = origColor * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                spriteBatch.Draw(tx, NPC.oldPos[k] - Main.screenPosition, npc.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, npc.frame, origColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            origColor.A = 0;
            spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, npc.frame, origColor * 0.6f, NPC.rotation, origin, NPC.scale*1.05f, effects, 0f);

            Texture2D tg = Common.MythContent.QuickTexture("Bosses/CorruptMoth/NPCs/CorruptMothGlow");

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            float t = (float)Main.timeForVisualEffects * 0.2f;
            for (int i = 0; i < 6; i++)//周围的幻影
            {
                Color color = NPC.GetAlpha(Color.White) * (phamtomDis / 120f);
                spriteBatch.Draw(tx, npc.Center + (t * 0.1f + i * t / 6).ToRotationVector2() * phamtomDis - Main.screenPosition, npc.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            if (lightVisual > 0)//发光特效
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Color color = Color.White * ((1 + NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) * lightVisual;
                    spriteBatch.Draw(tx, NPC.oldPos[k] - Main.screenPosition, npc.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
                }
            }
            Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White) * 0.5f, NPC.rotation, origin, 1f, effects, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
