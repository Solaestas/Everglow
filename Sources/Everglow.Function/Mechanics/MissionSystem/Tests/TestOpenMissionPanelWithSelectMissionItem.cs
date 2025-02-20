using Everglow.Commons.Mechanics.MissionSystem.UI;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

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
		MissionContainer.Instance.ShowWithMission(new TextureMissionIconTestMission().Name);
		return true;
	}
}