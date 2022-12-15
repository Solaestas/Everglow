﻿namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class DiamondSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.DiamondSlingshot>();
            Item.damage = 25;
            Item.width = 38;
            Item.height = 36;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.rare = 3;
            Item.value = Item.sellPrice(0, 0, 10, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Diamond, 8)
                .AddIngredient(ItemID.PlatinumBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}