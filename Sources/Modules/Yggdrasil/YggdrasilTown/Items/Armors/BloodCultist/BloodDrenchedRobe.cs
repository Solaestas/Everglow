using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.BloodCultist
{
	[AutoloadEquip(EquipType.Body)]
	public class BloodDrenchedRobe : ModItem
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
			Item.value = 2500;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Magic) += 6f;
		}
		public override void AddRecipes()
		{

		}
	}
}
