using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Everglow.Sources.Commons.Function.ObjectPool
{
    [TestClass]
    public class RenderTargetPoolTest
    {
        private readonly RenderTargetPool renderTargetPool;

        public RenderTargetPoolTest( )
        {
            renderTargetPool = new RenderTargetPool( );
        }

        [TestMethod]
        public void UT_RenderTargetPoolTest_ReleaseSequence1( )
        {
            // 这个获取释放队列应该让RenderTargetPool的对象数量一直保持1
            for ( int i = 0; i < 10; i++ )
            {
                var res = renderTargetPool.GetRenderTarget2D( );
                res.Release( );
            }
        }
    }
}