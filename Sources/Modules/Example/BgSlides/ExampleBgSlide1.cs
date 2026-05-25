using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Example.Items;

namespace Everglow.Example.BgSlides;

public class ExampleBgSlide1 : BgSlide
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		UniqueName = "ExampleBgSlide1";
		Texture = ModAsset.ExampleBgSlide1.Value;
		Distance = 45f;
	}

	public override bool CanActive()
	{
		return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ExampleBackgroundTool>();
	}
}