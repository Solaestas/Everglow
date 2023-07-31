using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items
{
	public class WaveSaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("Wave Chainsaw");
			// base.DisplayName.AddTranslation(GameCulture.English, "流波电锯");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 24;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			base.Item.width = 20;
			base.Item.height = 12;
			base.Item.useTime = 8;
			base.Item.useAnimation = 24;
			base.Item.channel = true;
			base.Item.noMelee = true;
			base.Item.autoReuse = true;
			base.Item.noUseGraphic = true;
			base.Item.tileBoost = 1;
			base.Item.axe = 18;
			base.Item.useStyle = 5;
			base.Item.knockBack = 4f;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item23;
			base.Item.value = Item.sellPrice(0, 4, 0, 0);
			base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.WaveSawPro>();
			base.Item.shootSpeed = 40f;
		}
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 30);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
    }
}
