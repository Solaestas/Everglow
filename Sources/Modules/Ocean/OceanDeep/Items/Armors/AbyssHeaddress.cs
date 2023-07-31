using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        0
	})]
	public class AbyssHeaddress : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
			// base.DisplayName.AddTranslation(GameCulture.Chinese, "渊海头饰");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "魔法伤害和暴击各增加18%,法力上限增加120");
		}
		public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 30, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 18;
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "DarkSeaBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void UpdateEquip(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            player.statManaMax += 120;
            player.GetCritChance(DamageClass.Magic) += 18;
            player.GetDamage(DamageClass.Magic) *= 1.18f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<Everglow.Ocean.Items.AbyssBreastplate>() && legs.type == ModContent.ItemType<Everglow.Ocean.Items.AbyssLegging>();
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            player.setBonus = "水下受到伤害减半\n魔法伤害提高15%,魔力消耗减少21%,魔法暴击率提高15%";
            mplayer.ab = 2;
            player.GetCritChance(DamageClass.Magic) += 15;
            player.GetDamage(DamageClass.Magic) *= 1.15f;
            player.manaCost *= 0.79f;
        }
    }
}
