using Everglow.Sources.Commons.Core.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace Everglow_UnitTest.ZYModule;

[TestClass]
public class RotationTest
{
    [TestMethod]
    public void ApproachTest()
    {
        Rotation rot = 1;
        Rotation target = -1;
        Rotation target2 = -3;
        Assert.IsTrue(rot.Approach(target, 1).Angle == 0);
        Assert.IsTrue(rot.Approach(target2, 1).Angle == 2);
    }

    [TestMethod]
    public void DistanceTest()
    {
        Rotation rot = 1;
        Rotation target = -1;
        Rotation target2 = -3;
        Assert.IsTrue(rot.Distance(target).Angle == 2);
        Assert.IsTrue(rot.Distance(target2).Angle == MathHelper.TwoPi - 4);
    }

}
