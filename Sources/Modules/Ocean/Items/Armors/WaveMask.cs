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
	public class WaveMask : ModItem
	{
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("");
            base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝面具");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "近战伤害和暴击各增加10%");
        }
        public override void SetDefaults()
        {
            base.item.width = 18;
            base.item.height = 18;
            base.item.value = Item.buyPrice(0, 21, 0, 0);
            base.item.rare = 11;
            base.item.defense = 36;
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
            player.meleeCrit += 10;
            player.meleeDamage *= 1.1f;
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
            player.setBonus = "敌人靠近时放出有追踪效果的爆炸泡泡\n敌人越多效果越强\n近战伤害提高10%,近战暴击提高10%";
            mplayer.bubble = 2;
            player.meleeCrit += 10;
            player.meleeDamage *= 1.1f;
        }
    }
}
