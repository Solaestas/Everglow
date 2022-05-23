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
            ICollider collider = new CRectangle(0, 0, 1, 1);
            ICollider rect = new CRectangle(0, 1, 1, 1);
            Assert.IsFalse(collider.Colliding(rect));
        }
    }
}
