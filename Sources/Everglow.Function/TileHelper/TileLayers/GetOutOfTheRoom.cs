using Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;
namespace Everglow.Commons.TileHelper.TileLayers;
internal class GetOutOfTheRoom : SidebarElementBase
{
	public override Texture2D Icon => ModAsset.GetOutOfTheRoom.Value;
	public override string Tooltip => "离开房间";

	public override void Invoke()
	{
		base.Invoke();
		Player player = Main.LocalPlayer;
		if (TileLayerSystem.PlayerZoneLayer[player.whoAmI] < 0)
		{
			int x = (int)(player.Center.X / 16);
			int y = (int)(player.Center.Y / 16);
			TileLayerSystem.LayerShallower(player, x, y);
			Main.NewText("已离开房间");
		}
		else
		{
			Main.NewText("你不在任何房间");
		}
	}
}