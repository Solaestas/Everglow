using Everglow.Myth.TheFirefly.Buffs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.Localization;

namespace Everglow.Myth.TheFirefly.Items.Potions
{
	public class ShadowPotion : ModItem
	{
		public static LocalizedText RestoreLifeText { get; private set; }

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 30;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
			Item.buffType = ModContent.BuffType<ShadowPotionBuff>();
			Item.buffTime = 14400;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//TODO: Use actual tooltips
		}
	}
}