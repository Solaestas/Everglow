using Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;
namespace Everglow.Example.TileLayers;
internal class GetOutOfTheRoom : SidebarElementBase
{
	public override Texture2D Icon => ModAsset.GetOutOfTheRoom.Value;
	public override string Tooltip => "离开房间";

	public override void Invoke()
	{
		base.Invoke();

		Main.NewText("已离开房间");
	}
}