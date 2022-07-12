using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.VFX
{
    public enum SortMode
    {
        Deferred,
        FrontToBack,
        BackToFront
    }
    public class VFXBatch
    {
        public GraphicsDevice GraphicsDevice { get; private set; }

        public VFXBatch(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
        }

        public void Begin()
        {

        }
        public void Begin(SortMode sortMode, BlendState blendState)
        {

        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            
        }
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {

        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {

        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0)
        {

        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth = 0)
        {

        }
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {

        }
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {

        }
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {

        }
        public void Flush()
        {

        }
        public void End()
        {

        }
    }
}
