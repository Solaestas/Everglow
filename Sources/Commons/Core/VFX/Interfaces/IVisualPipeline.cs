using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.VFX.Interfaces
{
    public interface IPipeline
    {
        /// <summary>
        /// 批量渲染同一种，或者同一类 VFX
        /// </summary>
        /// <param name="visuals"></param>
        public void Render(IEnumerable<IVisual> visuals);
        /// <summary>
        /// 用于加载Effect等资源
        /// </summary>
        public void Load();
        /// <summary>
        /// 用于释放资源
        /// </summary>
        public void Unload();
    }
}
