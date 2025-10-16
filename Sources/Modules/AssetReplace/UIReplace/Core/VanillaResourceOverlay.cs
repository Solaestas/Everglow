using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.AssetReplace.UIReplace.Core;

// Most code from Example Mod. In "VanillaLifeOverlay" and "VanillaManaOverlay"
internal class VanillaResourceOverlay : ModResourceOverlay
{
	// These fields are used to cache the replacement assets
	public static ClassicBar ClassicBar = new();
	public static FancyBar FancyBar = new();
	public static HorizontalBar HorizontalBar = new();

	public override void PostDrawResource(ResourceOverlayDrawContext context)
	{
		// Classic Bar
		DrawClassicBarOverlay(context);

		// Fancy Bar
		DrawFancyBarOverlay(context);

		// Horizontal Bar
		DrawHorizontalBarOverlay(context);
	}

	private static void DrawClassicBarOverlay(ResourceOverlayDrawContext context)
	{
		CheckAndDraw(context, TextureAssets.Mana, ClassicBar.BlueStar);
		CheckAndDraw(context, TextureAssets.Heart, ClassicBar.RedHeart);
		CheckAndDraw(context, TextureAssets.Heart2, ClassicBar.GoldHeart);
	}

	private static void DrawFancyBarOverlay(ResourceOverlayDrawContext context)
	{
		var vanillaBar = UIReplaceModule.TerrariaAssets.FancyBar;
		CheckAndDraw(context, vanillaBar.HeartLeft, FancyBar.HeartLeft);
		CheckAndDraw(context, vanillaBar.HeartMiddle, FancyBar.HeartMiddle);
		CheckAndDraw(context, vanillaBar.HeartRight, FancyBar.HeartRight);
		CheckAndDraw(context, vanillaBar.HeartRightFancy, FancyBar.HeartRightFancy);
		CheckAndDraw(context, vanillaBar.HeartFillRed, FancyBar.HeartFillRed);
		CheckAndDraw(context, vanillaBar.HeartFillGold, FancyBar.HeartFillGold);
		CheckAndDraw(context, vanillaBar.HeartSingleFancy, FancyBar.HeartSingleFancy);
		CheckAndDraw(context, vanillaBar.StarA, FancyBar.StarA);
		CheckAndDraw(context, vanillaBar.StarB, FancyBar.StarB);
		CheckAndDraw(context, vanillaBar.StarC, FancyBar.StarC);
		CheckAndDraw(context, vanillaBar.StarSingle, FancyBar.StarSingle);
		CheckAndDraw(context, vanillaBar.StarFill, FancyBar.StarFill);
	}

	private static void DrawHorizontalBarOverlay(ResourceOverlayDrawContext context)
	{
		var vanillaBar = UIReplaceModule.TerrariaAssets.HorizontalBar;
		CheckAndDraw(context, vanillaBar.HpFill, HorizontalBar.HpFill);
		CheckAndDraw(context, vanillaBar.HpFillGold, HorizontalBar.HpFillGold);
		CheckAndDraw(context, vanillaBar.MpFill, HorizontalBar.MpFill);
		CheckAndDraw(context, vanillaBar.PanelLeft, HorizontalBar.PanelLeft);
		CheckAndDraw(context, vanillaBar.HpPanelMiddle, HorizontalBar.HpPanelMiddle);
		CheckAndDraw(context, vanillaBar.HpPanelRight, HorizontalBar.HpPanelRight);
		CheckAndDraw(context, vanillaBar.MpPanelMiddle, HorizontalBar.MpPanelMiddle);
		CheckAndDraw(context, vanillaBar.MpPanelRight, HorizontalBar.MpPanelRight);
	}

	// This is a helper method for checking if a certain vanilla asset was drawn
	// If it is, draw the replacement
	private static void CheckAndDraw(ResourceOverlayDrawContext context, Asset<Texture2D> compareAsset,
		Asset<Texture2D> replacement)
	{
		if (context.texture != compareAsset)
		{
			return;
		}

		context.texture = replacement;
		context.Draw();
	}

	public override void Unload()
	{
		ClassicBar = null;
		FancyBar = null;
		HorizontalBar = null;
	}
}