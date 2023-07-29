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
	public class AbyssLegging : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("");
            // base.Tooltip.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "渊海护胫");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "增加27%移速,增加8%闪避");
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "DarkSeaBar", 15);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 30, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 20;
		}
        public override void UpdateEquip(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            mplayer.Misspossibility += 8;
            player.moveSpeed += 0.27f;
        }
    }
}
