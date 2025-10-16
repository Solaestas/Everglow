using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class IronClub_Item : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 85;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IronClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IronClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.IronBar, 12)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
