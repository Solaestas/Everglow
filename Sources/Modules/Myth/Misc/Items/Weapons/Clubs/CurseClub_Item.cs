using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CurseClub_Item : ClubItem
{
	public override void SetCustomDefaults()
	{
		Item.damage = 57;
		Item.value = 5000;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CurseClub>();
		ProjSmashType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CurseClub_smash>();
	}
}