using Terraria.Audio;
namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CyanVineThrowingSpear_Proj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1500;
		Projectile.aiStyle = -1;
	}
	internal bool Shot = false;
	internal int Power = 0;
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		int PlayerDir = -1;
		if (Main.MouseWorld.X > player.Center.X)
			PlayerDir = 1;
		if (Shot)
		{
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.velocity.Y += 0.25f;
			Projectile.velocity *= 0.995f;
			Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
		}
		else
		{
			Projectile.timeLeft = 1500;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.MountedCenter, new Vector2(0, -1 * player.gravDir));
			Projectile.Center = player.MountedCenter + Projectile.velocity.RotatedBy(Math.PI * -0.5) * 20 * PlayerDir - Projectile.velocity * (Power / 3f - 54) + new Vector2(0, 6 * player.gravDir);
			Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
			if (Power < 100)
				Power += 1;

			player.heldProj = Projectile.whoAmI;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 0.337f * PlayerDir + (float)(Math.PI * 0.25 + Math.PI * 0.6 * PlayerDir - (Power / 80d + 0.2) * PlayerDir));
			player.direction = PlayerDir;
		}

		if (player.controlUseItem && !Shot)
		{
			Shot = true;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.MountedCenter, new Vector2(0, -1 * player.gravDir)) * (Power + 100) / 8f;
			Projectile.damage = (int)(Projectile.damage * (Power / 30f + 1));
			SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
		}
		if (player.HeldItem.type != ModContent.ItemType<Items.CyanVine.CyanVineThrowingSpear>())
		{
			Projectile.active = false;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModAsset.CyanVineThrowingSpear_Proj.Value;
		Texture2D flag = ModAsset.CyanVineThrowingSpear_flag.Value;


		Vector2 redKnotPos = Projectile.Center - Main.screenPosition - Utils.SafeNormalize(Projectile.velocity, Vector2.zeroVector) * 40;
		Player player = Main.player[Projectile.owner];

		if(!Shot)
		{
			Main.spriteBatch.Draw(flag, redKnotPos, null, lightColor, 0, new Vector2(4, 0), Projectile.scale, SpriteEffects.None, 0);
		}
		else
		{
			Vector2 flagVec = Vector2.Lerp(Projectile.velocity, new Vector2(0, -10), 0.15f + MathF.Sin(Main.windSpeedCurrent + (float)Main.time * 0.9f + Projectile.whoAmI) * 0.25f);
			float rot = flagVec.ToRotation() + MathHelper.PiOver2;
			Main.spriteBatch.Draw(flag, redKnotPos, null, lightColor, rot, new Vector2(4, 0), Projectile.scale, SpriteEffects.None, 0);
		}
		Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition - Utils.SafeNormalize(Projectile.velocity, Vector2.zeroVector) * 40, null, lightColor, Projectile.rotation + 0.337f, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public override void OnKill(int timeLeft)
	{
		for (int x = 0; x < 16; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position, 40, 40, ModContent.DustType<Dusts.CyanVine>());
			d.velocity *= Projectile.velocity.Length() / 10f;
		}
		SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);

		Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d) * Projectile.velocity.Length() / 10f;
		Gore.NewGore(null, Projectile.Center + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineThrowingSpearGore0").Type, 1f);

		vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d) * Projectile.velocity.Length() / 10f;
		Gore.NewGore(null, Projectile.Center + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineThrowingSpearGore1").Type, 1f);

		vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d) * Projectile.velocity.Length() / 10f;
		Gore.NewGore(null, Projectile.Center + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineOre" + Main.rand.Next(11, 13).ToString()).Type, 1f);
	}
}

