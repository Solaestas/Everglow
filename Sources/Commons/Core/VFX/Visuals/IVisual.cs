using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Commons.Core.VFX.Visuals
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
        }

        /// <summary>
        /// 判断这个视觉特效是否需要绘制
        /// </summary>
        public bool Visible
        {
            get;
        }

        public int Type
        {
            get;
        }

        public CallOpportunity DrawLayer
        {
            get;
        }

        public void Update();

        public void Draw();

        public void Kill();
    }
}