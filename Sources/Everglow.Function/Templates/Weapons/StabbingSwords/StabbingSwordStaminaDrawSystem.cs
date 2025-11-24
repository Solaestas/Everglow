using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords;

public class StabbingSwordStaminaDrawSystem : ModSystem
{
	private Vector2 pos = Vector2.Zero;

	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(sBS.SortMode, BlendState.AlphaBlend, SamplerState.PointWrap, sBS.DepthStencilState, RasterizerState.CullNone, sBS.Effect, Main.GameViewMatrix.TransformationMatrix);
		Player p = Main.LocalPlayer;
		pos = Vector2.Lerp(pos, p.Center + new Vector2(-50 * p.direction, 40), 0.2f);
		p.GetModPlayer<StabbingSwordStaminaPlayer>().DrawStamina(pos);
		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}