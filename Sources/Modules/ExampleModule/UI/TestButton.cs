using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Commons.Core.UI.Preforms;

namespace Everglow.Sources.Modules.ExampleModule.UI
{
    public class TestButtonEvents : ContainerEvents //允许定制事件.
    {
        protected override void InterviewEvent( )
        {
            Container.ContainerElement.SetColor(Color.Yellow); //可交互的时候 按钮变成黄色.
            base.InterviewEvent( );
        }
        protected override void MouseLeftDownEvent( )
        {
            Dust.NewDust(Main.LocalPlayer.Center,0,0,DustID.Dirt,0,-10);
            Container.ContainerElement.SetColor(Color.Gray);
            //如果一开始可交互对象就是绑定的容器,
            //而且此时按着左键, 那么变成灰的.
            base.MouseLeftDownEvent( );
        }

        public TestButtonEvents( Container container ) : base(container) { }
    }
    //继承按钮类, 以设计一个通用的模板。
    public class TestButton : Button
    {
        protected override void InitializeContainer( )
        {
            Events = new TestButtonEvents(this); //将事件更改为自己定制的事件.
            base.InitializeContainer( );
        }
        protected override void ResetUpdate( )
        {
            ContainerElement.SetColor(Color.White);
            base.ResetUpdate( );
        }
    }
}