﻿namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class AmethystSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.AmethystSlingshot>();
            Item.damage = 15;
            Item.width = 38;
            Item.height = 36;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.rare = 3;
            Item.value = Item.sellPrice(0, 0, 8, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Amethyst, 8)
                .AddIngredient(ItemID.CopperBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
