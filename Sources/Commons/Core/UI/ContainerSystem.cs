using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.UI
{
    public class ContainerSystem : ModSystem
    {
        internal static ContainerPage Page { get; set; } = new ContainerPage( );

        public override void UpdateUI( GameTime gameTime )
        {
            Input.GetInformationFromDevice( );
            Page.DoReset( );
            Page.SeekAt( )?.Events.Update( );
            Page.CanSeek = false;
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