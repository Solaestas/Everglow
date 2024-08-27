namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class IchorClub_Item : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 47;
		Item.value = 5000;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IchorClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IchorClub_smash>();
	}
}