using Microsoft.Xna.Framework;

namespace Everglow.Sources.Commons.Core.Controls
{
    /// <summary>
    /// 表示一个可以拥有速度并根据其速度进行移动的控件.
    /// </summary>
    public interface IMoveableControl : ILayout
    {
        /// <summary>
        /// 控件的横向速度.
        /// </summary>
        public float VelocityX { get; set; }

        /// <summary>
        /// 控件的纵向速度.
        /// </summary>
        public float VelocityY { get; set; }

        /// <summary>
        /// 该方法决定了控件如何进行位置刷新.
        /// <para>[!] <seealso cref="VelocityX"/> 和 <seealso cref="VelocityY"/> 并不强制使用, </para>
        /// <para>[!] 但仍建议根据这两者进行位置的更新.</para>
        /// </summary>
        public void UpdateLocation( );
    }
}