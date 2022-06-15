namespace Everglow.Sources.Commons.Core.UI
{
    public class ContainerSystem : ModSystem
    {
        public static ContainerPage Page { get; private set; } = new ContainerPage( );

        /// <summary>
        /// 获取最后一次响应左键单击的容器实例.
        /// </summary>
        public static Container LeftClickContainer { get; private set; }

        /// <summary>
        /// 获取最后一次响应右键单击的容器实例.
        /// </summary>
        public static Container RightClickContainer { get; private set; }

        public override void UpdateUI( GameTime gameTime )
        {
            Input.GetInformationFromDevice( );
            if( Input.MouseLeftClick && Page.SeekAt( ) != null )
                LeftClickContainer = Page.SeekAt( );
            if( Input.MouseRightClick && Page.SeekAt( ) != null )
                RightClickContainer = Page.SeekAt( );
            Page.DoReset( );
            Page.SeekAt( )?.Events.Update( );
            Page.CanSeek = false;
            Page.DoUpdate( );

            base.UpdateUI(gameTime);
        }
        public override void PostDrawInterface( SpriteBatch spriteBatch )
        {
            Page.DoDraw( );
            base.PostDrawInterface(spriteBatch);
        }
    }
}