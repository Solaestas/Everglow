using Everglow.Commons.Graphics;
using Terraria.Audio;

namespace Everglow.Yggdrasil.GreenCore.Projectiles.Melee;

public class ShadowEulogistProj : MeleeProj
{
	public override string Texture => ModAsset.ShadowEulogist_Mod;
	public override void SetDef()
	{
		maxAttackType = 1;
		trailLength = 7;
		Projectile.hide = true;
		//shadertype = "Trail";
		Projectile.scale *= 1.0f;
        longHandle = true;
	}
	public override string TrailShapeTex()
	{
		return "Everglow/MEAC/Images/Melee";
	}
	public override string TrailColorTex()
	{
		return Texture + "_Color";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor);
	}
	public override BlendState TrailBlendState()
	{
		return CustomBlendStates.Reverse;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
        base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{	

	}
	public new void DrawBloom()
	{

	}
	public override void Attack()
	{
        useBloom = false;
        disFromPlayer = 20;
		//drawScaleFactor = 10.1f;
		Player player = Main.player[Projectile.owner];
        useTrail = true;
        
        if (attackType == 0)
        {
            if (timer < 20)//前摇
            {
                useTrail = false;
                LockPlayerDir(player);
                float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                mainVec = Vector2.Lerp(mainVec, Vector2Elipse(140, targetRot, -1.2f), 0.1f);
                mainVec += Projectile.DirectionFrom(player.Center) * 3;
                Projectile.rotation = mainVec.ToRotation();
            }
            if (timer == 20)
                AttSound(SoundID.Item1);
            if (timer > 20 && timer < 40)
            {
                isAttacking = true;
                Projectile.rotation += Projectile.spriteDirection * 0.3f;
                mainVec = Vector2Elipse(160, Projectile.rotation, -1.2f);
            }
			if(timer==40)
			{
				useTrail = false;
                AttSound(SoundID.Item1);
                float targetRot = +MathHelper.PiOver2 + player.direction * 0.5f;
                mainVec = Vector2Elipse(140, targetRot, 0f);
				Projectile.rotation = mainVec.ToRotation();
            }
            if (timer > 40 && timer < 50)
            {
                isAttacking = true;
                Projectile.rotation -= Projectile.spriteDirection * 0.6f;
                mainVec = Vector2Elipse(140, Projectile.rotation, 0f);
            }
            if (timer > 60)
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
                mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, 0.6f), 0.1f);
                mainVec += Projectile.DirectionFrom(player.Center) * 3;
                Projectile.rotation = mainVec.ToRotation();
            }
            if (timer == 20)
                AttSound(SoundID.Item1);
            if (timer > 30 && timer < 45)
            {
                isAttacking = true;
                Projectile.rotation += Projectile.spriteDirection * 0.4f;
                mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
            }
            if (timer > 50)
            {
                NextAttackType();
            }
        }

        if (isAttacking)
		{
			for (int i = 0; i < 2; i++)
			{
				/*
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
				d.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 3;
				d.noGravity = true;
				Dust d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<MothBlue2>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
				d2.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 3;
				d2.noGravity = true;*/
			}
		}
	}
}