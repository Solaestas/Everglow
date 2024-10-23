namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis : ModProjectile
{
	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 360000;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
	}

	public override void AI()
	{
		Projectile.position = Owner.position;
	}

	public override bool PreDraw(ref Color lightColor) => false;

	public override void PostDraw(Color lightColor)
	{
		Owner.heldProj = Projectile.whoAmI;
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (Owner.direction == -1)
		{
			se = SpriteEffects.FlipVertically;
		}

		float rot0 = Projectile.rotation - (float)(Math.PI * 0.25) + MathF.PI * 0.3f * Owner.direction;
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition, null, drawColor, rot0, texMain.Size() / 2f, 1f, se, 0);
	}
}