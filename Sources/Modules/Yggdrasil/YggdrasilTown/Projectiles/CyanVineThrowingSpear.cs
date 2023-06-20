using Everglow.Yggdrasil.Common;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.Audio;
namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CyanVineThrowingSpear : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.width = 40;
		Projectile.height = 40;
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
			Projectile.velocity.Y += 0.25f;
			Projectile.velocity *= 0.995f;
			Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
		}
		else
		{
			Projectile.timeLeft = 1500;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.Center, new Vector2(0, -1 * player.gravDir));
			Projectile.Center = player.Center + Projectile.velocity.RotatedBy(Math.PI * -0.5) * 20 * PlayerDir - Projectile.velocity * (Power / 3f - 16);
			Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
			if (Power < 100)
				Power++;

			player.heldProj = Projectile.whoAmI;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + (float)(Math.PI * 0.25 + Math.PI * 0.6 * PlayerDir - (Power / 40d - 1.0) * PlayerDir));
			player.direction = PlayerDir;
		}

		if (!player.controlUseItem && !Shot)
		{
			Shot = true;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.Center, new Vector2(0, -1 * player.gravDir)) * (Power + 100) / 8f;
			Projectile.damage = (int)(Projectile.damage * (Power + 100) / 100f);
			SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModAsset.Projectiles_CyanVineThrowingSpear.Value;
		Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public override void Kill(int timeLeft)
	{
		for (int x = 0; x < 16; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position, 40, 40, ModContent.DustType<CyanVine>());
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

