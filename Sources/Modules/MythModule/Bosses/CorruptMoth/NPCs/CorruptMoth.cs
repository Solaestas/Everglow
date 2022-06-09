using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Projectiles;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs
{
    [AutoloadBossHead]
    public class CorruptMoth : ModNPC
    {
        protected override bool CloneNewInstances => true;
        [CloneByReference]
        private Color[] BBowColors = new Color[BBowColorsWidth * BBowColorsHeight];
        private const int BBowColorsWidth = 60;
        private const int BBowColorsHeight = 60;

        [CloneByReference]
        private Color[] BArrowColors = new Color[BArrowColorsWidth * BArrowColorsHeight];
        private const int BArrowColorsWidth = 60;
        private const int BArrowColorsHeight = 60;

        [CloneByReference]
        private Color[] BSwordColors = new Color[BSwordColorsWidth * BSwordColorsHeight];
        private const int BSwordColorsWidth = 60;
        private const int BSwordColorsHeight = 60;

        [CloneByReference]
        private Color[] BFistColors = new Color[BFistColorsWidth * BFistColorsHeight];
        private const int BFistColorsWidth = 60;
        private const int BFistColorsHeight = 60;

        private bool canDespawn;
        public static int secondStageHeadSlot = -1;
        private bool Start = false;
        public static int StaTime = 0;
        private float PhamtomDis//特效幻影的距离和透明度
        {
            get => NPC.localAI[2];
            set => NPC.localAI[2] = value;
        }

        private float lightVisual = 0;
        [CloneByReference]
        private readonly Vector3[] cubeVec = new Vector3[]
        {
            new Vector3(1,1,1),
            new Vector3(1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(1,-1,1),
            new Vector3(-1,1,1),
            new Vector3(-1,1,-1),
            new Vector3(-1,-1,-1),
            new Vector3(-1,-1,1)
        };
        /// <summary>
        /// 发射弹幕，在调用时判断是否不为客户端
        /// </summary>
        private int Timer
        {
            set => NPC.ai[1] = value;
            get => (int)NPC.ai[1];
        }
        private Player Player => Main.player[NPC.target];
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
            //蠢死的写法，以后一定要写个负责在主线程调用的东西
            Texture2D tex = null;
            Everglow.HookSystem.AddMethod(() =>
            {
                if (tex is null)
                {
                    tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BBow");
                    tex.GetData(BBowColors);

                    tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BArrow");
                    tex.GetData(BArrowColors);

                    tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BSword");
                    tex.GetData(BSwordColors);

                    tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BFist");
                    tex.GetData(BFistColors);
                }
            }, Commons.Core.CallOpportunity.PostDrawEverything);

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
        public override void BossHeadSlot(ref int index)
        {

        }
        /// <summary>
        /// 发射弹幕，调用前检测服务端
        /// </summary>
        private void Create4DCube()
        {
            int scale = 300;
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
                            Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), NPC.damage / 5, 0, Main.myPlayer, NPC.whoAmI);
                            (proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(Vector3.Lerp(v1, v2, (float)i / (counts - 1)), w);
                            proj.netUpdate2 = true;
                        }
                    }
                    Vector3 v3 = cubeVec[ii] * scale;
                    Vector3 v4 = cubeVec[ii + 4] * scale;
                    for (int i = 1; i < counts - 1; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), NPC.damage / 5, 0, Main.myPlayer, NPC.whoAmI);
                        (proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(Vector3.Lerp(v3, v4, (float)i / (counts - 1)), w);
                        proj.netUpdate2 = true;
                    }
                }
            }

            for (int a = 0; a < cubeVec.Length; a++)
            {
                Vector3 v = cubeVec[a];
                for (int i = 1; i < counts - 1; i++)
                {
                    float c = (counts - 1) / 2;
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), NPC.damage / 5, 0, Main.myPlayer, NPC.whoAmI);
                    (proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(v * scale, (float)(i - c) * scale / c);
                    proj.netUpdate2 = true;
                }
            }
        }
        public override void AI()
        {
            NPC.netUpdate2 = true;
            bool phase2 = NPC.life < NPC.lifeMax * 0.6f;
            Lighting.AddLight(NPC.Center, 0f, 0f, 0.8f * (1 - NPC.alpha / 255f));
            NPC.friendly = NPC.dontTakeDamage;
            if (lightVisual > 0)//光效
            {
                lightVisual -= 0.04f;
            }
            else
            {
                lightVisual = 0;
            }

            //贴图旋转
            if (NPC.spriteDirection > 0)
            {
                NPC.rotation = NPC.velocity.Y / 15;
            }
            else
            {
                NPC.rotation = -NPC.velocity.Y / 15;
            }

            if (Math.Abs(NPC.rotation) > 1.2f)
            {
                NPC.rotation = Math.Sign(NPC.rotation) * 1.2f;
            }
            #region #前言

            if (!Start)
            {
                NPC.ai[0] = 0;
                //NPC.ai[0] = 114514;
                NPC.noTileCollide = true;
                Start = true;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Create4DCube();
                    for (int h = 0; h < 15; h++)
                    {
                        NPC.NewNPC(null, (int)NPC.Center.X + 25, (int)NPC.Center.Y + 150, ModContent.NPCType<MothSummonEffect>());
                    }
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

            if (NPC.ai[0] == 0)
            {
                NPC.dontTakeDamage = true;
                NPC.noTileCollide = false;
                NPC.noGravity = false;
                PhamtomDis = (150 - Timer) * 120f / 150;
                if (++Timer > 150)
                {
                    NPC.dontTakeDamage = false;
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    NPC.ai[0]++;
                    Timer = 0;
                }
            }//生成
            if (NPC.ai[0] == 1)
            {
                if (++Timer < 200)
                {
                    MoveTo(player.Center, 5, 40);
                    GetDir_ByPlayer();
                }
                if (Timer == 200)
                {
                    NPC.ai[2] = phase2 ? 1 : 0;
                }
                if (Timer > 200 && Timer < 650 && NPC.ai[2] == 0)//冲刺
                {
                    int tt = (Timer - 200) % 150;
                    Vector2 getVec = new Vector2(NPC.direction);
                    if (Timer > 500)
                    {
                        getVec = new Vector2(NPC.direction, 0);
                    }

                    if (tt < 50)
                    {
                        GetDir_ByPlayer();
                        MoveTo(player.Center - getVec * 300, 15, 15);
                    }
                    if (tt > 50 && tt < 70)//前摇
                    {
                        NPC.velocity = Vector2.Lerp(NPC.velocity, -getVec * 10, 0.1f);
                    }
                    if (tt > 30 && tt < 70)
                    {
                        PhamtomDis += (50 - tt) * 0.5f;
                    }

                    if (tt == 70)
                    {
                        lightVisual = 2;
                    }

                    if (tt > 70 && tt < 120)//冲刺
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            GreyVFx();
                            if (Timer > 500 && Timer % 10 == 0)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 6, 0f, Main.myPlayer);
                            }
                        }

                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                        if (Timer > 500)
                        {
                            NPC.velocity = Vector2.Lerp(NPC.velocity, getVec * 20, 0.15f);
                        }
                        else if (Vector2.Distance(NPC.Center, player.Center) > 100)
                        {
                            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 20, 0.1f);
                        }
                    }
                    if (tt > 120)
                    {
                        if (Vector2.Distance(NPC.Center, player.Center) > 600)
                        {
                            Timer++;
                        }

                        GetDir_ByPlayer();
                        MoveTo(player.Center, 6, 20);
                    }
                }
                if (Timer > 200 && Timer < 500 && NPC.ai[2] == 1)//二阶段冲刺
                {
                    int tt = Timer % 100;
                    Vector2 getVec = new Vector2(NPC.direction);
                    if (Timer > 400)
                    {
                        getVec = new Vector2(NPC.direction, 0);
                    }

                    if (tt < 20)
                    {
                        GetDir_ByPlayer();
                        MoveTo(player.Center + player.velocity * 10 - getVec * 300, 15, 15);
                    }
                    if (tt > 20 && tt < 40)//前摇
                    {
                        NPC.velocity = Vector2.Lerp(NPC.velocity, -getVec * 10, 0.1f);
                    }
                    if (tt > 0 && tt < 40)
                    {
                        PhamtomDis += (20 - tt) * 0.5f;
                    }

                    if (tt == 40)
                    {
                        lightVisual = 2;
                    }

                    if (tt > 40 && tt < 80)//冲刺
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            GreyVFx();
                            if (Timer % 6 == 0)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 6, 0f, Main.myPlayer);
                            }
                        }

                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                        if (Timer > 500)
                        {
                            NPC.velocity = Vector2.Lerp(NPC.velocity, getVec * 20, 0.15f);
                        }
                        else if (Vector2.Distance(NPC.Center, player.Center) > 100)
                        {
                            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 21, 0.1f);
                        }
                    }
                    if (tt > 80)
                    {
                        if (Vector2.Distance(NPC.Center, player.Center) > 600)
                        {
                            Timer++;
                        }

                        GetDir_ByPlayer();
                        MoveTo(player.Center, 6, 20);
                    }
                }
                if (Timer > (NPC.ai[2] == 1 ? 510 : 680))
                {
                    PhamtomDis = 0;
                    NPC.ai[2] = 0;
                    NPC.ai[0]++;
                    Timer = 0;
                    if (phase2)
                    {
                        Timer += 80;
                    }
                }
            }//冲刺1
            if (NPC.ai[0] == 2)
            {

                if (++Timer < 100)
                {
                    MoveTo(player.Center, 5, 40);
                    GetDir_ByPlayer();
                }
                if (Timer > 100 && Timer < 550)
                {
                    int tt = (Timer - 100) % 150;
                    if (tt < 20)
                    {
                        NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(0, -8), 0.1f);
                    }

                    if (tt > 20 && tt < 120)
                    {
                        int Freq = 27;
                        if (Main.expertMode)
                        {
                            Freq = 20;
                        }

                        if (Main.masterMode)
                        {
                            Freq = 16;
                        }

                        GetDir_ByVel();
                        if (!phase2)
                        {
                            MoveTo(player.Center + new Vector2(0, -230), Timer > 400 ? 18 : 10, 30);
                        }
                        else
                        {
                            MoveTo(player.Center + new Vector2(0, -200), Timer > 400 ? 22 : 15, 30);
                        }

                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.MothBlue>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.MothBlue2>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));

                        if (Timer % Freq == 0 && Main.netMode != NetmodeID.MultiplayerClient)
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
                    {
                        MoveTo(player.Center + NPC.DirectionFrom(player.Center) * 150, 10, 20);
                    }

                    if (Timer == 220 && phase2 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 v = new Vector2(0.1f + (i % 4) / 16f, 0).RotatedBy(i * MathHelper.TwoPi / 20);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<Projectiles.BlackCorruptRain3>(), NPC.damage / 5, 0f, Main.myPlayer, 0);
                        }
                    }
                }
                if (Timer > 550)
                {
                    Timer = 0;
                    if (NPC.life < NPC.lifeMax * 0.9f)
                    {
                        NPC.ai[0]++;
                    }
                    else
                    {
                        NPC.ai[0] = 1;
                    }
                }
            }//光球
            if (NPC.ai[0] == 3)
            {
                int endTime = 220;
                int counts = 2;
                if (phase2)
                {
                    counts = 3;
                    endTime = 150;
                }
                if (++Timer < 30)
                {
                    NPC.alpha += (int)(255 / 30f);
                    PhamtomDis += 5;
                    MoveTo(player.Center, 5, 40);
                    NPC.dontTakeDamage = true;
                }
                if (Timer == 40)
                {
                    NPC.Center = player.Center + Main.rand.NextVector2Unit() * new Vector2(1.4f, 1f) * 300;
                    PhamtomDis = 0;
                    NPC.alpha = 255;
                    NPC.netUpdate2 = true;
                    if (phase2)
                    {
                        Timer += 20;
                    }
                }
                if (Timer > 60 && Timer < 90)
                {
                    if (Timer == 70)
                    {
                        NPC.dontTakeDamage = false;
                    }

                    NPC.alpha -= (int)(255 / 30f);
                    PhamtomDis += (75 - Timer);
                    NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionFrom(player.Center) * 10, 0.08f);
                }
                if (Timer == 90)
                {
                    lightVisual = 2;
                }

                if (Timer > 90 && Timer < 130)
                {
                    GreyVFx();
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                    if (Timer % 8 == 0 && NPC.ai[2] == 2 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 6, 0f, Main.myPlayer);
                    }

                    if (Vector2.Distance(NPC.Center, player.Center) > 100)
                    {
                        NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 20, 0.1f);
                    }
                }
                if (Timer > 130 && Timer < endTime)
                {
                    MoveTo(player.Center, 5, 40);
                    GetDir_ByPlayer();
                }
                if (Timer > endTime)
                {
                    Timer = 0;
                    if (++NPC.ai[2] >= counts)
                    {
                        NPC.ai[0]++;
                        NPC.ai[2] = 0;
                    }
                }
            }//瞬移冲刺
            if (NPC.ai[0] == 4)
            {
                if (PhamtomDis > 0)
                {
                    PhamtomDis -= 1;
                }

                if (++Timer < 60)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 10, 20);
                    GetDir_ByPlayer();
                }
                else if (Timer == 60)
                {
                    NPC.ai[2] = phase2 ? 1 : 0;
                }
                if (Timer >= 60)
                {   //90,240,390
                    //140,240,340
                    if ((Timer + 60) % (NPC.ai[2] == 1 ? 100 : 150) == 0)
                    {
                        //NPC.Center += NPC.DirectionTo(player.Center)*Main.rand.Next(100,150);
                        for (int y = 0; y < 60; y++)
                        {
                            int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
                            Main.dust[index].noGravity = true;
                            Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)//发射弹幕
                        {
                            lightVisual = 1.5f;
                            PhamtomDis = 80;
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
                    }
                    MoveTo(player.Center + new Vector2(0, -300), 8, 40);
                    GetDir_ByPlayer();
                }
                if (Timer >= 400)
                {
                    PhamtomDis = 0;
                    NPC.ai[2] = 0;
                    Timer = 0;
                    if (phase2)
                    {
                        NPC.ai[0]++;
                    }
                    else
                    {
                        NPC.ai[0] = 0;
                    }
                }
            }//弹幕
            if (NPC.ai[0] == 5 || NPC.ai[0] == 7)
            {
                if (++Timer < 50)
                {
                    MoveTo(player.Center, 8, 20);
                    GetDir_ByPlayer();
                    if (Timer > 20)
                    {
                        PhamtomDis = MathHelper.Lerp(PhamtomDis, 120, 0.1f);
                    }
                }

                if (Timer > 50 && Timer < 80)
                {
                    StraightMoveTo(player.Center + NPC.DirectionFrom(player.Center) * 200, 0.1f);
                    GetDir_ByPlayer();
                }
                if (Timer > 70 && Timer < 90)
                {
                    PhamtomDis = MathHelper.Lerp(PhamtomDis, 0, 0.1f);
                }

                if (Timer > 80 && Timer < 90)
                {
                    NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center).RotatedBy(NPC.spriteDirection * 1.57f) * 20f, 0.1f);
                }
                if (Timer > 90 && Timer < 130)
                {
                    lightVisual = 1;
                    SpinAI(NPC, player.Center, NPC.spriteDirection * MathHelper.TwoPi / 30, true);
                    int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
                    if (Timer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.05f + new Vector2(0, -2), ModContent.ProjectileType<Projectiles.BlackCorruptRain>(), NPC.damage / 4, 0f, Main.myPlayer, 1);
                    }
                    //GetDir_ByVel();
                }
                if (Timer > 130)
                {
                    NPC.velocity *= 0.5f;
                    NPC.ai[0]++;

                    Timer = 0;
                }
            }//绕玩家转圈发弹幕
            if (NPC.ai[0] == 6)
            {
                if (++Timer < 60)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 20, 20);
                    GetDir_ByPlayer();
                }
                if (Timer == 60)
                {
                    lightVisual += 1;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Create4DCube();
                    }
                }
                if (Timer >= 60 && Timer <= 210 && (Timer - 10) % 50 == 0)
                {
                    int counts = Timer / 10;
                    for (int y = 0; y < counts; y++)
                    {
                        int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
                    }
                }
                if (Timer == 60 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 vel = (i * MathHelper.TwoPi / 30).ToRotationVector2();
                        var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<ButterflyDream>(), 1, 0, Main.myPlayer, NPC.whoAmI, 1);
                        proj.timeLeft = 800;
                        proj.netUpdate2 = true;
                    }
                }
                if (Timer > 60 && Timer < 260)
                {
                    PhamtomDis = MathHelper.Lerp(PhamtomDis, 120, 0.02f);
                    NPC.velocity *= 0.95f;
                    GetDir_ByPlayer();
                }
                if (Timer > 260 && Timer < 960)
                {
                    PhamtomDis = MathHelper.Lerp(PhamtomDis, 0, 0.02f);
                    MoveTo(player.Center, 3, 20);
                    GetDir_ByPlayer();
                }
                if (Timer > 960)
                {
                    Timer = 0;
                    if (NPC.life < NPC.lifeMax * 0.4f)
                    {
                        NPC.ai[0]++;
                    }
                    else
                    {
                        NPC.ai[0] = 1;
                        Timer += 150;
                    }
                }
            }//超立方体
            if (NPC.ai[0] == 8)
            {
                if (++Timer < 60)
                {
                    MoveTo(player.Center - new Vector2(0, 500), 10, 20);

                }
                if (Timer > 60 && Timer < 120)
                {
                    GetDir_ByPlayer();
                    MoveTo(player.Center - new Vector2(+player.velocity.X * 10, 500), 40, 20);
                }
                if (Timer == 120)
                {
                    NPC.velocity = new Vector2(0, 25);
                    NPC.ai[2] = 6;//ai2->发射数量
                }
                if (Timer >= 120 && Timer <= 160)
                {
                    if (Timer % 8 == 0)
                    {
                        if (Timer < 140)
                        {
                            NPC.ai[2] += 2;
                        }
                        else
                        {
                            NPC.ai[2] -= 2;
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < NPC.ai[2]; i++)
                            {
                                Vector2 vel = (i * MathHelper.TwoPi / NPC.ai[2]).ToRotationVector2();
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel * NPC.ai[2] / 2, ModContent.ProjectileType<ButterflyDream>(), 1, 0, Main.myPlayer, -vel.Y);
                            }
                        }
                    }
                }
                if (Timer > 160 && Timer < 240)
                {
                    GetDir_ByPlayer();
                    MoveTo(player.Center, 5, 20);
                }
                if (Timer >= 240)
                {
                    NPC.ai[0]++;
                    Timer = 0;
                }
            }//下冲+蝶弹
            if (NPC.ai[0] == 9)
            {
                if (++Timer < 60)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 10, 20);
                    GetDir_ByPlayer();
                }
                if (Timer == 60 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MothBall>(), NPC.damage / 6, 0, Main.myPlayer);
                }
                if (Timer > 60 && Timer < 700)
                {
                    MoveTo(player.Center, 6, 20);
                    GetDir_ByPlayer();
                }
                if (Timer > 700)
                {
                    NPC.ai[0]++;
                    Timer = 0;
                }
            }//飞蛾球
            if (NPC.ai[0] == 10)
            {
                if (Timer == 0)
                {
                    Stack<NPC> butterfies = new();
                    foreach (NPC NPC in Main.npc)//记录所有蝴蝶
                    {
                        if (NPC.active && NPC.type == ModContent.NPCType<Butterfly>())
                        {
                            butterfies.Push(NPC);
                        }
                    }

                    int scale = 15;
                    float rot = 3.14f;//把贴图旋转为向右边

                    //Texture2D tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BBow");
                    //Color[] colors = new Color[tex.Width * tex.Height];
                    //tex.GetData(colors);
                    for (int y = 0; y < BBowColorsHeight; y++)
                    {
                        for (int x = 0; x < BBowColorsWidth; x++)
                        {
                            int i = y * BBowColorsHeight + x;
                            if (BBowColors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 1;//切换为弓AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = NPC.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).targetPos = new Vector2((x - BBowColorsWidth / 2f) * scale, (y - BBowColorsWidth / 2f) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    //tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BArrow");
                    //colors = new Color[tex.Width * tex.Height];
                    //tex.GetData(colors);
                    for (int y = 0; y < BArrowColorsHeight; y++)
                    {
                        for (int x = 0; x < BArrowColorsWidth; x++)
                        {
                            int i = y * BArrowColorsHeight + x;
                            if (BArrowColors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 2;//切换为箭AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = NPC.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).targetPos = new Vector2((x - BArrowColorsWidth / 2f) * scale, (y - BArrowColorsHeight / 2f) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
                    {
                        butterfly.ai[0] = -1;//切换为游荡AI
                        butterfly.ai[1] = 0;//清空计时器
                        butterfly.ai[3] = NPC.whoAmI;
                    }
                }
                Timer++;
                if (Timer < 120)
                {
                    MoveTo(player.Center + new Vector2(0, -300), 8, 20);
                    GetDir_ByPlayer();
                }
                if (Timer > 120)
                {
                    MoveTo(player.Center + new Vector2(0, -300), 5, 20);
                    GetDir_ByPlayer();
                }
                if (Timer > 120 && Timer < 180)
                {
                    if (Timer < 150)
                    {
                        PhamtomDis += 4;
                    }
                    else
                    {
                        PhamtomDis -= 4;
                    }
                }
                if (Timer > 340 && Timer < 400)
                {
                    if (Timer < 370)
                    {
                        PhamtomDis += 4;
                    }
                    else
                    {
                        PhamtomDis -= 4;
                    }
                }
                if (Timer == 400)//第二次射箭
                {

                }
                if (Timer > 480)
                {
                    NPC.ai[0]++;
                    Timer = 0;
                }
            }//弓
            if (NPC.ai[0] == 11)
            {
                if (Timer < 180)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 8, 20);
                    if (Timer > 140 && Timer < 160)
                    {
                        PhamtomDis += 5;
                    }
                    else
                    {
                        PhamtomDis -= 5;
                    }
                }
                if (Timer == 180)//开始挥剑
                {
                    lightVisual += 2;
                    NPC.velocity = NPC.DirectionTo(player.Center) * 20;
                }
                if (Timer > 220)
                {
                    MoveTo(player.Center, 5, 20);
                }
                GetDir_ByPlayer();
                if (Timer == 0)
                {
                    Stack<NPC> butterfies = new();
                    foreach (NPC NPC in Main.npc)//记录所有蝴蝶
                    {
                        if (NPC.active && NPC.type == ModContent.NPCType<Butterfly>())
                        {
                            butterfies.Push(NPC);
                        }
                    }

                    float rot = 0.785f;
                    int scale = 15;
                    //Texture2D tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BSword");
                    //Color[] colors = new Color[tex.Width * tex.Height];
                    //tex.GetData(colors);
                    for (int y = 0; y < BSwordColorsHeight; y++)
                    {
                        for (int x = 0; x < BSwordColorsWidth; x++)
                        {
                            int i = y * BSwordColorsHeight + x;
                            if (BSwordColors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 3;//切换为剑AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = NPC.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).targetPos = new Vector2(x * scale, (y - BSwordColorsHeight) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
                    {
                        butterfly.ai[0] = -1;//切换为游荡AI
                        butterfly.ai[1] = 0;//清空计时器
                        butterfly.ai[3] = NPC.whoAmI;
                    }
                }
                Timer++;
                if (Timer > 240)
                {
                    Timer = 0;
                    NPC.ai[0]++;
                }
            }//剑
            if (NPC.ai[0] == 12)
            {
                if (Timer > 120 && Timer < 160)
                {
                    MoveTo(player.Center + NPC.DirectionFrom(player.Center) * 600, 8, 20);
                    if (Timer < 140)
                    {
                        PhamtomDis += 5;
                    }
                    else
                    {
                        PhamtomDis -= 5;
                    }
                }
                else if (Timer < 300)
                {
                    MoveTo(player.Center + new Vector2(0, -200), 8, 20);
                }

                GetDir_ByPlayer();
                if (Timer == 0)
                {
                    Stack<NPC> butterfies = new();
                    foreach (NPC NPC in Main.npc)//记录所有蝴蝶
                    {
                        if (NPC.active && NPC.type == ModContent.NPCType<Butterfly>())
                        {
                            butterfies.Push(NPC);
                        }
                    }

                    float rot = 3.14f;
                    int scale = 10;
                    //Texture2D tex = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/BFist");
                    //Color[] colors = new Color[tex.Width * tex.Height];
                    //tex.GetData(colors);
                    for (int y = 0; y < BFistColorsHeight; y++)
                    {
                        for (int x = 0; x < BFistColorsWidth; x++)
                        {
                            int i = y * BFistColorsHeight + x;
                            if (BFistColors[i] == new Color(58, 169, 255) && butterfies.Count > 0)
                            {
                                NPC butterfly = butterfies.Pop();
                                butterfly.ai[0] = 4;//切换为拳AI
                                butterfly.ai[1] = 0;//清空计时器
                                butterfly.ai[3] = NPC.whoAmI;
                                butterfly.velocity = Vector2.Zero;
                                (butterfly.ModNPC as Butterfly).targetPos = new Vector2((x - BFistColorsWidth / 2) * scale, (y - BFistColorsHeight / 2) * scale).RotatedBy(rot);//指定其目标
                            }
                        }
                    }
                    foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
                    {
                        butterfly.ai[0] = -1;//切换为游荡AI
                        butterfly.ai[1] = 0;//清空计时器
                        butterfly.ai[3] = NPC.whoAmI;
                    }
                }
                Timer++;
                if (Timer > 300)
                {
                    NPC.ai[0]++;
                    Timer = 0;
                }
            }//拳
            if (NPC.ai[0] == 13)//
            {
                int counts = 0;
                foreach (NPC NPC in Main.npc)
                {
                    if (NPC.type == ModContent.NPCType<Butterfly>() && NPC.active)
                    {
                        counts++;
                    }
                }
                if (Timer == 0)
                {
                    NPC.dontTakeDamage = true;
                    foreach (NPC NPC in Main.npc)
                    {
                        if (NPC.type == ModContent.NPCType<Butterfly>() && NPC.active && NPC.ai[0] == -1)
                        {
                            NPC.ai[0]--;
                            NPC.ai[2] = Main.rand.NextFloat() * 6.28f;
                        }
                    }
                }
                if (Timer > 300 && Timer % 60 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
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
                Timer++;
                PhamtomDis = MathHelper.Lerp(PhamtomDis, MathHelper.Clamp(counts, 0, 40) * 3, 0.1f);
                if (counts > 0)
                {
                    MoveTo(player.Center, 5, 20);
                    GetDir_ByPlayer();
                }
                else
                {
                    NPC.ai[0] = 1;
                    Timer = 150;
                    NPC.dontTakeDamage = false;
                }
            }
            for (int i = NPC.oldPos.Length - 1; i > 0; i--)
            {
                NPC.oldPos[i] = NPC.oldPos[i - 1];
            }

            NPC.oldPos[0] = NPC.Center;
        }
        private void MoveTo(Vector2 targetPos, float Speed, float n)
        {
            Vector2 targetVec = Utils.SafeNormalize(targetPos - NPC.Center, Vector2.Zero) * Speed;
            NPC.velocity = (NPC.velocity * n + targetVec) / (n + 1);
        }
        private void StraightMoveTo(Vector2 targetPos, float n)
        {
            NPC.Center = Vector2.Lerp(NPC.Center, targetPos, n);
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
        /// <summary>
        /// 发射弹幕，在调用时判断是否不为客户端
        /// </summary>
        private void GreyVFx()
        {
            if (NPC.velocity.Length() > 10 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Timer % 12 == 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, Main.rand.NextFloat(0, 50f)).RotatedByRandom(3.14), NPC.velocity, ModContent.ProjectileType<Projectiles.MothGrey>(), 0, 0f, Main.myPlayer, 1);
                }
                if (Timer % 12 == 6)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, Main.rand.NextFloat(0, 50f)).RotatedByRandom(3.14), NPC.velocity, ModContent.ProjectileType<Projectiles.MothGrey>(), 0, 0f, Main.myPlayer, -1);
                }
            }
        }
        private void GetDir_ByPlayer()
        {
            NPC.direction = NPC.spriteDirection = (NPC.Center.X - Player.Center.X) > 0 ? -1 : 1;
        }
        private void GetDir_ByVel()
        {
            NPC.direction = NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
        }
        //int locktime = 0;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (lightVisual < 1)
            {
                lightVisual += 0.3f;
            }
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
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Texture2D tx = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(tx.Width, tx.Height / 4) / 2;
            Color origColor = NPC.GetAlpha(drawColor);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Color color = origColor * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                spriteBatch.Draw(tx, NPC.oldPos[k] - Main.screenPosition, NPC.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, NPC.frame, origColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            origColor.A = 0;
            spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, NPC.frame, origColor * 0.6f, NPC.rotation, origin, NPC.scale * 1.05f, effects, 0f);

            Texture2D tg = Common.MythContent.QuickTexture("Bosses/CorruptMoth/NPCs/CorruptMothGlow");

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            float t = (float)Main.timeForVisualEffects * 0.2f;
            for (int i = 0; i < 6; i++)//周围的幻影
            {
                Color color = NPC.GetAlpha(Color.White) * (PhamtomDis / 120f);
                spriteBatch.Draw(tx, NPC.Center + (t * 0.1f + i * t / 6).ToRotationVector2() * PhamtomDis - Main.screenPosition, NPC.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            if (lightVisual > 0)//发光特效
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Color color = Color.White * ((1 + NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) * lightVisual;
                    spriteBatch.Draw(tx, NPC.oldPos[k] - Main.screenPosition, NPC.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
                }
            }
            Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White) * 0.5f, NPC.rotation, origin, 1f, effects, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
