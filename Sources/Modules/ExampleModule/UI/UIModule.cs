using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.UI;

namespace Everglow.Sources.Modules.ExampleModule.UI
{
    public class UIModule : IModule
    {
        public string Name { get; } = "测试用用户交互界面";

        public void Load()
        {
            ContainerPage.RegisterContainerPage(new UIModuleContainerPage()); //在IModule类内手动注册容器页.
        }

        public void Unload()
        {

        }
    }
}
