using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class NightfireStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 22;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 12;
		Item.knockBack = 0.5f;
		Item.mana = 8;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = Item.useAnimation = 32;
		Item.noMelee = true;
		Item.autoReuse = false;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 1);

		Item.shoot = ModContent.ProjectileType<NightfireStaff_Projectile>();
		Item.shootSpeed = 8;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectile(source, Main.MouseWorld - 4 * velocity + new Vector2(0, 20).RotatedBy(velocity.ToRotation() - MathHelper.Pi), velocity, type, damage, knockback, player.whoAmI, velocity.ToRotation(), 1);
		Projectile.NewProjectile(source, Main.MouseWorld - 4 * velocity - new Vector2(0, 20).RotatedBy(velocity.ToRotation() - MathHelper.Pi), velocity, type, damage, knockback, player.whoAmI, velocity.ToRotation(), -1);
		return false;
	}
}

public class NightfireStaff_Projectile : ModProjectile
{
	public const int VelocityXMax = 20;
	public const int VelocityYMax = 20;
	public const int Acceleration = 3;
	public const int Duration = 1000;

	/// <summary>
	/// The main direction of the pair of projectile shot by weapon
	/// </summary>
	public float MainDirection => Projectile.ai[0];

	/// <summary>
	/// Used to represent this projectile is which one of the pair of projectile shot by weapon
	/// </summary>
	public float Inversed => Projectile.ai[1];

	public override void SetStaticDefaults()
	{
		Main.projFrames[Type] = 4;
	}

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = 0;
		Projectile.penetrate = 2;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.timeLeft = Duration;
	}

	public override void AI()
	{
		// Update texture frame
		// ====================
		if (Main.time % 5 == 0)
		{
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
		}

		// Use percentage to represent the progress of projectile life cycle.
		// The progress will affect: lighting strength, velocity.
		// ======================================================
		var progress = (Projectile.timeLeft - Duration) * 1f / Duration;

		// Add green light to simulate the light of firefly.
		// Lighting Strength = Basic Strength + Progress Strength + Random Strength
		// ========================================================================
		var lightStrength = 0.7f + 0.3f * progress + 0.2f * Main.rand.NextFloat(-1, 1);
		Lighting.AddLight(Projectile.Center, 0, 0.52f * lightStrength, 0);

		// Calculate the velocity of projectile.
		// =====================================
		var velocity = new Vector2(0);

		// Velocity on main direction.
		if (velocity.X + Acceleration > VelocityXMax)
		{
			velocity.X = VelocityXMax;
		}
		else
		{
			velocity.X = velocity.X + Acceleration;
		}

		// Velocity vertical to the main direction
		velocity.Y = Inversed * VelocityYMax * MathF.Cos(progress * 100 + MathHelper.Pi / 6);

		// Rotate the velocity to main direction
		Projectile.velocity = velocity.RotatedBy(MainDirection);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		// Draw firefly framed texture
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		Rectangle frame = texture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
		float rotation = Projectile.velocity.ToRotation();
		SpriteEffects spriteEffect = SpriteEffects.FlipHorizontally;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, rotation, frame.Size() / 2, Projectile.scale, spriteEffect, 0);
		return false;
	}
}