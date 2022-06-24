using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public class VortexVanquisher : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 4;
            TrailLength = 20;
            longHandle = true;
            shadertype = "Trail";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/img_color";
        }
        public override float TrailAlpha(float factor)
        {
            if(attackType==3)
            {
                return base.TrailAlpha(factor) * 1.5f;
            }
            return base.TrailAlpha(factor)*1.3f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.Additive;
        }
        
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //伤害倍率
            if(attackType==0)
            {
                damage *= 2;
            }
            if(attackType==4)
            {
                damage *= 4;
            }
        }
        public override void Attack()
        {
            UseTrail = true;
            
            if (attackType == 0) 
            {
                int t = Timer;
                if (t < 20)
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.8f;
                    mainVec = Vector2.Lerp(mainVec, player.DirectionTo(Main.MouseWorld) * 150, 0.2f);
                    disFromPlayer = MathHelper.Lerp(disFromPlayer, -30, 0.2f);
                    projectile.rotation = mainVec.ToRotation();
                }
                if (t >= 20 && t < 40)
                {
                    if (t == 20)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, projectile.Center);
                        if (Main.myPlayer == projectile.owner)
                        {
                            Projectile.NewProjectile(player.GetSource_FromAI(), projectile.Center, Vector2.Normalize(mainVec) * 20, ModContent.ProjectileType<DashingLightEff>(), 1, 0, projectile.owner);
                        }
                    }
                    if (t < 30)
                    {
                        disFromPlayer += 20;
                        isAttacking = true;
                    }
                    else
                    {
                        disFromPlayer -= 20;
                    }
                }
                if (t > 40)
                {
                    disFromPlayer = 6;
                    NextAttackType();
                }
            }
            if (attackType == 1)
            {
                if (Timer < 20)
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 1.2f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f);
                    mainVec += projectile.DirectionFrom(player.Center) * 3;
                    projectile.rotation = mainVec.ToRotation();
                }
                if(Timer==20)
                {
                    AttSound(SoundID.Item1);
                }
                if (Timer > 20 && Timer < 35)
                {

                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.4f;
                    mainVec = Vector2Elipse(180, projectile.rotation, -1.2f);
                }
                if (Timer > 40)
                {
                    NextAttackType();
                }
            }
            if(attackType==2)
            {
                if (Timer < 20)
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2()*180, 0.15f);
                    projectile.rotation = mainVec.ToRotation();
                }
                if(Timer==20)
                {
                    for(int i=0;i<30;i++)
                    {
                        int r = 6;
                        Vector2 pos = projectile.Center + Vector2.Lerp(-mainVec,mainVec,i/30f)-new Vector2(r);
                        Dust.NewDust(pos,r*2,r*2,DustID.YellowTorch);
                    }
                    mainVec = projectile.rotation.ToRotationVector2() * 0.001f;
                    projectile.Center += new Vector2(projectile.spriteDirection * 120, 0);
                    if (Main.myPlayer == projectile.owner)
                    {
                        Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center - Vector2.Normalize(mainVec) * 300, Vector2.Normalize(mainVec) * 15, ModContent.ProjectileType<DashingLightEff>(), projectile.damage, 0, projectile.owner, 1).CritChance = projectile.CritChance;
                    }
                }
                if (Timer > 50)
                    NextAttackType();
            }
            if(attackType==3)
            {
                if(Timer==0)
                {
                    AttSound(SoundID.NPCHit4);
                    projectile.velocity = Vector2.Zero;
                    LockPlayerDir(player);
                }
                if(Timer<120)
                {
                    if(Timer%10==0)
                    {
                        AttSound(SoundID.Item1);
                    }
                    CanIgnoreTile = true;
                    isAttacking = true;
                    projectile.extraUpdates = 2;
                    projectile.Center += projectile.velocity;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Normalize(Main.MouseWorld-player.Center)*180,0.06f);
                    projectile.rotation += 0.3f * projectile.spriteDirection;
                    mainVec = projectile.rotation.ToRotationVector2() * 160;
                }
                if(Timer>120)
                {
                    CanIgnoreTile = false;
                    projectile.extraUpdates=1;
                    NextAttackType();
                }
            }
            if (attackType == 4)
            {
                if(Timer==0)
                {
                    UseTrail = false;
                    Vector2 vec = Vector2.Normalize(Main.MouseWorld - player.Center);
                    projectile.rotation=vec.ToRotation();
                    mainVec = vec * 160;
                    player.velocity += vec * 20;
                    LockPlayerDir(player);
                    if(Main.myPlayer==projectile.owner)
                    {
                        Projectile.NewProjectile(projectile.GetSource_FromAI(),projectile.Center,Vector2.Normalize(mainVec)*25,ModContent.ProjectileType<DashingLightEff>(),0,0,projectile.owner);
                    }
                }
                if (Timer < 20)
                {
                    if (player.velocity.Length() > 15)
                        player.velocity *= 0.9f;
                    if (Timer < 10)
                    {
                        disFromPlayer += 30;
                        isAttacking = true;
                    }
                    else
                    {
                        disFromPlayer -= 30;
                    }
                }
                if(Timer>40)
                {
                    NextAttackType();
                }
            }
        }
    }
}

