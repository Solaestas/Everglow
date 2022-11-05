using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;

namespace Everglow.Sources.Modules.FoodModule.Projectiles
{
    public class FryingPan : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 1;//循环攻击方式的总数

            trailLength = 20;//拖尾的长度

            shadertype = "Trail";

            drawScaleFactor = 1f;

            disFromPlayer = 2;

           Projectile.height = 100;//判定区域的宽度，默认为15

           Projectile.scale = 1f;//总大小，有需要时可以使用


            /*
             * 若要增加剑的宽度，需要增大scale并在Attack()函数中降低mainVec的长度
             */
        }
 
        //一定程度上决定拖尾的亮度/不透明度
        public override float TrailAlpha(float factor)
        {
            return base.TrailAlpha(factor) * 1.3f;
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
            base.DrawSelf(spriteBatch, lightColor, 60, 60, 1.1f, "", 0.5);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //调整各个攻击方式的伤害倍率等等

            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();

            if (attackType == 100)
            {
                damage *= 5;
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

                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.8f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(105, targetRot, -1.2f, Projectile.ai[0], 1000), 0.2f);
                    Projectile.rotation = mainVec.ToRotation();

                    //向内的粒子效果
                    Vector2 r = Main.rand.NextVector2Unit();
                    float dis1 = MathHelper.Clamp(chargeTime1 - timer, 0, chargeTime1) / 1;
                    float dis2 = MathHelper.Clamp(chargeTime2 - timer, 0, chargeTime2) / 1;
                    float dis3 = MathHelper.Clamp(chargeTime3 - timer, 0, chargeTime3) / 1;
                    Dust d1 = Dust.NewDustDirect(Projectile.Center + r * dis1, 10, 10, DustID.AncientLight, 0, 0, 0, Color.White, 0.8f);
                    d1.velocity = -r * 4;
                    d1.position += Main.rand.NextVector2Unit() * 5;
                    d1.noGravity = true;

                    Dust d2 = Dust.NewDustDirect(Projectile.Center + r * dis2, 10, 10, DustID.AncientLight, 0, 0, 0, Color.White, 0.8f);
                    d2.velocity = -r * 4;
                    d2.position += Main.rand.NextVector2Unit() * 5;
                    d2.noGravity = true;

                    Dust d3 = Dust.NewDustDirect(Projectile.Center + r * dis3, 10, 10, DustID.AncientLight, 0, 0, 0, Color.White, 0.8f);
                    d3.velocity = -r * 4;
                    d3.position += Main.rand.NextVector2Unit() * 5;
                    d3.noGravity = true;
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
                    if(timer >= chargeTime2)
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
                            mainVec = Vector2Elipse(70, Projectile.rotation, 0f, Projectile.ai[0]);
                            Projectile.rotation += Projectile.spriteDirection * 0.35f;
                        }
                        if(!state2)
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
                            mainVec = Vector2Elipse(70, Projectile.rotation, 0f, Projectile.ai[0]);
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
                            mainVec = Vector2Elipse(100, Projectile.rotation, -1.2f, Projectile.ai[0]);
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

    }
}
