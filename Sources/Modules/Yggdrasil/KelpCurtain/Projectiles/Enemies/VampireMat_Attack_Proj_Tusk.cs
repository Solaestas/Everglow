using Everglow.Yggdrasil.KelpCurtain.NPCs.VampireMat;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Enemies;

public class VampireMat_Attack_Proj_Tusk : ModProjectile
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MagicProjectiles;

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
		float drawScale = Projectile.scale;
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Lighting.GetColor(Projectile.Center.ToTileCoordinates()), Projectile.rotation, tex.Size() * 0.5f, drawScale, SpriteEffects.None, 0);
		return false;
	}
}