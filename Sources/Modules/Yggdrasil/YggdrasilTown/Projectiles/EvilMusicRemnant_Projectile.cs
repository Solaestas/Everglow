using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Projectile : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 18;
		Projectile.height = 24;

		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.timeLeft = 300;
	}

	public override void OnSpawn(IEntitySource source)
	{
		// Select random texture
		var num = Main.rand.Next(3);
		switch (num)
		{
			case 1:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile2_Mod;
				Projectile.width = 18;
				Projectile.height = 24;
				break;
			case 2:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile3_Mod;
				Projectile.width = 10;
				Projectile.height = 22;
				break;
			default:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile1_Mod;
				Projectile.width = 22;
				Projectile.height = 24;
				break;
		}
	}

	private string ProjectileTexture { get; set; } = ModAsset.EvilMusicRemnant_Projectile1_Mod;

	public override string Texture => ProjectileTexture;

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(ProjectileTexture).Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() / 2, 1, SpriteEffects.None, 0);
		;
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!target.active && !target.friendly && !target.townNPC)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EvilMusicRemnant_Minion>(), Projectile.damage, Projectile.knockBack);
		}
	}
}