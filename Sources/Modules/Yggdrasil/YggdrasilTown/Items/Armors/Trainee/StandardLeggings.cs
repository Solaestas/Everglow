using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Trainee
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class StandardLeggings : ModItem
	{


		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(3);//TODO
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = 1000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			player.jumpSpeedBoost += 5;
			player.moveSpeed += 5;
		}
		public override void AddRecipes()
		{

		}
	}
}
