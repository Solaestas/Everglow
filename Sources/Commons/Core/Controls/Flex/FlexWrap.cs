using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Controls.Flex
{
    /// <summary>
    /// 容器弹性布局: 换行.
    /// <para>决定容器主轴的方向上, 子容器过多时如何换行.</para>
    /// </summary>
    public enum FlexWrap
    {
        /// <summary>
        /// 不换行.
        /// </summary>
        Nowrap,
        /// <summary>
        /// 换行, 第一行在主轴头部.
        /// </summary>
        Wrap,
        /// <summary>
        /// 换行, 第一行在主轴尾部.
        /// </summary>
        WrapReverse
    }
}