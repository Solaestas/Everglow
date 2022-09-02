using Terraria.GameContent;

namespace Everglow.Sources.Commons.Core.UI.Preforms
{
    /// <summary>
    /// 一块可拖动的底板.
    /// <para>[!] 若你想关闭它的拖动功能, </para>
    /// <para>[!] 请在||| 初始化后 |||将 <seealso cref="ContainerEvents.Drop"/> 设置为 <see href="false"/>.</para>
    /// </summary>
    public class Panel : Container
    {
        private int _borderSize;
        public Texture2D Image { get; private set; }

        public Panel() { }
        public Panel(Texture2D img, int borderSize)
        {
            Image = img;
            _borderSize = borderSize;
        }

        protected override void InitializeContainer()
        {
            Events.Drag = true;
            Events.CanGetForPointer = false;
            base.InitializeContainer();
        }

        protected override void DrawSelf()
        {
            Main.spriteBatch.End();
            RasterizerState OverflowHiddenRasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };
            Rectangle clippingRectangle = BaseRectangle;
            Main.spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
            Main.spriteBatch.GraphicsDevice.RasterizerState = OverflowHiddenRasterizerState;
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null);
            if (Image != null)
                Main.spriteBatch.Draw(Image, BaseRectangle, ContainerElement.Color);
            else
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, BaseRectangle, ContainerElement.Color);
            base.DrawSelf();
        }
        protected override void PostDraw()
        {
            base.PostDraw();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
        }
    }
}