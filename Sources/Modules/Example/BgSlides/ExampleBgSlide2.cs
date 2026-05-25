using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Example.Items;

namespace Everglow.Example.BgSlides;

public class ExampleBgSlide2 : BgSlide
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		UniqueName = "ExampleBgSlide2";
		Texture = ModAsset.ExampleBgSlide2.Value;
		Distance = 15f;
	}

	public override bool CanActive()
	{
		return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ExampleBackgroundTool>();
	}
}