using System.Reflection;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.UI;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class MissionPresentationSystemTest
{
	[TestMethod]
	public void RefreshRequestState_IsExposedByPresentationSystem()
	{
		const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
		const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;

		PropertyInfo property = typeof(MissionPresentationSystem).GetProperty("NeedRefresh", PublicInstance);
		Assert.IsNotNull(property);
		Assert.AreEqual(typeof(bool), property.PropertyType);
		Assert.IsTrue(property.CanRead);
		Assert.IsTrue(property.CanWrite);
		Assert.IsNull(typeof(MissionPresentationService).GetProperty("NeedRefresh", PublicInstance));
		Assert.IsNull(typeof(MissionPresentationSystem).GetMethod("RequestRefresh", NonPublicInstance));
		Assert.IsNull(typeof(MissionContainer).GetField("_needRefresh", NonPublicInstance));
		Assert.IsNull(typeof(MissionContainer).GetMethod("RequestRefresh", NonPublicInstance));
	}
}
