using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons
{
    public class LavaDrink : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔岩畅饮");
            base.Tooltip.SetDefault("");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.thrown = true;
            base.item.damage = 70;
            base.item.crit = 15;
            base.item.width = 40;
            base.item.height = 40;
            base.item.useTime = 18;
            base.item.useAnimation = 18;
            base.item.useStyle = 5;
            base.item.noMelee = true;
            base.item.knockBack = 2f;
            base.item.autoReuse = true;
            base.item.value = Item.sellPrice(0, 1, 0, 0);
            base.item.shoot = base.mod.ProjectileType("LavaDrink");
            base.item.noUseGraphic = true;
            base.item.rare = 6;
            base.item.UseSound = SoundID.Item5;
            base.item.shootSpeed = 27f;
        }
		public override void AddRecipes()
        {
        }
	}
}
