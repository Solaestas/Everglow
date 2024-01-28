namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class SpikeClub : ClubItem
{
	//TODO:Translate:尖刺棍棒
	public override void SetDef()
	{
		Item.damage = 21;
		Item.value = 450;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.SpikeClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.SpikeClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Spike, 114)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
