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
	public class AbyssHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
			// base.DisplayName.AddTranslation(GameCulture.Chinese, "渊海头盔");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "远程伤害和暴击各增加13%");
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
            player.GetCritChance(DamageClass.Ranged) += 13;
            player.GetDamage(DamageClass.Ranged) *= 1.13f;
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
            player.setBonus = "水下受到伤害减半\n远程伤害提高11%,远程暴击率提高16%";
            mplayer.ab = 2;
            player.GetCritChance(DamageClass.Ranged) += 12;
            player.GetDamage(DamageClass.Ranged) *= 1.11f;
        }
    }
}
