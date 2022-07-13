using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule;
using Terraria.Audio;
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
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
        {
            base.DrawSelf(spriteBatch, lightColor, 85, 30, 0.9f, "Projectiles/ScaleWingBladeProjGlow");
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();

            if (attackType == 100)
            {
                damage *= 3;
                knockback *= 3;
                if (Projectile.owner == Player.whoAmI)
                {
                    int counts = Main.rand.Next(4, 9);
                    for (int i = 0; i < counts; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2Unit() * Main.rand.Next(6, 13), ModContent.ProjectileType<ButterflyDreamFriendly>(), damage / 4, 0, Main.myPlayer, target.whoAmI);
                        proj.netUpdate2 = true;
                        proj.CritChance = Projectile.CritChance;
                    }
                }
                Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12, 150)).RotatedByRandom(6.283);
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
                Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 50, 50)).RotatedByRandom(6.283);
            }
        }
        public override void Attack()
        {
            Player player = Main.player[Projectile.owner];
            useTrail = true;
            float timeMul = 1f - GetMeleeSpeed(player) / 100f;
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
                if (timer == 20)
                    AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
                if (timer > 30 && timer < 50)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.25f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
                }
                if (timer > 50 + 20 * timeMul)
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
                if (timer == 20)
                    AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
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
                    mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 100, 0.15f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer > 10 && timer < 30)
                {
                    isAttacking = true;
                    Projectile.rotation -= Projectile.spriteDirection * 0.26f;
                    mainVec = Projectile.rotation.ToRotationVector2() * 90;
                }
                if (timer > 30 && timer < 50)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.25f;
                    mainVec = Projectile.rotation.ToRotationVector2() * 130;
                }
                if (timer == 1 || timer == 20)
                {
                    useTrail = false;
                    AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
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
                                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + vel, vel + Main.rand.NextVector2Unit() * 5, ModContent.ProjectileType<ButterflyDreamFriendly>(), Projectile.damage / 2, 0, Main.myPlayer, target.whoAmI);
                                proj.netUpdate2 = true;
                                proj.CritChance = Projectile.CritChance;
                            }
                        }
                    }
                }

                if (timer > 55 + 25 * timeMul)
                {
                    NextAttackType();
                }
            }
            if (attackType == 100)//右键攻击
            {
                if (timer < 60)
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, -1.2f), 0.1f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();

                    Vector2 r = Main.rand.NextVector2Unit();
                    float dis = MathHelper.Clamp(60 - timer, 0, 60) * 2;
                    Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.7f, 1.7f));
                    d.velocity = -r * 4;
                    d.position += Main.rand.NextVector2Unit() * 5;
                    d.noGravity = true;

                    Dust d2 = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                    d2.velocity = -r * 4;
                    d2.position += Main.rand.NextVector2Unit() * 5;
                    d2.alpha = (int)(d2.scale * 50);
                }
                else if (timer < 100)
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    Projectile.ai[0] = GetAngToMouse();
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.8f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(110, targetRot, -1.2f, Projectile.ai[0], 1000), 0.1f);
                    Projectile.rotation = mainVec.ToRotation();

                    Vector2 r = Main.rand.NextVector2Unit();
                    float dis = 0;
                    Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.7f, 1.7f));
                    d.velocity = -r * 4;
                    d.position += Main.rand.NextVector2Unit() * 5;
                    d.noGravity = true;

                    Dust d2 = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                    d2.velocity = -r * 4;
                    d2.position += Main.rand.NextVector2Unit() * 5;
                    d2.alpha = (int)(d2.scale * 50);
                }
                if (timer == 105)
                {
                    AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleePowerSwing"));
                }
                if (timer > 100)
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
                    Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
                    d.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 3;
                    d.noGravity = true;
                }
                /*for (int i = 0; i < 8; i++)//加上这个有点奇怪，特效过多，光污染了
                {
                    Dust d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                    d2.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 0.3f;
                    d2.alpha = (int)(d2.scale * 50);
                }*/
            }
        }
    }
}