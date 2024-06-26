namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CrystalClub_Item : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 51;
		Item.value = 5000;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrystalClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrystalClub_smash>();
	}
}
