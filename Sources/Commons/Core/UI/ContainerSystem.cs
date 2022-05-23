using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.UI
{
    public class ContainerSystem : ModSystem
    {
        internal static Container Page { get; set; } = new Container( );

        bool _started = false;
        public override void UpdateUI( GameTime gameTime )
        {
            Input.GetInformationFromDevice( );
            Page.SeekAt()?.Events.Update( );
            Page.CanSeek = false;
            Page.ContainerElement.SetLayerout( 0 , 0 , Main.screenWidth , Main.screenHeight );
            Page.DoUpdate( );
            base.UpdateUI( gameTime );
        }
        public override void PostDrawInterface( SpriteBatch spriteBatch )
        {
            Page.DoDraw( );
            base.PostDrawInterface( spriteBatch );
        }
    }
}