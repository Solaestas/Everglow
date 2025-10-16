using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CobaltClub_Item : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 41;
		Item.value = 2005;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CobaltClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CobaltClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CobaltBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
