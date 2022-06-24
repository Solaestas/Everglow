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
            trailLength = 20;
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
                Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), mVec.ToRotation(), new Vector2(0, tex.Height / 2), new Vector2(mVec.Length() / tex.Width, 1.2f) * Projectile.scale, SpriteEffects.None, 0);
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void Attack()
        {
            Player player = Main.player[Projectile.owner];
            useTrail = true;
            if (attackType == 0)
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
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
                if (timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 1)
            {
                if (timer == 0)
                {
                    LockPlayerDir(player);
                    useTrail = false;
                    Projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.6f;
                }
                if (timer == 1)
                    AttSound(SoundID.Item1);
                if (timer < 20)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.22f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, -0.6f);
                }
                if (timer > 40)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                if (timer == 0 )
                {
                    LockPlayerDir(player);
                    useTrail = false;
                    Projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.1f;
                }
                if (timer < 60)
                {
                    isAttacking = true;
                    if (timer % 15 == 0)
                    {
                        AttSound(SoundID.Item1);
                        useTrail = false;
                    }
                    if (timer % 30 < 15)
                    {
                        Projectile.rotation += Projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, Projectile.rotation, 1f);
                    }
                    else
                    {
                        Projectile.rotation -= Projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, Projectile.rotation, -1f);
                    }

                }
                if (timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 3)
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.7f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer > 30 && timer < 40)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.55f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, -1.2f);
                }
                if (timer == 30)
                    AttSound(SoundID.Item1);
                if (timer > 30 && timer < 50)
                {
                    
                }
                if (timer > 100)
                {
                    NextAttackType();
                }
            }
            if (isAttacking)
                for (int i = 1; i < 4; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center + i * mainVec / 3, 20, 20, 172, 0, 0, 0, default, 1.5f);
                    d.velocity *= 0;
                    d.noGravity = true;
                }
        }
    }
}

