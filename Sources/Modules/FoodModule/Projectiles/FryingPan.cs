using Everglow.Sources.Modules.MEACModule.Projectiles;
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
            maxAttackType = 3;//循环攻击方式的总数

            trailLength = 25;//拖尾的长度

            shadertype = "Trail";

            drawScaleFactor = 1f;

            disFromPlayer = 2;

           Projectile.height = 100;//判定区域的宽度，默认为15

           Projectile.scale = 1.2f;//总大小，有需要时可以使用


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
            base.DrawSelf(spriteBatch, lightColor, 60, 60, 0.9f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //调整各个攻击方式的伤害倍率等等

            if (attackType == 100)
            {
                damage *= 10;

            }
            else
            {

            }
        }

        //攻击方式编辑
        public override void Attack()
        {
            useTrail = true;
            if (attackType == 0)//一个带前后摇的普通挥砍
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;//前摇阶段关掉拖尾

                    LockPlayerDir(Player);//根据鼠标位置锁定玩家方向

                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.5f;//前摇时，剑的目标旋转角度

                    mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 120, 0.1f);//使用插值使武器向目标不断接近

                    Projectile.rotation = mainVec.ToRotation();//记录rotation，挥动时需要使用
                }
                if (timer == 30)//开始攻击时，播放音效
                    AttSound(SoundID.Item1);
                if (timer > 30 && timer < 50)//挥动
                {
                    isAttacking = true;//将这个设定为true才有判定

                    Projectile.rotation += Projectile.spriteDirection * 0.25f;//旋转rotation，0.25为转动速度

                    mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);//根据rotation变化mainVec
                }
                if (timer > 70)
                {
                    NextAttackType();//切换到下一个at
                }
            }
            if (attackType == 1 || attackType == 2)//两下以透视投影圆为基础的斜向挥砍（常用）
            {
                float rot1 = attackType == 1 ? -0.8f : 0.8f;
                if (timer < 10)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.5f;

                    //使用函数Vector2Elipse(长度，角度，rot1)找到在圆透视投影上的向量
                    //其中rot1为整个圆绕x轴旋转的角度，也就是斜砍的斜向角度（
                    //这里设定斜向角度为第一下-1，第二下1

                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(120, targetRot, rot1), 0.2f);

                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 10)
                    AttSound(SoundID.Item1);
                if (timer > 10 && timer < 25)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.35f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, rot1);
                }
                if (timer > 30)
                {
                    NextAttackType();
                }
            }
            if (attackType == 3)//横斩，代码和上面斜向的完全一样，但是倾斜角度更大
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(120, targetRot, -1.3f), 0.1f);
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 30)
                    AttSound(SoundID.Item1);
                if (timer > 30 && timer < 45)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.36f;
                    mainVec = Vector2Elipse(150, Projectile.rotation, -1.3f);
                }
                if (timer > 50)
                {
                    NextAttackType();
                }
            }

            if (attackType == 100)//右键长按蓄力斩的写法。因为不在循环内，所以这个type数值可以随便写，由Item切换到这个attackType
            {
                int chargeTime = 150;
                if (timer < 10000)//蓄力中
                {
                    useTrail = false;
                    LockPlayerDir(Player);

                    Projectile.ai[0] = GetAngToMouse();//获取往鼠标的方向

                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.8f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(105, targetRot, -1.2f, Projectile.ai[0], 1000), 0.1f);
                    Projectile.rotation = mainVec.ToRotation();

                    //向内的粒子效果
                    Vector2 r = Main.rand.NextVector2Unit();
                    float dis = MathHelper.Clamp(chargeTime - timer, 0, chargeTime) / 1;
                    Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, DustID.AncientLight, 0, 0, 0, Color.White, 0.8f);
                    d.velocity = -r * 4;
                    d.position += Main.rand.NextVector2Unit() * 5;
                    d.noGravity = true;
                }
                if (timer == chargeTime)//蓄力完成时
                {
                    //播放音效。
                    SoundStyle sound = SoundID.Item4;
                    sound.Volume *= 0.4f;
                    SoundEngine.PlaySound(sound, Projectile.Center);
                }
                if (!Player.controlUseTile && timer >= chargeTime && timer < 10000)//松开右键，且蓄力已经完成时
                {
                    //进入攻击状态
                    timer = 10000;
                    AttSound(SoundID.Item71);
                }
                if (timer >= 10000)//开始挥动攻击
                {
                    isAttacking = true;
                    if (timer < 10010)
                    {
                        isAttacking = true;
                        mainVec = Vector2Elipse(60, Projectile.rotation, 0f);
                        Projectile.rotation += Projectile.spriteDirection * 0.5f;
                    }
                    if (timer == 10015)
                    {
                        Projectile.friendly = false;
                    }
                    if (timer > 10020)
                    {
                        End();
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
