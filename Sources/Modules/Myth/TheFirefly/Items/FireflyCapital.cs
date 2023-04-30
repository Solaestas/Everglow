using Everglow.Myth.TheFirefly.WorldGeneration;

namespace Everglow.Myth.TheFirefly.Items;

public class FireflyCapital : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;
		Item.maxStack = 999;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 7;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = false;
	}
	public override bool? UseItem(Player player)
	{
		if (player.itemAnimation == player.itemAnimationMax)
		{
			MothLand mothLand = ModContent.GetInstance<MothLand>();
			player.position = new Vector2(mothLand.fireflyCenterX, mothLand.fireflyCenterY) * 16;
			//if (SubWorldModule.SubworldSystem.IsActive<MothWorld>())
			//	SubWorldModule.SubworldSystem.Exit();
			//else
			//{
			//	SubworldSystem.Enter<MothWorld>();
			//	if (!SubWorldModule.SubworldSystem.Enter<MothWorld>())
			//		Main.NewText("Fail!");
			//}
		}
		return base.UseItem(player);
	}
}