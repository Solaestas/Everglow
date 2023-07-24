using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        (Terraria.ModLoader.EquipType)1
    })]
	public class WaveBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("");
            base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝胸甲");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "伤害和暴击各增加14%,增加6%闪避");
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 18);
            modRecipe.AddIngredient(null, "VoidBubble", 9);
            modRecipe.AddIngredient(null, "OceanDustCore", 9);
            modRecipe.AddIngredient(null, "BladeScale", 9);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Misspossibility += 6;
            player.meleeCrit += 14;
            player.rangedCrit += 14;
            player.magicCrit += 14;
            player.minionDamage *= 1.14f;
            player.meleeDamage *= 1.14f;
            player.thrownDamage *= 1.14f;
            player.magicDamage *= 1.14f;
            player.rangedDamage *= 1.14f;
        }
        public override void SetDefaults()
		{
			base.item.width = 18;
			base.item.height = 18;
			base.item.value = Item.buyPrice(0, 21, 0, 0);
			base.item.rare = 11;
			base.item.defense = 30;
		}
        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
