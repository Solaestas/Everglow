using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
