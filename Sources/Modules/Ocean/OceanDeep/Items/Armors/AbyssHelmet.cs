using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Armors
{
	[AutoloadEquip(new EquipType[]
	{
        0
	})]
	public class AbyssHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
			base.DisplayName.AddTranslation(GameCulture.Chinese, "渊海头盔");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "远程伤害和暴击各增加13%");
		}
		public override void SetDefaults()
		{
			base.item.width = 18;
			base.item.height = 18;
			base.item.value = Item.buyPrice(0, 30, 0, 0);
			base.item.rare = 11;
			base.item.defense = 18;
		}
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "DarkSeaBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            player.rangedCrit += 13;
            player.rangedDamage *= 1.13f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == base.mod.ItemType("AbyssBreastplate") && legs.type == base.mod.ItemType("AbyssLegging");
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            player.setBonus = "水下受到伤害减半\n远程伤害提高11%,远程暴击率提高16%";
            mplayer.ab = 2;
            player.rangedCrit += 12;
            player.rangedDamage *= 1.11f;
        }
    }
}
