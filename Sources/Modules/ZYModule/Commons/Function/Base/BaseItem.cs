using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base
{
    internal abstract class BaseItem : ModItem
    {
        public Asset<Texture2D> Asset => ModContent.Request<Texture2D>(Texture);
        protected override bool CloneNewInstances => true;
        public override bool IsCloneable => true;
    }
}
