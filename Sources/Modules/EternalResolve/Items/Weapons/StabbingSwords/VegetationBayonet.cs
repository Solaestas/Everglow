using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class VegetationBayonet : StabbingSwordItem
	{
		internal int specialDelay = 0;
		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.knockBack = 1.08f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.shoot = ModContent.ProjectileType<VegetationBayonet_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.Vine, 9).
				//任意木头*14
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
		public override void UpdateInventory(Player player)
		{
			if (specialDelay > 360)
			{
				player.AddBuff(BuffID.Rabies, 2);
			}
			if (player.ownedProjectileCounts[Item.shoot] <= 0)
			{
				specialDelay = 0;
			}
		}
	}
}
