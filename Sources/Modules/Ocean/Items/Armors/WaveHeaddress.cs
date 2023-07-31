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
	public class WaveHeaddress : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
			// base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝头饰");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "魔法伤害和暴击各增加14%,法力上限增加100");
		}
		public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.buyPrice(0, 21, 0, 0);
			base.Item.rare = 11;
			base.Item.defense = 16;
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.AddIngredient(null, "VoidBubble", 6);
            modRecipe.AddIngredient(null, "OceanDustCore", 6);
            modRecipe.AddIngredient(null, "BladeScale", 6);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void UpdateEquip(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            player.statManaMax += 100;
            player.GetCritChance(DamageClass.Magic) += 14;
            player.GetDamage(DamageClass.Magic) *= 1.14f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<Everglow.Ocean.Items.WaveBreastplate>() && legs.type == ModContent.ItemType<Everglow.Ocean.Items.WaveLegging>();
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            player.setBonus = "敌人靠近时放出有追踪效果的爆炸泡泡\n敌人越多效果越强\n魔法伤害提高12%,魔力消耗减少18%,魔法暴击率提高12%";
            mplayer.bubble = 2;
            player.GetCritChance(DamageClass.Magic) += 12;
            player.GetDamage(DamageClass.Magic) *= 1.12f;
            player.manaCost *= 0.82f;
        }
    }
}
