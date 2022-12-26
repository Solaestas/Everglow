﻿using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class TopazSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.TopazSlingshot>();
            Item.damage = 17;
            Item.width = 38;
            Item.height = 36;
            Item.useTime = 39;
            Item.useAnimation = 39;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 10, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Topaz, 8)
                .AddIngredient(ItemID.TinBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
