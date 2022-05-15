namespace Everglow.Sources.Commons.Core.Controls
{
    /// <summary>
    /// 表示一个允许进行百分比缩放的控件.
    /// </summary>
    public interface IScaleableControl
    {
        /// <summary>
        /// 控件的缩放.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 控件缩放的增量.
        /// </summary>
        public float ScaleIncrement { get; set; }

        /// <summary>
        /// 缩放的目标值.
        /// </summary>
        public float ScaleTarget { get; set; }

        /// <summary>
        /// 该方法决定了控件如何进行缩放刷新.
        /// <para>[!] <seealso cref="ScaleIncrement"/> 和 <seealso cref="ScaleTarget"/> 并不强制使用, </para>
        /// <para>[!] 但仍建议你根据这两者进行缩放的更新.</para>
        /// </summary>
        public void UpdateScale( );

    }
}