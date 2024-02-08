namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CurseClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 57;
		Item.value = 5000;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CurseClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CurseClub_smash>();
	}
}
