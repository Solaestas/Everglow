using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Controls
{
    /// <summary>
    /// 表示一个提供了基本布局数据的对象.
    /// </summary>
    public interface ILayout
    {
        /// <summary>
        /// 对象相对屏幕左上角的横坐标.
        /// </summary>
        float LocationX { get; set; }
        /// <summary>
        /// 对象相对屏幕左上角的纵坐标.
        /// </summary>
        float LocationY { get; set; }
        /// <summary>
        /// 对象的宽度.
        /// </summary>
        float Width { get; set; }
        /// <summary>
        /// 对象的高度.
        /// </summary>
        float Height { get; set; }
    }
}