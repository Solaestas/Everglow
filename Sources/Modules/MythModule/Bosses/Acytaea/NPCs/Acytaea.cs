using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles;

using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.NPCs;

[AutoloadHead]
//[AutoloadBossHead]
public class Acytaea : ModNPC
{
    private bool canDespawn = false;
    private int kill = -5;
    private int TranIndex = 0;
    public bool isFly = false;
    public bool isBattle = false;
    public bool canUseWing = false;
    private Vector2 aiMpos = new Vector2(200, 0);
    private int firstDir = -1;
    private int Dam = 140;
    public static int minorDir = -1;
    private bool checkSpwan = true;
    public static float ToPlayerRot = 0;
    public static readonly Vector2 RightWingPos = new Vector2(-18, 0);
    public static readonly Vector2 RightArmPos = new Vector2(-10, 0);
    public static readonly Vector2 LeftWingPos = new Vector2(-18, 0);
    public static float LeftArmRot = 0;
    public static float RightArmRot = 0;
    public static float BladePro = 0;
    public static float BladeGlowPro = 0;
    public static float BladeRot = 0;
    public static float OldBladeRot = 0;
    public static float BladeSquz = 1;
    public static float AimBladeSquz = 1;
    public static float BowRot = 0;
    private int wingFrame = 0;
    private int headFrame = 0;
    private bool HasBlade = false;
    private bool HasCricle = false;
    private float ACircleR = 130;
    private float CirR0 = 0;
    private float CirPro0 = 0;
    private bool HasBow = false;
    private bool HasBook = false;
    private float COmega = 0;
    private int DrawAI = 0;
    public static int NPCWHOAMI = -1;
    private float SwirlPro = 12;
    private int BookFrame = 0;
    private bool CloseBook = true;
    public static int BossIndex = 0;
    public static Vector2[] OldBladePos = new Vector2[70];
    private Vector2[] Skirt1 = new Vector2[10];
    private Vector2[] Skirt2 = new Vector2[11];
    private Vector2[] AIMSkirt2 = new Vector2[11];
    private Vector2[] vSkirt2 = new Vector2[11];
    public override string HeadTexture => NPC.boss ?
        "Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Acytaea_Head_Boss" :
        "Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Acytaea_Head";
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Acytaea");
        Main.npcFrameCount[NPC.type] = 25;
        NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
        NPCID.Sets.AttackFrameCount[NPC.type] = 4;
        NPCID.Sets.DangerDetectRange[NPC.type] = 400;
        NPCID.Sets.AttackType[NPC.type] = 2;
        NPCID.Sets.AttackTime[NPC.type] = 60;
        NPCID.Sets.AttackAverageChance[NPC.type] = 15;
        NPCID.Sets.ActsLikeTownNPC[Type] = true;
        DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雅斯塔亚");
    }
    public override void SetDefaults()
    {
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.width = 34;
        NPC.height = 48;
        NPC.aiStyle = 7;
        NPC.damage = 100;
        NPC.defense = 100;
        NPC.lifeMax = 250;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath6;
        NPC.knockBackResist = 0.5f;
        NPC.boss = false;
        NPC.friendly = true;
        AnimationType = 22;
        //NPC.aiStyle = -1;
        //NPC.lifeMax = 50000;
        //NPC.life = 50000;
        //NPC.localAI[0] = 0;
        NPCID.Sets.TrailingMode[NPC.type] = 0;
        NPCID.Sets.TrailCacheLength[NPC.type] = 8;
        NPC.knockBackResist = 0;
        Music = MusicLoader.GetMusicSlot(Mod, "Musics/Acytaea");
    }
    public override bool CheckActive()
    {
        return canDespawn;
    }
    public override void AI()
    {
        if (isBattle)
        {
            NPC.aiStyle = -1;
            if (checkSpwan)
            {
                checkSpwan = false;
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.active = false;
                NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, NPC.type);
                //TODO 改用HJson
                #region 对话
                //if (Language.ActiveCulture.Name == "zh-Hans")
                //{
                //    int h = Main.rand.Next(6);
                //    if (h == 0)
                //    {
                //        Main.NewText("唔,好像……玩过了点", new Color(193, 0, 29));
                //    }
                //    if (h == 1)
                //    {
                //        Main.NewText("下手好像,有点重了", new Color(193, 0, 29));
                //    }
                //    if (h == 2)
                //    {
                //        Main.NewText("抱歉……我看你好像比其他生物强了不少,没控制住力度", new Color(193, 0, 29));
                //    }
                //    if (h == 3)
                //    {
                //        Main.NewText("诶,你的头怎么掉了", new Color(193, 0, 29));
                //    }
                //    if (h == 4)
                //    {
                //        Main.NewText("下次我会放点水", new Color(193, 0, 29));
                //    }
                //    if (h == 5)
                //    {
                //        Main.NewText("怎么,这就死了?", new Color(193, 0, 29));
                //    }
                //}
                //else if (Language.ActiveCulture.Name == "en-US")
                //{
                //    int h = Main.rand.Next(6);
                //    if (h == 0)
                //    {
                //        Main.NewText("Hmm...seems like I've done it too far", new Color(193, 0, 29));
                //    }
                //    if (h == 1)
                //    {
                //        Main.NewText("It might be a little bit heavy, to you", new Color(193, 0, 29));
                //    }
                //    if (h == 2)
                //    {
                //        Main.NewText("Sorry, I didn't control it much cuz you seemed to be much stronger than other creatures", new Color(193, 0, 29));
                //    }
                //    if (h == 3)
                //    {
                //        Main.NewText("Hey, where's your head?", new Color(193, 0, 29));
                //    }
                //    if (h == 4)
                //    {
                //        Main.NewText("Next time I'll try to be more gentle", new Color(193, 0, 29));
                //    }
                //    if (h == 5)
                //    {
                //        Main.NewText("What, died already?", new Color(193, 0, 29));
                //    }
                //}
                //else if (Language.ActiveCulture.Name == "ru-RU")
                //{
                //    int h = Main.rand.Next(6);
                //    if (h == 0)
                //    {
                //        Main.NewText("Хмм... кажется я зашла слишком далеко", new Color(193, 0, 29));
                //    }
                //    if (h == 1)
                //    {
                //        Main.NewText("Это может быть немного тяжело, для тебя", new Color(193, 0, 29));
                //    }
                //    if (h == 2)
                //    {
                //        Main.NewText("Прости, я не контролировала это потому что ты казался намного сильнее других существ", new Color(193, 0, 29));
                //    }
                //    if (h == 3)
                //    {
                //        Main.NewText("Хей, где твоя голова?", new Color(193, 0, 29));
                //    }
                //    if (h == 4)
                //    {
                //        Main.NewText("В следующий раз я постараюсь быть более нежнее", new Color(193, 0, 29));
                //    }
                //    if (h == 5)
                //    {
                //        Main.NewText("Что, ты уже умер?", new Color(193, 0, 29));
                //    }
                //}
                //else
                //{
                //    int h = Main.rand.Next(6);
                //    if (h == 0)
                //    {
                //        Main.NewText("Hmm...seems like I've done it too far", new Color(193, 0, 29));
                //    }
                //    if (h == 1)
                //    {
                //        Main.NewText("It might be a little bit heavy, to you", new Color(193, 0, 29));
                //    }
                //    if (h == 2)
                //    {
                //        Main.NewText("Sorry, I didn't control it much cuz you seemed to be much stronger than other creatures", new Color(193, 0, 29));
                //    }
                //    if (h == 3)
                //    {
                //        Main.NewText("Hey, where's your head?", new Color(193, 0, 29));
                //    }
                //    if (h == 4)
                //    {
                //        Main.NewText("Next time I'll try to be more gentle", new Color(193, 0, 29));
                //    }
                //    if (h == 5)
                //    {
                //        Main.NewText("What, died already?", new Color(193, 0, 29));
                //    }
                //}
                #endregion
                if ((player.Center - NPC.Center).Length() > 6000)
                {
                    NPC.active = false;
                }
                canDespawn = true;
            }
            else
            {
                if ((player.Center - NPC.Center).Length() > 15000)
                {
                    canDespawn = true;
                    NPC.active = false;
                }
                canDespawn = false;
            }
            NPC.localAI[0] += 1;
            minorDir = NPC.spriteDirection * -1;
            if (NPC.localAI[0] == 1)
            {
                if (Main.rand.Next(100) > 50)
                {
                    firstDir = 1;
                }
                HasCricle = false;
                ACircleR = 0;
                COmega = 0;
                SwirlPro = 12;
                for (int y = 0; y < 200; y++)
                {
                    if (Main.npc[y].active)
                    {
                        if (Main.npc[y].type == ModContent.NPCType<AcytaeaShadow>() || Main.npc[y].type == ModContent.NPCType<AcytaeaShadow2>() || Main.npc[y].type == ModContent.NPCType<AcytaeaShadow3>())
                        {
                            Main.npc[y].active = false;
                        }
                    }
                }
                if (Main.expertMode)
                {
                    Dam = 180 / 2;
                }
                if (Main.masterMode)
                {
                    Dam = 240 / 4;
                }
                //NPC.localAI[0] = 8345;

            }
            NPC.damage = Dam;
            if (NPC.localAI[0] > 0 && NPC.localAI[0] <= 400)
            {
                if (BossIndex != 0)
                {
                    BossIndex = 0;
                }
                if (!Main.dedServ)
                {
                    Music = MusicLoader.GetMusicSlot(Mod, "Musics/Acytaea");
                }
                aiMpos = new Vector2(200 * firstDir, 0);
                if ((aiMpos + player.Center - NPC.Center).Length() < 240)
                {
                    if (NPC.Center.X > player.Center.X)
                    {
                        NPC.spriteDirection = -1;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                    NPC.rotation *= 0.99f;
                }
                else
                {
                    if (NPC.velocity.X > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        NPC.spriteDirection = -1;
                    }
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                }
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = Vector2.Normalize(v0 - NPC.Center - NPC.velocity);
                    NPC.noGravity = true;
                    //NPC.velocity.Y += 0.1f;
                    if (wingFrame == 2)
                    {
                        NPC.velocity.Y += 0.24f * v1.Y;
                    }
                    if (wingFrame == 3)
                    {
                        NPC.velocity.Y += 0.72f * v1.Y;
                    }
                    if (NPC.velocity.Length() > 10)
                    {
                        NPC.velocity *= 0.96f;
                    }
                    NPC.velocity.X += v1.X * 0.08f;
                    NPC.velocity.Y += 0.08f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//前往固定位置
            if (NPC.localAI[0] > 400 && NPC.localAI[0] <= 500)
            {
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.96f;
                canUseWing = false;
            }//刹车
            if (NPC.localAI[0] > 500 && NPC.localAI[0] <= 530)
            {
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.96f;
                canUseWing = false;
                RightArmRot += (float)(Math.PI * 1.1 / 30f) * minorDir;
                if (NPC.localAI[0] == 520)
                {
                    SoundEngine.PlaySound(new SoundStyle("MythMod/Sounds/AcytaeaPortalOpening"), NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(88 * minorDir - (minorDir - 1) * 8, -158), Vector2.Zero, ModContent.ProjectileType<AcytaeaEffect>(), 0, 1, Main.myPlayer, minorDir);
                }
            }//上特效
            if (NPC.localAI[0] > 530 && NPC.localAI[0] <= 650)
            {
                HasBlade = true;//拿刀
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.96f;
                float a0 = (NPC.localAI[0] - 530) / 120f;
                BladePro = a0 * a0;
                canUseWing = false;
            }//抬右手
            if (NPC.localAI[0] > 650 && NPC.localAI[0] <= 680)
            {
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 650) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                RightArmRot = CosA0 * (float)(Math.PI * 1.1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                canUseWing = false;//不准飞
            }//第一刀
            if (NPC.localAI[0] > 690 && NPC.localAI[0] <= 720)
            {
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 692)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 690) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                RightArmRot = (1 - CosA0) * (float)(Math.PI * 1.1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                canUseWing = false;//不准飞
            }//第二刀
            if (NPC.localAI[0] > 730 && NPC.localAI[0] <= 820)
            {
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 732)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.25f, 0.4f);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 730) / 90d * Math.PI) + 1) * 2f;//构造辅助函数
                RightArmRot = (CosA0 + 1) * (float)(Math.PI * 1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                canUseWing = false;//不准飞
            }//第三刀
            if (NPC.localAI[0] > 830 && NPC.localAI[0] <= 860)
            {
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 832)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.75f, 0.9f);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 830) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                RightArmRot = (0.7f - CosA0) * (float)(Math.PI * 1.6) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                canUseWing = false;//不准飞
            }//第四刀
            if (NPC.localAI[0] > 860 && NPC.localAI[0] <= 900)
            {
                HasBlade = true;//拿刀
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                canUseWing = false;//不准飞
            }//休息
            if (NPC.localAI[0] > 900 && NPC.localAI[0] <= 920)
            {
                Player Lplayer = Main.LocalPlayer;
                int f0 = (int)(NPC.localAI[0] - 900) * 10;
                /*for (int h = 0; h < 10; h++)
                {
                    Vector2 v0 = Lplayer.Center + new Vector2(0, (h + f0) * 5).RotatedBy((h + f0) / 4d);
                    Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 1f)).RotatedByRandom(6.28);
                    Dust.NewDustDirect(v0, 0, 0, ModContent.DustType<Dusts.CosmicFlame2>(), v1.X, v1.Y, 0, default, ((h + f0) + 10) / 18f);
                }*/
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaEffectUp>(), Dam, 3, player.whoAmI);
            }//过屏
            if (NPC.localAI[0] > 940 && NPC.localAI[0] <= 980)
            {
                if (!HasCricle)
                {
                    HasCricle = true;
                }
                float AR = (980 - NPC.localAI[0]) / 3f;
                if (HasCricle)
                {
                    ACircleR += AR;
                    if (ACircleR > 80)
                    {
                        ACircleR *= 0.98f;
                    }
                }
            }//神环
            if (NPC.localAI[0] > 900 && NPC.localAI[0] <= 960)
            {
                if (BossIndex == 0)
                {
                    BossIndex = 1;
                }

                HasBlade = true;//拿刀
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                BladeGlowPro = (float)(-Math.Cos((NPC.localAI[0] - 900) / 60d * Math.PI) + 1) / 2f;
                canUseWing = false;//不准飞
                RightArmRot = (float)(1.1 * Math.PI);//旋转角度撕裂感
                BladeRot = (float)(RightArmRot - Math.PI * 1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
            }//释放能量
            if (NPC.localAI[0] > 960 && NPC.localAI[0] <= 990)
            {
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 962)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 960) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                RightArmRot = CosA0 * (float)(Math.PI * 1.1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                BladeGlowPro = 1;
                canUseWing = false;//不准飞
                float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                if (NPC.localAI[0] == 975)
                {
                    SoundEngine.PlaySound(SoundID.Item71, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(NPC.spriteDirection * 60, 0), new Vector2(2 * NPC.spriteDirection, 0), ModContent.ProjectileType<AcytaeaLight>(), Dam, 3, player.whoAmI, ka);
                }
            }//第五刀
            if (NPC.localAI[0] > 1000 && NPC.localAI[0] <= 1030)
            {
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 1002)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 1000) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                RightArmRot = (1 - CosA0) * (float)(Math.PI * 1.1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                BladeGlowPro = 1;
                canUseWing = false;//不准飞
                float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                if (NPC.localAI[0] == 1015)
                {
                    SoundEngine.PlaySound(SoundID.Item71, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(NPC.spriteDirection * 60, 0), new Vector2(2 * NPC.spriteDirection, 0), ModContent.ProjectileType<AcytaeaLight>(), Dam, 3, player.whoAmI, ka);
                }
            }//第六刀
            if (NPC.localAI[0] > 1030 && NPC.localAI[0] <= 1200)//拉位置
            {
                HasBlade = true;//拿刀
                aiMpos = new Vector2(500 * firstDir, 0);
                if ((aiMpos + player.Center - NPC.Center).Length() < 240)
                {
                    if (NPC.Center.X > player.Center.X)
                    {
                        NPC.spriteDirection = -1;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                    NPC.rotation *= 0.99f;
                }
                else
                {
                    if (NPC.velocity.X > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        NPC.spriteDirection = -1;
                    }
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                }
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = Vector2.Normalize(v0 - NPC.Center - NPC.velocity);
                    NPC.noGravity = true;
                    //NPC.velocity.Y += 0.1f;
                    if (wingFrame == 2)
                    {
                        NPC.velocity.Y += 0.24f * v1.Y;
                    }
                    if (wingFrame == 3)
                    {
                        NPC.velocity.Y += 0.72f * v1.Y;
                    }
                    if (NPC.velocity.Length() > 10)
                    {
                        NPC.velocity *= 0.96f;
                    }
                    NPC.velocity.X += v1.X * 0.08f;
                    NPC.velocity.Y += 0.08f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;//可以飞了
                BladePro = 1;//进程打满
                BladeGlowPro = 1;
            }//换位
            if (NPC.localAI[0] > 1200 && NPC.localAI[0] <= 1500)//释放幻影
            {
                HasBlade = true;//拿刀
                if (NPC.localAI[0] > 1208 && NPC.localAI[0] < 1244)
                {
                    if (NPC.localAI[0] % 5 == 0)
                    {
                        int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow>(), 0, (float)(NPC.localAI[0] % 7));
                        Main.npc[g].velocity = new Vector2(0, 16).RotatedBy(NPC.localAI[0] % 7 * Math.PI / 3.5d);
                    }
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.96f;
                canUseWing = false;
            }//释放幻影
            if (NPC.localAI[0] > 1290 && NPC.localAI[0] <= 1920)
            {
                aiMpos = new Vector2(400, 0).RotatedBy(NPC.localAI[0] / 40d);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] % 70 > 0 && NPC.localAI[0] % 70 <= 30)
                {
                    HasBlade = true;//拿刀
                    if (NPC.localAI[0] % 70 < 2)
                    {
                        AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
                        Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center);
                        ToPlayerRot = 0;//记录玩家方向
                    }
                    float CosA0 = (float)(Math.Cos(NPC.localAI[0] % 70 / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                    RightArmRot = CosA0 * (float)(Math.PI * 1.1) * minorDir + ToPlayerRot;//旋转角度撕裂感
                    OldBladeRot = BladeRot;//保证旋转方向正确记录
                    BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                    BladePro = 1;//进程打满
                    BladeGlowPro = 1;
                    canUseWing = false;//不准飞
                    float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                    if (NPC.localAI[0] % 70 == 15)
                    {
                        for (int k = -3; k < 5; k += 2)
                        {
                            SoundEngine.PlaySound(SoundID.Item71, NPC.Center);
                            Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<BloodBlade>(), Dam, 3, player.whoAmI, ka);
                        }
                    }
                }//正刀
                if (NPC.localAI[0] % 40 > 0 && NPC.localAI[0] % 70 <= 70)
                {
                    HasBlade = true;//拿刀
                    if (NPC.localAI[0] % 70 < 42)
                    {
                        AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
                        Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center);
                        ToPlayerRot = 0;//记录玩家方向
                    }
                    float CosA0 = (float)(Math.Cos((NPC.localAI[0] % 70 - 40) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
                    RightArmRot = (1 - CosA0) * (float)(Math.PI * 1.1) * minorDir + ToPlayerRot;//旋转角度撕裂感
                    OldBladeRot = BladeRot;//保证旋转方向正确记录
                    BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                    BladePro = 1;//进程打满
                    BladeGlowPro = 1;
                    canUseWing = false;//不准飞
                    float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                    if (NPC.localAI[0] % 70 == 45)
                    {
                        for (int k = -3; k < 5; k += 2)
                        {
                            SoundEngine.PlaySound(SoundID.Item71, NPC.Center);
                            Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<BloodBlade>(), Dam, 3, player.whoAmI, ka);
                        }
                    }
                }//反刀
            }//环绕,砍击
            if (NPC.localAI[0] > 1920 && NPC.localAI[0] <= 1954)//打爆幻影
            {

                if (NPC.localAI[0] % 2 == 1)
                {
                    //TODO 震屏
                    //MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
                    //mplayer.ShakeStrength = 7;
                    //mplayer.Shake = 1;
                }

                HasBlade = true;//拿刀
                /*if(NPC.localAI[0] % 4 == 0)
                {
                    if (!Filters.Scene["AntiColor"].IsActive())
                    {
                        Filters.Scene.Activate("AntiColor");
                    }
                }
                if (NPC.localAI[0] % 4 == 2)
                {
                    if (Filters.Scene["AntiColor"].IsActive())
                    {
                        Filters.Scene.Deactivate("AntiColor");
                    }
                }*/
                if (NPC.localAI[0] % 4 == 0)
                {
                    bool Trans = true;
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].type == ModContent.NPCType<AcytaeaShadow>() && Main.npc[k].active)
                        {
                            if (Main.npc[k].ai[0] == NPC.localAI[0] % 7)
                            {
                                if (NPC.Center.X > Main.npc[k].Center.X)
                                {
                                    NPC.spriteDirection = -1;
                                }
                                else
                                {
                                    NPC.spriteDirection = 1;
                                }
                                Vector2 vb = Main.npc[k].Center - NPC.Center;
                                BladeRot = (float)(Math.Atan2(vb.Y, vb.X) - Math.PI * 1.1);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                                NPC.position = Main.npc[k].position;
                                Main.npc[k].active = false;
                                for (int j = 0; j < 20; j++)
                                {
                                    Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28) + new Vector2(12, 0).RotatedBy(Main.npc[k].ai[0] * Math.PI / 7d * 2);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v * 2f, v, ModContent.ProjectileType<BrokenAcytaea>(), 0, 1, Main.myPlayer);
                                }
                                Trans = false;
                                break;
                            }
                        }
                    }
                    if (Trans)
                    {
                        Vector2 va = player.position + new Vector2(450, 0);
                        Vector2 vb = va - NPC.Center;
                        BladeRot = (float)Math.Atan2(vb.Y, vb.X);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                        NPC.position = va;
                        for (int s = 0; s < 11; s++)
                        {
                            Skirt2[s] = NPC.Center + new Vector2((s - 4) * 4 * -NPC.spriteDirection, 20);
                        }
                    }
                }

                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.96f;
                canUseWing = false;
            }//打爆幻影
            if (NPC.localAI[0] > 1960 && NPC.localAI[0] <= 2080)//回收碎片
            {
                float s = (NPC.localAI[0] - 1960) / 120f;
                float s2 = s * s * 12;
                SwirlPro = 1 - s2;
                COmega += s / 400f;
                COmega *= 0.99f;
                if (Terraria.Graphics.Effects.Filters.Scene["AntiColor"].IsActive())
                {
                    Terraria.Graphics.Effects.Filters.Scene.Deactivate("AntiColor");
                }
                HasBlade = true;//拿刀
                                //SoundEngine.PlaySound(new SoundStyle("MythMod/Sounds/Item_71_Flurry"), NPC.Center); //TO DO: Make sound follow NPC/relative to NPC.
                for (int u = 0; u < 1000; u++)
                {
                    if (Main.projectile[u].type == ModContent.ProjectileType<BrokenAcytaea>())
                    {
                        Vector2 v0 = NPC.Center;
                        Vector2 v1 = Vector2.Normalize(v0 - Main.projectile[u].Center);
                        if ((v0 - Main.projectile[u].Center).Length() < 16)
                        {
                            Main.projectile[u].Kill();
                        }
                        v1 = (v0 - Main.projectile[u].Center + v1 * 60f) / 480f;
                        Main.projectile[u].velocity += v1;
                        Main.projectile[u].velocity *= 0.96f;

                    }
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.96f;
                canUseWing = false;
            }//回收碎片
            if (NPC.localAI[0] > 2080 && NPC.localAI[0] <= 2380)
            {
                if (NPC.localAI[0] > 2300)
                {
                    float s = (NPC.localAI[0] - 2300) / 80f;
                    float s2 = s * s * 12;
                    SwirlPro = s2;
                    COmega -= s / 400f;
                    COmega *= 0.99f;
                    if (COmega < 0.0005f)
                    {
                        COmega = 0;
                    }
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 2082)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.25f, 0.4f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaTornadoHit>(), Dam, 1, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaTornado>(), 0, 1, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaTornado2>(), 0, 1, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaTornado3>(), 0, 1, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaTornado4>(), 0, 1, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaTornado5>(), 0, 1, Main.myPlayer);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 2080) / 300d * Math.PI) + 1) * 16f;//构造辅助函数
                RightArmRot = (CosA0 + 1) * (float)(Math.PI * 1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                BladePro = 1;//进程打满
                BladeGlowPro = 1;
                float b0 = CosA0 / 40f;
                NPC.velocity.X -= b0 * b0;
                NPC.velocity *= 0.98f;
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                canUseWing = true;//不准飞
            }//龙卷风
            if (NPC.localAI[0] > 2400 && NPC.localAI[0] <= 2500)
            {
                COmega = 0;
                SwirlPro = 12;
                aiMpos = new Vector2(-600, -400);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                if (NPC.localAI[0] == 2498)
                {
                    for (int k = -3; k < 5; k += 2)
                    {
                        Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<BloodBlade>(), Dam, 3, player.whoAmI);
                    }
                    float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vf * 60, vf * 2, ModContent.ProjectileType<AcytaeaLight>(), Dam, 3, player.whoAmI, ka);
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//左上
            if (NPC.localAI[0] > 2500 && NPC.localAI[0] <= 2600)
            {
                aiMpos = new Vector2(600, -400);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                if (NPC.localAI[0] == 2598)
                {
                    for (int k = -3; k < 5; k += 2)
                    {
                        Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<BloodBlade>(), Dam, 3, player.whoAmI);
                    }
                    float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vf * 60, vf * 2, ModContent.ProjectileType<AcytaeaLight>(), Dam, 3, player.whoAmI, ka);
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//右上
            if (NPC.localAI[0] > 2600 && NPC.localAI[0] <= 2700)
            {
                aiMpos = new Vector2(-600, 400);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                if (NPC.localAI[0] == 2698)
                {
                    for (int k = -3; k < 5; k += 2)
                    {
                        Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<BloodBlade>(), Dam, 3, player.whoAmI);
                    }
                    float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vf * 60, vf * 2, ModContent.ProjectileType<AcytaeaLight>(), Dam, 3, player.whoAmI, ka);
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//左下
            if (NPC.localAI[0] > 2700 && NPC.localAI[0] <= 2800)
            {
                aiMpos = new Vector2(600, 400);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                if (NPC.localAI[0] == 2798)
                {
                    for (int k = -3; k < 5; k += 2)
                    {
                        Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<BloodBlade>(), Dam, 3, player.whoAmI);
                    }
                    float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vf * 60, vf * 2, ModContent.ProjectileType<AcytaeaLight>(), Dam, 3, player.whoAmI, ka);
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//右下
            if (NPC.localAI[0] > 2800 && NPC.localAI[0] <= 2900)
            {
                aiMpos = new Vector2(-500, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//归位
            if (NPC.localAI[0] > 2900 && NPC.localAI[0] <= 3060)
            {
                if (NPC.alpha < 255)
                {
                    NPC.alpha += 5;
                }
                else
                {
                    NPC.alpha = 255;
                    aiMpos = new Vector2(350, 0);
                    if (NPC.Center.X > player.Center.X)
                    {
                        NPC.spriteDirection = -1;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                    isFly = true;
                    if (isFly)
                    {
                        Vector2 v0 = aiMpos + player.Center;
                        Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                        v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                        NPC.noGravity = true;
                        NPC.velocity += v1;
                        NPC.velocity *= 0.96f;
                    }
                    canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                }

                if (NPC.localAI[0] == 2905)
                {
                    for (int f = 0; f < 4; f++)
                    {
                        int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -100, (f - 1.5f) * 200, 1);
                        Vector2 vc = (new Vector2(-100, (f - 1.5f) * 200) - aiMpos) / 30f;
                        Main.npc[g].velocity = vc;
                    }
                }
            }//隐没
            if (NPC.localAI[0] > 3060 && NPC.localAI[0] <= 3112)
            {
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 5;
                }
                else
                {
                    NPC.alpha = 0;
                }
                aiMpos = new Vector2(350, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
            }//显现
            if (NPC.localAI[0] > 3212 && NPC.localAI[0] <= 3250)
            {
                aiMpos = new Vector2(350, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                HasBlade = true;//拿刀
                if (NPC.localAI[0] < 3214)
                {
                    AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
                }
                float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 3212) / 38d * Math.PI) + 1) / 2f;//构造辅助函数
                RightArmRot = CosA0 * (float)(Math.PI * 1.1) * minorDir;//旋转角度撕裂感
                OldBladeRot = BladeRot;//保证旋转方向正确记录
                BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (minorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
                NPC.velocity *= 0.96f;//考虑阻力
                BladePro = 1;//进程打满
                BladeGlowPro = 1;
                canUseWing = false;//不准飞
                float ka = Main.rand.NextFloat(Main.rand.NextFloat(0.65f, 1f), 1f);
                if (NPC.localAI[0] == 3227)
                {
                    for (int f = 0; f < 1000; f++)
                    {
                        if (Main.projectile[f].ai[0] == 1)
                        {
                            Main.projectile[f].ai[0] = 0;
                            Main.projectile[f].velocity = Vector2.Normalize(player.Center - Main.projectile[f].Center) * 4f;
                        }
                    }
                }
            }//放刀
            if (NPC.localAI[0] > 3248 && NPC.localAI[0] <= 3270)
            {
                float AR = (NPC.localAI[0] - 3270) * 2f;
                if (HasCricle)
                {
                    if (ACircleR <= 4)
                    {
                        ACircleR = 0;
                    }
                    else
                    {
                        ACircleR += AR;
                    }
                }
            }//收环
            if (NPC.localAI[0] > 3270 && NPC.localAI[0] <= 3330)
            {
                if (!HasCricle)
                {
                    HasCricle = true;
                }
                float AR = (3330 - NPC.localAI[0]) / 6f;
                if (HasCricle)
                {
                    ACircleR += AR;
                    if (ACircleR > 80)
                    {
                        ACircleR *= 0.98f;
                    }
                }
            }//神环
            if (NPC.localAI[0] > 3250 && NPC.localAI[0] <= 3500)
            {
                aiMpos = new Vector2(350, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 3252)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = 0;
                }
                if (NPC.localAI[0] % 20 == 0 && NPC.localAI[0] > 3300)
                {
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    vf = new Vector2(-1, 0);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                }
            }//←
            if (NPC.localAI[0] > 3500 && NPC.localAI[0] <= 3750)
            {
                aiMpos = new Vector2(0, -350);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 3502)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1.5);
                }
                if (NPC.localAI[0] % 18 == 0 && NPC.localAI[0] > 3550)
                {
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    vf = new Vector2(0, 1);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                }
            }//↓
            if (NPC.localAI[0] > 3750 && NPC.localAI[0] <= 4000)
            {
                aiMpos = new Vector2(-350, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 3752)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1);
                }
                if (NPC.localAI[0] % 16 == 0 && NPC.localAI[0] > 3800)
                {
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    vf = new Vector2(1, 0);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                }
            }//→
            if (NPC.localAI[0] > 4000 && NPC.localAI[0] <= 4250)
            {
                aiMpos = new Vector2(0, 350);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 4002)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 0.5);
                }
                if (NPC.localAI[0] % 10 == 0 && NPC.localAI[0] > 4050)
                {
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    vf = new Vector2(0, -1);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                }
            }//↑
            if (NPC.localAI[0] > 4250 && NPC.localAI[0] <= 4800)
            {
                double Ro1 = (NPC.localAI[0] - 4250) / 100d;
                double Ro = Ro1 * Ro1 * Math.PI;
                aiMpos = new Vector2(0, 350).RotatedBy(Ro);
                if ((aiMpos + player.Center - NPC.Center).Length() < 240)
                {
                    if (NPC.Center.X > player.Center.X)
                    {
                        NPC.spriteDirection = -1;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                    NPC.rotation *= 0.99f;
                }
                else
                {
                    if (NPC.velocity.X > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        NPC.spriteDirection = -1;
                    }
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                }
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 48f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.8f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 4252)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                }
                Vector2 vk = Vector2.Normalize(player.Center - NPC.Center);
                BowRot = (float)Math.Atan2(vk.Y, vk.X) + (float)Math.PI;
                if (NPC.localAI[0] % 8 == 0 && NPC.localAI[0] > 4270)
                {
                    Vector2 va = player.Center - NPC.Center;
                    Vector2 vf = Vector2.Normalize(va);
                    for (int g = 0; g < 6; g++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf.RotatedBy(Math.Sqrt(Math.Abs(g - 2.5)) * Math.Sign(g - 2.5) * (400 - va.Length()) / 200f) * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    }
                    if (NPC.localAI[0] < 4799)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaBow>(), Dam, 3, player.whoAmI, (float)(Math.Atan2(va.Y, va.X) + Math.PI));
                    }
                }
                if (NPC.localAI[0] == 4799)
                {
                    for (int g = 0; g < Main.projectile.Length; g++)
                    {
                        if (Main.projectile[g].type == ModContent.ProjectileType<AcytaeaBow>())
                        {
                            Main.projectile[g].timeLeft = 30;
                        }
                    }
                }
            }//旋转
            if (NPC.localAI[0] > 4800 && NPC.localAI[0] <= 5400)
            {
                aiMpos = new Vector2(450, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 4802)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                }
                if (NPC.localAI[0] % 200 == 30 && NPC.localAI[0] > 4800)
                {
                    Vector2 vf = Vector2.Normalize(player.Center - NPC.Center);
                    vf = new Vector2(-1, 0);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow2>(), Dam, 3, player.whoAmI, 1);
                }
            }//分裂箭
            if (NPC.localAI[0] > 5400 && NPC.localAI[0] <= 5600)
            {
                aiMpos = new Vector2(-450, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 5402)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1);
                }
                if (NPC.localAI[0] % 16 == 0 && NPC.localAI[0] > 5440)
                {
                    Vector2 vf = new Vector2(1, 0);
                    Vector2 vg = vf.RotatedBy(Math.PI / 2d) * 16;
                    Vector2 vh = vf.RotatedBy(Math.PI / 2d) * -16;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vg, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vh, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                }
            }//→→→
            if (NPC.localAI[0] > 5600 && NPC.localAI[0] <= 5700)
            {
                aiMpos = new Vector2(0, -90);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 5602)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 0.5);
                }
                if (NPC.localAI[0] == 5685)
                {
                    for (int d = -9; d < 10; d++)
                    {
                        Vector2 vf = new Vector2(0, -1).RotatedBy(d / 13d);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 14, ModContent.ProjectileType<AcytaeaArrowGra>(), Dam, 3, player.whoAmI);
                    }
                    for (int d = -9; d < 9; d++)
                    {
                        Vector2 vf = new Vector2(0, -1).RotatedBy((d + 0.5) / 13d);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 12, ModContent.ProjectileType<AcytaeaArrowGra>(), Dam, 3, player.whoAmI);
                    }
                }
            }//↖↑↗
            if (NPC.localAI[0] > 5700 && NPC.localAI[0] <= 6000)
            {
                aiMpos = new Vector2(-120, -90);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 5702)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 0.5);
                }
                if (NPC.localAI[0] == 5865)
                {
                    Vector2 vf = new Vector2(0, -1);
                    int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Main.projectile[f].scale = 3;
                }
                if (NPC.localAI[0] >= 5905 && NPC.localAI[0] < 5950)
                {
                    float PoX = NPC.localAI[0] - 5905;
                    Vector2 vf = new Vector2(0, -1);
                    int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(PoX * 72, -300), Vector2.Zero, ModContent.ProjectileType<AcytaeaWarningArrow>(), Dam, 3, player.whoAmI);
                    int g = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-PoX * 72, -300), Vector2.Zero, ModContent.ProjectileType<AcytaeaWarningArrow>(), Dam, 3, player.whoAmI);
                }
            }//↓↓↓
            if (NPC.localAI[0] > 6000 && NPC.localAI[0] <= 6200)
            {
                aiMpos = new Vector2(-400, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                if (NPC.localAI[0] < 6100)
                {
                    isFly = true;
                    if (isFly)
                    {
                        Vector2 v0 = aiMpos + player.Center;
                        Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                        v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                        NPC.noGravity = true;
                        NPC.velocity += v1;
                        NPC.velocity *= 0.96f;
                    }
                    canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                }
                else
                {
                    NPC.velocity *= 0.9f;
                }

                if (NPC.localAI[0] < 6102)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1);
                }
                /*if(NPC.localAI[0]==6147)
                {
                    DSdx = (NPC.Center - player.Center).X;
                    DSdy = (NPC.Center - player.Center).Y;
                }*/
                if (NPC.localAI[0] > 6148 && NPC.localAI[0] < 6184)
                {
                    if (NPC.localAI[0] % 7 == 0)
                    {
                        float K = (NPC.localAI[0] - 6148) * 10;
                        NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow3>(), 0, K, K * 0.7f, 1, 1);
                        NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow3>(), 0, K, -K * 0.7f, -1, 1);
                    }
                }
            }//幻影箭阵
            if (NPC.localAI[0] > 6200 && NPC.localAI[0] <= 6450)
            {
                NPC.velocity *= 0.9f;
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                if (NPC.localAI[0] < 6202)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1);
                }
                /*if(NPC.localAI[0]==6147)
                {
                    DSdx = (NPC.Center - player.Center).X;
                    DSdy = (NPC.Center - player.Center).Y;
                }*/
                if (NPC.localAI[0] > 6208 && NPC.localAI[0] < 6424)
                {
                    if (NPC.localAI[0] % 20 == 0)
                    {
                        Vector2 vf = new Vector2(0.8f, 0.12f);
                        Vector2 vg = new Vector2(0.8f, -0.12f);
                        Vector2 vh = new Vector2(0.6f, 0.17f);
                        Vector2 vi = new Vector2(0.6f, -0.17f);
                        int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                        int g = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vg * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                        int h = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vh * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                        int i = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vi * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                        Main.projectile[f].timeLeft = 100;
                        Main.projectile[g].timeLeft = 100;
                        Main.projectile[h].timeLeft = 100;
                        Main.projectile[i].timeLeft = 100;
                        for (int z = 0; z < 200; z++)
                        {
                            if (Main.npc[z].active && Main.npc[z].type == ModContent.NPCType<AcytaeaShadow3>())
                            {
                                Vector2 vλ = new Vector2(0.5f, -0.5f * Main.npc[z].ai[2]);
                                int λ = Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.npc[z].Center, vλ * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                                Main.projectile[λ].timeLeft = 100;
                            }
                        }
                    }
                }
                if (NPC.localAI[0] == 6434)
                {
                    BowRot = (float)(Math.PI * 1.25);
                    Vector2 vf = new Vector2(1f, 0.7f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 25, ModContent.ProjectileType<AcytaeaArrow3>(), Dam, 3, player.whoAmI);
                }
                if (NPC.localAI[0] == 6449)
                {
                    BowRot = (float)(Math.PI * 0.75);
                    Vector2 vf = new Vector2(1f, -0.7f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 25, ModContent.ProjectileType<AcytaeaArrow3>(), Dam, 3, player.whoAmI);
                }
            }//放幻影箭
            if (NPC.localAI[0] > 6450 && NPC.localAI[0] <= 6550)
            {
                aiMpos = new Vector2(0, -150);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 6452)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 0.5);
                }
                if (NPC.localAI[0] == 6535)
                {
                    for (int d = -30; d < 31; d++)
                    {
                        if ((d + 30) % 24 < 12)
                        {
                            Vector2 vf = new Vector2(0, -1).RotatedBy(d / 13d);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 14, ModContent.ProjectileType<AcytaeaArrowGra>(), Dam, 3, player.whoAmI);
                        }
                    }
                }
            }//↖↖ ↑ ↗↗
            if (NPC.localAI[0] > 6550 && NPC.localAI[0] <= 6950)
            {
                aiMpos = new Vector2(900, -400);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 6552)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1.5);
                }
                if (NPC.Center.X - player.Center.X > 600)
                {
                    NPC.position = player.Center + new Vector2(-600, -400);
                }
                if (NPC.localAI[0] % 4 == 0 && NPC.localAI[0] > 6590)
                {
                    Vector2 vf = new Vector2(0, 1);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 2, ModContent.ProjectileType<AcytaeaArrowGra>(), Dam, 3, player.whoAmI);
                }
            }//↓↓↓
            if (NPC.localAI[0] > 6950 && NPC.localAI[0] <= 7150)
            {
                aiMpos = new Vector2(0, -450);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 6952)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1.5);
                }
                if (NPC.localAI[0] % 16 == 0 && NPC.localAI[0] > 6970)
                {
                    Vector2 vf = new Vector2(0, 1);
                    Vector2 vg = vf.RotatedBy(Math.PI / 2d) * 16;
                    Vector2 vh = vf.RotatedBy(Math.PI / 2d) * -16;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vg, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vh, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf.RotatedBy(0.6) * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf.RotatedBy(-0.6) * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                }
            }//↙↓↓↓↘
            if (NPC.localAI[0] > 7150 && NPC.localAI[0] <= 7500)
            {
                aiMpos = new Vector2(450, 0);
                NPC.spriteDirection = -1;
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 80f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    float k = 1 - (NPC.localAI[0] - 7150) / 50f;
                    if (k < 0)
                    {
                        k = 0;
                    }
                    float a = k;
                    NPC.velocity *= a;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 7152)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = 0;
                }
                if (NPC.localAI[0] == 7205)
                {
                    float Dx = NPC.Center.X - player.Center.X;
                    for (int f = 0; f < 7; f++)
                    {
                        NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow3>(), 0, 0, (float)Math.Sqrt(f + 1) * 72, 0, -1);
                        NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow3>(), 0, 0, (float)Math.Sqrt(f + 1) * -72, 0, -1);
                    }
                }
                if (NPC.localAI[0] > 7250 && NPC.localAI[0] < 7450)
                {
                    if (NPC.localAI[0] % 20 == 0)
                    {
                        Vector2 vf = new Vector2(-1f, 0);
                        int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                        Main.projectile[f].timeLeft = 100;
                        for (int z = 0; z < 200; z++)
                        {
                            if (Main.npc[z].active && Main.npc[z].type == ModContent.NPCType<AcytaeaShadow3>())
                            {
                                Vector2 vλ = new Vector2(-1f * (float)Math.Sqrt(Math.Abs(Main.npc[z].ai[1] / 300f) + 1), 0f);
                                int λ = Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.npc[z].Center, vλ * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                                Main.projectile[λ].timeLeft = 100;
                            }
                        }
                    }
                }
                if (NPC.localAI[0] == 7464)
                {
                    BowRot = (float)(Math.PI * 0.5);
                    Vector2 vf = new Vector2(0, -1f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 25, ModContent.ProjectileType<AcytaeaArrow3>(), Dam, 3, player.whoAmI);
                }
                if (NPC.localAI[0] == 7489)
                {
                    BowRot = (float)(-Math.PI * 0.5);
                    Vector2 vf = new Vector2(0, 1f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 25, ModContent.ProjectileType<AcytaeaArrow3>(), Dam, 3, player.whoAmI);
                }
            }//左侧幻影
            if (NPC.localAI[0] > 7500 && NPC.localAI[0] <= 7800)
            {
                aiMpos = new Vector2(-500, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 7502)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                    BowRot = (float)(Math.PI * 1);
                }
                if (NPC.localAI[0] % 3 == 0 && NPC.localAI[0] > 7540)
                {
                    Vector2 vf = new Vector2(1, 0).RotatedBy(Math.Sin(NPC.localAI[0] / 15d) * 0.75);
                    int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 20, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Main.projectile[f].timeLeft = 100;
                    vf = new Vector2(1, 0).RotatedBy(-Math.Sin(NPC.localAI[0] / 15d) * 0.75);
                    f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 20, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    Main.projectile[f].timeLeft = 100;
                }
            }//↗→↘
            if (NPC.localAI[0] > 7800 && NPC.localAI[0] <= 8400)
            {
                double Ro = (float)((NPC.localAI[0] - 7800) / 300d * Math.PI);
                aiMpos = new Vector2(0, 350).RotatedBy(Ro);
                if ((aiMpos + player.Center - NPC.Center).Length() < 240)
                {
                    if (NPC.Center.X > player.Center.X)
                    {
                        NPC.spriteDirection = -1;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                    NPC.rotation *= 0.99f;
                }
                else
                {
                    if (NPC.velocity.X > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        NPC.spriteDirection = -1;
                    }
                    NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                }
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 48f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.8f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 7802)
                {
                    NPC.alpha = 0;
                    HasBlade = false;
                    HasBow = true;
                    BladeRot = 0;
                    BladePro = 0;
                    BladeGlowPro = 0;
                    RightArmRot = 0;
                }
                Vector2 vk = Vector2.Normalize(player.Center - NPC.Center);
                BowRot = (float)Math.Atan2(vk.Y, vk.X) + (float)Math.PI;
                if (NPC.localAI[0] % 60 == 30 && NPC.localAI[0] > 7820)
                {
                    Vector2 va = player.Center - NPC.Center;
                    Vector2 vf = Vector2.Normalize(va);
                    for (int g = 0; g < 12; g++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf.RotatedBy((g - 5.5) / 6d * Math.PI) * 34, ModContent.ProjectileType<AcytaeaArrow>(), Dam, 3, player.whoAmI);
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaCenterBow>(), Dam, 0, player.whoAmI);
                }
            }//旋转
            if (NPC.localAI[0] > 8400 && NPC.localAI[0] <= 8440)
            {
                float AR = (NPC.localAI[0] - 8440) * 2f;
                if (HasCricle)
                {
                    if (ACircleR <= 4)
                    {
                        ACircleR = 0;
                    }
                    else
                    {
                        ACircleR += AR;
                    }
                }
            }//收环
            if (NPC.localAI[0] > 8500 && NPC.localAI[0] <= 8560)
            {
                if (!HasBook)
                {
                    HasBook = true;
                }
                if (HasBow)
                {
                    HasBow = false;
                }
                if (!HasCricle)
                {
                    HasCricle = true;
                }
                float AR = (8560 - NPC.localAI[0]) / 5f;
                if (HasCricle)
                {
                    ACircleR += AR;
                    if (ACircleR > 80)
                    {
                        ACircleR *= 0.98f;
                    }
                }
            }//神环
            if (NPC.localAI[0] > 8520 && NPC.localAI[0] <= 9000)
            {
                aiMpos = new Vector2(-500, 0);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.96f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 8522)
                {
                    NPC.alpha = 0;
                    RightArmRot = 0;
                }
                if (NPC.localAI[0] % 15 == 0 && NPC.localAI[0] > 8560)
                {
                    Vector2 vf = new Vector2(1, 0).RotatedBy(Math.Sin(NPC.localAI[0] / 15d) * 0.75);
                    int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 20, ModContent.ProjectileType<Metero>(), Dam, 3, player.whoAmI, -1);
                    vf = new Vector2(1, 0).RotatedBy(-Math.Sin(NPC.localAI[0] / 15d) * 0.75);
                    f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 20, ModContent.ProjectileType<Metero>(), Dam, 3, player.whoAmI, 1);
                }
            }//散漫法术
            if (NPC.localAI[0] > 9000 && NPC.localAI[0] <= 9600)
            {
                double k0 = (NPC.localAI[0] - 9000) / 80d;
                aiMpos = new Vector2(0, -445).RotatedBy(Math.Sin(k0 * k0) * 0.4f);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 80f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    NPC.velocity *= 0.92f;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 9002)
                {
                    NPC.alpha = 0;
                    RightArmRot = 0;
                }
                if (NPC.localAI[0] % 14 == 0 && NPC.localAI[0] > 9000)
                {
                    Vector2 v0 = Vector2.Normalize(player.Center - NPC.Center) * 0.3f;
                    Vector2 vf = new Vector2(0, -1) + v0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 20, ModContent.ProjectileType<Metero2>(), Dam, 3, player.whoAmI, 1);
                }
                if (NPC.localAI[0] % 14 == 7 && NPC.localAI[0] > 9000)
                {
                    Vector2 v0 = Vector2.Normalize(player.Center - NPC.Center) * 0.3f;
                    Vector2 vf = new Vector2(0, -1) + v0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vf * 20, ModContent.ProjectileType<Metero2>(), Dam, 3, player.whoAmI, -1);
                }
            }//弹珠
            if (NPC.localAI[0] > 9600 && NPC.localAI[0] <= 10600)
            {
                aiMpos = new Vector2(-300, -245);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    float k0 = (NPC.localAI[0] - 9600) / 100f;
                    if (k0 >= 1)
                    {
                        k0 = 1;
                    }
                    NPC.velocity *= 1 - k0;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 9602)
                {
                    NPC.alpha = 0;
                    RightArmRot = 0;
                }
                if (NPC.localAI[0] == 9820)
                {
                    for (int h = 0; h < 5; h++)
                    {
                        Vector2 v0 = Vector2.Normalize(player.Center - NPC.Center) * 100f;
                        Vector2 vf = v0.RotatedBy(h / 2.5 * Math.PI);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vf, Vector2.Zero, ModContent.ProjectileType<AcytaeaLaserBall>(), Dam, 3, player.whoAmI, -1);
                        for (int a = 0; a < 3; a++)
                        {
                            Vector2 va = Vector2.Normalize(player.Center - NPC.Center).RotatedByRandom(6.283) * Main.rand.NextFloat(0.4f, 6f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vf, va, ModContent.ProjectileType<AcytaeaLaserLight>(), Dam, 3, player.whoAmI, -1);
                        }
                    }
                }
                if (NPC.localAI[0] >= 9820 && NPC.localAI[0] % 15 == 0)
                {
                    for (int h = 0; h < 20; h++)
                    {
                        Vector2 v0 = Vector2.Normalize(player.Center - NPC.Center) * 1f;
                        Vector2 vf = v0.RotatedBy(h / 10d * Math.PI);
                        if (h % 4 >= 2)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vf, vf, ModContent.ProjectileType<Metero3>(), Dam, 3, player.whoAmI, -1);
                        }
                    }
                }
            }//激光法阵
            if (NPC.localAI[0] > 10600 && NPC.localAI[0] <= 11000)
            {
                aiMpos = new Vector2(0, -245);
                if (NPC.Center.X > player.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                isFly = true;
                if (isFly)
                {
                    Vector2 v0 = aiMpos + player.Center;
                    Vector2 v1 = Vector2.Normalize(v0 - NPC.Center);
                    v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
                    NPC.noGravity = true;
                    NPC.velocity += v1;
                    float k0 = (NPC.localAI[0] - 9600) / 100f;
                    if (k0 >= 1)
                    {
                        k0 = 1;
                    }
                    NPC.velocity *= 1 - k0;
                }
                canUseWing = (aiMpos + player.Center - NPC.Center).Length() > 1 && (aiMpos + player.Center - NPC.Center).Y < 0;
                if (NPC.localAI[0] < 10602)
                {
                    NPC.alpha = 0;
                    RightArmRot = 0;
                }
                if (NPC.localAI[0] >= 10620 && NPC.localAI[0] % 60 == 0)
                {
                    Vector2 v0 = Vector2.Normalize(player.Center - NPC.Center) * 12f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v0, ModContent.ProjectileType<Metero4>(), Dam, 3, player.whoAmI, -1);
                }
            }//激光法阵
            if (NPC.localAI[0] > 11000)
            {
                NPC.alpha = 0;
                HasBlade = false;
                HasBow = false;
                BladeRot = 0;
                BladePro = 0;
                BladeGlowPro = 0;
                RightArmRot = 0;
                NPC.localAI[0] = 960;
                BladeGlowPro = 0;
            }
            if (isFly)
            {
                if (DrawAI % 6 == 0)
                {
                    if (wingFrame > 0)
                    {
                        if (wingFrame < 3)
                        {
                            wingFrame += 1;
                        }
                        else
                        {
                            wingFrame = 0;
                        }
                    }
                    else
                    {
                        if (canUseWing)
                        {
                            if (wingFrame < 3)
                            {
                                wingFrame += 1;
                            }
                            else
                            {
                                wingFrame = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                wingFrame = 0;
            }
            if (NPC.collideX)
            {
                NPC.velocity.X *= (float)(2 - Math.Pow(1.01, NPC.velocity.Length()));//空气阻力于速度指数相关
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y *= (float)(2 - Math.Pow(1.01, NPC.velocity.Length()));//空气阻力于速度指数相关
            }
            NPCWHOAMI = NPC.whoAmI;
            if (kill > 0)
            {
                NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
                NPC.velocity *= 0.8f;
                kill--;
                if (kill >= 30)
                {
                    Player Lplayer = Main.LocalPlayer;
                    int f0 = (kill - 30) * 10;
                    for (int h = 0; h < 10; h++)
                    {
                        Vector2 v0 = Lplayer.Center + new Vector2(0, (h + f0) * 5).RotatedBy((h + f0) / 4d);
                        Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 1f)).RotatedByRandom(6.28);
                        Dust.NewDustDirect(v0, 0, 0, ModContent.DustType<Dusts.CosmicFlame2>(), v1.X, v1.Y, 0, default, (h + f0 + 10) / 18f);
                    }
                }
                if (kill < 70)
                {
                    if (BossIndex != 0)
                    {
                        BossIndex = 0;
                    }
                }
                if (kill < 3)
                {
                    NPC.active = false;
                    NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, NPC.type);
                }
            }
            if (!Collision.CanHit(NPC, player))
            {
                TranIndex++;
                if (TranIndex > 60)
                {
                    NPC.position = player.position + new Vector2(0, -Main.rand.NextFloat(200, 300)).RotatedBy(Main.rand.NextFloat(-0.7f, 0.7f));
                    TranIndex = 0;
                }
            }
            else
            {
                TranIndex = 0;
                NPC.aiStyle = 7;
            }
            if (!NPC.boss)
            {
                BossIndex = 0;
            }
            if (!NPC.boss)
            {
                NPCHappiness NH = NPC.Happiness;
                NH.SetBiomeAffection<ForestBiome>((AffectionLevel)50);
                NH.SetBiomeAffection<SnowBiome>((AffectionLevel)70);
                NH.SetBiomeAffection<CrimsonBiome>((AffectionLevel)90);
                NH.SetBiomeAffection<CorruptionBiome>((AffectionLevel)90);
                NH.SetBiomeAffection<UndergroundBiome>((AffectionLevel)(-20));
                NH.SetBiomeAffection<DesertBiome>((AffectionLevel)20);
                NH.SetBiomeAffection<DungeonBiome>((AffectionLevel)(-50));
                NH.SetBiomeAffection<OceanBiome>((AffectionLevel)50);
                NH.SetBiomeAffection<JungleBiome>((AffectionLevel)30);
            }
        }
    }
    public override bool CanChat()
    {
        if (NPC.boss)
        {
            return false;
        }
        return true;
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (NPC.AnyNPCs(NPC.type))
        {
            return 0f;
        }
        return 2f;
    }
    public override string GetChat()
    {
        IList<string> list = new List<string>();
        if (Language.ActiveCulture.Name == "zh-Hans")
        {
            if (Main.dayTime)
            {
                list.Add("你在干嘛啊");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("对了,要好好感谢向导");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("其实服装师以前成就厉害的,暗影魔法大师呢");
                }
                list.Add("没事不要烦我");
                list.Add("干嘛!");
                list.Add("额，好吧，我承认我的脾气有点暴躁");
                list.Add("我觉得你是不是喜欢我");
                list.Add("你烦不烦啊!");
                list.Add("不就是砍了你一刀么,没什么大不了");
                list.Add("嘻嘻嘻");
            }
            else
            {
                list.Add("晚上就应该出去浪");
                list.Add("你在干嘛啊");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("对了,要好好感谢向导");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("其实服装师以前成就厉害的,暗影魔法大师呢");
                }
                list.Add("没事不要烦我");
                list.Add("干嘛!");
                list.Add("额,好吧,我承认我的脾气有点暴躁");
                list.Add("我觉得你是不是喜欢我");
                list.Add("你烦不烦啊!");
                list.Add("不就是砍了你一刀么,没什么大不了");
                list.Add("嘻嘻嘻");
            }
        }
        else if (Language.ActiveCulture.Name == "en-US")
        {
            if (Main.dayTime)
            {
                list.Add("What are you doing?");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("Btw, you should be thankful to the Guide");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("The Clothier was actually a Senior Dark Mage");
                }
                list.Add("Don't bother me if you only tell nonsense");
                list.Add("What's up?");
                list.Add("Err, well, I am a little grumpy");
                list.Add("You love me, don't you?");
                list.Add("How boring you are!");
                list.Add("I just cut you open, not a big deal");
                list.Add("*Chunckle");
            }
            else
            {
                list.Add("YOLO, go on adventures at night");
                list.Add("What are you doing?");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("Btw, you should be thankful to the Guide");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("The Clothier was actually a Senior Dark Mage");
                }
                list.Add("Don't bother me if you only tell nonsense");
                list.Add("What's up?");
                list.Add("Err, well, I am a little grumpy");
                list.Add("♥ Wanna eat me? ♥");
                list.Add("How boring you are!");
                list.Add("I just cut you open, not a big deal");
                list.Add("*Chunckle");
            }
        }
        else if (Language.ActiveCulture.Name == "ru-RU")
        {
            if (Main.dayTime)
            {
                list.Add("Что ты делаешь?");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("Кстати, ты должен быть благодарен Гиду");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("Портной на самом деле был Старшим Тёмным Магом");
                }
                list.Add("Не беспокойся меня если ты говоришь только глупости");
                list.Add("Как дела?");
                list.Add("Эээ, да, я немного сварливая");
                list.Add("Ты любишь меня, не так ли?");
                list.Add("Какой ты скучный!");
                list.Add("Я просто разрезала тебя, ничего страшного");
                list.Add("*Смеётся");
            }
            else
            {
                list.Add("Ты живёшь один раз, иди путешествовать ночью");
                list.Add("Что ты делаешь?");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("Кстати, ты должен быть благодарен Гиду");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("Портной на самом деле был Старшим Тёмным Магом");
                }
                list.Add("Не беспокойся меня если ты говоришь только глупости");
                list.Add("Как дела?");
                list.Add("Эээ, да, я немного сварливая");
                list.Add("♥ Хочешь съесть меня? ♥");
                list.Add("Какой ты скучный!");
                list.Add("Я просто разрезала тебя, ничего страшного");
                list.Add("*Смеётся");
            }
        }
        else
        {
            if (Main.dayTime)
            {
                list.Add("What are you doing?");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("Btw, you should be thankful to the Guide");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("The Clothier was actually a Senior Dark Mage");
                }
                list.Add("Don't bother me if you only tell nonsense");
                list.Add("What's up?");
                list.Add("Err, well, I am a little grumpy");
                list.Add("You love me, don't you?");
                list.Add("How boring you are!");
                list.Add("I just cut you open, not a big deal");
                list.Add("*Chunckle");
            }
            else
            {
                list.Add("YOLO, go on adventures at night");
                list.Add("What are you doing?");
                if (NPC.CountNPCS(22) != 0)
                {
                    list.Add("Btw, you should be thankful to the Guide");
                }
                if (NPC.CountNPCS(54) != 0)
                {
                    list.Add("The Clothier was actually a Senior Dark Mage");
                }
                list.Add("Don't bother me if you only tell nonsense");
                list.Add("What's up?");
                list.Add("Err, well, I am a little grumpy");
                list.Add("♥ Wanna eat me? ♥");
                list.Add("How boring you are!");
                list.Add("I just cut you open, not a big deal");
                list.Add("*Chunckle");
            }
        }
        return list[Main.rand.Next(list.Count)];
    }
    public override void SetChatButtons(ref string button, ref string button2)
    {
        if (Language.ActiveCulture.Name == "zh-Hans")
        {
            button = Language.GetTextValue("挑战");
            button2 = Language.GetTextValue("帮助");
        }
        else
        {
            button = Language.GetTextValue("Challenge");
            button2 = Language.GetTextValue("Help");
        }
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        string tx1 = "我很喜欢这种鸟语花香的绿色树林";
        string tx2 = "去抓蝴蝶吗?";
        if (Language.ActiveCulture.Name == "en-US")
        {
            tx1 = "I love the forest with birds singing and flowers blooming";
            tx2 = "Catching butterflies?";
        }
        else if (Language.ActiveCulture.Name == "ru-RU")
        {
            tx1 = "Я люблю лес где птички поют и цветочки благоухают";
            tx2 = "Может половить бабочек?";
        }
        else
        {
            tx1 = "I love the forest with birds singing and flowers blooming";
            tx2 = "Catching butterflies?";
        }
        // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
            
				new FlavorTextBestiaryInfoElement(tx1),
            new FlavorTextBestiaryInfoElement(tx2),
            new FlavorTextBestiaryInfoElement("Mods.MythMod.Bestiary.Acytaea")
        });
    }
    public override void HitEffect(int hitDirection, double damage)
    {
        if (NPC.life <= 0)
        {
            NPC.life = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;
            kill = 60;
        }
    }
    public override void OnChatButtonClicked(bool firstButton, ref bool shop)
    {
        shop = false;
        /*Main.npcChatText = "";*/
        if (firstButton)
        {
            NPC.friendly = false;
            NPC.aiStyle = -1;
            NPC.lifeMax = 275000;
            NPC.life = 275000;
            NPC.boss = true;
            NPC.localAI[0] = 0;
            isBattle = true;
            NPC.width = 40;
            NPC.height = 56;
            NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<FakeAcytaea>());
        }
        else
        {
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                IList<string> list = new List<string>
                {
                    //list.Add("用诡药和大理石块可以合成封印碎片,把它放进大理石神庙碎片堆");
                    "向日葵也可以制作武器",
                    "游戏前期弹弓很实用",
                    //list.Add("想做手机却不想钓鱼?去打封魔石瓶,里面封印的力量和一次上古航海有关");
                    //list.Add("传说云间生长着一株光之花");
                    "棍棒可以解决绝大多数近身威胁",
                    "闪避伤害后,仍然会获得无敌时间",
                    "月总后霜月和南瓜月会加强,期间掉落的魂是个好东西",
                    "空中的飞龙有极低概率掉落雷之花"
                };
                //list.Add("激光剑是晶光剑的升级版,像我一样,又厉害又好看");

                if (WorldGen.crimson)
                {
                    list.Add("猩红之地上高耸起不寻常的活体组织。解开那里的谜题,我有点好奇那\"大牙齿\"的力量还剩多少");
                    list.Add("在世界右半边地下的中间有一块奇怪地形，一只以灵魂为食的虫子栖息在那儿。去把它的茧打破看看");
                }
                else
                {
                    list.Add("在腐化之地的下方有一块不会蔓延的奇怪地形,这是因为那里的灵魂几乎都被一只大肉虫吸收了。现在它应该在茧里呆着,去把茧打破看看");//fix:"在腐化之地的下方有一块不会蔓延的奇怪地形，这是因为那里的灵魂几乎都被一只大肉虫吸收了。现在它应该在茧里呆着，去把茧打破看看"
                    list.Add("地表某处高耸着一块由血肉构成的地形,我记得那是一颗活着的牙齿导致的。解开那里的谜题看看会发生什么");
                }
                list.Add("有时我也想让一些有趣的生物给我按摩,你打Boss的时候可以叫上我");

                list.Add("我用强大的翼肌来振翅,比你们那些用飞翔之魂驱动的冒牌货好多了");
                list.Add("头上的角?撞到会疼,烦死了");
                list.Add("问我多高吗,算上角刚好一米八");
                list.Add("头发?天生就是这种颜色的吖");
                list.Add("我从没有被任何力量打败过,被我斩去的神明不下十个,你就是个普通的人类罢了");
                list.Add("最疯狂的一件事?我大概...屠灭了一整个城市,只用了一分钟");
                list.Add("不准问我年龄!生日的话,嗯,可以,4月19日");
                list.Add("克苏鲁？它好丑,听说你们有人被它丑疯了？");
                list.Add("你应当感到荣幸，我可是你们口中的\"永恒\"");
                list.Add("我身上是有鳞片的,只不过我可以控制它们收起来");
                Main.npcChatText = list[Main.rand.Next(list.Count)];
            }
            else
            {
                IList<string> list = new List<string>
                {
                    "Sunflower can be used to craft weapons",
                    "Slingshots are pretty useful in the early gameplay",
                    "Clubs can deal with most melee threats",
                    "You'll still have immunity frames after dodging",
                    "Wyverns rarely drop Flower of Lightning"
                };

                if (WorldGen.crimson)
                {
                    list.Add("An unusual living tissue rised in The Crimson. Solve the puzzle there, I wonder how much power left for that \"Giant Tooth\"");
                    list.Add("There's a strange biome lies in right underground part of the world, a giant insect inhabits there. Break its cocoon and see what will happen");
                }
                else
                {
                    list.Add("There's a strange non-spreading biome under The Corruption, that's because a giant larva absorbed most soul there. Now it should be staying in its cocoon, go and break the cocoon");
                    list.Add("A biome made of flesh rised somewhere on the surface. I remember it's due to a living tooth. Solve the puzzle there and see what will happen");
                }
                list.Add("Sometimes I want interesting creatures to give me massages, you can take me when fighting the Bosses");

                list.Add("I strike my wings by powerful wing muscles, much better than your imitations driven by Soul of Flight");
                list.Add("The horns? It hurts when hit, so sick");
                list.Add("How tall I am? Exactly 1.8m including the horns");
                list.Add("My hair? The color is natural!");
                list.Add("I've never been defeated, more than ten gods I've slayed, you're nobody");
                list.Add("The most insane thing I've done? Well...destroying a city, in ten minutes");
                list.Add("Never ask about my age!Birthday? Can, April 19th");
                list.Add("Cthulu...so ugly, I heard that some of you had gone mad because of its face");
                list.Add("You should feel honored, I'm who you say \"eternal\"");
                list.Add("I do have scales, but I can hide them");
                Main.npcChatText = list[Main.rand.Next(list.Count)];
            }
        }
    }
    public override void SetupShop(Chest shop, ref int nextSlot)
    {
    }
    public override void TownNPCAttackStrength(ref int damage, ref float knockback)
    {
        damage = 30;
        knockback = 2f;
    }
    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
    {
        cooldown = 60;
        randExtraCooldown = 60;
    }
    public override void TownNPCAttackMagic(ref float auraLightMultiplier)
    {
        for (int d = 0; d < Main.projectile.Length; d++)
        {
            if (Main.projectile[d].type == ModContent.ProjectileType<AcytaeaMagicBook>())
            {
                return;
            }
        }
        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaMagicBook>(), 0, 1, Main.myPlayer);
    }
    public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
    {
        projType = ModContent.ProjectileType<AcytaeaMagicBook>();
        attackDelay = 60;
    }
    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
    {
        multiplier = 2f;
    }
    public static void DrawAll(SpriteBatch sb)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (BladePro == 1 && NPCWHOAMI != -1 && Main.npc[NPCWHOAMI].localAI[0] > 900)
        {
            if (Main.npc[NPCWHOAMI].alpha >= 240)
            {
                return;
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            Vector2 vBla = new Vector2(88 - (minorDir - 1) * 8, -158).RotatedBy(BladeRot);
            vBla.Y *= BladeSquz;
            Vector2 vc = Main.npc[NPCWHOAMI].Center + vBla;
            BladeSquz = BladeSquz * 0.75f + AimBladeSquz * 0.25f;

            if (!Main.gamePaused)
            {
                for (int h = 59; h > 0; h--)
                {
                    OldBladePos[h] = OldBladePos[h - 1];
                }
                OldBladePos[0] = vc;
            }
            int MaxH = 0;
            for (int h = 0; h < 60; h++)
            {
                if (OldBladePos[h + 1] == Vector2.Zero)
                {
                    break;
                }
                MaxH++;
            }
            Vector2 vf = Main.npc[NPCWHOAMI].Center + RightArmPos * Main.npc[NPCWHOAMI].spriteDirection - Main.screenPosition + new Vector2(-7f, 3).RotatedBy(RightArmRot);

            for (int h = 0; h < 60; h++)
            {
                Color color3 = new Color(255, 255, 255, 0);

                if (OldBladePos[h + 1] == Vector2.Zero)
                {
                    break;
                }
                Vector2 v1 = OldBladePos[h + 1] - vf;
                Vector2 v0 = OldBladePos[h] - vf;
                if (BladeRot < OldBladeRot)
                {
                    Vx.Add(new Vertex2D(OldBladePos[h] - Main.screenPosition, color3, new Vector3(h / 60f, 0, 0)));
                    Vx.Add(new Vertex2D(OldBladePos[h + 1] - Main.screenPosition, color3, new Vector3((h + 1) / 60f, 0, 0)));
                    Vx.Add(new Vertex2D(vf, color3, new Vector3(0, 1, 0)));
                }
                else
                {
                    Vx.Add(new Vertex2D(OldBladePos[h + 1] - Main.screenPosition, color3, new Vector3((h + 1) / 60f, 0, 0)));
                    Vx.Add(new Vertex2D(OldBladePos[h] - Main.screenPosition, color3, new Vector3(h / 60f, 0, 0)));
                    Vx.Add(new Vertex2D(vf, color3, new Vector3(0, 1, 0)));
                }
                if (!Main.gamePaused)
                {
                    if (h % 5 == 0)
                    {
                        for (int j = 0; j < Main.player.Length; j++)
                        {
                            if (!Main.player[j].dead)
                            {
                                if ((Main.player[j].Center - OldBladePos[h]).Length() < 40)
                                {
                                    Projectile.NewProjectile(Main.npc[NPCWHOAMI].GetSource_FromAI(), Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), Main.npc[NPCWHOAMI].damage, 0, j, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
            Texture2D t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/RedBloodScaleShader").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (!isBattle)
        {
            return;
        }
        Player player = Main.player[NPC.target];
        CirR0 += 0.001f + COmega;
        CirPro0 += 0.1f;
        if (!Main.gamePaused)
        {
            DrawAI++;
        }
        if (DrawAI % 480 == 440)
        {
            headFrame = 1;
        }
        if (DrawAI % 480 == 455)
        {
            headFrame = 0;
        }
        SpriteEffects effects = SpriteEffects.None;
        if (NPC.spriteDirection == 1)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        Color color = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
        color = NPC.GetAlpha(color) * ((255 - NPC.alpha) / 255f);
        for (int h = 0; h < 8; h++)
        {
            if (h % 2 == 1)
            {
                Texture2D tx = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaShadow").Value;
                if (NPC.oldPos[h] != Vector2.Zero)
                {
                    float ColR = Math.Clamp(NPC.velocity.Length() / 10f, 0, 1) * (8 - h) / 8f * color.R / 255f;
                    float ColG = Math.Clamp(NPC.velocity.Length() / 10f, 0, 1) * (8 - h) / 8f * color.G / 255f;
                    float ColB = Math.Clamp(NPC.velocity.Length() / 10f, 0, 1) * (8 - h) / 8f * color.B / 255f;
                    //float ColA = Math.Clamp(NPC.velocity.Length() / 10f, 0, 1) * (8 - h) / 8f * color.A;
                    Color colorφ = new Color(ColR, ColG, ColB, 0);
                    spriteBatch.Draw(tx, NPC.oldPos[h] - Main.screenPosition + NPC.Size / 2f, null, colorφ, NPC.rotation, NPC.Size / 2f, NPC.scale, effects, 0f);
                }
            }
        }
        if (minorDir == 1)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaRightWing").Value, NPC.Center + RightWingPos * NPC.spriteDirection - Main.screenPosition, new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaLeftWing").Value, NPC.Center + LeftWingPos * NPC.spriteDirection - Main.screenPosition, new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
        }
        else
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaRightWing").Value, NPC.Center + RightWingPos * NPC.spriteDirection - Main.screenPosition + new Vector2(10, 0), new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaLeftWing").Value, NPC.Center + LeftWingPos * NPC.spriteDirection - Main.screenPosition + new Vector2(10, 0), new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
        }
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaLeftArm").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation + LeftArmRot, NPC.Size / 2f, 1f, effects, 0f);
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaBody").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaLeg").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
        if (HasBlade)
        {
            if (NPC.localAI[0] <= 900)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                if (BladePro == 1)
                {
                    List<Vertex2D> Vx = new List<Vertex2D>();
                    Vector2 vBla = new Vector2(88 - (minorDir - 1) * 8, -158).RotatedBy(BladeRot);
                    vBla.Y *= BladeSquz;
                    Vector2 vc = NPC.Center + vBla;
                    BladeSquz = BladeSquz * 0.75f + AimBladeSquz * 0.25f;

                    if (!Main.gamePaused)
                    {
                        for (int h = 59; h > 0; h--)
                        {
                            OldBladePos[h] = OldBladePos[h - 1];
                        }
                        OldBladePos[0] = vc;
                    }
                    int MaxH = 0;
                    for (int h = 0; h < 60; h++)
                    {
                        if (OldBladePos[h + 1] == Vector2.Zero)
                        {
                            break;
                        }
                        MaxH++;
                    }
                    Vector2 vf = NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition + new Vector2(-7f, 3).RotatedBy(RightArmRot);

                    for (int h = 0; h < 60; h++)
                    {
                        Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                        color3.A = 0;
                        color3.R = (byte)(color3.R * (MaxH - h - 1) / (float)MaxH * (255 - NPC.alpha) / 255f);
                        color3.G = (byte)(color3.G * (MaxH - h - 1) / (float)MaxH * (255 - NPC.alpha) / 255f);
                        color3.B = (byte)(color3.B * (MaxH - h - 1) / (float)MaxH * (255 - NPC.alpha) / 255f);
                        if (OldBladePos[h + 1] == Vector2.Zero)
                        {
                            break;
                        }
                        Vector2 v1 = OldBladePos[h + 1] - vf;
                        Vector2 v0 = OldBladePos[h] - vf;
                        if (BladeRot < OldBladeRot)
                        {
                            Vx.Add(new Vertex2D(OldBladePos[h] - Main.screenPosition, color3, new Vector3(h / 60f, 0, 0)));
                            Vx.Add(new Vertex2D(OldBladePos[h + 1] - Main.screenPosition, color3, new Vector3((h + 1) / 60f, 0, 0)));
                            Vx.Add(new Vertex2D(vf, color3, new Vector3(0, 1, 0)));
                        }
                        else
                        {
                            Vx.Add(new Vertex2D(OldBladePos[h + 1] - Main.screenPosition, color3, new Vector3((h + 1) / 60f, 0, 0)));
                            Vx.Add(new Vertex2D(OldBladePos[h] - Main.screenPosition, color3, new Vector3(h / 60f, 0, 0)));
                            Vx.Add(new Vertex2D(vf, color3, new Vector3(0, 1, 0)));
                        }
                        if (!Main.gamePaused)
                        {
                            if (h % 5 == 0)
                            {
                                for (int j = 0; j < Main.player.Length; j++)
                                {
                                    if (!Main.player[j].dead)
                                    {
                                        if ((Main.player[j].Center - OldBladePos[h]).Length() < 40)
                                        {
                                            Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), Dam, 0, j, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Texture2D t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/GlodenBloodScaleShader").Value;
                    Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                }
            }

        }
        if (HasBlade)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx = new List<Vertex2D>();
            Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
            color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
            color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
            color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
            color3.A = (byte)(color3.A * (255 - NPC.alpha) / 255f);
            if (BladePro < 1)
            {
                Vector2 vc = NPC.Center + new Vector2(88 * minorDir - (minorDir - 1) * 8, -158) - Main.screenPosition;
                Vector2 vd = new Vector2(17.02f, 0).RotatedBy(0.4 * minorDir) + vc;
                Vector2 ve = new Vector2(-17.02f, 0).RotatedBy(0.4 * minorDir) + vc;
                Vector2 vf = new Vector2(0, 185 * BladePro).RotatedBy(0.4 * minorDir) + vc;
                Vector2 vg = new Vector2(17.02f, 0).RotatedBy(0.4 * minorDir) + vf;
                Vector2 vh = new Vector2(-17.02f, 0).RotatedBy(0.4 * minorDir) + vf;

                Vx.Add(new Vertex2D(vg, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
                Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
                Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f + BladePro * 0.65f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f - BladePro * 0.65f, 0, 1), 0)));

                Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
                Vx.Add(new Vertex2D(ve, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f + BladePro * 0.65f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f - BladePro * 0.65f, 0, 1), 0)));
                Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f + BladePro * 0.65f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f - BladePro * 0.65f, 0, 1), 0)));
            }
            else
            {
                Vector2 vBla = new Vector2(88 - (minorDir - 1) * 8, -158).RotatedBy(BladeRot);
                vBla.Y *= BladeSquz;
                Vector2 vc = NPC.Center + vBla - Main.screenPosition;
                Vector2 vd = new Vector2(17.02f, 0).RotatedBy(0.4 + BladeRot) + vc;
                Vector2 ve = new Vector2(-17.02f, 0).RotatedBy(0.4 + BladeRot) + vc;
                Vector2 vf = NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition + new Vector2(-7f, 3).RotatedBy(RightArmRot);
                Vector2 vg = new Vector2(17.02f, 0).RotatedBy(0.4 + BladeRot) + vf;
                Vector2 vh = new Vector2(-17.02f, 0).RotatedBy(0.4 + BladeRot) + vf;

                Vx.Add(new Vertex2D(vg, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
                Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
                Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));

                Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
                Vx.Add(new Vertex2D(ve, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));
                Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(11.85f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));
            }

            Texture2D t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/GlodenBloodScaleMirror").Value;
            if (minorDir == -1)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/GlodenBloodScale").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
        if (HasBlade)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx = new List<Vertex2D>();
            Color color3 = new Color((int)(200 * BladeGlowPro), (int)(200 * BladeGlowPro), (int)(200 * BladeGlowPro), 0);
            color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
            color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
            color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
            color3.A = (byte)(color3.A * (255 - NPC.alpha) / 255f);
            Vector2 vBla = new Vector2(88 - (minorDir - 1) * 8, -158).RotatedBy(BladeRot);
            vBla.Y *= BladeSquz;
            Vector2 vc = NPC.Center + vBla - Main.screenPosition;
            Vector2 vd = new Vector2(17.02f, 0).RotatedBy(0.4 + BladeRot) + vc;
            Vector2 ve = new Vector2(-17.02f, 0).RotatedBy(0.4 + BladeRot) + vc;
            Vector2 vf = NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition + new Vector2(-7f, 3).RotatedBy(RightArmRot);
            Vector2 vg = new Vector2(17.02f, 0).RotatedBy(0.4 + BladeRot) + vf;
            Vector2 vh = new Vector2(-17.02f, 0).RotatedBy(0.4 + BladeRot) + vf;

            Vx.Add(new Vertex2D(vg, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
            Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
            Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));

            Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
            Vx.Add(new Vertex2D(ve, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));
            Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(11.85f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));

            Texture2D t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/GlodenBloodScaleGlowMirror").Value;
            if (minorDir == -1)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/GlodenBloodScaleGlow").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
        if (HasCricle)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx = new List<Vertex2D>();

            Vector2 vf = NPC.Center - Main.screenPosition;

            for (int h = 0; h < 90; h++)
            {
                Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                Vector2 v0 = new Vector2(0, ACircleR).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, ACircleR).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                if (h % 2 == 0)
                {
                    Vx.Add(new Vertex2D(vf + v0, color3, new Vector3((0.999f + CirPro0) / 30f % 1f, 0, 0)));
                    Vx.Add(new Vertex2D(vf + v1, color3, new Vector3(CirPro0 / 30f % 1f, 0, 0)));
                    Vx.Add(new Vertex2D(vf, color3, new Vector3((0.5f + CirPro0) / 30f % 1f, 1, 0)));
                }
                else
                {
                    Vx.Add(new Vertex2D(vf + v0, color3, new Vector3(CirPro0 / 30f % 1f, 0, 0)));
                    Vx.Add(new Vertex2D(vf + v1, color3, new Vector3((0.999f + CirPro0) / 30f % 1f, 0, 0)));
                    Vx.Add(new Vertex2D(vf, color3, new Vector3((0.5f + CirPro0) / 30f % 1f, 1, 0)));
                }
            }
            Texture2D t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle3").Value;
            if (NPC.localAI[0] > 3270 && NPC.localAI[0] <= 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle4").Value;
            }
            if (NPC.localAI[0] > 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle4").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx9 = new List<Vertex2D>();
            for (int h = 0; h < 90; h++)
            {
                Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                Vector2 v0 = new Vector2(0, ACircleR).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, ACircleR).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                Vx9.Add(new Vertex2D(vf + v0, color3, new Vector3(h / 30f % 1f, 0, 0)));
                Vx9.Add(new Vertex2D(vf + v1, color3, new Vector3((0.999f + h) / 30f % 1f, 0, 0)));
                Vx9.Add(new Vertex2D(vf, color3, new Vector3((0.5f + h) / 30f % 1f, 1, 0)));
            }
            t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle4").Value;
            if (NPC.localAI[0] > 3270 && NPC.localAI[0] <= 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle8").Value;
            }
            if (NPC.localAI[0] > 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle12").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirrorsss
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx9.ToArray(), 0, Vx9.Count / 3);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx2 = new List<Vertex2D>();

            if (NPC.localAI[0] > 3270)
            {
                if (NPC.localAI[0] > 8500)
                {
                    for (int h = 0; h < 16; h++)
                    {
                        Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                        color3.A = 0;
                        color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                        color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                        color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                        Vector2 v0 = new Vector2(0, ACircleR * 0.67f).RotatedBy(h / 8d * Math.PI + CirR0 * -3.6f);
                        Vector2 v1 = new Vector2(0, ACircleR * 0.67f).RotatedBy((h + 1) / 8d * Math.PI + CirR0 * -3.6f);
                        Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(0, 0, 0)));
                        Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(1, 0, 0)));
                        Vx2.Add(new Vertex2D(vf, color3, new Vector3(0.5f, 1, 0)));
                    }
                }
                else
                {
                    for (int h = 0; h < 30; h++)
                    {
                        Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                        color3.A = 0;
                        color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                        color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                        color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                        Vector2 v0 = new Vector2(0, ACircleR * 0.5f).RotatedBy(h / 15d * Math.PI + CirR0 * -3.6f);
                        Vector2 v1 = new Vector2(0, ACircleR * 0.5f).RotatedBy((h + 1) / 15d * Math.PI + CirR0 * -3.6f);
                        Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(0, 0, 0)));
                        Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(1, 0, 0)));
                        Vx2.Add(new Vertex2D(vf, color3, new Vector3(0.5f, 1, 0)));
                    }
                }
            }
            else
            {
                for (int h = 0; h < 90; h++)
                {
                    Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                    color3.A = 0;
                    color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                    color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                    color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                    Vector2 v0 = new Vector2(0, ACircleR * 0.8f).RotatedBy(h / 45d * Math.PI + CirR0 * -3.6f);
                    Vector2 v1 = new Vector2(0, ACircleR * 0.8f).RotatedBy((h + 1) / 45d * Math.PI + CirR0 * -3.6f);
                    Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(0, 0, 0)));
                    Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(1, 0, 0)));
                    Vx2.Add(new Vertex2D(vf, color3, new Vector3(0.5f, 1, 0)));
                }
            }
            t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle2").Value;
            if (NPC.localAI[0] > 3270 && NPC.localAI[0] <= 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle11").Value;
            }
            if (NPC.localAI[0] > 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle13").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);

            if (NPC.localAI[0] <= 3270)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<Vertex2D> Vx3 = new List<Vertex2D>();
                List<Vertex2D> Vx4 = new List<Vertex2D>();
                for (int h = 0; h < 90; h++)
                {
                    Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                    color3.A = 0;
                    color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                    color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                    color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                    Vector2 v0 = new Vector2(0, ACircleR * 0.8f).RotatedBy(h / 45d * Math.PI + CirR0 * -10.6f);
                    Vector2 v1 = new Vector2(0, ACircleR * 0.8f).RotatedBy((h + 1) / 45d * Math.PI + CirR0 * -10.6f);
                    if (h % 9 >= 4)
                    {
                        Vx3.Add(new Vertex2D(vf + v0, color3, new Vector3(0, 0, 0)));
                        Vx3.Add(new Vertex2D(vf + v1, color3, new Vector3(1, 0, 0)));
                        Vx3.Add(new Vertex2D(vf, color3, new Vector3(0.5f, 1, 0)));
                    }
                    else
                    {
                        Vx4.Add(new Vertex2D(vf + v0, color3, new Vector3(0, 0, 0)));
                        Vx4.Add(new Vertex2D(vf + v1, color3, new Vector3(1, 0, 0)));
                        Vx4.Add(new Vertex2D(vf, color3, new Vector3(0.5f, 1, 0)));
                    }
                }
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle4").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx4.ToArray(), 0, Vx4.Count / 3);
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);
            }


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx5 = new List<Vertex2D>();
            for (int h = 0; h < 90; h++)
            {
                Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                Vector2 v0 = new Vector2(0, ACircleR).RotatedBy(h / 45d * Math.PI + CirR0 * -19f);
                Vector2 v1 = new Vector2(0, ACircleR).RotatedBy((h + 1) / 45d * Math.PI + CirR0 * -19f);
                Vx5.Add(new Vertex2D(vf + v0, color3, new Vector3(h / 30f % 1f, 0, 0)));
                Vx5.Add(new Vertex2D(vf + v1, color3, new Vector3((0.999f + h) / 30f % 1f, 0, 0)));
                Vx5.Add(new Vertex2D(vf, color3, new Vector3((0.5f + h) / 30f % 1f, 1, 0)));
            }
            t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle5").Value;
            if (NPC.localAI[0] > 3270 && NPC.localAI[0] <= 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle9").Value;
            }
            if (NPC.localAI[0] > 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle14").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx5.ToArray(), 0, Vx5.Count / 3);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx6 = new List<Vertex2D>();
            for (int h = 0; h < 90; h++)
            {
                Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                Vector2 v0 = new Vector2(0, ACircleR).RotatedBy(h / 45d * Math.PI + CirR0 * -12f);
                Vector2 v1 = new Vector2(0, ACircleR).RotatedBy((h + 1) / 45d * Math.PI + CirR0 * -12f);
                Vx6.Add(new Vertex2D(vf + v0, color3, new Vector3(h / 30f % 1f, 0, 0)));
                Vx6.Add(new Vertex2D(vf + v1, color3, new Vector3((0.999f + h) / 30f % 1f, 0, 0)));
                Vx6.Add(new Vertex2D(vf, color3, new Vector3((0.5f + h) / 30f % 1f, 1, 0)));
            }
            t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle6").Value;
            if (NPC.localAI[0] > 3270 && NPC.localAI[0] <= 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle10").Value;
            }
            if (NPC.localAI[0] > 8500)
            {
                t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle15").Value;
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx6.ToArray(), 0, Vx6.Count / 3);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx7 = new List<Vertex2D>();
            List<Vertex2D> Vx8 = new List<Vertex2D>();
            for (int h = 0; h < 72; h++)
            {
                Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
                Vector2 v0 = new Vector2(0, ACircleR * 0.65f).RotatedBy(h / 36d * Math.PI + CirR0 * -12f);
                Vector2 v1 = new Vector2(0, ACircleR * 0.65f).RotatedBy((h + 1) / 36d * Math.PI + CirR0 * -12f);
                if (h % 12 >= SwirlPro)
                {
                    Vx7.Add(new Vertex2D(vf + v0, color3, new Vector3(h / 30f % 1f, 0, 0)));
                    Vx7.Add(new Vertex2D(vf + v1, color3, new Vector3((0.999f + h) / 30f % 1f, 0, 0)));
                    Vx7.Add(new Vertex2D(vf, color3, new Vector3((0.5f + h) / 30f % 1f, 1, 0)));
                }
                else
                {
                    Vx8.Add(new Vertex2D(vf + v0, color3, new Vector3(h / 30f % 1f, 0, 0)));
                    Vx8.Add(new Vertex2D(vf + v1, color3, new Vector3((0.999f + h) / 30f % 1f, 0, 0)));
                    Vx8.Add(new Vertex2D(vf, color3, new Vector3((0.5f + h) / 30f % 1f, 1, 0)));
                }
            }
            t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle7").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx7.ToArray(), 0, Vx7.Count / 3);
            t = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle4").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx8.ToArray(), 0, Vx8.Count / 3);

        }
        if (minorDir == 1)
        {
            if (RightArmRot <= Math.PI / 2d)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaRightArm").Value, NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition, null, color, NPC.rotation + RightArmRot, new Vector2(33, 23), 1f, effects, 0f);
            }
        }
        else
        {
            if (RightArmRot >= -Math.PI / 2d)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaRightArm").Value, NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition + new Vector2(10, 0), null, color, NPC.rotation + RightArmRot, new Vector2(17, 23), 1f, effects, 0f);
            }
        }
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaHead").Value, NPC.Center - Main.screenPosition, new Rectangle(0, headFrame * 56, 50, 56), color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaEye").Value, NPC.Center - Main.screenPosition, new Rectangle(0, headFrame * 56, 50, 56), new Color(255, 255, 255, 0), NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
        if (minorDir == 1)
        {
            if (RightArmRot > Math.PI / 2d)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaRightArm").Value, NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition, null, color, NPC.rotation + RightArmRot, new Vector2(33, 23), 1f, effects, 0f);
            }
        }
        else
        {
            if (RightArmRot < -Math.PI / 2d)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaRightArm").Value, NPC.Center + RightArmPos * NPC.spriteDirection - Main.screenPosition + new Vector2(10, 0), null, color, NPC.rotation + RightArmRot, new Vector2(17, 23), 1f, effects, 0f);
            }
        }
        if (NPC.localAI[0] > 2020 && NPC.localAI[0] < 2070)
        {
            Texture2D tx = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaShadow").Value;
            for (int k = 0; k < 15; k++)
            {
                float Col = ((float)-Math.Cos((NPC.localAI[0] - 2020) / 25d * Math.PI) + 1) / 2f;
                Color colorφ = new Color(Col, Col, Col, 0);
                Vector2 c = new Vector2(0, 12).RotatedBy(k / 7.5 * Math.PI) * Col;
                spriteBatch.Draw(tx, NPC.Center - Main.screenPosition + c, null, colorφ, NPC.rotation, NPC.Size / 2f, NPC.scale, effects, 0f);
            }
        }
        if (HasBow)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaBow").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation + BowRot, new Vector2(30, 28), 1f, SpriteEffects.None, 0f);
        }
        if (HasBook)
        {
            if (CloseBook)
            {
                if (NPC.localAI[0] % 6 == 0 && !Main.gamePaused)
                {
                    if (BookFrame < 2)
                    {
                        BookFrame++;
                    }
                    else
                    {
                        CloseBook = false;
                        BookFrame = 0;
                    }
                }
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaMagicBook2").Value, NPC.Center - Main.screenPosition, new Rectangle(0, BookFrame * 56, 50, 56), color, NPC.rotation, new Vector2(25, 28), 1f, effects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaMagicBook").Value, NPC.Center - Main.screenPosition, new Rectangle(0, BookFrame * 56, 50, 56), color, NPC.rotation, new Vector2(25, 28), 1f, effects, 0f);
            }
        }
        if (!CloseBook)
        {
            if (NPC.localAI[0] % 6 == 0 && !Main.gamePaused)
            {
                if (BookFrame < 3)
                {
                    BookFrame++;
                }
                else
                {
                    BookFrame = 0;
                }
            }
        }
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        /*for (int s = 0; s < 10; s++)
        {
            Skirt1[s] = NPC.Center + new Vector2((s - 3) * 2.2f * (-NPC.spriteDirection), 6);
        }
        for (int s = 0; s < 11; s++)
        {
            AIMSkirt2[s] = NPC.Center + new Vector2((s - 4) * 4 * (-NPC.spriteDirection), 40);
        }
        if (Skirt2[0] == Vector2.Zero)
        {
            for (int s = 0; s < 11; s++)
            {
                Skirt2[s] = NPC.Center + new Vector2((s - 4) * 4 * (-NPC.spriteDirection), 20);
            }
        }
        for (int s = 0; s < 11; s++)
        {
            Vector2 Va = (AIMSkirt2[s] - Skirt2[s]) / 100f;
            vSkirt2[s] += Va;
            vSkirt2[s] *= 0.96f;
            if (Collision.SolidCollision(Skirt2[s] + vSkirt2[s], 1, 1) && Va.Length() < 0.2f)
            {
                vSkirt2[s] *= 0;
            }
            Skirt2[s] += vSkirt2[s];
        }
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        List<Vertex2D> Vy = new List<Vertex2D>();

        Vector2 vq = NPC.Center - Main.screenPosition;

        for (int s = 0; s < 10; s++)
        {
            Color color3 = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
            color3.A = (byte)(color3.A * (255 - NPC.alpha) / 255f);
            color3.R = (byte)(color3.R * (255 - NPC.alpha) / 255f);
            color3.G = (byte)(color3.G * (255 - NPC.alpha) / 255f);
            color3.B = (byte)(color3.B * (255 - NPC.alpha) / 255f);
            Vy.Add(new Vertex2D(Skirt2[s] - Main.screenPosition + new Vector2(-1, 0), color3, new Vector3(1, 0, 0)));
            Vy.Add(new Vertex2D(Skirt1[s] - Main.screenPosition, color3, new Vector3(1, 1, 0)));
            Vy.Add(new Vertex2D(Skirt2[s + 1] - Main.screenPosition + new Vector2(1, 0), color3, new Vector3(0, 0, 0)));
            if (s < 9)
            {
                Vy.Add(new Vertex2D(Skirt1[s] - Main.screenPosition + new Vector2(-1, 0), color3, new Vector3(1, 0, 0)));
                Vy.Add(new Vertex2D(Skirt1[s + 1] - Main.screenPosition + new Vector2(1, 0), color3, new Vector3(0, 0, 0)));
                Vy.Add(new Vertex2D(Skirt2[s] - Main.screenPosition, color3, new Vector3(1, 1, 0)));
            }
        }
        Texture2D ta = ModContent.Request<Texture2D>("MythMod/NPCs/Acytaea/AcytaeaCircle4").Value;
        Main.graphics.GraphicsDevice.Textures[0] = ta;//GlodenBloodScaleMirror
        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vy.ToArray(), 0, Vy.Count / 3);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);*/
        if (isBattle)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
