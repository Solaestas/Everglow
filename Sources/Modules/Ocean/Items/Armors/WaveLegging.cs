using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        (Terraria.ModLoader.EquipType)2
	})]
	public class WaveLegging : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("");
            base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝护胫");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "增加24%移速,增加7%闪避");
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 15);
            modRecipe.AddIngredient(null, "VoidBubble", 7);
            modRecipe.AddIngredient(null, "OceanDustCore", 7);
            modRecipe.AddIngredient(null, "BladeScale", 7);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Misspossibility += 7;
            player.moveSpeed += 0.24f;
        }
        public override void SetDefaults()
		{
			base.item.width = 18;
			base.item.height = 18;
			base.item.value = Item.buyPrice(0, 21, 0, 0);
			base.item.rare = 11;
			base.item.defense = 17;
		}
	}
}
