using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;

namespace Everglow.Sources.Commons.Core.UI.Preforms
{
    /// <summary>
    /// 表示一个按钮.
    /// </summary>
    public class Button : Container
    {
        /// <summary>
        /// 按钮显示的图片.
        /// </summary>
        public Texture2D Image { get; private set; }

        public Button( ) { }
        public Button( Texture2D img )
        {
            Image = img;
        }

        protected override void DrawSelf( )
        {
            if ( Image == null )
                Main.spriteBatch.Draw( TextureAssets.MagicPixel.Value, BaseRectangle, ContainerElement.Color );
            else
                Main.spriteBatch.Draw( Image, BaseRectangle, ContainerElement.Color );
            base.DrawSelf( );
        }
    }
}