using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.SquamousShell;

[AutoloadEquip(EquipType.Wings)]
public class BlueyWings : ModItem
{
	public override void SetStaticDefaults()
	{
		ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 6.25f, 1.5f);
	}

	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 20;
		Item.value = Item.buyPrice(gold: 1, silver: 33);
		Item.rare = ItemRarityID.Green;
		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetModPlayer<BlueyWingsPlayer>().EnableBlueyWings = true;
	}
}

public class BlueyWingsPlayer : ModPlayer
{
	public bool EnableBlueyWings { get; set; } = false;

	public override void ResetEffects()
	{
		EnableBlueyWings = false;
	}

	public override void PostUpdate()
	{
		bool inYggdrasilTownBiome = Player.InModBiome(ModContent.GetInstance<YggdrasilTownBiome>());
	}
}