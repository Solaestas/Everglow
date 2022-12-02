namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class StarSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            Item.damage = 18;
            Item.crit = 12;
            Item.width = 32;
            Item.height = 30;
            ProjType = ModContent.ProjectileType<Projectiles.StarSlingshot>();
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 25)
                .AddIngredient(ItemID.SunplateBlock, 30)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}