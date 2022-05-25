using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
namespace Everglow_UnitTest.ZYModule
{
    [TestClass]
    public class ColliderTest
    {
        [TestMethod]
        public void Collision()
        {
            ICollider rec = new CRectangle(0, 0, 1, 1);
            ICollider collider = new CRectangle(0, -1, 1, 1);
            ICollider line = new CLine(new Vector2(0, 0), new Vector2(1, 0));
            CCircle circle = new CCircle(new Vector2(0, 0), 1);
            Assert.IsFalse(rec.Colliding(line));
        }
    }
}
