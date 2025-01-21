using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria;
using Terraria.ID;

namespace Everglow.UnitTests.Functions.UI;

[TestClass]
public class ItemDrawerTest
{
	[TestMethod]
	public void Create_Should_Return_Correctly_When_ParamIsValid()
	{
		var str = ItemDrawer.Create(1);
		Assert.AreEqual("[ItemDrawer,Type='1']", str);

		str = ItemDrawer.Create(1, 2);
		Assert.AreEqual("[ItemDrawer,Type='1',Stack='2']", str);

		str = ItemDrawer.Create(1, 2, new Microsoft.Xna.Framework.Color(1, 1, 1));
		Assert.AreEqual("[ItemDrawer,Type='1',Stack='2',StackColor='1,1,1,255']", str);

		str = ItemDrawer.Create(1, 2, new Microsoft.Xna.Framework.Color(1, 1, 1), 10);
		Assert.AreEqual("[ItemDrawer,Type='1',Stack='2',StackColor='1,1,1,255',StackFontSize='30']", str);
	}
}