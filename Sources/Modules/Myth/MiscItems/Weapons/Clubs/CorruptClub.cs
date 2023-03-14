namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class CorruptClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 12;
			Item.value = 169;
			ProjType = ModContent.ProjectileType<Projectiles.CorruptClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 18)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
