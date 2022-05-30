using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.UI
{
    /// <summary>
    /// 容器页: 建议每个模块仅使用一次.
    /// </summary>
    public class ContainerPage : Container
    {
        public override sealed bool GetInterviewState( )
        {
            return false;
        }
        public static void RegisterContainerPage( ContainerPage containerPage )
        {
            containerPage.DoInitialize( );
            ContainerSystem.Page.Register( containerPage );
        }
        protected override sealed void SetLayerout( ref ContainerElement containerElement )
        {
            containerElement.SetLayerout( 0, 0, Main.screenWidth, Main.screenHeight );
            base.SetLayerout( ref containerElement );
        }
    }
}