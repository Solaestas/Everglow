using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;

public class MagicalBoomerangProj : ModProjectile
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeProjectiles;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 3600;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.width = 30;
        Projectile.height = 30;
        Timer = 0;
    }

    public bool Returning = false;

    public int Timer = 0;

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Returning)
        {
            return false;
        }
        Projectile.velocity *= -1;
        Returning = true;
        return false;
    }

    public override void OnSpawn(IEntitySource source)
    {
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Timer++;
        Projectile.rotation += 0.3f;
        if (!Returning && Timer > 60)
        {
            Returning = true;
        }
        if (Returning)
        {
            Projectile.tileCollide = false;
            Vector2 toPlayer = player.Center + player.velocity - Projectile.Center - Projectile.velocity;
            float speed = 13f;
            speed += (Timer - 60) / 10f;
            if (toPlayer.Length() < speed * 2)
            {
                Projectile.Kill();
            }
            Projectile.velocity = Projectile.velocity * 0.9f + toPlayer.NormalizeSafe() * speed * 0.1f;
        }
        if (Timer > 5 && Main.mouseLeft && Main.mouseLeftRelease)
        {
            Projectile.Kill();
            float speed = 13f;
            Vector2 startVel = new Vector2(0, 1).RotatedByRandom(MathHelper.TwoPi) * speed;
            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = startVel.RotatedBy(i / 8f * MathHelper.TwoPi);
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<MagicalBoomerangSubProj>(), (int)(Projectile.damage * 0.7), Projectile.knockBack * 0.7f, Projectile.owner);
                proj.rotation = vel.ToRotation();
                for (int step = 0; step < 20; step++)
                {
                    var dustVFX = new MagicalBoomerangDust
                    {
                        velocity = Vector2.zeroVector,
                        Active = true,
                        Visible = true,
                        position = Projectile.Center + vel / 5f * step,
                        maxTime = Main.rand.Next(30, 46),
                        scale = Main.rand.NextFloat(3, 4),
                        rotation = Main.rand.NextFloat(MathHelper.TwoPi),
                        ai = new float[] { 0, 0, 0 },
                    };
                    Ins.VFXManager.Add(dustVFX);

                    float distanceSide = (20 - step) / 5f;
                    var dustVFXLeft = new MagicalBoomerangDust
                    {
                        velocity = Vector2.zeroVector,
                        Active = true,
                        Visible = true,
                        position = Projectile.Center + vel / 5f * step + vel.RotatedBy(1.2f) / 6f * distanceSide,
                        maxTime = Main.rand.Next(30, 46),
                        scale = Main.rand.NextFloat(3, 4),
                        rotation = Main.rand.NextFloat(MathHelper.TwoPi),
                        ai = new float[] { 0, 0, 0 },
                    };
                    Ins.VFXManager.Add(dustVFXLeft);
                    var dustVFXRight = new MagicalBoomerangDust
                    {
                        velocity = Vector2.zeroVector,
                        Active = true,
                        Visible = true,
                        position = Projectile.Center + vel / 5f * step + vel.RotatedBy(-1.2f) / 6f * distanceSide,
                        maxTime = Main.rand.Next(30, 46),
                        scale = Main.rand.NextFloat(3, 4),
                        rotation = Main.rand.NextFloat(MathHelper.TwoPi),
                        ai = new float[] { 0, 0, 0 },
                    };
                    Ins.VFXManager.Add(dustVFXRight);
                }
            }
        }
        Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.7f, 1f) * 0.5f);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!Returning)
        {
            Returning = true;
        }
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void OnKill(int timeLeft) => base.OnKill(timeLeft);

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D boomerang = ModAsset.MagicalBoomerangProj.Value;
        Texture2D boomerangGlow = ModAsset.MagicalBoomerangProj_glow.Value;
        Main.EntitySpriteDraw(boomerang, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, boomerang.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
        Main.EntitySpriteDraw(boomerangGlow, Projectile.Center - Main.screenPosition, null, new Color(0.3f, 0.7f, 1f, 0), Projectile.rotation, boomerangGlow.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
        return false;
    }
}