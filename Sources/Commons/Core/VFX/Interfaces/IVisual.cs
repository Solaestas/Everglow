using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Commons.Core.VFX.Interfaces
{
    [ModuleDependency(typeof(VFXManager))]
    public interface IVisual : IModule
    {
        /// <summary>
        /// 判断这个视觉特效是否还处于激活状态。我们需要保证如果它不是激活状态那么以后不会再用到它
        /// </summary>
        public bool Active
        {
            get;
            set;
        }
        public CallOpportunity DrawLayer { get; }
        public void Update();
        public void Draw();
        public void Kill();
    }
}
