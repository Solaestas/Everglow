using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Example.Items;

public class ExampleStabbingSwordConfigDrawer : ModSystem
{
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(sBS.SortMode, BlendState.AlphaBlend, SamplerState.PointWrap, sBS.DepthStencilState, RasterizerState.CullNone, sBS.Effect, Main.UIScaleMatrix);
		Main.LocalPlayer.GetModPlayer<ExampleStabbingSword_Config>().Draw();
		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}