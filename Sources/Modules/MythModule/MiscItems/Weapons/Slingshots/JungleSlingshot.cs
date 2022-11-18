namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class JungleSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            Item.damage = 22;
            Item.crit = 4;
            Item.width = 34;
            Item.height = 34;
            ProjType = ModContent.ProjectileType<Projectiles.JungleSlingshot>();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 80, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Vine, 8)
                .AddIngredient(ItemID.Stinger, 6)
                .AddIngredient(ItemID.JungleSpores, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}