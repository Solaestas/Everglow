using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;

[AutoloadEquip(EquipType.Head)]
public class MossyMolluscsHelmet : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;
		Item.value = Item.buyPrice(silver: 60);
		Item.rare = ItemRarityID.Green;
		Item.defense = 2;
	}

	override public void UpdateEquip(Player player)
	{
		player.GetDamage<RangedDamageClass>() += 0.04f; // // Increases ranged damage by 4%.
		player.GetCritChance<RangedDamageClass>() += 6; // Increase ranged critical chance by 6%.
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<ShellMolluscsBreastPlate>() && legs.type == ModContent.ItemType<MolluscsLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.armorPenetration += 3; // Increases armor penetration by 3.
		player.GetModPlayer<MolluscsRangedSetPlayer>().Enabled = true; // 15% chance to not consume ammo
	}

	public class MolluscsRangedSetPlayer : ModPlayer
	{
		public bool Enabled { get; set; } = false;

		public override void ResetEffects()
		{
			Enabled = false;
		}

		public override bool CanConsumeAmmo(Item weapon, Item ammo)
		{
			return Enabled ? Main.rand.NextFloat() >= 0.15f : true; // 15% chance to not consume ammo when using ranged weapons while wearing the Molluscs armor set.
		}
	}
}