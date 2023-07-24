using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items
{
	public class WaveSaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Wave Chainsaw");
			base.DisplayName.AddTranslation(GameCulture.English, "流波电锯");
		}
		public override void SetDefaults()
		{
			base.item.damage = 24;
			base.item.melee = true;
			base.item.width = 20;
			base.item.height = 12;
			base.item.useTime = 8;
			base.item.useAnimation = 24;
			base.item.channel = true;
			base.item.noMelee = true;
			base.item.autoReuse = true;
			base.item.noUseGraphic = true;
			base.item.tileBoost = 1;
			base.item.axe = 18;
			base.item.useStyle = 5;
			base.item.knockBack = 4f;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item23;
			base.item.value = Item.sellPrice(0, 4, 0, 0);
			base.item.shoot = base.mod.ProjectileType("WaveSawPro");
			base.item.shootSpeed = 40f;
		}
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 30);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
