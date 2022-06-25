using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public class ScaleWingBladeProj : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 2;
            trailLength = 20;
            shadertype = "Trail";
            Projectile.scale *= 1.1f;
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
        }
        public override float TrailAlpha(float factor)
        {
            return base.TrailAlpha(factor) * 1.35f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            float texWidth = 85;//转换成水平贴图时候的宽度
            float texHeight = 30;//转换成水平贴图时候的高度
            float Size = 0.9f;//放大的几何倍数
            double baseRotation = 0.79;//这个是刀刃倾斜度与水平的夹角

            float exScale = 1;
            if (longHandle)
            {
                exScale += 1f;
            }
            Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

            double ProjRotation = mainVec.ToRotation() + Math.PI / 4;

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = Projectile.Center - Main.screenPosition;
            Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * Size;
            Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * Size;

            Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
            ITexNormal.X /= tex.Width;
            ITexNormal.Y /= tex.Height;
            Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
            JTexNormal.X /= tex.Width;
            JTexNormal.Y /= tex.Height;

            Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = Vector2.Normalize(INormal) * origin.Y - Vector2.Normalize(JNormal) * origin.X;
            Vector2 TopRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) + Vector2.Normalize(INormal) * origin.Y;
            Vector2 BottomLeft = -Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y) - Vector2.Normalize(JNormal) * origin.X;
            Vector2 BottomRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) - Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y);


            Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
            Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
            Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

            if (Main.player[Projectile.owner].direction == -1)
            {
                sourceTopLeft = sourceBottomLeft;
                sourceTopRight = sourceBottomRight;
                sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            }

            List<Vertex2D> vertex2Ds = new List<Vertex2D>
                {
                    new Vertex2D(drawCenter + TopLeft, Projectile.GetAlpha(lightColor), new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter + BottomLeft, Projectile.GetAlpha(lightColor), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, Projectile.GetAlpha(lightColor), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

                    new Vertex2D(drawCenter + BottomRight, Projectile.GetAlpha(lightColor), new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + BottomLeft, Projectile.GetAlpha(lightColor), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, Projectile.GetAlpha(lightColor), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
                };

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
            base.DrawSelf(spriteBatch,lightColor);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (attackType == 100)
            {
                damage *= 3;
                if(Projectile.owner==Player.whoAmI)
                {
                    int counts = Main.rand.Next(4,9);
                    for(int i=0;i<counts;i++)
                    {
                        Projectile proj=Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),target.Center,Main.rand.NextVector2Unit()*Main.rand.Next(6,13),ModContent.ProjectileType<ButterflyDreamFriendly>(),damage/4,0,Main.myPlayer,target.whoAmI);
                        proj.netUpdate2 = true;
                        proj.CritChance = Projectile.CritChance;
                        
                    }
                }
            }
            else
            {
                if (Projectile.owner == Player.whoAmI)
                {
                    int counts = Main.rand.Next(1, 3);
                    for (int i = 0; i < counts; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2Unit() * Main.rand.Next(6, 13), ModContent.ProjectileType<ButterflyDreamFriendly>(), damage / 4, 0, Main.myPlayer, target.whoAmI);
                        proj.netUpdate2 = true;
                        proj.CritChance = Projectile.CritChance;

                    }
                }
            }
        }
        public override void Attack()
        {
            Player player = Main.player[Projectile.owner];
            useTrail = true;
            float timeMul = 1f-GetMeleeSpeed(player)/100f;
            if (attackType == 0)
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, -1.2f), 0.1f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 30)
                    AttSound(SoundID.Item1);
                if (timer >30&&timer< 50)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.25f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
                }
                if (timer > 50+20*timeMul)
                {
                    NextAttackType();
                }
            }
            if (attackType == 1)
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.1f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 30)
                    AttSound(SoundID.Item1);
                if (timer > 30 && timer < 50)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.25f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
                }
                if (timer > 50)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                if (timer < 10)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = +MathHelper.PiOver2 + player.direction * 0.7f;
                    mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2()*100, 0.15f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer > 10 && timer < 30)
                {
                    isAttacking = true;
                    Projectile.rotation -= Projectile.spriteDirection * 0.26f;
                    mainVec = Projectile.rotation.ToRotationVector2() * 90;
                }
                if(timer>30&&timer<50)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.25f;
                    mainVec = Projectile.rotation.ToRotationVector2() * 130;
                }
                if (timer == 10 || timer == 30)
                {
                    useTrail = false;
                    AttSound(SoundID.Item1);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        //寻敌
                        NPC target = null;
                        float maxdis = 1000;
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.CanBeChasedBy())
                            {
                                float dis = Vector2.Distance(Projectile.Center, npc.Center);
                                if (dis < maxdis)
                                {
                                    maxdis = dis;
                                    target = npc;
                                }
                            }
                        }
                        if (target != null)
                        {
                            for (int i = Main.rand.Next(2); i < 2; i++)
                            {
                                Vector2 vel = new Vector2(Projectile.spriteDirection * 10, 0);
                                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center+vel, vel + Main.rand.NextVector2Unit() * 5, ModContent.ProjectileType<ButterflyDreamFriendly>(), Projectile.damage / 2, 0, Main.myPlayer, target.whoAmI);
                                proj.netUpdate2 = true;
                                proj.CritChance = Projectile.CritChance;
                            }
                        }
                    }
                }
                
                if (timer > 55+25*timeMul)
                {
                    NextAttackType();
                }
            }
            if(attackType==100)//右键攻击
            {
                if(timer<60)
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, -1.2f), 0.1f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();

                    Vector2 r = Main.rand.NextVector2Unit();
                    float dis = MathHelper.Clamp(60- timer, 0, 60) *2;
                    Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueGlow>(), 0, 0, 0, default, 1.2f);
                    d.velocity = -r * 4;
                    d.position += Main.rand.NextVector2Unit() * 5;
                    d.noGravity = true;
                }
                else if(timer<100)
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    Projectile.ai[0] = GetAngToMouse();
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.8f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(110, targetRot, -1.2f, Projectile.ai[0], 1000), 0.1f);
                    Projectile.rotation = mainVec.ToRotation();

                    Vector2 r = Main.rand.NextVector2Unit();
                    float dis = 0;
                    Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueGlow>(), 0, 0, 0, default, 1.2f);
                    d.velocity = -r * 4;
                    d.position += Main.rand.NextVector2Unit() * 5;
                    d.noGravity = true;
                }
                if(timer==100)
                {
                    AttSound(SoundID.Item1);
                }
                if(timer>100)
                {
                    isAttacking = true;
                    if (timer < 115)
                    {
                        isAttacking = true;
                        mainVec = Vector2Elipse(220, Projectile.rotation, -1.2f, Projectile.ai[0], 1000);
                        Projectile.rotation += Projectile.spriteDirection * 0.42f;
                    }
                    if (timer == 115)
                    {
                        Projectile.friendly = false;
                    }
                    if (timer > 130)
                    {
                        End();
                    }
                }
            }
            if (isAttacking)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<BlueGlow>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1.3f));
                    d.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 3;
                    d.noGravity = true;
                }
            }
        }
    }
}

