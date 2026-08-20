using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class PlayerMissionViewAdapterTest
{
	[TestMethod]
	public void Create_IncludesSourceAndObjectiveIcons()
	{
		var source = new StubSource("source");
		var subSource = new StubSource("sub-source");
		var icon = new StubIcon();
		var mission = new StubMission
		{
			SourceValue = source,
			SubSourceValue = subSource,
		};
		mission.Objectives.Add(new StubObjective("objective") { Icon = icon });

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.HasCount(2, view.Icons);
		var sourceIcon = (MissionSourceIcon)view.Icons[0];
		Assert.AreSame(source, sourceIcon.Source);
		Assert.AreSame(subSource, sourceIcon.SubSource);
		Assert.AreSame(icon, view.Icons[1]);
	}

	[TestMethod]
	public void Create_SnapshotsObjectiveIcons()
	{
		var objectiveIcon = new StubIcon();
		var addedLater = new StubIcon();
		var objective = new StubObjective("objective") { Icon = objectiveIcon };
		var mission = new StubMission();
		mission.Objectives.Add(objective);

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		objective.Icon = addedLater;

		Assert.HasCount(2, view.Icons);
		Assert.IsInstanceOfType<MissionSourceIcon>(view.Icons[0]);
		Assert.AreSame(objectiveIcon, view.Icons[1]);
	}

	[TestMethod]
	public void Create_NoObjectiveIconsProducesSourceIcon()
	{
		var mission = new StubMission();

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.IsNotNull(view.Icons);
		Assert.HasCount(1, view.Icons);
		Assert.IsInstanceOfType<MissionSourceIcon>(view.Icons[0]);
	}
}
