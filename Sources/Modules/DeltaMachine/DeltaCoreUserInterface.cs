using Everglow.Sources.Commons.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.DeltaMachine
{
    /// <summary>
    /// 戴尔塔机器模块中负责维护用户交互界面的类.
    /// </summary>
    public class DeltaCoreUserInterface : ContainerPage
    {
        public static List<Container> RockContainers;

        protected override void InitializeContainer( )
        {
            RockContainers = new List<Container>( );
            base.InitializeContainer( );
        }
    }
}