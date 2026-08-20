using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class WorldMissionViewAdapterTest
{
	[TestMethod]
	public void Create_IncludesSourceAndObjectiveIcons()
	{
		var source = new StubSource("world-source");
		var objectiveIcon = new StubIcon();
		var mission = new StubMission { SourceValue = source };
		mission.Objectives.Add(new StubObjective { Icon = objectiveIcon });

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.HasCount(2, view.Icons);
		var sourceIcon = (MissionSourceIcon)view.Icons[0];
		Assert.AreSame(source, sourceIcon.Source);
		Assert.IsNull(sourceIcon.SubSource);
		Assert.AreSame(objectiveIcon, view.Icons[1]);
	}

	[TestMethod]
	public void Create_NoObjectiveIconsProducesSourceIcon()
	{
		var mission = new StubMission();

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.HasCount(1, view.Icons);
		Assert.IsInstanceOfType<MissionSourceIcon>(view.Icons[0]);
	}
}
