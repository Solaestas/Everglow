namespace Everglow.Myth.Misc.Items.Accessories;

public class GreenGel : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 34;
		Item.height = 26;
		Item.value = 1563;
		Item.accessory = true;
		Item.rare = 3;
	}
	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.statLifeMax2 += 30;
		player.lifeRegen += 4;
	}
}
