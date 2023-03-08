using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.FoodModule.Dusts;
using System.Threading;

namespace Everglow.Sources.Modules.FoodModule.Projectiles
{
    public class FryingPan : MeleeProj
    {
        public override void SetDef()
        {
            Projectile.width = 60;

            Projectile.height = 60;

            maxAttackType = 0;//循环攻击方式的总数

            trailLength = 10;//拖尾的长度

            shadertype = "Trail";

            drawScaleFactor = 1f;

            disFromPlayer = 0;

            Projectile.height = 20;//判定区域的宽度，默认为15

            Projectile.scale = 1f;//总大小，有需要时可以使用

            Projectile.timeLeft = 3000;


            /*
             * 若要增加剑的宽度，需要增大scale并在Attack()函数中降低mainVec的长度
             */
        }

        //一定程度上决定拖尾的亮度/不透明度
        public override float TrailAlpha(float factor)
        {
            if (attackType == 0)
            {
                return base.TrailAlpha(factor) * 1.2f;
            }
            else
            {
                return base.TrailAlpha(factor) * 0.9f;
            }
        }
        public override string TrailShapeTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/Melee";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/FoodModule/Images/PanColor";
        }

        //拖尾的混合模式，通常使用NonPremultiplied（暗）或者Additive（亮）
        public override BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
        {
            base.DrawSelf(spriteBatch, lightColor, 70, 40, 1f, "", 0.666667);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //调整各个攻击方式的伤害倍率等等
            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();

            if (attackType == 100)
            {
                damage = (int)(damage * 1.85);
                knockback *= 2;
                Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12, 150)).RotatedByRandom(6.283);
            }
        }

        //攻击方式编辑

        internal bool state1 = false;
        internal bool state2 = false;
        internal bool state3 = false;
        public override void Attack()
        {
            useTrail = true;
            if (attackType == 100)//右键长按蓄力斩的写法。因为不在循环内，所以这个type数值可以随便写，由Item切换到这个attackType
            {
                int chargeTime1 = 30;
                int chargeTime2 = 60;
                int chargeTime3 = 90;

                if (timer < 10000)//蓄力中
                {
                    useTrail = false;
                    LockPlayerDir(Player);

                    Projectile.ai[0] = GetAngToMouse();//获取往鼠标的方向
                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(77, targetRot, 0f, Projectile.ai[0], 1000), 0.2f);


                    Projectile.rotation = mainVec.ToRotation();

                    //向内的粒子效果
                    for (int x = 0; x < 2; x++)
                    {
                        Vector2 r = Main.rand.NextVector2Unit();
                        float dis1 = MathHelper.Clamp(chargeTime1 - timer, 0, chargeTime1) * 4;
                        float dis2 = MathHelper.Clamp(chargeTime2 - timer, 0, chargeTime2) * 4;
                        float dis3 = MathHelper.Clamp(chargeTime3 - timer, 0, chargeTime3) * 4;
                        Dust d1 = Dust.NewDustDirect(Projectile.Center + r * dis1, 10, 10, ModContent.DustType<BlackPanDust>(), 0, 0, 100, new Color(0, Projectile.owner, 20), 0.8f);
                        d1.alpha = 255;

                        Dust d2 = Dust.NewDustDirect(Projectile.Center + r * dis2, 10, 10, ModContent.DustType<BlackPanDust2>(), 0, 0, 100, new Color(0, Projectile.owner, 20), 2f);
                        d2.alpha = 255;

                        Dust d3 = Dust.NewDustDirect(Projectile.Center + r * dis3, 10, 10, ModContent.DustType<BlackPanDust3>(), 0, 0, 100, new Color(0, Projectile.owner, 20), 3f);
                        d3.alpha = 255;
                    }

                }
                SoundStyle sound = SoundID.Item4;
                sound.Volume *= 0.4f;

                if (timer == chargeTime1)//蓄力完成时
                {
                    //播放音效。
                    SoundEngine.PlaySound(sound, Projectile.Center);
                }
                if (timer == chargeTime2)
                {
                    SoundEngine.PlaySound(sound, Projectile.Center);
                }
                if (timer == chargeTime3)
                {
                    SoundEngine.PlaySound(sound, Projectile.Center);
                }

                if (!Player.controlUseTile && timer >= chargeTime1 && timer < 10000)//松开右键，且蓄力已经完成时
                {
                    //进入攻击状态
                    state1 = true;

                    if (timer >= chargeTime2)
                    {
                        state2 = true;
                    }
                    if (timer >= chargeTime3)
                    {
                        state3 = true;
                    }
                    timer = 10000;
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                }

                if (timer >= 10000)//开始挥动攻击
                {
                    isAttacking = true;

                    if (state1 == true)
                    {
                        if (timer < 10015)
                        {
                            isAttacking = true;
                            mainVec = Vector2Elipse(77, Projectile.rotation, 0f, Projectile.ai[0]);
                            Projectile.rotation += Projectile.spriteDirection * 0.35f;
                        }
                        if (!state2)
                        {
                            if (timer >= 10020)
                            {
                                Projectile.friendly = false;
                                state1 = false;
                                End();
                            }
                        }
                    }

                    if (state2 == true)
                    {
                        if (timer > 10015 && timer < 10020)
                        {
                            useTrail = false;
                        }
                        if (timer == 10020)
                        {
                            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                        }
                        if (timer >= 10020 && timer < 10035)
                        {
                            useTrail = true;
                            isAttacking = true;
                            mainVec = Vector2Elipse(77, Projectile.rotation, 0f, Projectile.ai[0]);
                            Projectile.rotation += Projectile.spriteDirection * -0.35f;
                        }
                        if (!state3)
                        {
                            if (timer >= 10040)
                            {
                                Projectile.friendly = false;
                                state1 = false;
                                state2 = false;
                                End();
                            }
                        }
                    }

                    if (state3 == true)
                    {
                        if (timer > 10040 && timer < 10050)
                        {
                            useTrail = false;
                        }
                        if (timer == 10050)
                        {
                            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                        }
                        if (timer >= 10050 && timer < 10075)
                        {
                            useTrail = true;
                            isAttacking = true;
                            Projectile.rotation += Projectile.spriteDirection * 0.25f;
                            mainVec = Vector2Elipse(120, Projectile.rotation, -1.2f, Projectile.ai[0]);
                        }
                        if (timer >= 10080)
                        {
                            Projectile.friendly = false;
                            state1 = false;
                            state2 = false;
                            state3 = false;
                            End();
                        }
                    }
                }
            }
            if (isAttacking)
            {
                //攻击时的粒子之类的
            }
        }
        private void PlayerAnimation(Player player)
        {
            Vector2 vToMouse = Main.MouseWorld - player.Top;
            int overTimer = timer - 10000;
            float YDevideX = MathF.Abs(vToMouse.X) / (vToMouse.Length() + 0.001f);
            if (timer > 10000)
            {
                if (overTimer < 24)
                {
                    float BodyRotation = (float)(Math.Sin((overTimer) / 40d * Math.PI)) * 0.2f * player.direction * player.gravDir * YDevideX;
                    player.fullRotation = BodyRotation;
                    player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                    player.legRotation = -BodyRotation;
                    player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
                }
                else if(overTimer < 50)
                {
                    float BodyRotation = (float)(Math.Sin((overTimer - 30) / 40d * Math.PI)) * -0.2f * player.direction * player.gravDir * YDevideX;
                    player.fullRotation = BodyRotation;
                    player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                    player.legRotation = -BodyRotation;
                    player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
                }
                else
                {
                    float BodyRotation = (float)(Math.Sin((overTimer) / 40d * Math.PI)) * -0.6f * player.direction * player.gravDir * YDevideX;
                    player.fullRotation = BodyRotation;
                    player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                    player.legRotation = -BodyRotation;
                    player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
                }
            }
            else
            {
                float BodyRotation = (float)(Math.Sin(-Math.Min(timer * 0.5f, 45) / 90d * Math.PI)) * 0.3f * player.direction * player.gravDir * YDevideX;
                player.fullRotation = BodyRotation;
                player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                player.legRotation = -BodyRotation;
                player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
            }
        }

        int hittimes = 49;
        public override void AI()
        {
            if (attackType == 0)
            {
                Projectile.tileCollide = true;
                Projectile.ignoreWater = false;
                isAttacking = true;
                longHandle = true;
                Player player = Main.player[Projectile.owner];
                Projectile.Center += Projectile.velocity;
                useTrail = true;
                if (timer % 10 == 0)
                {
                    AttSound(SoundID.Item1);
                }
                if (hittimes > 0)
                {
                    foreach (Projectile proj in Main.projectile)
                    {
                        if (proj.active && proj != Projectile && proj.GetGlobalProjectile<Canhitproj>().Canhit && proj.penetrate != -1)
                        {
                            if ((Projectile.Center - proj.Center).Length() <= (float)Math.Sqrt(proj.width ^ 2 + proj.height ^ 2) * proj.scale / 2 + 42 * Projectile.scale)
                            {
                                Vector2 v1 = proj.velocity;
                                Vector2 v2 = Projectile.velocity;

                                float m1 = MathF.Pow(proj.width * proj.height, 1.5f) * proj.knockBack * proj.scale;
                                float m2 = MathF.Pow(Projectile.width * Projectile.height, 1.5f) * Projectile.knockBack * Projectile.scale / 50;

                                Vector2 newvelocity1 = (v1 * (m1 - m2) + 2 * m2 * v2) / (m1 + m2);
                                Vector2 newvelocity2 = (v2 * (m2 - m1) + 2 * m1 * v1) / (m1 + m2);
                                Vector2 dustvelocity = newvelocity1 - v1;

                                if (newvelocity1.Length() <= v1.Length())
                                {
                                    proj.velocity = Vector2.Normalize(newvelocity1) * v1.Length();
                                }
                                else
                                {
                                    proj.velocity = newvelocity1;
                                }
                                Projectile.velocity = newvelocity2;//这里是质心动量守恒的弹性碰撞

                                Projectile.NewProjectile(null, Projectile.Center - (Vector2.Normalize(dustvelocity).RotatedBy(Math.PI / 4) * 32), Vector2.Normalize(dustvelocity) * 15, ProjectileID.Spark, Projectile.damage, Projectile.knockBack, player.whoAmI);

                                int dust1 = Dust.NewDust(Projectile.Center - (Vector2.Normalize(dustvelocity).RotatedBy(Math.PI / 4) * 32), 0, 0, ModContent.DustType<MothSmog>(), Vector2.Normalize(dustvelocity).X * 5, Vector2.Normalize(dustvelocity).Y * 10, 100, default, Main.rand.NextFloat(3.7f, 5.1f));
                                Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50);
                                Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);

                                SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
                                proj.GetGlobalProjectile<Canhitproj>().Canhit = false;
                                hittimes--;
                                Projectile.timeLeft = 2880;
                            }
                        }
                    }
                }
                Projectile.rotation += 0.3f * Projectile.spriteDirection;
                mainVec = Projectile.rotation.ToRotationVector2() * 38;
                if (Projectile.timeLeft <= 2880)
                {
                    Projectile.velocity = Projectile.velocity * 0.8f + Vector2.Normalize(player.Center - Projectile.velocity - Projectile.Center) * 12.5f;
                    Projectile.tileCollide = false;
                    Projectile.rotation += 0.3f * Projectile.spriteDirection;
                    mainVec = Projectile.rotation.ToRotationVector2() * 38;
                    if ((player.Center - Projectile.Center).Length() < 32)
                    {
                        Projectile.timeLeft = 0;
                    }
                }

                if (useTrail)
                {
                    trailVecs.Enqueue(mainVec);
                    if (trailVecs.Count > trailLength)
                    {
                        trailVecs.Dequeue();
                    }
                }
                else//清空！
                {
                    trailVecs.Clear();
                }
            }
            else
            {
                PlayerAnimation(Player);
                base.AI();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/FoodModule/Sounds/Pan1").WithVolumeScale(1), Projectile.Center);
            if (attackType == 0)
            {
                if (Projectile.timeLeft > 2880)
                {
                    if (Projectile.ai[1] != 0)
                    {
                        return;
                    }
                    Projectile.velocity.X = Projectile.velocity.X * -1f;
                    Projectile.velocity.Y = Projectile.velocity.Y * -1f;
                    Projectile.timeLeft = 2880;
                }
            }

            return;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/FoodModule/Sounds/Pan1").WithVolumeScale(1), Projectile.Center);
            if (attackType == 0)
            {
                if (Projectile.timeLeft > 2880)
                {
                    if (Projectile.ai[1] != 0)
                    {
                        return;
                    }
                    Projectile.velocity.X = Projectile.velocity.X * -1f;
                    Projectile.velocity.Y = Projectile.velocity.Y * -1f;

                    Projectile.timeLeft = 2880;
                }
            }
            return;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/FoodModule/Sounds/Pan1" ).WithVolumeScale(1), Projectile.Center);
            if (attackType == 0)
            {
                if (Projectile.timeLeft > 2880)
                {
                    if (Projectile.ai[1] != 0)
                    {
                        return true;
                    }
                    Projectile.soundDelay = 10;
                    if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                    {
                        Projectile.velocity.X = oldVelocity.X * -1f;
                    }
                    if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                    {
                        Projectile.velocity.Y = oldVelocity.Y * -1f;
                    }
                    Projectile.timeLeft = 2880;
                }
            }
            return false;
        }
    }

    public class Canhitproj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool Canhit = true;
    }
}
