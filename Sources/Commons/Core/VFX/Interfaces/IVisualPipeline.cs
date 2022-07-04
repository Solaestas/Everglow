using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.VFX.Interfaces
{
    public interface IVisualPipeline
    {
        /// <summary>
        /// 批量渲染同一种，或者同一类 VFX
        /// </summary>
        /// <param name="visuals"></param>
        public void Render(IEnumerable<IVisual> visuals);
    }
}
