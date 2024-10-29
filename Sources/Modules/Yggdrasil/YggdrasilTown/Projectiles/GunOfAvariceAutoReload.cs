using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class GunOfAvariceAutoReload : ModProjectile
{
	private bool ReloadStatus { get => !(Projectile.ai[0] == 0); }

	private bool HasNotPlayedSound { get; set; } = true;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 62;
		Projectile.height = 32;
		Projectile.timeLeft = Items.Weapons.GunOfAvarice.AutoReloadDuration;
		Projectile.penetrate = -1;
		Projectile.hide = true;
	}

	public override void AI()
	{
		if (Projectile.timeLeft <= 15 && HasNotPlayedSound)
		{
			HasNotPlayedSound = false;
			SoundEngine.PlaySound(new SoundStyle("Everglow/Yggdrasil/YggdrasilTown/Sounds/GunReload2"));
		}

		Console.WriteLine(Owner.HeldItem);
	}
}