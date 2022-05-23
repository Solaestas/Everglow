using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.UI
{
    public class ContainerSystem : ModSystem
    {
        internal static Container Page { get; private set; } = new Container( );

        public override void UpdateUI( GameTime gameTime )
        {
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