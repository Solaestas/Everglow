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
    public class ScaleWingBladeProj : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 3;
            TrailLength = 20;
            shadertype = "Trail";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
        }
        public override float TrailAlpha(float factor)
        {
            return base.TrailAlpha(factor) * 1.5f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor)
        {
            if (attackType != 114514)
                base.DrawSelf(Main.spriteBatch, lightColor);
            else
            {
                Vector2 mVec = mainVec / 2;
                Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
                Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor), mVec.ToRotation(), new Vector2(0, tex.Height / 2), new Vector2(mVec.Length() / tex.Width, 1.2f) * projectile.scale, SpriteEffects.None, 0);
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void Attack()
        {
            Player player = Main.player[projectile.owner];
            UseTrail = true;
            if (attackType == 0)
            {
                if (Timer < 30)//前摇
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += projectile.DirectionFrom(player.Center) * 3;
                    projectile.rotation = mainVec.ToRotation();
                }
                if (Timer == 30)
                    AttSound(SoundID.Item1);
                if (Timer >30&&Timer< 50)
                {
                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.25f;
                    mainVec = Vector2Elipse(120, projectile.rotation, 0.6f);
                }
                if (Timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 1)
            {
                if (Timer == 0)
                {
                    LockPlayerDir(player);
                    UseTrail = false;
                    projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.6f;
                }
                if (Timer == 1)
                    AttSound(SoundID.Item1);
                if (Timer < 20)
                {
                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.22f;
                    mainVec = Vector2Elipse(120, projectile.rotation, -0.6f);
                }
                if (Timer > 40)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                if (Timer == 0 )
                {
                    LockPlayerDir(player);
                    UseTrail = false;
                    projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.1f;
                }
                if (Timer < 60)
                {
                    isAttacking = true;
                    if (Timer % 15 == 0)
                    {
                        AttSound(SoundID.Item1);
                        UseTrail = false;
                    }
                    if (Timer % 30 < 15)
                    {
                        projectile.rotation += projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, projectile.rotation, 1f);
                    }
                    else
                    {
                        projectile.rotation -= projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, projectile.rotation, -1f);
                    }

                }
                if (Timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 3)
            {
                if (Timer < 30)//前摇
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.7f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += projectile.DirectionFrom(player.Center) * 3;
                    projectile.rotation = mainVec.ToRotation();
                }
                if (Timer > 30 && Timer < 40)
                {
                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.55f;
                    mainVec = Vector2Elipse(120, projectile.rotation, -1.2f);
                }
                if (Timer == 30)
                    AttSound(SoundID.Item1);
                if (Timer > 30 && Timer < 50)
                {
                    
                }
                if (Timer > 100)
                {
                    NextAttackType();
                }
            }
            if (isAttacking)
                for (int i = 1; i < 4; i++)
                {
                    Dust d = Dust.NewDustDirect(projectile.Center + i * mainVec / 3, 20, 20, 172, 0, 0, 0, default, 1.5f);
                    d.velocity *= 0;
                    d.noGravity = true;
                }
        }
    }
}

