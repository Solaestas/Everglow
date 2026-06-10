using Everglow.Yggdrasil.KelpCurtain.NPCs.VampireMat;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Enemies;

public class VampireMat_Attack_Proj_Tusk : ModProjectile
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MagicProjectiles;

	public int Timer = 0;

	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 600;
		Projectile.penetrate = -1;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.friendly = false;
		Projectile.hostile = true;
	}

	public override void AI()
	{
		Projectile.rotation = Projectile.velocity.ToRotationSafe();
		Timer++;
	}

	public override void OnKill(int timeLeft)
	{
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		VampireMat.VampireMatHitCommonEffect(target);
		base.OnHitPlayer(target, info);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Texture2D bloom = ModAsset.VampireMat_Attack_Proj_Tusk_bloom.Value;
		Texture2D white = ModAsset.VampireMat_Attack_Proj_Tusk_White.Value;
		float drawScale = Projectile.scale;
		lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() * 0.5f, drawScale, SpriteEffects.None, 0);
		Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0) * (MathF.Sin(Projectile.timeLeft * 0.5f) * 0.35f + 1f) * 0.5f, Projectile.rotation, bloom.Size() * 0.5f, drawScale, SpriteEffects.None, 0);
		if(Timer < 6 || (Timer < 30 && Timer % 8 < 4))
		{
			Main.EntitySpriteDraw(white, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.White, lightColor, 0.75f), Projectile.rotation, white.Size() * 0.5f, drawScale, SpriteEffects.None, 0);
		}
		return false;
	}
}