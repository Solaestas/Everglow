using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base
{
    internal abstract class BaseItem : ModItem
    {
        protected override bool CloneNewInstances => true;
        public override bool IsCloneable => true;
    }
}
