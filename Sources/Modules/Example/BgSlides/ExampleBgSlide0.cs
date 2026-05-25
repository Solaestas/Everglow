using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Example.Items;

namespace Everglow.Example.BgSlides;

public class ExampleBgSlide0 : BgSlide
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		UniqueName = "ExampleBgSlide0";
		Texture = ModAsset.ExampleBgSlide0.Value;
	}

	public override bool CanActive()
	{
		return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ExampleBackgroundTool>();
	}
}