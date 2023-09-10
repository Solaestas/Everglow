using System;
using Everglow.Ocean.Common;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.OceanDeep.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        0
	})]
	public class AbyssMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
			// base.// DisplayName.AddTranslation(GameCulture.Chinese, "渊海面具");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "近战伤害和暴击各增加12%");
		}
		public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 30, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 40;
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
            player.GetCritChance(DamageClass.Generic) += 12;
            player.GetDamage(DamageClass.Melee) *= 1.12f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.Armors.AbyssBreastplate>() && legs.type == ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.Armors.AbyssLegging>();
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            player.setBonus = "敌人靠近时放出有追踪效果的爆炸泡泡\n敌人越多效果越强";
            mplayer.bubble = 2;
            player.GetCritChance(DamageClass.Generic) += 10;
            player.GetDamage(DamageClass.Melee) *= 1.1f;
        }
    }
}
