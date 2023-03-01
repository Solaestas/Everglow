namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class SapphireSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.SapphireSlingshot>();
            Item.damage = 19;
            Item.width = 38;
            Item.height = 36;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.rare = 3;
            Item.value = Item.sellPrice(0, 0, 12, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sapphire, 8)
                .AddIngredient(ItemID.SilverBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
