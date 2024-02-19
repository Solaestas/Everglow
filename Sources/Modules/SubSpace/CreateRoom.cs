using SubworldLibrary;

namespace Everglow.SubSpace;

public class CreateRoom : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;
		Item.maxStack = 1;
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
			Main.NewText("maxTilesX : " + Main.maxTilesX + " ,maxTilesY : " + Main.maxTilesY);
			Point point = new Point((int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f));
			if (SubworldSystem.IsActive<RoomWorld>())
				SubworldSystem.Exit();
			else
			{
				if (!SubworldSystem.Enter<RoomWorld>())
					Main.NewText("Fail!");
			}
		}
		return base.UseItem(player);
	}
}