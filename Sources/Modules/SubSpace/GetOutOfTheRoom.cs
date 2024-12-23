using Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;
using SubworldLibrary;

namespace Everglow.SubSpace;

public class GetOutOfTheRoom : SidebarElementBase
{
	public override Texture2D Icon => ModAsset.GetOutOfTheRoom.Value;

	public override string Tooltip => "离开房间";

	public override bool Visible => SubworldSystem.IsActive<RoomWorld>();

	public override void Invoke()
	{
		base.Invoke();
		RoomManager.ExitALevelOfRoom();
	}
}