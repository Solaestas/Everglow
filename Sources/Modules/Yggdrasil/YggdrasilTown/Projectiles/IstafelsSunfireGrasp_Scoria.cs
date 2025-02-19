using Everglow.Yggdrasil.YggdrasilTown.Buffs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class IstafelsSunfireGrasp_Scoria : ModProjectile
{
	public const int ProjectileVelocityYMax = 16;
	public const int ContactDamage = 25;
	public const int BuffDuration = 960;
	public const int ProjectileSize = 20;
	public const int TimeLeftMax = 600;

	private bool HasNotCollideTile { get; set; } = true;

	public override string Texture => Commons.ModAsset.Point_Mod;

	public override void SetDefaults()
	{
		Projectile.width = ProjectileSize;
		Projectile.height = ProjectileSize;

		Projectile.DamageType = DamageClass.Default;

		Projectile.timeLeft = TimeLeftMax;

		Projectile.friendly = true;
		Projectile.hostile = false;

		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
		Projectile.localNPCHitCooldown = 10;
	}

	public override void AI()
	{
		Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0f);

		if (HasNotCollideTile)
		{
			// Simulate gravity
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > ProjectileVelocityYMax)
			{
				Projectile.velocity.Y = ProjectileVelocityYMax;
			}
		}
	}

	public override bool? CanCutTiles() => true;

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.velocity = Vector2.Zero;
		Projectile.tileCollide = false;
		HasNotCollideTile = false;

		return false;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		// Let the projectile deals true damage, minus base damage 1
		modifiers.FinalDamage.Flat += ContactDamage - 1;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<Charred>(), BuffDuration);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var drawColor = new Color(1f, 0.6f, 0f, 0f) * (Projectile.timeLeft / (float)TimeLeftMax);
		var scale = new Vector2(ProjectileSize) * 2.8f / texture.Size();
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, drawColor, 0, texture.Size() / 2, scale, SpriteEffects.None, 0);
		return false;
	}

	public override void OnKill(int timeLeft)
	{
		for (int i = 0; i < 20; i++)
		{
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, Projectile.velocity.X, Projectile.velocity.Y, Scale: 3f);
		}
	}
}