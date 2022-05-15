using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Controls.Flex
{
    /// <summary>
    /// 弹性布局: 定义子容器在主轴上的对齐方式.
    /// </summary>
    public enum JustifyContent
    {
        /// <summary>
        /// 左对齐.
        /// </summary>
        FlexStart,
        /// <summary>
        /// 右对齐.
        /// </summary>
        FlexEnd,
        /// <summary>
        /// 居中.
        /// </summary>
        Center,
        /// <summary>
        /// 两端对齐, 项目之间的间隔都相等.
        /// </summary>
        SpaceBetween,
        /// <summary>
        /// 每个项目两侧的间隔相等. 项目之间的间隔比项目与边框的间隔大一倍.
        /// </summary>
        SpaceAround,
    }
}