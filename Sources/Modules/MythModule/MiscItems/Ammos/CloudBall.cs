﻿using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Ammos
{
    public class CloudBall : ModItem
    {
        /*public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloud Cluster");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "云团");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Облачный кластер");
        }*/

        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 14;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.CloudBall>();
            Item.shootSpeed = 4;
            Item.crit = 2;
            Item.ammo = Item.type;
            Item.maxStack = 999;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ItemID.Cloud)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}
