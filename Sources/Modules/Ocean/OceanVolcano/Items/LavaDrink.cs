using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons
{
    public class LavaDrink : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("熔岩畅饮");
            // base.Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.DamageType = DamageClass.Throwing;
            base.Item.damage = 70;
            base.Item.crit = 15;
            base.Item.width = 40;
            base.Item.height = 40;
            base.Item.useTime = 18;
            base.Item.useAnimation = 18;
            base.Item.useStyle = 5;
            base.Item.noMelee = true;
            base.Item.knockBack = 2f;
            base.Item.autoReuse = true;
            base.Item.value = Item.sellPrice(0, 1, 0, 0);
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.LavaDrink>();
            base.Item.noUseGraphic = true;
            base.Item.rare = 6;
            base.Item.UseSound = SoundID.Item5;
            base.Item.shootSpeed = 27f;
        }
		public override void AddRecipes()
        {
        }
	}
}
