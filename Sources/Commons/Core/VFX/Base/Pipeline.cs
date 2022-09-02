using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using ReLogic.Content;

namespace Everglow.Sources.Commons.Core.VFX.Base
{
    internal abstract class Pipeline : IPipeline
    {
        protected Asset<Effect> effect;
        /// <summary>
        /// 准备开始渲染
        /// </summary>
        public abstract void BeginRender();
        /// <summary>
        /// 结束渲染，刷新VFXBatch
        /// </summary>
        public abstract void EndRender();

        public virtual void Load() { }

        public void Render(IEnumerable<IVisual> visuals)
        {
            BeginRender();
            foreach (var visual in visuals)
            {
                visual.Draw();
            }
            EndRender();
        }

        public virtual void Unload() { }
    }
}
