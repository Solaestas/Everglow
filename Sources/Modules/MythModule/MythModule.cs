using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Modules.MythModule.TheFirefly.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule
{
    public class MythModule: IModule
    {
        public string Name { get; } = "神话";

        public void Load( )
        {
            ContainerPage.RegisterContainerPage(new FireflyContainerPage()); //在IModule类内手动注册容器页.
        }

        public void Unload( )
        {

        }
    }
}
