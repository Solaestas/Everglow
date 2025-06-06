using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;

[AutoloadEquip(EquipType.Body)]
public class ShellMolluscsBreastPlate : ModItem
{
	public const int BuffDuration = 25 * 60; // 10 seconds in frames
	public const int CooldownDuration = 40 * 60; // 35 seconds in frames

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
		Item.defense = 5;
	}

	override public void UpdateEquip(Player player)
	{
		player.GetCritChance<GenericDamageClass>() += 6; // Increase critical chance by 6%.
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return (head.type == ModContent.ItemType<MossyMolluscsHelmet>() || head.type == ModContent.ItemType<PearlMolluscsHelmet>())
			&& legs.type == ModContent.ItemType<MolluscsLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		// TODO: Replace mouse click with a hotkey or a more suitable trigger
		if (MouseUtils.MouseMiddle.IsClicked && !player.HasBuff<MolluscsSetCooldown>())
		{
			player.AddBuff(ModContent.BuffType<MolluscsSetBuff>(), BuffDuration);
			player.AddBuff(ModContent.BuffType<MolluscsSetCooldown>(), CooldownDuration);
		}
	}
}