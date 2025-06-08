using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Ruin;

[AutoloadEquip(EquipType.Head)]
public class RuinMask : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 18;
		Item.height = 18;

		Item.defense = 1;

		Item.value = Item.buyPrice(gold: 1);
		Item.rare = ItemRarityID.Gray;
	}

	public override void UpdateEquip(Player player)
	{
		player.maxMinions += 1;
		player.statManaMax2 += 40;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<RuinMagicRobe>() && legs.type == ModContent.ItemType<RuinLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.maxMinions += 1;
		player.GetModPlayer<RuinSetPlayer>().RuinSetEnable = true;
	}

	public class RuinSetPlayer : ModPlayer
	{
		public bool RuinSetEnable { get; set; } = false;

		public bool RuinSetBuffActive => RuinSetEnable && Player.HasBuff<RuinSetBuff>();

		public override void ResetEffects()
		{
			RuinSetEnable = false;
		}

		public override void UpdateEquips()
		{
			if (RuinSetEnable)
			{
				if (MouseUtils.MouseMiddle.IsClicked && Player.HasBuff<RuinSetCooldown>())
				{
					Player.AddBuff(ModContent.BuffType<RuinSetBuff>(), 30 * 60);
					Player.AddBuff(ModContent.BuffType<RuinSetCooldown>(), 120 * 60);
				}
			}
		}
	}
}