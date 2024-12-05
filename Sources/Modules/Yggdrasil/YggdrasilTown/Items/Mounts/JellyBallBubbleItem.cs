namespace Everglow.Yggdrasil.YggdrasilTown.Items.Mounts;

public class JellyBallBubbleItem : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 64;
		Item.height = 64;
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Swing;

		// TODO: Adjust sound
		Item.UseSound = SoundID.Item3;
		Item.noMelee = true;
		Item.mountType = ModContent.MountType<JellyBallBubble>();

		Item.value = Item.buyPrice(silver: 63, copper: 50);
		Item.rare = ItemRarityID.Blue;
	}
}