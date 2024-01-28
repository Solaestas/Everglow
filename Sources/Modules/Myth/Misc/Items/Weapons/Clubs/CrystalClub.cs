namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CrystalClub : ClubItem
{
	//TODO:Translate:水晶棍棒
	public override void SetDef()
	{
		Item.damage = 51;
		Item.value = 5000;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrystalClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrystalClub_smash>();
	}
}
