namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class MeteorClub : ClubItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}
		public static short GetGlowMask = 0;
		public override void SetDef()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.damage = 16;
			Item.value = 576;
			ProjType = ModContent.ProjectileType<Projectiles.MeteorClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.MeteoriteBar, 18)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
