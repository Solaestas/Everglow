using Terraria.Localization;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.NPCs.SquamousShell
{
    [AutoloadBossHead]
    public class SquamousShell : ModNPC
    {
        internal float TexWidth = 150;
        internal float TexHeight = 300;
        /// <summary>
        /// 交换两个变量的值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public void Exchange(ref float value1, ref float value2)
        {
            (value1, value2) = (value2, value1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldRot"></param>
        /// <returns></returns>
        public double CheckRotDir(double OldRot)
        {
            return -OldRot;
        }
        public Vector2 JointPointCalculate(Vector2 Point1, Vector2 Point2, float Length)
        {
            Vector2 delP = (Point2 - Point1) * 0.5f;//Point1到Point2的半程
            float L1 = delP.Length();//半程的长度
            float outLength = (float)Math.Sqrt(Math.Max(Length * Length - L1 * L1, 0));
            Vector2 v0 = Vector2.Normalize(delP).RotatedBy(Math.PI / 2d * NPC.spriteDirection);
            Vector2 v1 = new Vector2(0, 1).RotatedBy(NPC.rotation * NPC.spriteDirection);
            Vector2 v2 = (v0 + v1) / 2f;
            return (Point1 + delP + v2 * outLength);
        }
        public double GetVector2Rot(Vector2 value)
        {
            return Math.Atan2(value.Y, value.X) * NPC.spriteDirection + Math.PI / 2d;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Squamous Shell");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "龙鳞古壳");
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 30;
            NPC.width = 140;
            NPC.height = 90;
            NPC.defense = 15;
            NPC.lifeMax = 6000;
            NPC.knockBackResist = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 20000;
            NPC.localAI[0] = 0;
        }

        public override void OnSpawn(IEntitySource source)//初始化
        {
            //Leg1FCen = NPC.Center + new Vector2(-83, 40);
            //Leg1FRot = 0.2873 * Math.PI;
            //Leg1MCen = NPC.Center + new Vector2(-62, 43);
            //Leg1MRot = -1.1339 * Math.PI;
            //Leg1BCen = NPC.Center + new Vector2(-44, 41);
            //Leg1BRot = 0.2078 * Math.PI;

            Leg1FTip = NPC.Center + new Vector2(-103, 60);
            Leg1FRoot = NPC.Center + new Vector2(-67, 26);

            Leg1MTip = Leg1FRoot;
            Leg1MRoot = NPC.Center + new Vector2(-48, 61);

            Leg1BTip = Leg1MRoot;
            Leg1BRoot = NPC.Center + new Vector2(-29, 28);


            Leg2FCen = NPC.Center + new Vector2(4, 44);
            Leg2FRot = 0.0072 * Math.PI;
            Leg2MCen = NPC.Center + new Vector2(-4, 45);
            Leg2MRot = -0.9114 * Math.PI;
            Leg2BCen = NPC.Center + new Vector2(-16, 43);
            Leg2BRot = -0.2671 * Math.PI;
            Leg3FCen = NPC.Center + new Vector2(77, 39);
            Leg3FRot = -0.2082 * Math.PI;
            Leg3MCen = NPC.Center + new Vector2(55, 43);
            Leg3MRot = -0.892 * Math.PI;
            Leg3BCen = NPC.Center + new Vector2(35, 41);
            Leg3BRot = -0.2693 * Math.PI;

            Leg1Root = NPC.Center + new Vector2(-35, 40);
            Leg2Root = NPC.Center + new Vector2(-15, 49);
            Leg3Root = NPC.Center + new Vector2(26, 48);
        }


        private Vector2 Leg1Root;
        private Vector2 Leg2Root;
        private Vector2 Leg3Root;
        float SetX = 0;

        public class SquamousLeg
        {
            internal Vector2 Tip;
            internal Vector2 Root;

            internal float TexX;
            internal float TexY;
            internal float TexWidth;
            internal float TexHeight;
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            NPC.localAI[0] += 1;
            
            if (Main.mouseLeft)
            {
                SetX += 0.5f;
            }
            if (Main.mouseRight)
            {
                SetX -= 0.5f;
            }
            if (NPC.localAI[0] < 1000)
            {
                if (NPC.localAI[0] <= 60000 && NPC.localAI[0] > 0)
                {
                    
                    //Leg1Root = NPC.Center + new Vector2(-35 * NPC.spriteDirection, 30).RotatedBy(NPC.rotation);
                    //Leg2Root = NPC.Center + new Vector2(-15 * NPC.spriteDirection, 30).RotatedBy(NPC.rotation);
                    //Leg3Root = NPC.Center + new Vector2(26 * NPC.spriteDirection, 30).RotatedBy(NPC.rotation);

                    //Leg1FCen = NPC.Center + new Vector2((SetX - 83) * NPC.spriteDirection, 40).RotatedBy(NPC.rotation);
                    //Leg1FRot = 0.2873 * Math.PI + NPC.rotation * NPC.spriteDirection;

                    //Vector2 Leg1FTop = new Vector2(0,-L1Length * 0.35f).RotatedBy(Leg1FRot * NPC.spriteDirection) + Leg1FCen;
                    //Vector2 Joint1 = JointPointCalculate(Leg1FTop, Leg1Root, LLength * 0.85f);

                    //Leg1MCen = (Joint1 + Leg1FTop) * 0.5f;
                    //Leg1MRot = GetVector2Rot(Leg1FTop - Joint1);
                    //Leg1BCen = (Joint1 + Leg1Root) * 0.5f;
                    //Leg1BRot = GetVector2Rot(Joint1 - Leg1Root);

                    //Leg2FCen = NPC.Center + new Vector2(4 * NPC.spriteDirection, 44).RotatedBy(NPC.rotation);
                    //Leg2FRot = 0.0072 * Math.PI + NPC.rotation * NPC.spriteDirection;
                    //Leg2MCen = NPC.Center + new Vector2(-4 * NPC.spriteDirection, 45).RotatedBy(NPC.rotation);
                    //Leg2MRot = -0.9114 * Math.PI + NPC.rotation * NPC.spriteDirection;
                    //Leg2BCen = NPC.Center + new Vector2(-16 * NPC.spriteDirection, 43).RotatedBy(NPC.rotation);
                    //Leg2BRot = -0.2671 * Math.PI + NPC.rotation * NPC.spriteDirection;


                    //Leg3FCen = NPC.Center + new Vector2(77 * NPC.spriteDirection, 39).RotatedBy(NPC.rotation);
                    //Leg3FRot = -0.2082 * Math.PI + NPC.rotation * NPC.spriteDirection;
                    //Leg3MCen = NPC.Center + new Vector2(55 * NPC.spriteDirection, 43).RotatedBy(NPC.rotation);
                    //Leg3MRot = -0.892 * Math.PI + NPC.rotation * NPC.spriteDirection;
                    //Leg3BCen = NPC.Center + new Vector2(35 * NPC.spriteDirection, 41).RotatedBy(NPC.rotation);
                    //Leg3BRot = -0.2693 * Math.PI + NPC.rotation * NPC.spriteDirection;
                    //if (NPC.spriteDirection == -1)
                    //{
                    //    Leg1FRot = CheckRotDir(Leg1FRot);
                    //    Leg1MRot = CheckRotDir(Leg1MRot);
                    //    Leg1BRot = CheckRotDir(Leg1BRot);
                    //    Leg2FRot = CheckRotDir(Leg2FRot);
                    //    Leg2MRot = CheckRotDir(Leg2MRot);
                    //    Leg2BRot = CheckRotDir(Leg2BRot);
                    //    Leg3FRot = CheckRotDir(Leg3FRot);
                    //    Leg3MRot = CheckRotDir(Leg3MRot);
                    //    Leg3BRot = CheckRotDir(Leg3BRot);
                    //}
                    //Leg1Tip = Leg1FCen + new Vector2(0, L1Length / 2f - 4).RotatedBy(Leg1FRot);
                    //Leg2Tip = Leg2FCen + new Vector2(0, L2Length / 2f - 4).RotatedBy(Leg2FRot);
                    //Leg3Tip = Leg3FCen + new Vector2(0, L3Length / 2f - 4).RotatedBy(Leg3FRot);
                    //if (Collision.SolidCollision(Leg1Tip, 2, 2) || Collision.SolidCollision(Leg2Tip, 2, 2) || Collision.SolidCollision(Leg3Tip, 2, 2))
                    //{
                    //    NPC.noGravity = true;
                    //    NPC.velocity *= 0;
                    //}
                    //if (Main.mouseMiddle && Main.mouseMiddleRelease)
                    //{
                    //    NPC.spriteDirection *= -1;
                    //}


                    //int num6 = Dust.NewDust(Leg1Tip, 0, 0, 75, 0, 0);
                    //Main.dust[num6].velocity *= 0;
                    //Main.dust[num6].noGravity = true;
                }
            }
            else
            {
                NPC.localAI[0] = 0;
            }
        }

        private float LLength = 54;
        private float LWidth = 28;
        private float L1Length = 72;
        private float L2Length = 62;
        private float L3Length = 72;



        private Vector2 Leg1FTip;
        private Vector2 Leg1FRoot;

        private float Leg1FrontX = 13 / 150f;
        private float Leg1FrontY = 246 / 320f;
        private float Leg1FrontWidth = 25 / 150f;
        private float Leg1FrontHeight = 71 / 320f;



        private Vector2 Leg1MTip;
        private Vector2 Leg1MRoot;

        private float Leg1MiddleX = 14 / 150f;
        private float Leg1MiddleY = 180 / 320f;
        private float Leg1MiddleWidth = 23 / 150f;
        private float Leg1MiddleHeight = 53 / 320f;



        private Vector2 Leg1BTip;
        private Vector2 Leg1BRoot;

        private float Leg1BackX = 12 / 150f;
        private float Leg1BackY = 112 / 320f;
        private float Leg1BackWidth = 23 / 150f;
        private float Leg1BackHeight = 55 / 320f;



        private Vector2 Leg2FCen;
        private double Leg2FRot;
        private Vector2 Leg2MCen;
        private double Leg2MRot;
        private Vector2 Leg2BCen;
        private double Leg2BRot;
        private Vector2 Leg3FCen;
        private double Leg3FRot;
        private Vector2 Leg3MCen;
        private double Leg3MRot;
        private Vector2 Leg3BCen;
        private double Leg3BRot;
        private Vector2 Leg1Tip;
        private Vector2 Leg2Tip;
        private Vector2 Leg3Tip;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            Texture2D tex0 = YggdrasilContent.QuickTexture("YggdrasilTown/NPCs/SquamousShell/SquamousShell_Boss");
            Vector2 Cen = NPC.Center - Main.screenPosition;
            Color DrawC = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
            Vector2 BodyLT = Cen + new Vector2(-75, -51).RotatedBy(NPC.rotation);
            Vector2 BodyRT = Cen + new Vector2(75, -51).RotatedBy(NPC.rotation);
            Vector2 BodyLD = Cen + new Vector2(-75, 51).RotatedBy(NPC.rotation);
            Vector2 BodyRD = Cen + new Vector2(75, 51).RotatedBy(NPC.rotation);

            Vector2 DLeg2FCen = Leg2FCen - Main.screenPosition;
            Vector2 DLeg2MCen = Leg2MCen - Main.screenPosition;
            Vector2 DLeg2BCen = Leg2BCen - Main.screenPosition;
            Vector2 DLeg3FCen = Leg3FCen - Main.screenPosition;
            Vector2 DLeg3MCen = Leg3MCen - Main.screenPosition;
            Vector2 DLeg3BCen = Leg3BCen - Main.screenPosition;
            Vector2 LegLT = new Vector2(-LWidth / 2f, -LLength / 2f);
            Vector2 LegRT = new Vector2(LWidth / 2f, -LLength / 2f);
            Vector2 LegLD = new Vector2(-LWidth / 2f, LLength / 2f);
            Vector2 LegRD = new Vector2(LWidth / 2f, LLength / 2f);

            Vector2 Leg1LT = new Vector2(-LWidth / 2f, -L1Length / 2f);
            Vector2 Leg1RT = new Vector2(LWidth / 2f, -L1Length / 2f);
            Vector2 Leg1LD = new Vector2(-LWidth / 2f, L1Length / 2f);
            Vector2 Leg1RD = new Vector2(LWidth / 2f, L1Length / 2f);


            Vector2 Leg2LT = new Vector2(-LWidth / 2f, -L2Length / 2f);
            Vector2 Leg2RT = new Vector2(LWidth / 2f, -L2Length / 2f);
            Vector2 Leg2LD = new Vector2(-LWidth / 2f, L2Length / 2f);
            Vector2 Leg2RD = new Vector2(LWidth / 2f, L2Length / 2f);


            Vector2 Leg3LT = new Vector2(-LWidth / 2f, -L3Length / 2f);
            Vector2 Leg3RT = new Vector2(LWidth / 2f, -L3Length / 2f);
            Vector2 Leg3LD = new Vector2(-LWidth / 2f, L3Length / 2f);
            Vector2 Leg3RD = new Vector2(LWidth / 2f, L3Length / 2f);

            float BeL = 2 / TexWidth ;
            //float BeT = 0 / TexWidth ;
            float BeR = 148 / TexWidth ;
            float BeD = 102 / TexHeight;

            float Leg1BackX = 12 / TexWidth ;
            float Leg1BackY = 112 / TexHeight;
            float Leg1BackWidth = 23 / TexWidth ;
            float Leg1BackHeight= 55 / TexHeight;

            float Leg1ML = 14 / TexWidth ;
            float Leg1MT = 180 / TexHeight;
            float Leg1MR = 37 / TexWidth ;
            float Leg1MD = 233 / TexHeight;

            float Leg1FL = 13 / TexWidth ;
            float Leg1FT = 246 / TexHeight;
            float Leg1FR = 38 / TexWidth ;
            float Leg1FD = 317 / TexHeight;

            float Leg2BL = 12 / TexWidth ;
            float Leg2BT = 112 / TexHeight;
            float Leg2BR = 35 / TexWidth ;
            float Leg2BD = 167 / TexHeight;

            float Leg2ML = 14 / TexWidth ;
            float Leg2MT = 180 / TexHeight;
            float Leg2MR = 37 / TexWidth ;
            float Leg2MD = 233 / TexHeight;

            float Leg2FL = 64 / TexWidth ;
            float Leg2FT = 248 / TexHeight;
            float Leg2FR = 93 / TexWidth ;
            float Leg2FD = 309 / TexHeight;

            float Leg3BL = 12 / TexWidth ;
            float Leg3BT = 112 / TexHeight;
            float Leg3BR = 35 / TexWidth ;
            float Leg3BD = 167 / TexHeight;

            float Leg3ML = 14 / TexWidth ;
            float Leg3MT = 180 / TexHeight;
            float Leg3MR = 37 / TexWidth ;
            float Leg3MD = 233 / TexHeight;

            float Leg3FL = 38 / TexWidth ;
            float Leg3FT = 246 / TexHeight;
            float Leg3FR = 13 / TexWidth ;
            float Leg3FD = 317 / TexHeight;

            if (NPC.spriteDirection == -1)
            {
                Exchange(ref Leg1FL, ref Leg1FR);
                Exchange(ref Leg1ML, ref Leg1MR);

                Exchange(ref Leg2FL, ref Leg2FR);
                Exchange(ref Leg2ML, ref Leg2MR);
                Exchange(ref Leg2BL, ref Leg2BR);
                Exchange(ref Leg3FL, ref Leg3FR);
                Exchange(ref Leg3ML, ref Leg3MR);
                Exchange(ref Leg3BL, ref Leg3BR);
            }
            List<Vertex2D> BodyV = new List<Vertex2D>
            {
                //绘制甲虫第一条腿,内侧

                //new Vertex2D(DLeg1BCen + LegRT.RotatedBy(Leg1BRot), DrawC, new Vector3(Leg1BR, Leg1BT, 0)),
                //new Vertex2D(DLeg1BCen + LegRD.RotatedBy(Leg1BRot), DrawC, new Vector3(Leg1BR, Leg1BD, 0)),
                //new Vertex2D(DLeg1BCen + LegLD.RotatedBy(Leg1BRot), DrawC, new Vector3(Leg1BL, Leg1BD, 0)),

                //new Vertex2D(DLeg1BCen + LegRT.RotatedBy(Leg1BRot), DrawC, new Vector3(Leg1BR, Leg1BT, 0)),
                //new Vertex2D(DLeg1BCen + LegLD.RotatedBy(Leg1BRot), DrawC, new Vector3(Leg1BL, Leg1BD, 0)),
                //new Vertex2D(DLeg1BCen + LegLT.RotatedBy(Leg1BRot), DrawC, new Vector3(Leg1BL, Leg1BT, 0)),

                //绘制甲虫第二条腿,内侧

                new Vertex2D(DLeg2BCen + LegRT.RotatedBy(Leg2BRot), DrawC, new Vector3(Leg2BR, Leg2BT, 0)),
                new Vertex2D(DLeg2BCen + LegRD.RotatedBy(Leg2BRot), DrawC, new Vector3(Leg2BR, Leg2BD, 0)),
                new Vertex2D(DLeg2BCen + LegLD.RotatedBy(Leg2BRot), DrawC, new Vector3(Leg2BL, Leg2BD, 0)),

                new Vertex2D(DLeg2BCen + LegRT.RotatedBy(Leg2BRot), DrawC, new Vector3(Leg2BR, Leg2BT, 0)),
                new Vertex2D(DLeg2BCen + LegLD.RotatedBy(Leg2BRot), DrawC, new Vector3(Leg2BL, Leg2BD, 0)),
                new Vertex2D(DLeg2BCen + LegLT.RotatedBy(Leg2BRot), DrawC, new Vector3(Leg2BL, Leg2BT, 0)),


                //绘制甲虫第三条腿,内侧
                new Vertex2D(DLeg3BCen + LegRT.RotatedBy(Leg3BRot), DrawC, new Vector3(Leg3BR, Leg3BT, 0)),
                new Vertex2D(DLeg3BCen + LegRD.RotatedBy(Leg3BRot), DrawC, new Vector3(Leg3BR, Leg3BD, 0)),
                new Vertex2D(DLeg3BCen + LegLD.RotatedBy(Leg3BRot), DrawC, new Vector3(Leg3BL, Leg3BD, 0)),

                new Vertex2D(DLeg3BCen + LegRT.RotatedBy(Leg3BRot), DrawC, new Vector3(Leg3BR, Leg3BT, 0)),
                new Vertex2D(DLeg3BCen + LegLD.RotatedBy(Leg3BRot), DrawC, new Vector3(Leg3BL, Leg3BD, 0)),
                new Vertex2D(DLeg3BCen + LegLT.RotatedBy(Leg3BRot), DrawC, new Vector3(Leg3BL, Leg3BT, 0)),

                //绘制甲虫身体

                new Vertex2D(BodyRT, DrawC, new Vector3(NPC.spriteDirection == 1 ? BeR : BeL, 0, 0)),
                new Vertex2D(BodyRD, DrawC, new Vector3(NPC.spriteDirection == 1 ? BeR : BeL, BeD, 0)),
                new Vertex2D(BodyLD, DrawC, new Vector3(NPC.spriteDirection == 1 ? BeL : BeR, BeD, 0)),

                new Vertex2D(BodyRT, DrawC, new Vector3(NPC.spriteDirection == 1 ? BeR : BeL, 0, 0)),
                new Vertex2D(BodyLD, DrawC, new Vector3(NPC.spriteDirection == 1 ? BeL : BeR, BeD, 0)),
                new Vertex2D(BodyLT, DrawC, new Vector3(NPC.spriteDirection == 1 ? BeL : BeR, 0, 0)),

                //绘制甲虫第一条腿

                //new Vertex2D(DLeg1MCen + LegRT.RotatedBy(Leg1MRot), DrawC, new Vector3(Leg1MR, Leg1MT, 0)),
                //new Vertex2D(DLeg1MCen + LegRD.RotatedBy(Leg1MRot), DrawC, new Vector3(Leg1MR, Leg1MD, 0)),
                //new Vertex2D(DLeg1MCen + LegLD.RotatedBy(Leg1MRot), DrawC, new Vector3(Leg1ML, Leg1MD, 0)),

                //new Vertex2D(DLeg1MCen + LegRT.RotatedBy(Leg1MRot), DrawC, new Vector3(Leg1MR, Leg1MT, 0)),
                //new Vertex2D(DLeg1MCen + LegLD.RotatedBy(Leg1MRot), DrawC, new Vector3(Leg1ML, Leg1MD, 0)),
                //new Vertex2D(DLeg1MCen + LegLT.RotatedBy(Leg1MRot), DrawC, new Vector3(Leg1ML, Leg1MT, 0)),

                //new Vertex2D(DLeg1FCen + Leg1RT.RotatedBy(Leg1FRot), DrawC, new Vector3(Leg1FR, Leg1FT, 0)),
                //new Vertex2D(DLeg1FCen + Leg1RD.RotatedBy(Leg1FRot), DrawC, new Vector3(Leg1FR, Leg1FD, 0)),
                //new Vertex2D(DLeg1FCen + Leg1LD.RotatedBy(Leg1FRot), DrawC, new Vector3(Leg1FL, Leg1FD, 0)),

                //new Vertex2D(DLeg1FCen + Leg1RT.RotatedBy(Leg1FRot), DrawC, new Vector3(Leg1FR, Leg1FT, 0)),
                //new Vertex2D(DLeg1FCen + Leg1LD.RotatedBy(Leg1FRot), DrawC, new Vector3(Leg1FL, Leg1FD, 0)),
                //new Vertex2D(DLeg1FCen + Leg1LT.RotatedBy(Leg1FRot), DrawC, new Vector3(Leg1FL, Leg1FT, 0)),

                //绘制甲虫第二条腿

                new Vertex2D(DLeg2MCen + LegRT.RotatedBy(Leg2MRot), DrawC, new Vector3(Leg2MR, Leg2MT, 0)),
                new Vertex2D(DLeg2MCen + LegRD.RotatedBy(Leg2MRot), DrawC, new Vector3(Leg2MR, Leg2MD, 0)),
                new Vertex2D(DLeg2MCen + LegLD.RotatedBy(Leg2MRot), DrawC, new Vector3(Leg2ML, Leg2MD, 0)),

                new Vertex2D(DLeg2MCen + LegRT.RotatedBy(Leg2MRot), DrawC, new Vector3(Leg2MR, Leg2MT, 0)),
                new Vertex2D(DLeg2MCen + LegLD.RotatedBy(Leg2MRot), DrawC, new Vector3(Leg2ML, Leg2MD, 0)),
                new Vertex2D(DLeg2MCen + LegLT.RotatedBy(Leg2MRot), DrawC, new Vector3(Leg2ML, Leg2MT, 0)),

                new Vertex2D(DLeg2FCen + Leg2RT.RotatedBy(Leg2FRot), DrawC, new Vector3(Leg2FR, Leg2FT, 0)),
                new Vertex2D(DLeg2FCen + Leg2RD.RotatedBy(Leg2FRot), DrawC, new Vector3(Leg2FR, Leg2FD, 0)),
                new Vertex2D(DLeg2FCen + Leg2LD.RotatedBy(Leg2FRot), DrawC, new Vector3(Leg2FL, Leg2FD, 0)),

                new Vertex2D(DLeg2FCen + Leg2RT.RotatedBy(Leg2FRot), DrawC, new Vector3(Leg2FR, Leg2FT, 0)),
                new Vertex2D(DLeg2FCen + Leg2LD.RotatedBy(Leg2FRot), DrawC, new Vector3(Leg2FL, Leg2FD, 0)),
                new Vertex2D(DLeg2FCen + Leg2LT.RotatedBy(Leg2FRot), DrawC, new Vector3(Leg2FL, Leg2FT, 0)),

                //绘制甲虫第三条腿
                new Vertex2D(DLeg3MCen + LegRT.RotatedBy(Leg3MRot), DrawC, new Vector3(Leg3MR, Leg3MT, 0)),
                new Vertex2D(DLeg3MCen + LegRD.RotatedBy(Leg3MRot), DrawC, new Vector3(Leg3MR, Leg3MD, 0)),
                new Vertex2D(DLeg3MCen + LegLD.RotatedBy(Leg3MRot), DrawC, new Vector3(Leg3ML, Leg3MD, 0)),

                new Vertex2D(DLeg3MCen + LegRT.RotatedBy(Leg3MRot), DrawC, new Vector3(Leg3MR, Leg3MT, 0)),
                new Vertex2D(DLeg3MCen + LegLD.RotatedBy(Leg3MRot), DrawC, new Vector3(Leg3ML, Leg3MD, 0)),
                new Vertex2D(DLeg3MCen + LegLT.RotatedBy(Leg3MRot), DrawC, new Vector3(Leg3ML, Leg3MT, 0)),

                new Vertex2D(DLeg3FCen + Leg3RT.RotatedBy(Leg3FRot), DrawC, new Vector3(Leg3FR, Leg3FT, 0)),
                new Vertex2D(DLeg3FCen + Leg3RD.RotatedBy(Leg3FRot), DrawC, new Vector3(Leg3FR, Leg3FD, 0)),
                new Vertex2D(DLeg3FCen + Leg3LD.RotatedBy(Leg3FRot), DrawC, new Vector3(Leg3FL, Leg3FD, 0)),

                new Vertex2D(DLeg3FCen + Leg3RT.RotatedBy(Leg3FRot), DrawC, new Vector3(Leg3FR, Leg3FT, 0)),
                new Vertex2D(DLeg3FCen + Leg3LD.RotatedBy(Leg3FRot), DrawC, new Vector3(Leg3FL, Leg3FD, 0)),
                new Vertex2D(DLeg3FCen + Leg3LT.RotatedBy(Leg3FRot), DrawC, new Vector3(Leg3FL, Leg3FT, 0))
            };

            DrawRectangle(BodyV, Leg1BRoot, Leg1BTip, LWidth, DrawC, new Vector4(Leg1BackX, Leg1BackY,Leg1BackWidth,Leg1BackHeight));
            
            Main.graphics.GraphicsDevice.Textures[0] = tex0;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, BodyV.ToArray(), 0, BodyV.Count / 3);
        }
        /// <summary>
        /// 画矩形,Texcoord的XYZW分别是坐标和宽高0<{x y z w}<1
        /// </summary>
        /// <param name="drawList"></param>
        /// <param name="Top"></param>
        /// <param name="Bottom"></param>
        /// <param name="Width"></param>
        /// <param name="color"></param>
        /// <param name="TexCoord"></param>
        public void DrawRectangle(List<Vertex2D> drawList, Vector2 Top, Vector2 Bottom, float Width,Color color, Vector4 TexCoord)
        {
            Vector2 topToBottom = Bottom - Top;
            Vector2 normalizedAndVerticalizedTopToBottom = Utils.SafeNormalize(topToBottom, Vector2.Zero).RotatedBy(Math.PI / 0.5);

            drawList.Add(new Vertex2D(Top + normalizedAndVerticalizedTopToBottom * Width, color, new Vector3(TexCoord.XY(), 0)));
            drawList.Add(new Vertex2D(Bottom + normalizedAndVerticalizedTopToBottom * Width, color, new Vector3(TexCoord.X + TexCoord.Z, TexCoord.Y, 0)));
            drawList.Add(new Vertex2D(Top - normalizedAndVerticalizedTopToBottom * Width, color, new Vector3(TexCoord.X, TexCoord.Y + TexCoord.W, 0)));

            drawList.Add(new Vertex2D(Bottom + normalizedAndVerticalizedTopToBottom * Width, color, new Vector3(TexCoord.X + TexCoord.Z, TexCoord.Y, 0)));
            drawList.Add(new Vertex2D(Bottom - normalizedAndVerticalizedTopToBottom * Width, color, new Vector3(TexCoord.X + TexCoord.Z, TexCoord.Y + TexCoord.W, 0)));
            drawList.Add(new Vertex2D(Top - normalizedAndVerticalizedTopToBottom * Width, color, new Vector3(TexCoord.X + TexCoord.Z, TexCoord.Y, 0)));
        }
        public override void HitEffect(int hitDirection, double damage)
        {
        }
    }
}
