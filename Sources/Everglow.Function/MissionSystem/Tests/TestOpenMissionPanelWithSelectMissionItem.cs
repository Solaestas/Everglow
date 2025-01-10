using Everglow.Commons.MissionSystem.TestMissions;
using Everglow.Commons.UI.UIContainers.Mission;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestOpenMissionPanelWithSelectMissionItem : ModItem
{
	public override string Texture => ModAsset.ArrowDown_Mod;

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 20;
	}

	public override bool? UseItem(Player player)
	{
		MissionContainer.Instance.ShowWithMission(TextureMissionIconTestMission.Create().Name);
		return true;
	}
}