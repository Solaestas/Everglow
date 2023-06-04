using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class PlatinumStabbingSword : StabbingSwordItem
	{
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.knockBack = 1.91f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.shoot = ModContent.ProjectileType<PlatinumStabbingSword_Pro>();
			PowerfulStabProj = 1;
            base.SetDefaults();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PlatinumBar, 17).
                AddTile(TileID.Anvils).
                Register();
            base.AddRecipes();
        }
    }
}