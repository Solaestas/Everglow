using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Terraria.Audio;
using Terraria.GameContent;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class NehemothBullet : ModProjectile
{
	public const int BuffDuration = 4 * 60;

	public bool HasNotShot { get; private set; } = true;

	public override void SetStaticDefaults()
	{
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = 1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 600;
		Projectile.alpha = 255;
		Projectile.light = 0.5f;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 1;
		Projectile.scale = 0.6f;

		AIType = ProjectileID.Bullet;
		Projectile.velocity = Projectile.velocity.NormalizeSafe() * 24f;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Main.rand.NextFloat() > 0.34)
		{
			target.AddBuff(ModContent.BuffType<Plague>(), BuffDuration);
		}
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		if (Main.rand.NextFloat() > 0.34)
		{
			target.AddBuff(ModContent.BuffType<Plague>(), BuffDuration);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		for (int k = 0; k < Projectile.oldPos.Length; k++)
		{
			Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
		}

		return true;
	}

	public override void OnKill(int timeLeft)
	{
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
	}
}