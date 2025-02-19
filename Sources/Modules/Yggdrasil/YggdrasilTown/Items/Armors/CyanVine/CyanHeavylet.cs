using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.CyanVine;

[AutoloadEquip(EquipType.Head)]
public class CyanHeavylet : ModItem
{
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
		Item.defense = 4;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<CyanBreastplate>() && legs.type == ModContent.ItemType<CyanLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.statDefense += 3;
		player.lifeRegen += 2;
		player.runAcceleration += 0.008f; // 通过命中断点获得玩家的基础跑步加速度为0.08，提升10%也就是0.008
	}

	public override void UpdateEquip(Player player)
	{
		player.endurance += 0.04f;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();

		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}