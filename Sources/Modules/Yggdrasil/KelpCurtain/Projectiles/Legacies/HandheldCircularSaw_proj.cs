using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Legacies;

public class HandheldCircularSaw_proj : ModProjectile
{
	public override string Texture => "Everglow/Yggdrasil/KelpCurtain/Projectiles/Legacies/HandheldCircularSaw_handle";
	public override void SetDefaults()
	{
		Projectile.width = 50;
		Projectile.height = 50;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
	}
	public override void OnSpawn(IEntitySource source)
	{
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathF.PI * 0.75f);

		player.heldProj = Projectile.whoAmI;
		Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
		MouseToPlayer = Vector2.Normalize(MouseToPlayer);
		if (player.controlUseItem)
		{
			Projectile.rotation = (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) + Math.PI * 0.25);
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer) * 32f;
			Projectile.velocity *= 0;
			if (player.itemTime == 0)
			{
				player.itemTime = player.itemTimeMax;
			}
		}
		if (!player.controlUseItem)
		{
			Projectile.Kill();
		}
		if (Projectile.Center.X < player.MountedCenter.X)
			player.direction = -1;
		else
		{
			player.direction = 1;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		var texSaw = ModAsset.HandheldCircularSaw_saw.Value;
		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == -1)
			se = SpriteEffects.FlipVertically;
		float rot0 = Projectile.rotation - (float)(Math.PI * 0.25) + MathF.PI * 0.3f * player.direction;
		Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
		MouseToPlayer = Vector2.Normalize(MouseToPlayer);

		Main.spriteBatch.Draw(texSaw, Projectile.Center - Main.screenPosition - new Vector2(0, 6) + MouseToPlayer * 12, new Rectangle(0, 0, 34, 34), drawColor, (float)Main.time * 0.75f, new Vector2(17), 1f, se, 0);
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, drawColor, rot0, texMain.Size() / 2f, 1f, se, 0);

	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return base.Colliding(projHitbox, targetHitbox);
	}
}