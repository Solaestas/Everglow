namespace Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;

public class SlashAllTheItem : SidebarElementBase
{
	public override Texture2D Icon => ModAsset.SlashAllTheItem.Value;

	public override string Tooltip => "清理背包";

	public override bool Visible => true;

	public override void Invoke()
	{
		base.Invoke();
		for (int i = 0; i < 50; i++)
		{
			var item = Main.LocalPlayer.inventory[i];
			if (!item.favorited)
			{
				item.SetDefaults();
			}
		}
		Main.NewText("已清除背包");
	}
}