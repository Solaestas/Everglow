using Microsoft.Xna.Framework;

namespace Everglow.Sources.Commons.Core.UI
{
    /// <summary>
    /// 表示一个根据一组速度数据进行移动的容器方法.
    /// </summary>
    public interface IMoveContainerFunction
    {
        /// <summary>
        /// 容器的横向速度.
        /// </summary>
        public float VelocityX { get; set; }

        /// <summary>
        /// 容器的纵向速度.
        /// </summary>
        public float VelocityY { get; set; }

        /// <summary>
        /// 该方法决定了容器对速度的修改.
        /// </summary>
        public void UpdateLocation( ContainerElement containerElement );

    }
}