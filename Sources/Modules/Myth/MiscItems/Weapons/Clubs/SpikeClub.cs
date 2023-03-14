namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class SpikeClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 21;
			Item.value = 450;
			ProjType = ModContent.ProjectileType<Projectiles.SpikeClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Spike, 114)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
