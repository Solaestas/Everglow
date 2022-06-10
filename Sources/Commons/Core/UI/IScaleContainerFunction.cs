namespace Everglow.Sources.Commons.Core.UI
{
    /// <summary>
    /// 表示一个根据一组缩放数据进行缩放的对象.
    /// </summary>
    public interface IScaleContainerFunction
    {
        /// <summary>
        /// 容器的缩放中心.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// 容器缩放的增量.
        /// </summary>
        public float ScaleAdd { get; set; }

        /// <summary>
        /// 缩放的目标值.
        /// </summary>
        public float ScaleTarget { get; set; }

        /// <summary>
        /// 容器的缩放.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 该方法决定了容器如何进行缩放刷新.
        /// <para>[!] <seealso cref="ScaleAdd"/> 和 <seealso cref="ScaleTarget"/> 并不强制使用, </para>
        /// <para>[!] 但仍建议你根据这两者进行缩放的更新.</para>
        /// </summary>
        public void UpdateScale( ContainerElement containerElement );

    }
}