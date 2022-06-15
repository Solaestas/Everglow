using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Commons.Core.UI.Preforms;

namespace Everglow.Sources.Modules.ExampleModule.UI
{
    public class TestPanel : Panel
    {
        public Button Button;
        protected override void InitializeContainer( )
        {
            Button = new Button( );
            Button.Events.OnMouseLeftClick += ( ) =>
            {
                Main.NewText( "科林可可爱爱捏❤" );
            };
            Register( Button );
            base.InitializeContainer( );
            Button.ContainerElement.SetSize( 50, 50 ); //设置按钮的大小, 当然, 写在 SetLayerout 也是可以的.
        }
        protected override void SetLayerout( ref ContainerElement containerElement )
        {
            containerElement.SetColor( Color.Gray ); //设置颜色.
            containerElement.SetSize( 300, 200 ); //设置面板的大小.

            Button.ContainerElement.SetLocation( 16, 16 ); //设置按钮相对于自己的位置.
            base.SetLayerout( ref containerElement );
        }
    }
}