using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Commons.Core.UI.Preforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ExampleModule.UI
{
    public class TestPanel : Panel
    {
        public Button Button;
        public Panel Panel;
        protected override void InitializeContainer( )
        {
            Button = new Button( );
            Button.Events.OnMouseLeftClick += ( ) =>
            {
                Main.NewText( "科林可可爱爱捏❤" );
            };
            Register( Button );

            Panel = new Panel( );
            Panel.Events.OnMouseLeftClick += ( ) =>
            {
                Main.NewText( "剪裁响应测试." );
            };
            Register( Panel );
            base.InitializeContainer( );
            Button.ContainerElement.SetSize( 50 , 50 ); //设置按钮的大小, 当然, 写在 SetLayerout 也是可以的.
        }
        protected override void SetLayerout( ref ContainerElement containerElement )
        {
            containerElement.SetColor( Color.Gray ); //设置颜色.
            containerElement.SetSize( 300 , 200 ); //设置面板的大小.

            Button.ContainerElement.SetLocation( 10, 10 ); //设置按钮相对于自己的位置.

            Panel.ContainerElement.SetLocation( 200 , 100 );
            Panel.ContainerElement.SetSize( 500 , 500);
            Panel.ContainerElement.SetColor( Color.Blue ); //用于测试剪裁.

            base.SetLayerout( ref containerElement );
        }
    }
}