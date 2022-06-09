using Terraria.GameContent;

namespace Everglow.Sources.Commons.Core.UI.Preforms
{
    /// <summary>
    /// 一块可拖动的底板.
    /// <para>[!] 若你想关闭它的拖动功能, </para>
    /// <para>[!] 请在*初始化后*将 <seealso cref="ContainerEvents.Drag"/> 设置为 <see href="false"/>.</para>
    /// </summary>
    public class Panel : Container
    {
        public Texture2D Image { get; private set; }

        public Panel( ) { }
        public Panel( Texture2D img )
        {
            Image = img;
        }

        protected override void InitializeContainer( )
        {
            Events.Drag = true;
            Events.CanGetForPointer = false;
            EnableScissor = true;
            base.InitializeContainer( );
        }
        protected override void DrawSelf( )
        {
            if ( Image != null )
                Main.spriteBatch.Draw( Image, BaseRectangle, ContainerElement.Color );
            else
                Main.spriteBatch.Draw( TextureAssets.MagicPixel.Value, BaseRectangle, ContainerElement.Color );
            base.DrawSelf( );
        }
    }
}