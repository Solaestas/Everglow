using Everglow.Commons.DataStructures;
using Everglow.Commons.Enums;

namespace Everglow.Commons.Utilities.BackgroundHelper;

public class BackgroundSystem : ModSystem
{
	public override void OnModLoad()
	{
		if (Main.netMode != NetmodeID.Server)
		{
			Ins.HookManager.AddHook(CodeLayer.PostDrawBG, DrawBackground);
		}
	}

	private List<BgSlide> backgroundSlides = new List<BgSlide>();

	private void DrawBackground()
	{
		if (backgroundSlides.Count <= 0)
		{
			return;
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
		Effect lastEffect = null;
		foreach (var bg in backgroundSlides.OrderByDescending(x => x.Distance))
		{
			bg.Update();
			bool shouldChangeSpriteBatch = bg.Shader != lastEffect;
			if (shouldChangeSpriteBatch)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
				if (bg.Shader is not null)
				{
					bg.Shader.CurrentTechnique.Passes[0].Apply();
				}
			}
			bg.Draw();
			lastEffect = bg.Shader;
		}
		backgroundSlides.RemoveAll(x => x.Active == false);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public void AddBgSlide(BgSlide bg)
	{
		if (backgroundSlides.FindAll(x => x.UniqueName == bg.UniqueName).Count <= 0)
		{
			backgroundSlides.Add(bg);
		}
		else
		{
			bg.Active = false;
		}
	}

	public static void DrawBackgroundAnchorWithWorldPos(BgSlide bg)
	{
	}
}