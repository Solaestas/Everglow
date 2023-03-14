using Everglow.Myth.Common;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.Projectiles.Weapon
{
	public class ToothSpear : ModProjectile
	{
		protected virtual float HoldoutRangeMin => 24f;
		protected virtual float HoldoutRangeMax => 150f;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tooth Spear");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "龙齿矛");

		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int y = 0; y < 12; y++)
			{
				int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, 183, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
				Main.dust[num90].noGravity = true;
				Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
			}
			for (int y = 0; y < 48; y++)
			{
				int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4f));
				Main.dust[num90].noGravity = false;
				Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.4f, 1.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int y = 0; y < 12; y++)
			{
				int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, 183, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
				Main.dust[num90].noGravity = true;
				Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
			}
			for (int y = 0; y < 48; y++)
			{
				int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4f));
				Main.dust[num90].noGravity = false;
				Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.4f, 1.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
			}
			Projectile.Kill();
			return true;
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
		}
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			int duration = player.itemAnimationMax;
			player.heldProj = Projectile.whoAmI;
			MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();

			if (Projectile.timeLeft > duration)
				Projectile.timeLeft = duration;

			Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			if (Projectile.timeLeft < halfDuration + 2 && !max)
			{
				Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity * 15f, ModContent.ProjectileType<TuskSpear3>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				max = true;
			}
			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation. 
			if (Projectile.timeLeft < halfDuration)
				progress = Projectile.timeLeft / halfDuration;
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}
			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			// Apply proper rotation to the sprite.
			if (Projectile.spriteDirection == -1)
				// If sprite is facing left, rotate 45 degrees
				Projectile.rotation += MathHelper.ToRadians(45f);
			else
			{
				// If sprite is facing right, rotate 135 degrees
				Projectile.rotation += MathHelper.ToRadians(135f);
			}

			// Avoid spawning dusts on dedicated servers
			if (!Main.dedServ)
			{
			}

			return false; // Don't execute vanilla AI.
		}
		private Effect ef;
		private bool max = false;
		private Vector2[] vpos = new Vector2[15];
	}
}
