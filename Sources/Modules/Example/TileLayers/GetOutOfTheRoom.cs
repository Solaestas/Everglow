using Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;
namespace Everglow.Example.TileLayers;
internal class GetOutOfTheRoom : SidebarElementBase
{
	public override Texture2D Icon => ModAsset.GetOutOfTheRoom.Value;
	public override string Tooltip => "离开房间";

	public override void Invoke()
	{
		base.Invoke();
		Player player = Main.LocalPlayer;
		int x = (int)(player.Center.X / 16);
		int y = (int)(player.Center.Y / 16);
		TileLayerSystem.LayerChange(Main.LocalPlayer, x, y, TileLayerSystem.PlayerZoneLayer[player.whoAmI] + 1);
		Main.NewText("已离开房间");
	}
}