using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Melee
{
    class World : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            //Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        float[] OldRotation = new float[60];
        Vector2[] OldMouseWorld = new Vector2[60];
        Vector2[] OldMouseWorldII = new Vector2[60];
        Vector2[] OldMouseWorldIII = new Vector2[120];
        Vector2[] OldMouseWorldIV = new Vector2[120];
        public static Vector2[,] OldMouseWorldV = new Vector2[259, 240];
        public static float[,] OldWidth = new float[259, 240];
        public static float[,] OldWidthII = new float[259, 240];
        public static float[,] OldWidthIII = new float[259, 240];
        public static float[,] OldWidthIV = new float[259, 240];
        public static float[,] OldWidthV = new float[259, 240];
        int SecFrame = 8;
        Vector2 CatchPos = Vector2.Zero;
        int Mode = 0;
        int ModeRightClick = 0;
        Vector2 CrackPoint;
        int timer = 0;
        int Aimtimer = 0;
        public override void AI()
        {
            timer++;
            Player player = Main.player[Projectile.owner];
            if (Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.World>())
            {
                Projectile.timeLeft = 60;
            }
            if (Main.mouseRight)
            {
                ModeRightClick = 3;
            }
            if (ModeRightClick > 0)
            {
                ModeRightClick--;
            }
            if (ModeRightClick == 1)
            {
                if (Mode == 0)
                {
                    Mode = 1;
                    CrackPoint = Main.MouseWorld + new Vector2(Main.rand.NextFloat(150f, 600f), 0).RotatedByRandom(6.283);
                    Main.NewText("切换为斩切模式", 42, 161, 76);
                }
                else
                {
                    Mode = 0;
                    Main.NewText("切换为追随模式", 42, 161, 76);
                }
            }
            Projectile.Center = player.Center;

            if (CatchPos == Vector2.Zero)
            {
                CatchPos = Main.MouseWorld;
            }
            CatchPos = Main.MouseWorld * 0.75f + CatchPos * 0.25f;
            if (Mode == 1 && timer >= Aimtimer && Main.mouseLeft)
            {
                timer = 0;
                Aimtimer = Main.rand.Next(3, 9);
                CrackPoint = Main.MouseWorld + Vector2.Normalize(Main.MouseWorld - CrackPoint).RotatedBy(Main.rand.NextFloat(Main.rand.NextFloat(Main.rand.NextFloat(-0.5f, -0.2f), 0.2f), 0.5f)) * Main.rand.NextFloat(420f, 600f);
            }
            OldMouseWorld[0] = Main.MouseWorld;//记录数据模板,这里记录鼠标坐标
            if (Mode == 1)
            {
                OldMouseWorld[0] = CrackPoint;
            }
            for (int f = OldMouseWorld.Length - 1; f > 0; f--)
            {
                OldMouseWorld[f] = OldMouseWorld[f - 1];
            }
            if (OldMouseWorld[2] != Vector2.Zero)//第二帧需要特殊处理
            {
                OldMouseWorldII[0] = (Main.MouseWorld + OldMouseWorld[2]) / 2f;
                if (Mode == 1)
                {
                    OldMouseWorldII[0] = (CrackPoint + OldMouseWorld[2]) / 2f;
                }
            }//记录数据模板,这里取中点,为了平滑
            for (int f = OldMouseWorldII.Length - 1; f > 0; f--)
            {
                OldMouseWorldII[f] = OldMouseWorldII[f - 1];
            }

            OldMouseWorldIII[0] = OldMouseWorldII[0];//记录数据模板,这里记录鼠标坐标
            if (OldMouseWorldII[2] != Vector2.Zero)//第二帧需要特殊处理
            {
                OldMouseWorldIII[1] = (OldMouseWorldII[1] + OldMouseWorldII[2] + OldMouseWorld[2]) / 3f;//记录数据模板,这里取重心,为了平滑
            }
            for (int f = OldMouseWorldIII.Length - 1; f > 1; f -= 2)
            {
                OldMouseWorldIII[f] = OldMouseWorldIII[f - 2];
                OldMouseWorldIII[f - 1] = OldMouseWorldIII[f - 3];
            }

            if (OldMouseWorldIII[1] != Vector2.Zero)//第二帧需要特殊处理
            {
                OldMouseWorldIV[0] = (Main.MouseWorld + OldMouseWorldIII[1]) / 2f;//记录数据模板,这里在取了一次重心的重心曲线上取中点,再次平滑
                if (Mode == 1)
                {
                    OldMouseWorldIV[0] = (CrackPoint + OldMouseWorldIII[1]) / 2f;
                }
            }
            for (int f = OldMouseWorldIV.Length - 1; f > 0; f--)
            {
                OldMouseWorldIV[f] = OldMouseWorldIV[f - 1];
            }
            if (OldMouseWorldIV[2] != Vector2.Zero)//第二帧需要特殊处理
            {
                OldMouseWorldV[Projectile.owner, 0] = OldMouseWorldIV[0];//记录数据模板,这里记录鼠标坐标
                OldMouseWorldV[Projectile.owner, 1] = (OldMouseWorldIV[1] + OldMouseWorldIV[2] + OldMouseWorldIII[1]) / 3f;//记录数据模板,这里取重心,为了平滑
            }
            for (int f = 239; f > 1; f -= 2)
            {
                OldMouseWorldV[Projectile.owner, f] = OldMouseWorldV[Projectile.owner, f - 2];
                OldMouseWorldV[Projectile.owner, f - 1] = OldMouseWorldV[Projectile.owner, f - 3];
            }

            Vector2 ArrowToMouse = Main.MouseWorld - player.Center;
            if (Mode == 1)
            {
                ArrowToMouse = CrackPoint - player.Center;
            }
            OldRotation[0] = (float)(Math.Atan2(ArrowToMouse.Y, ArrowToMouse.X));//记录数据模板,这里记录旋转角度
            for (int f = OldRotation.Length - 1; f > 0; f--)
            {
                OldRotation[f] = OldRotation[f - 1];
            }

            OldWidth[Projectile.owner, 0] = Math.Clamp((OldMouseWorldV[Projectile.owner, 2] - OldMouseWorldV[Projectile.owner, 4]).Length() / 10f - 2, 0, 72);
            OldWidth[Projectile.owner, 1] = Math.Clamp((OldMouseWorldV[Projectile.owner, 3] - OldMouseWorldV[Projectile.owner, 5]).Length() / 10f - 2, 0, 72);//记录数据模板,这里记录撕裂宽度(由鼠标速度决定）
            if (SecFrame > 0)//第二帧需要特殊处理
            {
                OldWidth[Projectile.owner, 0] = 0;
                OldWidth[Projectile.owner, 1] = 0;
                SecFrame--;
            }
            /*for (int f = OldWidth.Length - 1; f > 0; f--)
            {
                OldWidth[Projectile.owner,f] = OldWidth[Projectile.owner,f - 1] * 0.97f;
            }*/
            for (int f = 239; f > 1; f -= 2)
            {
                OldWidth[Projectile.owner, f] = OldWidth[Projectile.owner, f - 2] * 0.88f;
                OldWidth[Projectile.owner, f - 1] = OldWidth[Projectile.owner, f - 3] * 0.88f;
            }

            OldWidthII[Projectile.owner, 0] = Math.Clamp((OldMouseWorldV[Projectile.owner, 2] - OldMouseWorldV[Projectile.owner, 4]).Length() / 19f - 0.7f, 0, 12);
            OldWidthII[Projectile.owner, 1] = Math.Clamp((OldMouseWorldV[Projectile.owner, 3] - OldMouseWorldV[Projectile.owner, 5]).Length() / 19f - 0.7f, 0, 12);//记录数据模板,这里记录撕裂宽度(由鼠标速度决定）
            if (SecFrame > 0)//第二帧需要特殊处理
            {
                OldWidthII[Projectile.owner, 0] = 0;
                OldWidthII[Projectile.owner, 1] = 0;
                SecFrame--;
            }
            /*for (int f = OldWidthII.Length - 1; f > 0; f--)
            {
                OldWidthII[Projectile.owner,f] = OldWidthII[Projectile.owner,f - 1] * 0.97f;
            }*/
            for (int f = 239; f > 1; f -= 2)
            {
                OldWidthII[Projectile.owner, f] = OldWidthII[Projectile.owner, f - 2] * 0.88f;
                OldWidthII[Projectile.owner, f - 1] = OldWidthII[Projectile.owner, f - 3] * 0.88f;
            }

            OldWidthIII[Projectile.owner, 0] = Math.Clamp((OldMouseWorldV[Projectile.owner, 2] - OldMouseWorldV[Projectile.owner, 4]).Length() / 6f - 6f, 0, 32);
            OldWidthIII[Projectile.owner, 1] = Math.Clamp((OldMouseWorldV[Projectile.owner, 3] - OldMouseWorldV[Projectile.owner, 5]).Length() / 6f - 6f, 0, 32);//记录数据模板,这里记录撕裂宽度(由鼠标速度决定）
            if (SecFrame > 0)//第二帧需要特殊处理
            {
                OldWidthIII[Projectile.owner, 0] = 0;
                OldWidthIII[Projectile.owner, 1] = 0;
                SecFrame--;
            }
            /*for (int f = OldWidthIII.Length - 1; f > 0; f--)
            {
                OldWidthIII[Projectile.owner,f] = OldWidthIII[Projectile.owner,f - 1] * 0.97f;
            }*/
            for (int f = 239; f > 1; f -= 2)
            {
                OldWidthIII[Projectile.owner, f] = OldWidthIII[Projectile.owner, f - 2] * 0.88f;
                OldWidthIII[Projectile.owner, f - 1] = OldWidthIII[Projectile.owner, f - 3] * 0.88f;
            }

            OldWidthIV[Projectile.owner, 0] = Math.Clamp((OldMouseWorldV[Projectile.owner, 2] - OldMouseWorldV[Projectile.owner, 4]).Length() / 8f - 7f, 0, 22);
            OldWidthIV[Projectile.owner, 1] = Math.Clamp((OldMouseWorldV[Projectile.owner, 3] - OldMouseWorldV[Projectile.owner, 5]).Length() / 8f - 7f, 0, 22);//记录数据模板,这里记录撕裂宽度(由鼠标速度决定）
            if (SecFrame > 0)//第二帧需要特殊处理
            {
                OldWidthIV[Projectile.owner, 0] = 0;
                OldWidthIV[Projectile.owner, 1] = 0;
                SecFrame--;
            }
            /*for (int f = OldWidthIV.Length - 1; f > 0; f--)
            {
                OldWidthIV[Projectile.owner,f] = OldWidthIV[Projectile.owner,f - 1] * 0.97f;
            }*/
            for (int f = 239; f > 1; f -= 2)
            {
                OldWidthIV[Projectile.owner, f] = OldWidthIV[Projectile.owner, f - 2] * 0.88f;
                OldWidthIV[Projectile.owner, f - 1] = OldWidthIV[Projectile.owner, f - 3] * 0.88f;
            }

            OldWidthV[Projectile.owner, 0] = Math.Clamp((OldMouseWorldV[Projectile.owner, 2] - OldMouseWorldV[Projectile.owner, 4]).Length() / 11f - 6f, 0, 32);
            OldWidthV[Projectile.owner, 1] = Math.Clamp((OldMouseWorldV[Projectile.owner, 3] - OldMouseWorldV[Projectile.owner, 5]).Length() / 11f - 6f, 0, 32);//记录数据模板,这里记录撕裂宽度(由鼠标速度决定）
            if (SecFrame > 0)//第二帧需要特殊处理
            {
                OldWidthV[Projectile.owner, 0] = 0;
                OldWidthV[Projectile.owner, 1] = 0;
                SecFrame--;
            }
            /*for (int f = OldWidthV.Length - 1; f > 0; f--)
            {
                OldWidthV[Projectile.owner,f] = OldWidthV[Projectile.owner,f - 1] * 0.97f;
            }*/
            for (int f = 239; f > 1; f -= 2)
            {
                OldWidthV[Projectile.owner, f] = OldWidthV[Projectile.owner, f - 2] * 0.88f;
                OldWidthV[Projectile.owner, f - 1] = OldWidthV[Projectile.owner, f - 3] * 0.88f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
        int[] HasNOHitV = new int[200];
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        //int TrueL = 0;
        float Omega;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D MainKnife = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/World").Value;
            List<Vertex2D> Knife = new List<Vertex2D>();
            float KnifeLength = 180;
            float StartLength = -90;
            Color lightC = Lighting.GetColor((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f));
            float Kcolor = Math.Clamp((7 - (OldWidth[player.whoAmI, 1] + OldWidth[player.whoAmI, 2] + OldWidth[player.whoAmI, 3]) / 3f) / 12f, 0, 1);
            var LineRot = OldMouseWorldV[Projectile.owner, 1] - OldMouseWorldV[Projectile.owner, 2];
            if (Kcolor < 0.4f)
            {
                Projectile.rotation = (float)(Math.Atan2(LineRot.Y, LineRot.X) + Math.PI) * 0.25f + Projectile.rotation * 0.75f;
                Omega = 0;
            }
            else
            {
                if (!Main.gamePaused)
                {
                    Projectile.rotation += Omega;
                    if (Omega < 0.02f)
                    {
                        Omega += 0.002f;
                    }
                }
            }

            float Kvec = Math.Clamp((60 - Projectile.timeLeft) / 30f, 0, 1);
            Kvec = (float)Math.Sqrt(Kvec);
            if (Main.mouseLeft)
            {
                Kvec = 0;
            }
            else
            {
                Kcolor = 1;
            }
            if (Projectile.timeLeft < 15)
            {
                Kcolor *= (Projectile.timeLeft * Projectile.timeLeft / 225f);
            }
            float R0 = lightC.R / 255f * Kcolor;
            float G0 = lightC.G / 255f * Kcolor;
            float B0 = lightC.B / 255f * Kcolor;
            float A0 = lightC.A / 255f * Kcolor;
            float AimRot = 2f;
            if (player.direction == -1)
            {
                AimRot = 1.14f;
            }

            Color trueC = new Color(R0, G0, B0, A0);
            Vector2 DrawPos = OldMouseWorldV[player.whoAmI, 1] * (1 - Kvec) + (player.Center + new Vector2(-24 * player.direction, 0)) * (Kvec);//滑动到玩家
            float TrueRot = Projectile.rotation * (1 - Kvec) + AimRot * Kvec;
            Knife.Add(new Vertex2D(DrawPos + new Vector2(StartLength, 0).RotatedBy(TrueRot) - Main.screenPosition, trueC, new Vector3(0, 1, 0)));
            Knife.Add(new Vertex2D(DrawPos + new Vector2(StartLength + KnifeLength, 0).RotatedBy(TrueRot) - Main.screenPosition, trueC, new Vector3(1, 0, 0)));
            Knife.Add(new Vertex2D(DrawPos + new Vector2(StartLength + KnifeLength / 2f, KnifeLength / 2f).RotatedBy(TrueRot) - Main.screenPosition, trueC, new Vector3(1, 1, 0)));

            Knife.Add(new Vertex2D(DrawPos + new Vector2(StartLength, 0).RotatedBy(TrueRot) - Main.screenPosition, trueC, new Vector3(0, 1, 0)));
            Knife.Add(new Vertex2D(DrawPos + new Vector2(StartLength + KnifeLength / 2f, -KnifeLength / 2f).RotatedBy(TrueRot) - Main.screenPosition, trueC, new Vector3(0, 0, 0)));
            Knife.Add(new Vertex2D(DrawPos + new Vector2(StartLength + KnifeLength, 0).RotatedBy(TrueRot) - Main.screenPosition, trueC, new Vector3(1, 0, 0)));


            Main.graphics.GraphicsDevice.Textures[0] = MainKnife;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Knife.ToArray(), 0, Knife.Count / 3);

            /*exture2D KnifeLight = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/WorldLight").Value;
            List<Vertex2D> Light = new List<Vertex2D>();
            for(int d = 2;d < 60;d++)
            {
                float Strength = (float)((OldRotation[d] - OldRotation[d - 1] + Math.PI * 20d) % (Math.PI * 2d)) / 16f;
                Color c0 = new Color(Strength, Strength, Strength,0);
                if(OldRotation[d] > OldRotation[d - 1])
                {
                    Light.Add(new Vertex2D(player.Center - Main.screenPosition, c0, new Vector3(0, 1, 0)));
                    Light.Add(new Vertex2D(player.Center + new Vector2(StartLength + KnifeLength, 0).RotatedBy(OldRotation[d - 1]) - Main.screenPosition, c0, new Vector3((d - 1) / 60f, 0, 0)));
                    Light.Add(new Vertex2D(player.Center + new Vector2(StartLength + KnifeLength, 0).RotatedBy(OldRotation[d]) - Main.screenPosition, c0, new Vector3((d) / 60f, 0, 0)));
                }
                else
                {
                    Light.Add(new Vertex2D(player.Center - Main.screenPosition, c0, new Vector3(0, 1, 0)));
                    Light.Add(new Vertex2D(player.Center + new Vector2(StartLength + KnifeLength, 0).RotatedBy(OldRotation[d]) - Main.screenPosition, c0, new Vector3((d) / 60f, 0, 0)));
                    Light.Add(new Vertex2D(player.Center + new Vector2(StartLength + KnifeLength, 0).RotatedBy(OldRotation[d - 1]) - Main.screenPosition, c0, new Vector3((d - 1) / 60f, 0, 0)));
                }
            }


            Main.graphics.GraphicsDevice.Textures[0] = KnifeLight;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Light.ToArray(), 0, Light.Count / 3);*/


            if (Main.mouseLeft)
            {
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(OldRotation[1] - Math.PI / 2d));
            }

            for (int k = 0; k < 5; ++k)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<Vertex2D> bars = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                //TrueL = 0;
                for (int i = 1; i < 240; ++i)
                {
                    if (OldMouseWorldV[Projectile.owner, i] == Vector2.Zero)
                        break;
                    //TrueL++;
                }
                float colorS = 254f / 255f;
                if (k > 0)
                {
                    colorS = 54f / 255f;
                }
                for (int i = 1; i < 240; ++i)
                {
                    float width = OldWidth[Projectile.owner, i] * Math.Clamp((i - 1) / 4f, 0, 1);
                    if (k == 1)
                    {
                        width = OldWidthII[Projectile.owner, i] * Math.Clamp((i - 1) / 4f, 0, 1);
                    }
                    if (k == 2)
                    {
                        width = OldWidthIII[Projectile.owner, i] * Math.Clamp((i - 1) / 4f, 0, 1);
                    }
                    if (k == 3)
                    {
                        width = OldWidthIV[Projectile.owner, i] * Math.Clamp((i - 1) / 4f, 0, 1);
                    }
                    if (k == 4)
                    {
                        width = OldWidthV[Projectile.owner, i] * Math.Clamp((i - 1) / 4f, 0, 1);
                    }
                    width *= Projectile.timeLeft / 60f;
                    if (OldMouseWorldV[Projectile.owner, i] == Vector2.Zero)
                        break;
                    var normalDir = OldMouseWorldV[Projectile.owner, i - 1] - OldMouseWorldV[Projectile.owner, i];

                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / 108f;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    Vector2 zero = Vector2.Zero;
                    if (k > 0)
                    {
                        zero = new Vector2(8, 6).RotatedBy(k * Math.PI / 2.5d) * Math.Clamp(i / 15f, 0, 1);
                    }
                    else
                    {
                        if ((OldMouseWorldV[Projectile.owner, i] - OldMouseWorldV[Projectile.owner, i - 1]).Length() > 5)
                        {
                            Vector2 Vpi = Vector2.Normalize((OldMouseWorldV[Projectile.owner, i] - OldMouseWorldV[Projectile.owner, i - 1])) * 5;
                            for (int j = 0; j < (OldMouseWorldV[Projectile.owner, i] - OldMouseWorldV[Projectile.owner, i - 1]).Length() / 5; j++)
                            {
                                Lighting.AddLight(OldMouseWorldV[Projectile.owner, i - 1] + Vpi * j, (float)(255 - Projectile.alpha) * 0.04f / 50f * (1 - Math.Clamp(factor, 0, 1)) * width / 24f, (float)(255 - Projectile.alpha) * 0.14f / 50f * (1 - Math.Clamp(factor, 0, 1)) * width / 24f, 0);
                            }
                        }
                    }
                    float TrueC = colorS * (1 - Math.Clamp(factor * 0.2f, 0, 1)) * 1.9f;
                    bars.Add(new Vertex2D(OldMouseWorldV[Projectile.owner, i] + zero + normalDir * width - Main.screenPosition, new Color(TrueC, TrueC, TrueC, Math.Clamp(0.45f - TrueC, 0, 1)), new Vector3(Math.Clamp(factor, 0, 1), 1, w)));
                    bars.Add(new Vertex2D(OldMouseWorldV[Projectile.owner, i] + zero + normalDir * -width - Main.screenPosition, new Color(TrueC, TrueC, TrueC, Math.Clamp(0.45f - TrueC, 0, 1)), new Vector3(Math.Clamp(factor, 0, 1), 0, w)));
                    if (k == 0)
                    {
                        if (i == 4)
                        {
                            for (int v = 0; v < 200; v++)
                            {
                                if (Math.Abs(Main.npc[v].Center.Y - OldMouseWorldV[Projectile.owner, i].Y) < Main.npc[v].height + 70 + width * 2 && Math.Abs(Main.npc[v].Center.X - OldMouseWorldV[Projectile.owner, i].X) < Main.npc[v].width + 70 + width * 2 && !Main.npc[v].dontTakeDamage && !Main.npc[v].friendly && Main.npc[v].active && HasNOHitV[v] == 0 && !Main.gamePaused)
                                {
                                    HasNOHitV[v] = 8;
                                    float Damk = Math.Clamp(width / 12f, 0.1f, 15f);
                                    if (Mode == 0)
                                    {
                                        Damk = Math.Clamp(width / 6f, 0.1f, 15f);
                                    }
                                    int Dam = (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f) * Damk);
                                    bool Crit = Main.rand.Next(100) < player.GetCritChance(DamageClass.Generic) + player.GetCritChance(DamageClass.Melee) + 15 + width * 1.5f;
                                    Main.npc[v].StrikeNPC(Dam, 2 * width / 72f, Math.Sign(Projectile.velocity.X), Crit);
                                    if (Crit)
                                    {
                                        MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();
                                        player.addDPS((int)((Dam - Main.npc[v].defDefense) * (0.6 + myplayer.CritDamage)));
                                    }
                                    else
                                    {
                                        player.addDPS((Dam - Main.npc[v].defDefense));
                                    }
                                    if (Mode == 1)
                                    {
                                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Main.npc[v].Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.WorldHit>(), 0, Projectile.knockBack, Projectile.owner, Math.Clamp(width / 12f, 0, 1f), 0f);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Main.npc[v].Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.WorldHit>(), 0, Projectile.knockBack, Projectile.owner, Math.Clamp(width / 4f, 0, 1f), 0f);
                                    }
                                }
                                if (HasNOHitV[v] > 0)
                                {
                                    HasNOHitV[v]--;
                                }
                            }
                        }
                        barsII.Add(new Vertex2D(OldMouseWorldV[Projectile.owner, i] + zero + normalDir * Math.Clamp(width * 1.6f, 0, 72) - Main.screenPosition, new Color(255, 255, 255, 255), new Vector3(Math.Clamp(factor, 0, 1), 1, w)));
                        barsII.Add(new Vertex2D(OldMouseWorldV[Projectile.owner, i] + zero + normalDir * -Math.Clamp(width * 1.6f, 0, 72) - Main.screenPosition, new Color(255, 255, 255, 255), new Vector3(Math.Clamp(factor, 0, 1), 0, w)));
                    }
                }
                List<Vertex2D> Vx = new List<Vertex2D>();
                if (bars.Count > 2)
                {
                    Vx.Add(bars[0]);
                    var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(colorS, colorS, colorS, 0), new Vector3(0, 0.5f, 1));
                    Vx.Add(bars[1]);
                    Vx.Add(vertex);
                    for (int i = 0; i < bars.Count - 2; i += 2)
                    {
                        Vx.Add(bars[i]);
                        Vx.Add(bars[i + 2]);
                        Vx.Add(bars[i + 1]);

                        Vx.Add(bars[i + 1]);
                        Vx.Add(bars[i + 2]);
                        Vx.Add(bars[i + 3]);
                    }
                }

                if (k == 0)
                {
                    List<Vertex2D> VxII = new List<Vertex2D>();
                    if (barsII.Count > 2)
                    {
                        VxII.Add(barsII[0]);
                        var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(colorS, colorS, colorS, 0), new Vector3(0, 0.5f, 1));
                        VxII.Add(barsII[1]);
                        VxII.Add(vertex);
                        for (int i = 0; i < barsII.Count - 2; i += 2)
                        {
                            VxII.Add(barsII[i]);
                            VxII.Add(barsII[i + 2]);
                            VxII.Add(barsII[i + 1]);

                            VxII.Add(barsII[i + 1]);
                            VxII.Add(barsII[i + 2]);
                            VxII.Add(barsII[i + 3]);
                        }
                    }
                    Texture2D t0 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapShade").Value;
                    Main.graphics.GraphicsDevice.Textures[0] = t0;//GlodenBloodScaleMirror
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
                }
                Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/World").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }
        }
    }
}
