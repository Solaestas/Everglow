using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;
using Terraria.GameContent.Creative;
using Terraria.GameInput;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Auburn;

[AutoloadEquip(EquipType.Head)]
public class AuburnHoodie : ModItem
{
	public const int BuffDuration = 45 * 60;
	public const int BuffCooldown = 60 * 60;

	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 22;
		Item.value = 2500;
		Item.rare = ItemRarityID.White;
		Item.defense = 1;
	}

	public override void UpdateEquip(Player player)
	{
		player.minionDamage += 0.08f;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<AuburnBreastplate>() && legs.type == ModContent.ItemType<AuburnBoots>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.maxMinions += 1;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient<LampWood_Wood>(30);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}

public class AuburnArmorSetPlayer : ModPlayer
{
	public bool EnableAuburnArmorSet { get; set; } = false;

	public override void ProcessTriggers(TriggersSet triggersSet)
	{
		if (EnableAuburnArmorSet)
		{
			return;
		}

		if (Player.dead)
		{
			return;
		}

		// TODO: add hotkey
		var ArmorSetBonusHotKey = new
		{
			JustPressed = false,
		};

		if (Main.myPlayer != Player.whoAmI || !ArmorSetBonusHotKey.JustPressed)
		{
			return;
		}

		if (Player.HasBuff<AuburnSelfReinforcing>() ||
			Player.HasBuff<AuburnSelfReinforcingCooldown>())
		{
			return;
		}

		Player.AddBuff(ModContent.BuffType<AuburnSelfReinforcing>(), AuburnHoodie.BuffDuration);
	}
}