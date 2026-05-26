using Everglow.Commons.DataStructures;
using Everglow.Commons.Enums;

namespace Everglow.Commons.Utilities.BackgroundHelper;

public class BackgroundSystem : ModSystem
{
	private static readonly Comparer<BackgroundSlideBase> BackgroundSlideComparer =
		Comparer<BackgroundSlideBase>.Create((x, y) =>
		{
			int cmp = y.Distance.CompareTo(x.Distance);
			if (cmp == 0)
			{
				cmp = x.UniqueName.CompareTo(y.UniqueName);
			}
			return cmp;
		});

	private List<BackgroundSlideBase> backgroundSlides;

	public override void Load()
	{
		backgroundSlides = [];
	}

	public override void Unload()
	{
		backgroundSlides.Clear();
		backgroundSlides = null;
	}

	public override void OnModLoad()
	{
		if (!NetUtils.IsServer)
		{
			Ins.HookManager.AddHook(CodeLayer.PostDrawBG, DrawBackground);
		}
	}

	public override void OnWorldUnload()
	{
		backgroundSlides.Clear();
	}

	public override void PostUpdateEverything()
	{
		backgroundSlides.RemoveAll(x => x.Active == false);
		foreach (var slide in backgroundSlides)
		{
			slide.Update();
		}
	}

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
		foreach (var bg in backgroundSlides)
		{
			bool shouldChangeSpriteBatch = bg.Shader != lastEffect;
			if (shouldChangeSpriteBatch)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
				bg.Shader?.CurrentTechnique.Passes[0].Apply();
			}
			bg.Draw();
			lastEffect = bg.Shader;
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public bool AddBackgroundSlide(BackgroundSlideBase bg)
	{
		int index = backgroundSlides.BinarySearch(bg, BackgroundSlideComparer);
		if (!bg.AllowMultiple
			&& index >= 0)
		{
			return false;
		}

		index = ~index;
		backgroundSlides.Insert(index, bg);

		return true;
	}
}