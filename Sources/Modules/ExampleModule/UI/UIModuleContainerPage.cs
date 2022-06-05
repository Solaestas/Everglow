using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Commons.Core.UI.Preforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ExampleModule.UI
{
    /// <summary>
    /// 每个模块有且建议仅有一个容器页.
    /// <para>[!] 若欲区分不同作用的容器, 使用多个容器页也可以.</para>
    /// </summary>
    public class UIModuleContainerPage : ContainerPage
    {

        public Button Button0; //可以使用private, 若需要与其他容器进行交互则建议使用internal .

        public TestButton TestButton0;

        public TestPanel TestPanel;

        protected override void InitializeContainer( )
        {
            Button0 = new Button( );
            Button0.Events.OnMouseLeftClick += ( ) =>
            {
                Main.NewText( "Button被左键单击了！" ); //添加左键单击事件.
            };
            Button0.Events.OnMouseLeftDown += ( ) =>
            {
                Main.NewText( "Button被左键长按了！" ); //添加左键长按事件.
            };
            Button0.Events.OnMouseLeftUp += ( ) =>
            {
                Main.NewText( "Button被左键松开了！" ); //添加左键松开事件.
            };
            Register( Button0 );

            TestButton0 = new TestButton( );
            Register( TestButton0 );

            TestPanel = new TestPanel( );
            Register( TestPanel );

            base.InitializeContainer( );
            //在初始化容器后进行布局的初次设置
            Button0.ContainerElement.SetLayerout( 66 , 66 , 66 , 66 );
            TestButton0.ContainerElement.SetLayerout( 800 , 400 , 50 , 50 );
        }
    }
}