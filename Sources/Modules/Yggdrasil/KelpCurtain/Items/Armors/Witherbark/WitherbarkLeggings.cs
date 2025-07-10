using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Witherbark
{
	[AutoloadEquip(EquipType.Legs)]
	public class WitherbarkLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 12;

			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 0, 60, 0);

			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.06f;
		}
	}
}