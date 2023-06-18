using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Myth.Bosses.EvilBottle.Items
{
    public class EvilBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("魔焰法瓶");
            // base.Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            base.Item.damage = 22;
            base.Item.DamageType = DamageClass.Throwing;
            base.Item.crit = 6;
            base.Item.width = 20;
            base.Item.height = 38;
            base.Item.useTime = 48;
            base.Item.useAnimation = 48;
            base.Item.useStyle = 5;
            base.Item.noMelee = true;
            base.Item.knockBack = 2f;
            base.Item.autoReuse = true;
            base.Item.value = Item.sellPrice(0, 0, 1, 0);
            base.Item.shoot = ModContent.ProjectileType<Projectiles.EvilBomb>();
            base.Item.noUseGraphic = true;
            base.Item.rare = 3;
            base.Item.UseSound = SoundID.Item5;
            base.Item.shootSpeed = 7f;
        }
		public override void AddRecipes()
        {
        }
	}
}
