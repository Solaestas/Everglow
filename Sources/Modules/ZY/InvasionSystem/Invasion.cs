using Everglow.Sources.Commons.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZY.InvasionSystem
{
    [ModuleDependency(typeof(InvasionSystem))]
    internal abstract class Invasion : IModule
    {
        public abstract string Name { get; }
        public const int VanillaCount = 5;
        //原版最高为5，所以这里从114开始
        private static int InvasionsCount = 114;
        public int InvasionID { get; private set; }
        public void Load()
        {
            InvasionID = InvasionsCount++;
        }

        public void Unload()
        {

        }

        public virtual void Draw()
        {

        }
    }
}
