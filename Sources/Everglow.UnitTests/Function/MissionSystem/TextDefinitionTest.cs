using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class TextDefinitionTest
{
	[TestMethod]
	[DataRow(null, "All")]
	[DataRow(MissionViewState.Active, "Accepted")]
	[DataRow(MissionViewState.Completed, "Completed")]
	public void GetPoolTypeText_ReturnsPresentationLabel(MissionViewState? state, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetPoolTypeText(state));
	}
}
