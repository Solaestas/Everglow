using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CrystalClub_Item : ClubItem
{
	public override void SetCustomDefaults()
	{
		Item.damage = 51;
		Item.value = 5000;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrystalClub>();
		ProjSmashType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrystalClub_smash>();
	}
}
