using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using ReLogic.Content;

namespace Everglow.Sources.Commons.Core.VFX.Base
{
    internal abstract class Pipeline : IVisualPipeline
    {
        protected Asset<Effect> effect;
        public abstract void BeginRender();

        public abstract void EndRender();

        public virtual void Load() { }

        public virtual void Render(IEnumerable<IVisual> visuals)
        {
            foreach (var visual in visuals)
            {
                visual.Draw();
            }
        }

        public virtual void Unload() { }
    }
}
