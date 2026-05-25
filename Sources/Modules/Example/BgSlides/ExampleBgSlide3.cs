using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Example.Items;

namespace Everglow.Example.BgSlides;

public class ExampleBgSlide3 : BgSlide
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		UniqueName = "ExampleBgSlide3";
		Texture = ModAsset.ExampleBgSlide3.Value;
		Distance = 5f;
	}

	public override bool CanActive()
	{
		return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ExampleBackgroundTool>();
	}
}