using Colin.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IFrameAnimation? FrameAnimation;

        public Button( )
        {
            _primitiveBatch = new PrimitiveBatch( 4 );
        }

        public Button( Texture2D img )
        {
            Image = img;
        }

        protected override void DrawSelf( )
        {
            if ( Image == null )
                _primitiveBatch.DrawRectangle( Location, Size, Color.White );
            else
                EngineInfo.SpriteBatch.Draw( Image, BaseRectangle, FrameAnimation?.GetFrame( ), Color.White );
            base.DrawSelf( );
        }

        PrimitiveBatch _primitiveBatch;

    }
}