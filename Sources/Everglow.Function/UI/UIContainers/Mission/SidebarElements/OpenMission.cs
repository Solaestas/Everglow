using Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;

namespace Everglow.Commons.UI.UIContainers.Mission.SidebarElements;

internal class OpenMission : SidebarElementBase
{
	public override Texture2D Icon => ModAsset.OpenMission.Value;

	public override string Tooltip => "打开任务面板";

	public override void Invoke()
	{
		base.Invoke();
		if (MissionContainer.Instance.IsVisible)
		{
			MissionContainer.Instance.Close();
		}
		else
		{
			MissionContainer.Instance.Show();
		}
	}
}