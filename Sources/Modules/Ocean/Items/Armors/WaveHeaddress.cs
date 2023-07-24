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
	public class WaveHeaddress : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
			base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝头饰");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "魔法伤害和暴击各增加14%,法力上限增加100");
		}
		public override void SetDefaults()
		{
			base.item.width = 18;
			base.item.height = 18;
			base.item.value = Item.buyPrice(0, 21, 0, 0);
			base.item.rare = 11;
			base.item.defense = 16;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.AddIngredient(null, "VoidBubble", 6);
            modRecipe.AddIngredient(null, "OceanDustCore", 6);
            modRecipe.AddIngredient(null, "BladeScale", 6);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            player.statManaMax += 100;
            player.magicCrit += 14;
            player.magicDamage *= 1.14f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == base.mod.ItemType("WaveBreastplate") && legs.type == base.mod.ItemType("WaveLegging");
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            player.setBonus = "敌人靠近时放出有追踪效果的爆炸泡泡\n敌人越多效果越强\n魔法伤害提高12%,魔力消耗减少18%,魔法暴击率提高12%";
            mplayer.bubble = 2;
            player.magicCrit += 12;
            player.magicDamage *= 1.12f;
            player.manaCost *= 0.82f;
        }
    }
}
