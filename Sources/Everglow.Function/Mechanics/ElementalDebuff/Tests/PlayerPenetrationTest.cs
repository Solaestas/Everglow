namespace Everglow.Commons.Mechanics.ElementalDebuff.Tests;

public class PlayerPenetrationTest : ModItem
{
	public override string Texture => ModAsset.Point_Mod;

	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 32;

		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetModPlayer<ElementalDebuffPlayer>().ElementPenetration += 0.1f;
	}
}