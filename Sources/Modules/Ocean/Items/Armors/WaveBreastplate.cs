using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        (Terraria.ModLoader.EquipType)1
    })]
	public class WaveBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("");
            // base.Tooltip.SetDefault("");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝胸甲");
            // base.Tooltip.AddTranslation(GameCulture.Chinese, "伤害和暴击各增加14%,增加6%闪避");
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 18);
            modRecipe.AddIngredient(null, "VoidBubble", 9);
            modRecipe.AddIngredient(null, "OceanDustCore", 9);
            modRecipe.AddIngredient(null, "BladeScale", 9);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void UpdateEquip(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            mplayer.Misspossibility += 6;
            player.GetCritChance(DamageClass.Generic) += 14;
            player.GetCritChance(DamageClass.Ranged) += 14;
            player.GetCritChance(DamageClass.Magic) += 14;
            player.GetDamage(DamageClass.Summon) *= 1.14f;
            player.GetDamage(DamageClass.Melee) *= 1.14f;
            player.GetDamage(DamageClass.Throwing) *= 1.14f;
            player.GetDamage(DamageClass.Magic) *= 1.14f;
            player.GetDamage(DamageClass.Ranged) *= 1.14f;
        }
        public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 21, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 30;
		}
        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
