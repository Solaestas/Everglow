using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.CyanVine;

[AutoloadEquip(EquipType.Head)]
public class CyanWarhelm : ModItem
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
		Item.defense = 3;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<CyanBreastplate>() && legs.type == ModContent.ItemType<CyanLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.GetDamage(DamageClass.Melee) += 0.01f;
		player.GetArmorPenetration(DamageClass.Melee) += 5;
	}

	public override void UpdateEquip(Player player)
	{
		player.GetDamage(DamageClass.Melee) += 0.04f;
		player.GetCritChance(DamageClass.Melee) += 4;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();

		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}