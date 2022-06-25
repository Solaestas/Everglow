using Terraria.Audio;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public class VortexVanquisher : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 4;
            trailLength = 20;
            longHandle = true;
            shadertype = "Trail";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/img_color";
        }
        public override float TrailAlpha(float factor)
        {
            if (attackType == 3)
            {
                return base.TrailAlpha(factor) * 1.5f;
            }
            return base.TrailAlpha(factor) * 1.3f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.Additive;
        }
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
        {
            base.DrawSelf(spriteBatch, lightColor, 240, 40, 0.75f, "Projectiles/VortexVanquisherGlow");
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //伤害倍率
            if (attackType == 0)
            {
                damage *= 2;
            }
            if (attackType == 4)
            {
                damage *= 4;
            }
        }
        public override void Attack()
        {
            useTrail = true;

            if (attackType == 0)
            {
                int t = timer;
                if (t < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 - Player.direction * 0.8f;
                    mainVec = Vector2.Lerp(mainVec, Player.DirectionTo(MouseWorld_WithoutGravDir) * 150, 0.2f);
                    disFromPlayer = MathHelper.Lerp(disFromPlayer, -30, 0.2f);
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (t >= 20 && t < 40)
                {
                    if (t == 20)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile.NewProjectile(Player.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 20, ModContent.ProjectileType<DashingLightEff>(), 1, 0, Projectile.owner);
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
                if (timer < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 - Player.direction * 1.2f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 20)
                {
                    AttSound(SoundID.Item1);
                }
                if (timer > 20 && timer < 35)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.4f;
                    mainVec = Vector2Elipse(180, Projectile.rotation, -1.2f);
                }
                if (timer > 40)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                if (timer < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = MathHelper.PiOver2 - Player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 180, 0.15f);
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 20)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        int r = 6;
                        Vector2 pos = Projectile.Center + Vector2.Lerp(-mainVec, mainVec, i / 30f) - new Vector2(r);
                        Dust.NewDust(pos, r * 2, r * 2, DustID.YellowTorch);
                    }
                    mainVec = Projectile.rotation.ToRotationVector2() * 0.001f;
                    Projectile.Center += new Vector2(Projectile.spriteDirection * 120, 0);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 mVec = MainVec_WithoutGravDir;
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center - Vector2.Normalize(mVec) * 300, Vector2.Normalize(mVec) * 15, ModContent.ProjectileType<DashingLightEff>(), Projectile.damage, 0, Projectile.owner, 1).CritChance = Projectile.CritChance;
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center - Vector2.Normalize(mVec) * 150, Vector2.Normalize(mVec) * 20, ModContent.ProjectileType<VortexVanquisher2>(), 0, 0, Projectile.owner, 1).scale=Projectile.scale*1.2f;
                    }
                }
                if (timer > 50)
                {
                    NextAttackType();
                }
            }
            if (attackType == 3)
            {
                if (timer == 0)
                {
                    AttSound(SoundID.NPCHit4);
                    Projectile.velocity = Vector2.Zero;
                    LockPlayerDir(Player);
                }
                if (timer < 120)
                {
                    if (timer % 10 == 0)
                    {
                        AttSound(SoundID.Item1);
                    }
                    CanIgnoreTile = true;
                    isAttacking = true;
                    Projectile.extraUpdates = 2;
                    Projectile.Center += Projectile.velocity;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(MouseWorld_WithoutGravDir - Player.Center) * 180, 0.06f);
                    Projectile.rotation += 0.3f * Projectile.spriteDirection;
                    mainVec = Projectile.rotation.ToRotationVector2() * 160;
                }
                if (timer > 120)
                {
                    CanIgnoreTile = false;
                    Projectile.extraUpdates = 1;
                    NextAttackType();
                }
            }
            if (attackType == 4)
            {
                if (timer == 0)
                {
                    useTrail = false;
                   
                    Vector2 vec = Vector2.Normalize(MouseWorld_WithoutGravDir - Player.Center);
                    Projectile.rotation = vec.ToRotation();
                    mainVec = vec * 160;
                    Player.velocity += Vector2.Normalize(Main.MouseWorld - Player.Center) * 20;
                    LockPlayerDir(Player);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 25, ModContent.ProjectileType<DashingLightEff>(), 0, 0, Projectile.owner);
                    }
                }
                if (timer < 20)
                {
                    if (Player.velocity.Length() > 15)
                    {
                        Player.velocity *= 0.9f;
                    }
                    if (timer < 10)
                    {
                        disFromPlayer += 30;
                        isAttacking = true;
                    }
                    else
                    {
                        disFromPlayer -= 30;
                    }
                }
                if (timer > 40)
                {
                    NextAttackType();
                }
            }
        }
    }
}

