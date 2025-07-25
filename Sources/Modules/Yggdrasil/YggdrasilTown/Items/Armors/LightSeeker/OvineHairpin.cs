using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LightSeeker;

[AutoloadEquip(EquipType.Head)]
public class OvineHairpin : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
	}

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = 2500;
		Item.rare = ItemRarityID.Green;
		Item.defense = 1;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<ConcentratingJacket>() && legs.type == ModContent.ItemType<ShadowlessBoots>();
	}

	public override void UpdateArmorSet(Player player)
	{
		Color lightColor = Lighting.GetColor(player.Center.ToTileCoordinates());

		float lightIntensity = MathHelper.Max(MathHelper.Max(lightColor.R, lightColor.G), lightColor.B);
		if (lightIntensity > 220)
		{
			player.GetDamage(DamageClass.Magic) += 0.1f;
		}

		player.manaCost *= 0.9f;
		player.GetDamage(DamageClass.Magic) += 0.05f;
	}

	public override void UpdateEquip(Player player)
	{
		Lighting.AddLight(player.Center, Vector3.one * 0.5f);

		player.GetCritChance(DamageClass.Magic) += 4f;
	}

	public override void AddRecipes()
	{
	}
}