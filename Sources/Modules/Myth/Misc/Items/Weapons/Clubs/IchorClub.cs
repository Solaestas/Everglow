namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class IchorClub : ClubItem
{
	//TODO:Translate:灵液棍棒
	public override void SetDef()
	{
		Item.damage = 47;
		Item.value = 5000;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IchorClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IchorClub_smash>();
	}
}