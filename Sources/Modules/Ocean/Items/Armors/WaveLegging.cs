using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        (Terraria.ModLoader.EquipType)2
	})]
	public class WaveLegging : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("");
            // base.Tooltip.SetDefault("");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝护胫");
            // base.Tooltip.AddTranslation(GameCulture.Chinese, "增加24%移速,增加7%闪避");
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 15);
            modRecipe.AddIngredient(null, "VoidBubble", 7);
            modRecipe.AddIngredient(null, "OceanDustCore", 7);
            modRecipe.AddIngredient(null, "BladeScale", 7);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void UpdateEquip(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            mplayer.Misspossibility += 7;
            player.moveSpeed += 0.24f;
        }
        public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 21, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 17;
		}
	}
}
