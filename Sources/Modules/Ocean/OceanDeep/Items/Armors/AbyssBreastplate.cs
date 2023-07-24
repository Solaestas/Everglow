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
	public class AbyssBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("");
            base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "渊海重甲");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "伤害和暴击各增加15%,增加7%闪避");
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "DarkSeaBar", 18);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void SetDefaults()
		{
			base.item.width = 18;
			base.item.height = 18;
			base.item.value = Item.buyPrice(0, 30, 0, 0);
			base.item.rare = 11;
			base.item.defense = 34;
		}
        public override void UpdateEquip(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Misspossibility += 7;
            player.meleeCrit += 15;
            player.rangedCrit += 15;
            player.magicCrit += 15;
            player.minionDamage *= 1.15f;
            player.meleeDamage *= 1.15f;
            player.thrownDamage *= 1.15f;
            player.magicDamage *= 1.15f;
            player.rangedDamage *= 1.15f;
        }
        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
