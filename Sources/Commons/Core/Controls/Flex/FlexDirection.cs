using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Controls.Flex
{
    /// <summary>
    /// 容器弹性布局: 方向.
    /// <para>决定容器主轴的方向.</para>
    /// </summary>
    public enum FlexDirection
    {
        /// <summary>
        /// 横向, 从左至右.
        /// </summary>
        Row,
        /// <summary>
        /// 横向, 从右至左.
        /// </summary>
        RowReverse,
        /// <summary>
        /// 纵向, 从上至下.
        /// </summary>
        Column,
        /// <summary>
        /// 纵向, 从下至上.
        /// </summary>
        ColumnReverse
    }
}