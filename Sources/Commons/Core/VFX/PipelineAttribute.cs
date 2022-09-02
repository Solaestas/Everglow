namespace Everglow.Sources.Commons.Core.VFX
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class PipelineAttribute : Attribute
    {
        public readonly Type[] types;

        public PipelineAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}