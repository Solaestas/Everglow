using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Witherbark;

[AutoloadEquip(EquipType.Head)]
public class WitherbarkHelmet : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 26;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(0, 0, 60, 0);

		Item.defense = 1;
	}

	public override void UpdateEquip(Player player)
	{
		player.maxMinions++;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<WitherbarkBreastPlate>() && legs.type == ModContent.ItemType<WitherbarkLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.maxMinions += 3;
		player.GetDamage<SummonDamageClass>() -= 0.3f;
		player.AddBuff(ModContent.BuffType<WitherbarkSetBuff>(), 2);

		if (player.ownedProjectileCounts[ModContent.ProjectileType<WitherbarkMinion>()] <= 0)
		{
			Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<WitherbarkMinion>(), 100, 0f, player.whoAmI);
		}
	}
}