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
	public class AbyssHeaddress : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
			base.DisplayName.AddTranslation(GameCulture.Chinese, "渊海头饰");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "魔法伤害和暴击各增加18%,法力上限增加120");
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
            player.statManaMax += 120;
            player.magicCrit += 18;
            player.magicDamage *= 1.18f;
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
            player.setBonus = "水下受到伤害减半\n魔法伤害提高15%,魔力消耗减少21%,魔法暴击率提高15%";
            mplayer.ab = 2;
            player.magicCrit += 15;
            player.magicDamage *= 1.15f;
            player.manaCost *= 0.79f;
        }
    }
}
