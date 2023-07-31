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
	public class AbyssBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("");
            // base.Tooltip.SetDefault("");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "渊海重甲");
            // base.Tooltip.AddTranslation(GameCulture.Chinese, "伤害和暴击各增加15%,增加7%闪避");
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "DarkSeaBar", 18);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 30, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 34;
		}
        public override void UpdateEquip(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            mplayer.Misspossibility += 7;
            player.GetCritChance(DamageClass.Generic) += 15;
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetCritChance(DamageClass.Magic) += 15;
            player.GetDamage(DamageClass.Summon) *= 1.15f;
            player.GetDamage(DamageClass.Melee) *= 1.15f;
            player.GetDamage(DamageClass.Throwing) *= 1.15f;
            player.GetDamage(DamageClass.Magic) *= 1.15f;
            player.GetDamage(DamageClass.Ranged) *= 1.15f;
        }
        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
