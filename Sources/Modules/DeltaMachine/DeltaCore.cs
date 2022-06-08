using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.UI;

namespace Everglow.Sources.Modules.DeltaMachine
{
    /// <summary>
    /// 戴尔塔机器模块: 戴尔塔核心.
    /// </summary>
    public class DeltaCore : IModule
    {
        public string Name => "DeltaMachine";

        public DeltaCoreUserInterface DeltaCoreUserInterface;

        public void Load( )
        {
            ContainerPage.RegisterContainerPage( DeltaCoreUserInterface = new DeltaCoreUserInterface( ) );
        }

        public void Unload( )
        {

        }
    }
}