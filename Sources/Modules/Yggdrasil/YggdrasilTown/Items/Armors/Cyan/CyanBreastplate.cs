using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Cyan
{
	[AutoloadEquip(EquipType.Body)]
	public class CyanBreastplate : ModItem
	{
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(20, 20);
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = 3750;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.jumpSpeedBoost += 0.05f;
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{

		}
	}
}
